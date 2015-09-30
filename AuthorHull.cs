using Facepunch.Actor;
using Facepunch.Intersect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AuthorSuiteCreation(Title="Author Hull", Description="Create a new character. Allows you to define hitboxes and fine tune ragdoll and joints.", Scripter="Pat", OutputType=typeof(Character), Ready=true)]
public class AuthorHull : AuthorCreation
{
	private const string suffix_rigid = "::RAGDOLL_OUTPUT::";

	private const string suffix_hitbox = "::HITBOX_OUTPUT::";

	[SerializeField]
	private GameObject modelPrefab;

	[SerializeField]
	private GameObject modelPrefabForHitBox;

	[SerializeField]
	private GameObject modelPrefabInstance;

	[SerializeField]
	private GameObject hitBoxOutputPrefab;

	[SerializeField]
	private GameObject ragdollOutputPrefab;

	[SerializeField]
	private Vector3 hitCapsuleCenter;

	[SerializeField]
	private float hitCapsuleRadius = 1f;

	[SerializeField]
	private float hitCapsuleHeight = 2.5f;

	[SerializeField]
	private int hitCapsuleDirection;

	[SerializeField]
	private bool drawBones;

	[SerializeField]
	private bool allowBonesOutsideOfModelPrefab;

	[SerializeField]
	private int ignoreCollisionDownSteps = 2;

	[SerializeField]
	private int ignoreCollisionUpSteps = 1;

	[SerializeField]
	private string hitBoxType = "HitBox";

	[SerializeField]
	private string hitBoxSystemType = "HitBoxSystem";

	[SerializeField]
	private string defaultBodyPartLayer = string.Empty;

	[SerializeField]
	private ActorRig actorRig;

	[SerializeField]
	private Character prototype;

	[SerializeField]
	private Ragdoll ragdollPrototype;

	[SerializeField]
	private Transform hitCapsuleTransform;

	[SerializeField]
	private Transform[] removeThese;

	[SerializeField]
	private float eyeHeight = 1f;

	[SerializeField]
	private GameObject generatedRigid;

	[SerializeField]
	private GameObject generatedHitBox;

	[SerializeField]
	private bool savedGenerated;

	[SerializeField]
	private Material[] materials;

	[SerializeField]
	private Vector3 editingAngles = Vector3.zero;

	[SerializeField]
	private bool editingCenterToRoot;

	[SerializeField]
	private BodyPartTransformMap bodyParts = new BodyPartTransformMap();

	[SerializeField]
	private string saveJSONGUID;

	[SerializeField]
	private string previewPrototypeGUID;

	[SerializeField]
	private bool removeAnimationFromRagdoll;

	[SerializeField]
	private string hitBoxLayerName = "Hitbox";

	[SerializeField]
	private bool useMeshesFromHitBoxOnRagdoll;

	private static bool once;

	private HitBoxSystem creatingSystem;

	private bool showAllBones;

	private readonly static MemberFilter actorRigSearch;

	static AuthorHull()
	{
		AuthorHull.actorRigSearch = new MemberFilter(AuthorHull.ActorRigSearch);
	}

	public AuthorHull() : this(typeof(Character))
	{
	}

	protected AuthorHull(Type type) : base(type)
	{
	}

	private static bool ActorRigSearch(MemberInfo m, object filterCriteria)
	{
		return ((FieldInfo)m).FieldType == typeof(ActorRig);
	}

	private void ApplyMaterials(GameObject instance)
	{
		SkinnedMeshRenderer componentInChildren;
		if (instance != null)
		{
			componentInChildren = instance.GetComponentInChildren<SkinnedMeshRenderer>();
		}
		else
		{
			componentInChildren = null;
		}
		SkinnedMeshRenderer skinnedMeshRenderer = componentInChildren;
		if (skinnedMeshRenderer)
		{
			skinnedMeshRenderer.sharedMaterials = this.materials;
		}
	}

	private IDMain ApplyPrototype(GameObject output, IDMain prototype)
	{
		int num;
		IDMain dMain = null;
		if (prototype)
		{
			Component[] components = prototype.GetComponents<Component>();
			Component[] componentArray = new Component[(int)components.Length];
			Component[] componentArray1 = new Component[(int)components.Length];
			int length = (int)components.Length;
			int num1 = -1;
			int num2 = 500;
			do
			{
			Label0:
				int num3 = 0;
				for (int i = 0; i < length; i++)
				{
					Component component = components[i];
					if (!component)
					{
						components[i] = null;
					}
					else if (!(component is Transform))
					{
						Component component1 = output.AddComponent(component.GetType());
						if (!component1)
						{
							num3++;
						}
						else
						{
							componentArray1[i] = component1;
							components[i] = null;
							componentArray[i] = component;
							if (component1 is IDMain)
							{
								dMain = (IDMain)component1;
							}
						}
					}
					else
					{
						components[i] = null;
						componentArray[i] = component;
						int num4 = i;
						num1 = num4;
						componentArray1[num4] = output.transform;
					}
				}
				if (num3 == 0)
				{
					num = num2;
					num2 = num - 1;
				}
				else
				{
					goto Label0;
				}
			}
			while (num <= 0);
			if (num2 < 0)
			{
				UnityEngine.Debug.LogError("Couldnt remake all components");
			}
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < length; k++)
				{
					if (k != num1)
					{
						Component component2 = componentArray[k];
						Component component3 = componentArray1[k];
						if (component2)
						{
							if (!component3)
							{
								UnityEngine.Debug.LogWarning(string.Concat("no dest for source ", component2), component2);
							}
							else
							{
								this.TransferComponentSettings(prototype.gameObject, output, componentArray, componentArray1, component2, component3);
								AuthorShared.SetDirty(component3);
							}
						}
						else if (!component3)
						{
							UnityEngine.Debug.LogWarning("no source or dest", output);
						}
						else
						{
							UnityEngine.Debug.LogWarning(string.Concat("no source for dest ", component3), component3);
						}
					}
				}
			}
			output.layer = prototype.gameObject.layer;
			output.tag = prototype.gameObject.tag;
		}
		return dMain;
	}

	private bool ApplyRig(GameObject output)
	{
		bool flag = false;
		if (!this.actorRig)
		{
			BoneStructure component = output.GetComponent<BoneStructure>();
			if (component)
			{
				UnityEngine.Object.DestroyImmediate(component, true);
			}
		}
		else
		{
			BoneStructure.EditorOnly_AddOrUpdateBoneStructure(output, this.actorRig);
			flag = true;
		}
		return flag;
	}

	private List<KeyValuePair<MethodInfo, MonoBehaviour>> CaptureFinalizeMethods(GameObject output, string methodName)
	{
		List<KeyValuePair<MethodInfo, MonoBehaviour>> keyValuePairs = new List<KeyValuePair<MethodInfo, MonoBehaviour>>();
		MonoBehaviour[] componentsInChildren = output.GetComponentsInChildren<MonoBehaviour>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			MonoBehaviour monoBehaviour = componentsInChildren[i];
			if (monoBehaviour)
			{
				MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int j = 0; j < (int)methods.Length; j++)
				{
					MethodInfo methodInfo = methods[j];
					if (methodInfo.Name == methodName)
					{
						keyValuePairs.Add(new KeyValuePair<MethodInfo, MonoBehaviour>(methodInfo, monoBehaviour));
					}
				}
			}
		}
		return keyValuePairs;
	}

	private void ChangedEditingOptions()
	{
		if (this.modelPrefabInstance)
		{
			this.modelPrefabInstance.transform.localEulerAngles = this.editingAngles;
			this.modelPrefabInstance.transform.localPosition = Vector3.zero;
			if (this.editingCenterToRoot)
			{
				Transform rootBone = AuthorShared.GetRootBone(this.modelPrefabInstance.GetComponentInChildren<SkinnedMeshRenderer>());
				if (rootBone)
				{
					this.modelPrefabInstance.transform.position = -rootBone.position;
				}
			}
		}
	}

	private void ChangedModelPrefab()
	{
		if (this.modelPrefabInstance)
		{
			UnityEngine.Object.DestroyImmediate(this.modelPrefabInstance);
		}
		this.modelPrefabInstance = AuthorShared.InstantiatePrefab(this.modelPrefab);
		this.modelPrefabInstance.transform.localPosition = Vector3.zero;
		this.modelPrefabInstance.transform.localRotation = Quaternion.identity;
		this.modelPrefabInstance.transform.localScale = Vector3.one;
	}

	private GameObject CreateEyes(GameObject output)
	{
		GameObject gameObject = new GameObject("Eyes");
		gameObject.transform.parent = output.transform;
		gameObject.transform.localPosition = new Vector3(0f, this.eyeHeight, 0f);
		return gameObject;
	}

	public HitBox CreateHitBox(GameObject target)
	{
		HitBox hitBox = AuthorShared.AddComponent<HitBox>(target, this.hitBoxType);
		AuthorShared.SetSerializedProperty(hitBox, "_hitBoxSystem", this.creatingSystem);
		hitBox.idMain = hitBox.hitBoxSystem.idMain;
		return hitBox;
	}

	public HitBoxSystem CreateHitBoxSystem(GameObject target)
	{
		return AuthorShared.AddComponent<HitBoxSystem>(target, this.hitBoxSystemType);
	}

	private void DestroyRepresentations(ref GameObject stored, string suffix)
	{
		if (stored)
		{
			UnityEngine.Object.DestroyImmediate(stored);
		}
		UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			UnityEngine.Object obj = objArray[i];
			if (obj && ((GameObject)obj).transform.parent == null && obj.name.EndsWith(suffix))
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	public override IEnumerable<AuthorPeice> DoSceneView()
	{
		if (this.drawBones && this.modelPrefabInstance != null)
		{
			Transform rootBone = AuthorShared.GetRootBone(this.modelPrefabInstance);
			if (rootBone)
			{
				Color color = AuthorShared.Scene.color;
				Color color1 = color * new Color(0.9f, 0.8f, 0.3f, 0.1f);
				List<Transform> transforms = rootBone.ListDecendantsByDepth();
				AuthorShared.Scene.color = color1;
				foreach (Transform transforms1 in transforms)
				{
					Vector3 vector3 = transforms1.parent.position;
					Vector3 vector31 = transforms1.position - vector3;
					float single = vector31.magnitude;
					if (single != 0f)
					{
						Quaternion quaternion = Quaternion.LookRotation(vector31, transforms1.up);
						AuthorShared.Scene.DrawBone(vector3, quaternion, single, Mathf.Min(single / 2f, 0.025f), Vector3.one * Mathf.Min(single, 0.05f));
					}
				}
				AuthorShared.Scene.color = color;
			}
		}
		return base.DoSceneView();
	}

	private bool EnsureItsAPrefab(ref UnityEngine.Object obj)
	{
		return !obj;
	}

	protected override IEnumerable<AuthorPalletObject> EnumeratePalletObjects()
	{
		AuthorPalletObject[] authorPalletObjectArray = AuthorHull.HitBoxItems.pallet;
		if (!AuthorHull.once)
		{
			authorPalletObjectArray[0].guiContent.image = AuthorShared.ObjectContent(null, typeof(BoxCollider)).image;
			authorPalletObjectArray[1].guiContent.image = AuthorShared.ObjectContent(null, typeof(SphereCollider)).image;
			authorPalletObjectArray[2].guiContent.image = AuthorShared.ObjectContent(null, typeof(CapsuleCollider)).image;
			AuthorHull.once = true;
		}
		return authorPalletObjectArray;
	}

	internal void FigureOutDefaultBodyPart(ref Transform bone, ref BodyPart bodyPart, ref Transform mirrored, ref BodyPart mirroredBodyPart)
	{
		Transform transforms;
		BodyPart bodyPart1 = (BodyPart)((int)bodyPart);
		for (BodyPart i = BodyPart.Undefined; i < (BodyPart.Neck | BodyPart.Brain | BodyPart.L_UpperArm0 | BodyPart.L_Hand | BodyPart.L_Finger_Index3 | BodyPart.L_Finger_Middle2 | BodyPart.L_Finger_Ring1 | BodyPart.L_Finger_Pinky0 | BodyPart.L_Finger_Pinky4 | BodyPart.L_Finger_Thumb3 | BodyPart.L_Thigh1 | BodyPart.L_Heel0 | BodyPart.L_EyeLidLower | BodyPart.L_Cheek); i = (BodyPart)((int)i + (int)BodyPart.Hip))
		{
			if (this.bodyParts.TryGetValue(i, out transforms) && transforms == bone)
			{
				bodyPart1 = i;
			}
		}
		if ((int)bodyPart1 != (int)bodyPart)
		{
			bodyPart = bodyPart1;
			if (!mirrored && ((BodyPart)((int)bodyPart)).IsSided())
			{
				bodyPart1 = bodyPart1.SwapSide();
				if (this.bodyParts.TryGetValue(bodyPart1, out mirrored))
				{
					mirroredBodyPart = bodyPart1;
				}
			}
		}
	}

	[Conditional("LOG_GENERATE")]
	private static void GenerateLog(object text)
	{
		UnityEngine.Debug.Log(text);
	}

	[Conditional("LOG_GENERATE")]
	private static void GenerateLog(object text, UnityEngine.Object obj)
	{
		UnityEngine.Debug.Log(text, obj);
	}

	private void GeneratePrefabInstances()
	{
		this.DestroyRepresentations(ref this.generatedRigid, "::RAGDOLL_OUTPUT::");
		this.generatedRigid = this.MakeColliderPrefab();
		this.DestroyRepresentations(ref this.generatedHitBox, "::HITBOX_OUTPUT::");
		this.generatedHitBox = this.MakeHitBoxPrefab();
		if (this.generatedHitBox && this.generatedRigid)
		{
			AuthorShared.AttributeKeyValueList attributeKeyValueList = AuthorHull.GenKVL(this.generatedHitBox, this.generatedRigid);
			attributeKeyValueList.Run(this.generatedHitBox);
			attributeKeyValueList.Run(this.generatedRigid);
			List<KeyValuePair<MethodInfo, MonoBehaviour>> keyValuePairs = this.CaptureFinalizeMethods(this.generatedHitBox, "OnAuthoredAsHitBoxPrefab");
			List<KeyValuePair<MethodInfo, MonoBehaviour>> keyValuePairs1 = this.CaptureFinalizeMethods(this.generatedRigid, "OnAuthoredAsRagdollPrefab");
			object[] objArray = new object[] { this.generatedRigid };
			foreach (KeyValuePair<MethodInfo, MonoBehaviour> keyValuePair in keyValuePairs)
			{
				if (!keyValuePair.Value)
				{
					continue;
				}
				try
				{
					keyValuePair.Key.Invoke(keyValuePair.Value, objArray);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, keyValuePair.Value);
				}
			}
			object[] objArray1 = new object[] { this.generatedHitBox };
			foreach (KeyValuePair<MethodInfo, MonoBehaviour> keyValuePair1 in keyValuePairs1)
			{
				if (!keyValuePair1.Value)
				{
					continue;
				}
				try
				{
					keyValuePair1.Key.Invoke(keyValuePair1.Value, objArray1);
				}
				catch (Exception exception1)
				{
					UnityEngine.Debug.LogException(exception1, keyValuePair1.Value);
				}
			}
		}
		AuthorShared.SetDirty(this.generatedRigid);
		AuthorShared.SetDirty(this.generatedHitBox);
	}

	private static AuthorShared.AttributeKeyValueList GenKVL(GameObject hitBox, GameObject rigid)
	{
		return new AuthorShared.AttributeKeyValueList(new object[] { AuthTarg.HitBox, hitBox, AuthTarg.Ragdoll, rigid });
	}

	[DebuggerHidden]
	private static IEnumerable<Collider> GetCollidersOnRigidbody(Rigidbody rb)
	{
		AuthorHull.<GetCollidersOnRigidbody>c__Iterator6 variable = null;
		return variable;
	}

	private Transform GetHitColliderParent(GameObject root)
	{
		SkinnedMeshRenderer skinnedMeshRenderer;
		Transform rootBone = AuthorShared.GetRootBone(root, out skinnedMeshRenderer);
		return (!skinnedMeshRenderer || !skinnedMeshRenderer.transform.parent ? rootBone : skinnedMeshRenderer.transform.parent);
	}

	private GameObject InstantiatePrefabWithRemovedBones(GameObject prefab)
	{
		GameObject gameObject = AuthorShared.InstantiatePrefab(prefab);
		if (this.modelPrefabInstance)
		{
			if (this.removeThese != null)
			{
				for (int i = 0; i < (int)this.removeThese.Length; i++)
				{
					if (this.removeThese[i])
					{
						Transform transforms = gameObject.transform.FindChild(AuthorShared.CalculatePath(this.removeThese[i], this.modelPrefabInstance.transform));
						if (transforms)
						{
							UnityEngine.Object.DestroyImmediate(transforms);
						}
					}
				}
			}
			if (!this.allowBonesOutsideOfModelPrefab && prefab != this.modelPrefab)
			{
				Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(true);
				for (int j = 0; j < (int)componentsInChildren.Length; j++)
				{
					Transform transforms1 = componentsInChildren[j];
					if (transforms1)
					{
						string str = AuthorShared.CalculatePath(transforms1, gameObject.transform);
						if (!string.IsNullOrEmpty(str))
						{
							if (!this.modelPrefabInstance.transform.Find(str))
							{
								UnityEngine.Debug.LogWarning(string.Concat("Deleted bone because it was not in the model prefab instance:", str), gameObject);
								UnityEngine.Object.DestroyImmediate(transforms1.gameObject);
							}
						}
					}
				}
			}
		}
		return gameObject;
	}

	protected override void LoadSettings(JSONStream stream)
	{
		stream.ReadSkip();
	}

	private GameObject MakeColliderPrefab()
	{
		int? nullable;
		List<Collider> colliders;
		Rigidbody rigidbody;
		Transform transforms;
		GameObject gameObject = this.InstantiatePrefabWithRemovedBones(this.modelPrefab);
		if (this.removeThese != null)
		{
			GameObject gameObject1 = gameObject;
			gameObject1.name = string.Concat(gameObject1.name, "::RAGDOLL_OUTPUT::");
		}
		Animation[] componentsInChildren = gameObject.GetComponentsInChildren<Animation>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			Animation animations = componentsInChildren[i];
			if (animations)
			{
				UnityEngine.Object.DestroyImmediate(animations, true);
			}
		}
		if (this.useMeshesFromHitBoxOnRagdoll && this.modelPrefabForHitBox && this.modelPrefabForHitBox != this.modelPrefab)
		{
			Renderer[] rendererArray = gameObject.GetComponentsInChildren<Renderer>();
			for (int j = 0; j < (int)rendererArray.Length; j++)
			{
				Renderer component = rendererArray[j];
				if (component)
				{
					if (component is MeshRenderer)
					{
						MeshFilter meshFilter = component.GetComponent<MeshFilter>();
						string str = AuthorShared.CalculatePath(component.transform, gameObject.transform);
						meshFilter.sharedMesh = this.modelPrefabForHitBox.transform.FindChild(str).GetComponent<MeshFilter>().sharedMesh;
						AuthorShared.SetDirty(meshFilter);
					}
					else if (component is SkinnedMeshRenderer)
					{
						((SkinnedMeshRenderer)component).sharedMesh = this.modelPrefabForHitBox.transform.FindChild(AuthorShared.CalculatePath(component.transform, gameObject.transform)).GetComponent<SkinnedMeshRenderer>().sharedMesh;
						AuthorShared.SetDirty(component);
					}
				}
			}
		}
		this.ApplyMaterials(gameObject);
		if (!string.IsNullOrEmpty(this.defaultBodyPartLayer))
		{
			nullable = new int?(LayerMask.NameToLayer(this.defaultBodyPartLayer));
		}
		else
		{
			nullable = null;
		}
		IEnumerator<AuthorPeice> enumerator = base.EnumeratePeices().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				AuthorPeice current = enumerator.Current;
				if (!current || !(current is AuthorChHit))
				{
					continue;
				}
				((AuthorChHit)current).CreateColliderOn(gameObject.transform, this.modelPrefabInstance.transform, true, nullable);
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		Transform rootBone = AuthorShared.GetRootBone(gameObject);
		Dictionary<Rigidbody, List<Collider>> rigidbodies = new Dictionary<Rigidbody, List<Collider>>();
		Collider[] colliderArray = rootBone.GetComponentsInChildren<Collider>();
		for (int k = 0; k < (int)colliderArray.Length; k++)
		{
			Collider collider = colliderArray[k];
			Rigidbody rigidbody1 = collider.attachedRigidbody;
			if (rigidbody1)
			{
				if (!rigidbodies.TryGetValue(rigidbody1, out colliders))
				{
					colliders = new List<Collider>();
					rigidbodies[rigidbody1] = colliders;
				}
				colliders.Add(collider);
			}
		}
		HashSet<KeyValuePair<Collider, Collider>> keyValuePairs = new HashSet<KeyValuePair<Collider, Collider>>();
		foreach (KeyValuePair<Rigidbody, List<Collider>> keyValuePair in rigidbodies)
		{
			Transform key = keyValuePair.Key.transform;
			Transform transforms1 = key.parent;
			int num = 0;
			while (true)
			{
				int num1 = num;
				num = num1 + 1;
				if (num1 >= this.ignoreCollisionUpSteps || !transforms1 || !transforms1.IsChildOf(rootBone))
				{
					break;
				}
				do
				{
					rigidbody = transforms1.rigidbody;
					if (rigidbody)
					{
						break;
					}
					transforms = transforms1.parent;
					transforms1 = transforms;
				}
				while (transforms && transforms1.IsChildOf(rootBone));
				if (rigidbody)
				{
					foreach (Collider value in keyValuePair.Value)
					{
						foreach (Collider item in rigidbodies[rigidbody])
						{
							keyValuePairs.Add(AuthorHull.MakeKV(value, item));
						}
					}
				}
			}
			if (this.ignoreCollisionDownSteps <= 0)
			{
				continue;
			}
			foreach (Transform transforms2 in key.ListDecendantsByDepth())
			{
				if (transforms2.rigidbody)
				{
					transforms1 = transforms2.parent;
					num = 0;
					while (transforms1 != key)
					{
						if (transforms1.rigidbody)
						{
							int num2 = num + 1;
							num = num2;
							if (num2 > this.ignoreCollisionDownSteps)
							{
								break;
							}
						}
						transforms1 = transforms1.parent;
					}
					if (num >= this.ignoreCollisionDownSteps)
					{
						continue;
					}
					foreach (Collider value1 in keyValuePair.Value)
					{
						foreach (Collider item1 in rigidbodies[transforms2.rigidbody])
						{
							keyValuePairs.Add(AuthorHull.MakeKV(value1, item1));
						}
					}
				}
			}
		}
		int count = keyValuePairs.Count;
		if (count > 0)
		{
			Collider[] key1 = new Collider[count];
			Collider[] colliderArray1 = new Collider[count];
			int num3 = 0;
			foreach (KeyValuePair<Collider, Collider> keyValuePair1 in keyValuePairs)
			{
				key1[num3] = keyValuePair1.Key;
				colliderArray1[num3] = keyValuePair1.Value;
				num3++;
			}
			IgnoreColliders ignoreCollider = gameObject.AddComponent<IgnoreColliders>();
			ignoreCollider.a = key1;
			ignoreCollider.b = colliderArray1;
		}
		this.CreateEyes(gameObject);
		if (this.ragdollPrototype)
		{
			this.ApplyPrototype(gameObject, this.ragdollPrototype);
		}
		this.ApplyRig(gameObject);
		return gameObject;
	}

	private GameObject MakeHitBoxPrefab()
	{
		SkinnedMeshRenderer skinnedMeshRenderer;
		int? nullable;
		IDRemoteBodyPart dRemoteBodyPart;
		GameObject gameObject;
		try
		{
			GameObject gameObject1 = this.InstantiatePrefabWithRemovedBones((!this.modelPrefabForHitBox ? this.modelPrefab : this.modelPrefabForHitBox));
			GameObject gameObject2 = gameObject1;
			gameObject2.name = string.Concat(gameObject2.name, "::HITBOX_OUTPUT::");
			this.ApplyMaterials(gameObject1);
			AuthorShared.GetRootBone(gameObject1, out skinnedMeshRenderer);
			GameObject gameObject3 = new GameObject("HB Hit", new Type[] { typeof(CapsuleCollider), typeof(Rigidbody) })
			{
				layer = LayerMask.NameToLayer(this.hitBoxLayerName)
			};
			GameObject layer = gameObject3;
			layer.transform.parent = (!skinnedMeshRenderer.transform.parent ? gameObject1.transform : skinnedMeshRenderer.transform.parent);
			CapsuleCollider capsuleCollider = layer.collider as CapsuleCollider;
			capsuleCollider.center = this.hitCapsuleCenter;
			capsuleCollider.height = this.hitCapsuleHeight;
			capsuleCollider.radius = this.hitCapsuleRadius;
			capsuleCollider.direction = this.hitCapsuleDirection;
			capsuleCollider.isTrigger = false;
			capsuleCollider.rigidbody.isKinematic = true;
			layer.layer = LayerMask.NameToLayer("Hitbox");
			HitBoxSystem hitBoxSystem = this.CreateHitBoxSystem(layer);
			HitBoxSystem hitBoxSystem1 = hitBoxSystem;
			this.creatingSystem = hitBoxSystem;
			HitBoxSystem dRemoteBodyPartCollection = hitBoxSystem1;
			if (dRemoteBodyPartCollection.bodyParts == null)
			{
				dRemoteBodyPartCollection.bodyParts = new IDRemoteBodyPartCollection();
			}
			List<HitShape> hitShapes = new List<HitShape>();
			if (!string.IsNullOrEmpty(this.defaultBodyPartLayer))
			{
				nullable = new int?(LayerMask.NameToLayer(this.defaultBodyPartLayer));
			}
			else
			{
				nullable = null;
			}
			IEnumerator<AuthorPeice> enumerator = base.EnumeratePeices().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AuthorPeice current = enumerator.Current;
					if (!current || !(current is AuthorChHit))
					{
						continue;
					}
					((AuthorChHit)current).CreateHitBoxOn(hitShapes, gameObject1.transform, this.modelPrefabInstance.transform, nullable);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			int num = 0;
			int count = hitShapes.Count;
			while (num < count)
			{
				if (hitShapes[num] == null)
				{
					int num1 = num;
					num = num1 - 1;
					hitShapes.RemoveAt(num1);
					count--;
				}
				num++;
			}
			hitShapes.Sort(HitShape.prioritySorter);
			dRemoteBodyPartCollection.shapes = hitShapes.ToArray();
			HitBox[] componentsInChildren = gameObject1.GetComponentsInChildren<HitBox>();
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				HitBox hitBox = componentsInChildren[i];
				try
				{
					bool flag = dRemoteBodyPartCollection.bodyParts.TryGetValue(hitBox.bodyPart, out dRemoteBodyPart);
					dRemoteBodyPartCollection.bodyParts[hitBox.bodyPart] = hitBox;
					Collider[] components = hitBox.GetComponents<Collider>();
					for (int j = 0; j < (int)components.Length; j++)
					{
						UnityEngine.Object.DestroyImmediate(components[j]);
					}
					if (flag)
					{
						UnityEngine.Debug.LogWarning(string.Concat(new object[] { "Overwrite ", hitBox.bodyPart, ". Was ", dRemoteBodyPart, ", now ", hitBox }), hitBox);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					UnityEngine.Debug.LogError(string.Format("{0}:{2}:{1}", hitBox, exception, hitBox.bodyPart));
					throw;
				}
			}
			AuthorShared.SetDirty(dRemoteBodyPartCollection);
			this.CreateEyes(gameObject1);
			IDMain dMain = null;
			dMain = this.ApplyPrototype(gameObject1, this.prototype);
			this.ApplyRig(gameObject1);
			AuthorShared.SetDirty(gameObject1);
			gameObject = gameObject1;
		}
		finally
		{
			this.creatingSystem = null;
		}
		return gameObject;
	}

	private static KeyValuePair<Collider, Collider> MakeKV(Collider a, Collider b)
	{
		if (string.Compare(a.name, b.name) < 0)
		{
			return new KeyValuePair<Collider, Collider>(b, a);
		}
		return new KeyValuePair<Collider, Collider>(a, b);
	}

	private void OnDrawGizmosSelected()
	{
		if (this.modelPrefabInstance)
		{
			Gizmos.matrix = this.modelPrefabInstance.transform.localToWorldMatrix;
			Transform hitColliderParent = this.GetHitColliderParent(this.modelPrefabInstance);
			if (hitColliderParent)
			{
				Gizmos.matrix = hitColliderParent.localToWorldMatrix;
				Gizmos2.DrawWireCapsule(this.hitCapsuleCenter, this.hitCapsuleRadius, this.hitCapsuleHeight, this.hitCapsuleDirection);
			}
		}
	}

	protected override bool OnGUICreationSettings()
	{
		Transform transforms;
		bool? nullable;
		bool flag = base.OnGUICreationSettings();
		bool flag1 = this.modelPrefab;
		GameObject gameObject = (GameObject)AuthorShared.ObjectField("Model Prefab", this.modelPrefab, typeof(GameObject), true, new GUILayoutOption[0]);
		if (gameObject != this.modelPrefab)
		{
			if (gameObject)
			{
				gameObject = (AuthorShared.GetObjectKind(gameObject) == AuthorShared.ObjectKind.Model ? AuthorShared.FindPrefabRoot(gameObject) : this.modelPrefab);
			}
			else
			{
				gameObject = this.modelPrefab;
			}
		}
		if (gameObject != this.modelPrefab)
		{
			this.modelPrefab = gameObject;
			this.ChangedModelPrefab();
			this.ChangedEditingOptions();
			flag = flag | 1;
		}
		bool flag2 = GUI.enabled;
		if (!flag1)
		{
			GUI.enabled = false;
		}
		bool flag3 = this.modelPrefabForHitBox;
		GameObject gameObject1 = (GameObject)AuthorShared.ObjectField("Override Model Prefab [HitBox]", (!flag3 ? this.modelPrefab : this.modelPrefabForHitBox), typeof(GameObject), true, new GUILayoutOption[0]);
		GUI.enabled = flag2;
		if (!gameObject1 || gameObject1 == this.modelPrefab)
		{
			if (flag1)
			{
				GUILayout.Label(AuthorHull.guis.notOverridingContent, AuthorShared.Styles.miniLabel, new GUILayoutOption[0]);
			}
			gameObject1 = null;
		}
		else
		{
			GUILayout.Label(AuthorHull.guis.overridingContent, AuthorShared.Styles.miniLabel, new GUILayoutOption[0]);
			bool flag4 = AuthorShared.Toggle("Use Meshes from Override in Ragdoll output", this.useMeshesFromHitBoxOnRagdoll, new GUILayoutOption[0]);
			if (flag4 != this.useMeshesFromHitBoxOnRagdoll)
			{
				this.useMeshesFromHitBoxOnRagdoll = flag4;
				flag = true;
			}
		}
		if (gameObject1 != this.modelPrefabForHitBox)
		{
			if (gameObject1)
			{
				gameObject1 = (AuthorShared.GetObjectKind(gameObject1) == AuthorShared.ObjectKind.Model ? AuthorShared.FindPrefabRoot(gameObject1) : this.modelPrefabForHitBox);
			}
			else
			{
				gameObject1 = this.modelPrefabForHitBox;
			}
		}
		if (gameObject1 != this.modelPrefabForHitBox)
		{
			this.modelPrefabForHitBox = gameObject1;
			flag = flag | 1;
		}
		ActorRig actorRig = (ActorRig)AuthorShared.ObjectField("Actor Rig", this.actorRig, typeof(ActorRig), AuthorShared.ObjectFieldFlags.Asset, new GUILayoutOption[0]);
		if (actorRig != this.actorRig && !actorRig)
		{
			actorRig = this.actorRig;
		}
		if (actorRig != this.actorRig)
		{
			this.actorRig = actorRig;
			flag = flag | 1;
		}
		Character character = (Character)AuthorShared.ObjectField("Prototype Prefab", this.prototype, typeof(IDMain), AuthorShared.ObjectFieldFlags.Prefab, new GUILayoutOption[0]);
		if (character != this.prototype && character && AuthorShared.GetObjectKind(character.gameObject) != AuthorShared.ObjectKind.Prefab)
		{
			character = this.prototype;
		}
		if (character != this.prototype)
		{
			this.prototype = character;
			flag = flag | 1;
		}
		Ragdoll ragdoll = (Ragdoll)AuthorShared.ObjectField("Prototype Ragdoll", this.ragdollPrototype, typeof(IDMain), AuthorShared.ObjectFieldFlags.Prefab, new GUILayoutOption[0]);
		if (ragdoll != this.ragdollPrototype && ragdoll && AuthorShared.GetObjectKind(ragdoll.gameObject) != AuthorShared.ObjectKind.Prefab)
		{
			ragdoll = this.ragdollPrototype;
		}
		if (ragdoll != this.ragdollPrototype)
		{
			this.ragdollPrototype = ragdoll;
			flag = flag | 1;
		}
		if (this.modelPrefabInstance)
		{
			bool flag5 = this.modelPrefabInstance.activeSelf;
			AuthorShared.BeginHorizontal(new GUILayoutOption[0]);
			if (AuthorShared.Toggle("Show Model Prefab", ref flag5, AuthorShared.Styles.miniButton, new GUILayoutOption[0]))
			{
				this.modelPrefabInstance.SetActive(flag5);
			}
			flag = flag | AuthorShared.Toggle("Render Bones", ref this.drawBones, AuthorShared.Styles.miniButton, new GUILayoutOption[0]);
			AuthorShared.EndHorizontal();
		}
		AuthorShared.BeginSubSection("Rendering", new GUILayoutOption[0]);
		if (AuthorShared.ArrayField<Material>("Materials", ref this.materials, (Material& material) => AuthorShared.ObjectField<Material>(new AuthorShared.Content(), ref material, typeof(Material), 0, new GUILayoutOption[0])))
		{
			flag = true;
			this.ApplyMaterials(this.modelPrefabInstance);
		}
		AuthorShared.EndSubSection();
		AuthorShared.BeginSubSection("Types", "AddComponent strings", new GUILayoutOption[0]);
		string str = AuthorShared.StringField("HitBox Type", this.hitBoxType, new GUILayoutOption[0]);
		string str1 = AuthorShared.StringField("HitBoxSystem Type", this.hitBoxSystemType, new GUILayoutOption[0]);
		AuthorShared.EndSubSection();
		AuthorShared.BeginSubSection("Hit Capsule", "Should be large enough to fit all boxes at any time", new GUILayoutOption[0]);
		Vector3 vector3 = AuthorShared.Vector3Field("Center", this.hitCapsuleCenter, new GUILayoutOption[0]);
		float single = AuthorShared.FloatField("Radius", this.hitCapsuleRadius, new GUILayoutOption[0]);
		float single1 = AuthorShared.FloatField("Height", this.hitCapsuleHeight, new GUILayoutOption[0]);
		int num = AuthorShared.IntField("Axis", this.hitCapsuleDirection, new GUILayoutOption[0]);
		float single2 = AuthorShared.FloatField("Eye Height", this.eyeHeight, new GUILayoutOption[0]);
		AuthorShared.EndSubSection();
		AuthorShared.BeginSubSection("Rigidbody", new GUILayoutOption[0]);
		flag = flag | AuthorShared.IntField("Ignore n. parent col.", ref this.ignoreCollisionUpSteps, new GUILayoutOption[0]);
		flag = flag | AuthorShared.IntField("Ignore n. child col.", ref this.ignoreCollisionDownSteps, new GUILayoutOption[0]);
		AuthorShared.EndSubSection();
		AuthorShared.BeginSubSection("Body Parts", new GUILayoutOption[0]);
		string str2 = AuthorShared.StringField("Default Hit Box Layer", this.defaultBodyPartLayer ?? string.Empty, new GUILayoutOption[0]);
		if (string.IsNullOrEmpty(this.defaultBodyPartLayer))
		{
			AuthorShared.Label("[the layer in the models will be used]", new GUILayoutOption[0]);
		}
		if (str2 != (this.defaultBodyPartLayer ?? string.Empty))
		{
			this.defaultBodyPartLayer = str2;
			flag = true;
		}
		bool flag6 = (this.bodyParts.Count == 0 ? true : AuthorShared.Toggle("Show Unassigned Parts", this.showAllBones, new GUILayoutOption[0]));
		for (BodyPart i = BodyPart.Undefined; i < (BodyPart.Neck | BodyPart.Brain | BodyPart.L_UpperArm0 | BodyPart.L_Hand | BodyPart.L_Finger_Index3 | BodyPart.L_Finger_Middle2 | BodyPart.L_Finger_Ring1 | BodyPart.L_Finger_Pinky0 | BodyPart.L_Finger_Pinky4 | BodyPart.L_Finger_Thumb3 | BodyPart.L_Thigh1 | BodyPart.L_Heel0 | BodyPart.L_EyeLidLower | BodyPart.L_Cheek); i = (BodyPart)((int)i + (int)BodyPart.Hip))
		{
			if ((this.bodyParts.TryGetValue(i, out transforms) || this.showAllBones) && AuthorShared.ObjectField<Transform>(i.ToString(), ref transforms, AuthorShared.ObjectFieldFlags.AllowScene | AuthorShared.ObjectFieldFlags.Instance, new GUILayoutOption[0]))
			{
				if (!transforms)
				{
					this.bodyParts.Remove(i);
				}
				else
				{
					BodyPart? nullable1 = this.bodyParts.BodyPartOf(transforms);
					if (nullable1.HasValue)
					{
						bool? nullable2 = AuthorShared.Ask(string.Concat(new object[] { "That transform was assigned do something else.\r\nChange it from ", nullable1.Value, " to ", i, "?" }));
						if (!nullable2.HasValue)
						{
							nullable = null;
						}
						else
						{
							nullable = new bool?(!nullable2.Value);
						}
						bool? nullable3 = nullable;
						if ((!nullable3.HasValue ? false : nullable3.Value))
						{
							goto Label0;
						}
						this.bodyParts.Remove(nullable1.Value);
					}
					this.bodyParts[i] = transforms;
				}
				flag = true;
			}
		Label0:
		}
		this.showAllBones = flag6;
		AuthorShared.BeginSubSection("Destroy Children", new GUILayoutOption[0]);
		AuthorShared.BeginSubSection(AuthorHull.guis.destroyDrop, "Remove these from generation", AuthorShared.Styles.miniLabel, new GUILayoutOption[0]);
		Transform transforms1 = (Transform)AuthorShared.ObjectField(null, typeof(Transform), AuthorShared.ObjectFieldFlags.AllowScene | AuthorShared.ObjectFieldFlags.Model | AuthorShared.ObjectFieldFlags.Instance, new GUILayoutOption[0]);
		AuthorShared.EndSubSection();
		if (transforms1 && (!this.modelPrefabInstance || !transforms1.IsChildOf(this.modelPrefabInstance.transform)))
		{
			UnityEngine.Debug.Log("Thats not a valid selection", transforms1);
			transforms1 = null;
		}
		bool flag7 = false;
		if (this.removeThese != null && (int)this.removeThese.Length > 0)
		{
			AuthorShared.BeginSubSection("These will be removed with generation", new GUILayoutOption[0]);
			for (int j = 0; j < (int)this.removeThese.Length; j++)
			{
				AuthorShared.BeginHorizontal(AuthorShared.Styles.gradientOutline, new GUILayoutOption[0]);
				if (AuthorShared.Button(AuthorShared.ObjectContent<Transform>(this.removeThese[j], typeof(Transform)), AuthorShared.Styles.peiceButtonLeft, new GUILayoutOption[0]) && this.removeThese[j])
				{
					AuthorShared.PingObject(this.removeThese[j]);
				}
				if (AuthorShared.Button(AuthorShared.Icon.delete, AuthorShared.Styles.peiceButtonRight, new GUILayoutOption[0]))
				{
					this.removeThese[j] = null;
					flag7 = true;
				}
				AuthorShared.EndHorizontal();
			}
			AuthorShared.EndSubSection();
		}
		AuthorShared.EndSubSection();
		AuthorShared.EndSubSection();
		AuthorShared.BeginSubSection("Output", "this is where stuff will be saved", new GUILayoutOption[0]);
		UnityEngine.Object obj = AuthorShared.ObjectField("OUTPUT HITBOX", this.hitBoxOutputPrefab, typeof(GameObject), AuthorShared.ObjectFieldFlags.Prefab | AuthorShared.ObjectFieldFlags.NotModel | AuthorShared.ObjectFieldFlags.NotInstance, new GUILayoutOption[0]);
		UnityEngine.Object obj1 = AuthorShared.ObjectField("OUTPUT RAGDOLL", this.ragdollOutputPrefab, typeof(GameObject), AuthorShared.ObjectFieldFlags.Prefab | AuthorShared.ObjectFieldFlags.NotModel | AuthorShared.ObjectFieldFlags.NotInstance, new GUILayoutOption[0]);
		AuthorShared.EndSubSection();
		AuthorShared.BeginSubSection("Authoring Helpers", "These do not output to the mesh, just are here to help you author", new GUILayoutOption[0]);
		Vector3 vector31 = AuthorShared.Vector3Field("Angles Offset", this.editingAngles, new GUILayoutOption[0]);
		bool flag8 = AuthorShared.Toggle("Origin To Root", this.editingCenterToRoot, new GUILayoutOption[0]);
		AuthorShared.EndSubSection();
		AuthorShared.BeginHorizontal(AuthorShared.Styles.box, new GUILayoutOption[0]);
		bool flag9 = GUI.enabled;
		if (!gameObject)
		{
			GUI.enabled = false;
		}
		if (AuthorShared.Button("Generate", AuthorShared.Styles.miniButtonLeft, new GUILayoutOption[0]))
		{
			this.GeneratePrefabInstances();
			this.savedGenerated = false;
			AuthorShared.SetDirty(this);
			flag = true;
		}
		GUI.enabled = (this.savedGenerated || !this.generatedRigid || !this.generatedHitBox || !this.hitBoxOutputPrefab || !this.ragdollOutputPrefab ? false : this.ragdollOutputPrefab != this.hitBoxOutputPrefab);
		if (AuthorShared.Button("Update Prefabs", AuthorShared.Styles.miniButtonRight, new GUILayoutOption[0]))
		{
			bool? nullable4 = AuthorShared.Ask("This will overwrite any changes made to the output prefab that may have been done externally\r\nStill go ahead?");
			if ((!nullable4.GetValueOrDefault() ? false : nullable4.HasValue))
			{
				this.UpdatePrefabs();
				this.savedGenerated = true;
				flag = true;
			}
		}
		GUI.enabled = flag9;
		AuthorShared.EndHorizontal();
		if (AuthorShared.Button("Save To JSON", new GUILayoutOption[0]))
		{
			base.SaveSettings();
		}
		if (this.prototype && AuthorShared.Button("Write JSON Serialized Properties from Prototype", new GUILayoutOption[0]))
		{
			this.PreviewPrototype();
		}
		if (str != this.hitBoxType || str1 != this.hitBoxSystemType)
		{
			this.hitBoxType = str;
			this.hitBoxSystemType = str1;
			flag = true;
		}
		else if (vector3 != this.hitCapsuleCenter || single != this.hitCapsuleRadius || single1 != this.hitCapsuleHeight || num != this.hitCapsuleDirection || single2 != this.eyeHeight)
		{
			this.hitCapsuleCenter = vector3;
			this.hitCapsuleRadius = single;
			this.hitCapsuleHeight = single1;
			this.hitCapsuleDirection = num;
			this.eyeHeight = single2;
			flag = true;
		}
		else if (vector31 != this.editingAngles || this.editingCenterToRoot != flag8)
		{
			this.editingAngles = vector31;
			this.editingCenterToRoot = flag8;
			flag = true;
			this.ChangedEditingOptions();
		}
		else if (obj != this.hitBoxOutputPrefab)
		{
			if (this.EnsureItsAPrefab(ref obj) && obj != this.hitBoxOutputPrefab)
			{
				this.hitBoxOutputPrefab = (GameObject)obj;
				flag = true;
			}
		}
		else if (obj1 != this.ragdollOutputPrefab)
		{
			if (this.EnsureItsAPrefab(ref obj1) && obj1 != this.ragdollOutputPrefab)
			{
				this.ragdollOutputPrefab = (GameObject)obj1;
				flag = true;
			}
		}
		else if (transforms1)
		{
			Array.Resize<Transform>(ref this.removeThese, (this.removeThese != null ? (int)this.removeThese.Length + 1 : 1));
			this.removeThese[(int)this.removeThese.Length - 1] = transforms1;
			flag7 = true;
		}
		if (flag7)
		{
			int num1 = 0;
			for (int k = 0; k < (int)this.removeThese.Length; k++)
			{
				if (this.removeThese[k])
				{
					int num2 = num1;
					num1 = num2 + 1;
					this.removeThese[num2] = this.removeThese[k];
				}
			}
			Array.Resize<Transform>(ref this.removeThese, num1);
			flag = true;
		}
		return flag;
	}

	[Conditional("EXPECT_CRASH")]
	private static void PreCrashLog(string text)
	{
		UnityEngine.Debug.Log(text);
	}

	protected void PreviewPrototype()
	{
		AuthorCreationProject authorCreationProject;
		using (Stream stream = base.GetStream(true, "protoprev", out authorCreationProject))
		{
			if (stream != null)
			{
				using (JSONStream jSONStream = JSONStream.CreateWriter(stream))
				{
					if (jSONStream == null)
					{
						return;
					}
				}
			}
		}
	}

	public override string RootBonePath(AuthorPeice callingPeice, Transform bone)
	{
		return AuthorShared.CalculatePath(bone, this.modelPrefabInstance.transform);
	}

	protected override void SaveSettings(JSONStream stream)
	{
		stream.WriteObjectStart();
		stream.WriteObjectStart("types");
		stream.WriteText("hitboxsystem", this.hitBoxSystemType);
		stream.WriteText("hitbox", this.hitBoxType);
		stream.WriteObjectEnd();
		stream.WriteObjectStart("assets");
		AuthorHull.WriteJSONGUID(stream, "model", this.modelPrefabInstance);
		stream.WriteArrayStart("materials");
		if (this.materials != null)
		{
			for (int i = 0; i < (int)this.materials.Length; i++)
			{
				AuthorHull.WriteJSONGUID(stream, this.materials[i]);
			}
		}
		stream.WriteArrayEnd();
		stream.WriteObjectStart("bodyparts");
		foreach (BodyPartPair<Transform> bodyPart in this.bodyParts)
		{
			stream.WriteText(bodyPart.key.ToString(), AuthorShared.CalculatePath(bodyPart.@value, this.modelPrefabInstance.transform));
		}
		stream.WriteObjectEnd();
		stream.WriteArrayStart("peices");
		IEnumerator<AuthorPeice> enumerator = base.EnumeratePeices().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				AuthorPeice current = enumerator.Current;
				stream.WriteObjectStart();
				stream.WriteText("type", current.GetType().AssemblyQualifiedName);
				stream.WriteText("id", current.peiceID);
				stream.WriteObjectStart("instance");
				current.SaveJsonProperties(stream);
				stream.WriteObjectEnd();
				stream.WriteObjectEnd();
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		stream.WriteArrayEnd();
		stream.WriteObjectEnd();
	}

	private void TransferComponentSettings(GameObject srcGO, GameObject dstGO, Component[] srcComponents, Component[] dstComponents, Component src, Component dst)
	{
		if (src is MonoBehaviour || !(src is SkinnedMeshRenderer))
		{
			return;
		}
		UnityEngine.Debug.LogWarning("Cannot copy skinned mesh renderers");
	}

	private void TransferComponentSettings(NavMeshAgent src, NavMeshAgent dst)
	{
		dst.radius = src.radius;
		dst.speed = src.speed;
		dst.acceleration = src.acceleration;
		dst.angularSpeed = src.angularSpeed;
		dst.stoppingDistance = src.stoppingDistance;
		dst.autoTraverseOffMeshLink = src.autoTraverseOffMeshLink;
		dst.autoRepath = src.autoRepath;
		dst.height = src.height;
		dst.baseOffset = src.baseOffset;
		dst.obstacleAvoidanceType = src.obstacleAvoidanceType;
		dst.walkableMask = src.walkableMask;
		dst.enabled = src.enabled;
	}

	private void UpdatePrefabs()
	{
	}

	private static void WriteJSONGUID(JSONStream stream, UnityEngine.Object obj)
	{
		string assemblyQualifiedName;
		string assetPath = AuthorShared.GetAssetPath(obj);
		string gUID = null;
		if (assetPath != string.Empty)
		{
			gUID = AuthorShared.PathToGUID(assetPath);
			if (string.IsNullOrEmpty(gUID))
			{
				gUID = null;
			}
		}
		else
		{
			assetPath = null;
		}
		if (!obj)
		{
			assemblyQualifiedName = null;
		}
		else
		{
			assemblyQualifiedName = obj.GetType().AssemblyQualifiedName;
		}
		stream.WriteObjectStart();
		stream.WriteText("path", assetPath);
		stream.WriteText("guid", gUID);
		stream.WriteText("type", assemblyQualifiedName);
		stream.WriteObjectEnd();
	}

	private static void WriteJSONGUID(JSONStream stream, string property, UnityEngine.Object obj)
	{
		stream.WriteProperty(property);
		AuthorHull.WriteJSONGUID(stream, obj);
	}

	private static class guis
	{
		public readonly static GUIContent overridingContent;

		public readonly static GUIContent notOverridingContent;

		public readonly static GUIContent destroyDrop;

		static guis()
		{
			AuthorHull.guis.overridingContent = new GUIContent("[overriding the hitbox output prefab]", "The HitBox prefab output will use the overriding mesh prefab provided, You must make sure that the heirarchy matches between the two");
			AuthorHull.guis.notOverridingContent = new GUIContent("[both outputs will use the same base]", "The HitBox prefab output will use the same mesh prefab as the one for the rigidbody");
			AuthorHull.guis.destroyDrop = new GUIContent("Destroy bone", "Drag a transform off the model instance that contains no ::'s");
		}
	}

	private static class HitBoxItems
	{
		public readonly static AuthorPalletObject.Validator validateByID;

		public readonly static AuthorPalletObject.Creator createSocketByID;

		public readonly static AuthorPalletObject[] pallet;

		static HitBoxItems()
		{
			AuthorHull.HitBoxItems.validateByID = new AuthorPalletObject.Validator(AuthorHull.HitBoxItems.ValidateByID);
			AuthorHull.HitBoxItems.createSocketByID = new AuthorPalletObject.Creator(AuthorHull.HitBoxItems.CreateByID<AuthorChHit>);
			AuthorPalletObject[] authorPalletObjectArray = new AuthorPalletObject[3];
			AuthorPalletObject authorPalletObject = new AuthorPalletObject()
			{
				guiContent = new GUIContent("Box"),
				validator = AuthorHull.HitBoxItems.validateByID,
				creator = AuthorHull.HitBoxItems.createSocketByID
			};
			authorPalletObjectArray[0] = authorPalletObject;
			authorPalletObject = new AuthorPalletObject()
			{
				guiContent = new GUIContent("Sphere"),
				validator = AuthorHull.HitBoxItems.validateByID,
				creator = AuthorHull.HitBoxItems.createSocketByID
			};
			authorPalletObjectArray[1] = authorPalletObject;
			authorPalletObject = new AuthorPalletObject()
			{
				guiContent = new GUIContent("Capsule"),
				validator = AuthorHull.HitBoxItems.validateByID,
				creator = AuthorHull.HitBoxItems.createSocketByID
			};
			authorPalletObjectArray[2] = authorPalletObject;
			AuthorHull.HitBoxItems.pallet = authorPalletObjectArray;
		}

		private static AuthorPeice CreateByID<TPeice>(AuthorCreation creation, AuthorPalletObject palletObject)
		where TPeice : AuthorPeice
		{
			TPeice component = (new GameObject(palletObject.guiContent.text, new Type[] { typeof(TPeice) })).GetComponent<TPeice>();
			component.peiceID = palletObject.guiContent.text;
			return (object)component;
		}

		private static bool ValidateByID(AuthorCreation creation, AuthorPalletObject palletObject)
		{
			return true;
		}
	}
}