using Facepunch.Intersect;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AuthorChHit : AuthorPeice
{
	[SerializeField]
	private HitShapeKind kind;

	[SerializeField]
	private Transform bone;

	[SerializeField]
	private Vector3 center;

	[SerializeField]
	private float radius = 0.5f;

	[SerializeField]
	private float height = 2f;

	[SerializeField]
	private float damageMultiplier = 1f;

	[SerializeField]
	private int hitPriority = 128;

	[SerializeField]
	private BodyPart bodyPart;

	[SerializeField]
	private BodyPart mirroredBodyPart;

	[SerializeField]
	private Vector3 size = Vector3.one;

	[SerializeField]
	private int capsuleAxis = 1;

	[SerializeField]
	private float mass = 1f;

	[SerializeField]
	private float drag;

	[SerializeField]
	private float angularDrag = 0.05f;

	[SerializeField]
	private bool isMirror;

	[SerializeField]
	private Transform mirrored;

	[SerializeField]
	private bool mirrorX;

	[SerializeField]
	private bool mirrorY;

	[SerializeField]
	private bool mirrorZ;

	[SerializeField]
	private AuthorChJoint[] myJoints;

	private Rect lastPopupRect;

	protected readonly static Color boneGizmoColor;

	protected readonly static Color mirroredGizmoColor;

	public AuthorChHit.Rep primary
	{
		get
		{
			AuthorChHit.Rep rep = new AuthorChHit.Rep();
			rep.hit = this;
			rep.bone = this.bone;
			rep.mirrored = false;
			int num = 0;
			bool flag = (bool)num;
			rep.flipZ = (bool)num;
			bool flag1 = flag;
			flag = flag1;
			rep.flipY = flag1;
			rep.flipX = flag;
			rep.center = this.center;
			rep.size = this.size;
			rep.radius = this.radius;
			rep.height = this.height;
			rep.capsuleAxis = this.capsuleAxis;
			rep.valid = rep.bone;
			return rep;
		}
		set
		{
			this.bone = value.bone;
			this.center = value.center;
			this.size = value.size;
			this.radius = value.radius;
			this.height = value.height;
			this.capsuleAxis = value.capsuleAxis;
		}
	}

	public AuthorChHit.Rep secondary
	{
		get
		{
			AuthorChHit.Rep center = new AuthorChHit.Rep();
			Transform transforms;
			center.hit = this;
			if (this.mirrored != this.bone)
			{
				transforms = this.mirrored;
			}
			else
			{
				transforms = null;
			}
			center.bone = transforms;
			center.mirrored = true;
			center.flipX = this.mirrorX;
			center.flipY = this.mirrorY;
			center.flipZ = this.mirrorZ;
			center.center = this.GetCenter(true);
			center.size = this.size;
			center.radius = this.radius;
			center.height = this.height;
			center.capsuleAxis = this.capsuleAxis;
			center.valid = center.bone;
			return center;
		}
		set
		{
			this.mirrored = value.bone;
			this.center = value.Flip(value.center);
			this.size = value.size;
			this.radius = value.radius;
			this.height = value.height;
			this.capsuleAxis = value.capsuleAxis;
		}
	}

	static AuthorChHit()
	{
		AuthorChHit.boneGizmoColor = new Color(1f, 1f, 1f, 0.3f);
		AuthorChHit.mirroredGizmoColor = new Color(0f, 0f, 0f, 0.3f);
	}

	public AuthorChHit()
	{
	}

	private void AddCharacterJoint()
	{
		this.AddJoint(AuthorChJoint.Kind.Character);
	}

	private void AddFixedJoint()
	{
		this.AddJoint(AuthorChJoint.Kind.Fixed);
	}

	private void AddHingeJoint()
	{
		this.AddJoint(AuthorChJoint.Kind.Hinge);
	}

	private AuthorChJoint AddJoint(AuthorChJoint.Kind kind)
	{
		AuthorChJoint authorChJoint = base.creation.CreatePeice<AuthorChJoint>(kind.ToString(), new Type[0]);
		if (authorChJoint)
		{
			authorChJoint.InitializeFromOwner(this, kind);
			Array.Resize<AuthorChJoint>(ref this.myJoints, (this.myJoints != null ? (int)this.myJoints.Length + 1 : 1));
			this.myJoints[(int)this.myJoints.Length - 1] = authorChJoint;
		}
		return authorChJoint;
	}

	private void AddSpringJoint()
	{
		this.AddJoint(AuthorChJoint.Kind.Spring);
	}

	private Collider CreateColliderOn(Transform instanceRoot, Transform root, Transform bone, bool mirrored)
	{
		if (!bone)
		{
			throw new ArgumentException("there was no bone");
		}
		string str = AuthorShared.CalculatePath(bone, root);
		Transform transforms = instanceRoot.FindChild(str);
		if (!transforms)
		{
			throw new MissingReferenceException(str);
		}
		switch (this.kind)
		{
			case HitShapeKind.Sphere:
			{
				SphereCollider center = transforms.gameObject.AddComponent<SphereCollider>();
				center.center = this.GetCenter(mirrored);
				center.radius = this.radius;
				break;
			}
			case HitShapeKind.Capsule:
			{
				CapsuleCollider capsuleCollider = transforms.gameObject.AddComponent<CapsuleCollider>();
				capsuleCollider.center = this.GetCenter(mirrored);
				capsuleCollider.radius = this.radius;
				capsuleCollider.height = this.height;
				capsuleCollider.direction = this.capsuleAxis;
				break;
			}
			case HitShapeKind.Line:
			{
				throw new NotSupportedException();
			}
			case HitShapeKind.Box:
			{
				BoxCollider boxCollider = transforms.gameObject.AddComponent<BoxCollider>();
				boxCollider.center = this.GetCenter(mirrored);
				boxCollider.size = this.size;
				break;
			}
			default:
			{
				throw new NotSupportedException();
			}
		}
		return transforms.collider;
	}

	public void CreateColliderOn(Transform instance, Transform root, bool addJoints)
	{
		this.CreateColliderOn(instance, root, addJoints, null);
	}

	public void CreateColliderOn(Transform instance, Transform root, bool addJoints, int? layerIndex)
	{
		if (this.bone)
		{
			this.CreatedCollider(this.CreateColliderOn(instance, root, this.bone, false), this.primary, addJoints, layerIndex);
		}
		if (this.mirrored && this.mirrored != this.bone)
		{
			this.CreatedCollider(this.CreateColliderOn(instance, root, this.mirrored, true), this.secondary, addJoints, layerIndex);
		}
	}

	private void CreatedCollider(Collider created, AuthorChHit.Rep repFormat, bool addJoints, int? layerIndex)
	{
		if (!created)
		{
			return;
		}
		repFormat.bone = created.transform;
		if (addJoints)
		{
			Rigidbody rigidbody = created.rigidbody;
			if (!rigidbody)
			{
				rigidbody = created.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.mass = this.mass;
			rigidbody.drag = this.drag;
			rigidbody.angularDrag = this.angularDrag;
			if (this.myJoints != null)
			{
				AuthorChJoint[] authorChJointArray = this.myJoints;
				for (int i = 0; i < (int)authorChJointArray.Length; i++)
				{
					AuthorChJoint authorChJoint = authorChJointArray[i];
					if (authorChJoint)
					{
						authorChJoint.AddJoint(repFormat.bone.root, ref repFormat);
					}
				}
			}
		}
		if (layerIndex.HasValue)
		{
			created.gameObject.layer = layerIndex.Value;
		}
	}

	public void CreateHitBoxOn(List<HitShape> list, Transform instance, Transform root)
	{
		this.CreateHitBoxOn(list, instance, root, null);
	}

	public void CreateHitBoxOn(List<HitShape> list, Transform instance, Transform root, int? layerIndex)
	{
		if (this.bone)
		{
			list.Add(this.CreateHitBoxOnDo(instance, root, this.bone, false, layerIndex));
		}
		if (this.mirrored && this.mirrored != this.bone)
		{
			list.Add(this.CreateHitBoxOnDo(instance, root, this.mirrored, true, layerIndex));
		}
	}

	private HitShape CreateHitBoxOnDo(Transform instanceRoot, Transform root, Transform bone, bool mirrored, int? layerIndex)
	{
		HitBox hitBox;
		Collider value = this.CreateColliderOn(instanceRoot, root, bone, mirrored);
		if (layerIndex.HasValue)
		{
			value.gameObject.layer = layerIndex.Value;
		}
		if (!(base.creation is AuthorHull))
		{
			hitBox = null;
		}
		else
		{
			hitBox = (base.creation as AuthorHull).CreateHitBox(value.gameObject);
		}
		hitBox.bodyPart = (!mirrored ? this.bodyPart : this.mirroredBodyPart);
		hitBox.priority = this.hitPriority;
		hitBox.damageFactor = this.damageMultiplier;
		return new HitShape(value);
	}

	private bool DoTransformHandles(Transform bone, ref Vector3 center, ref Vector3 size, ref float radius, ref float height, ref int capsuleAxis)
	{
		Matrix4x4 matrix4x4 = AuthorShared.Scene.matrix;
		if (bone)
		{
			AuthorShared.Scene.matrix = bone.transform.localToWorldMatrix;
		}
		bool flag = false;
		switch (this.kind)
		{
			case HitShapeKind.Sphere:
			{
				flag = flag | AuthorShared.Scene.SphereDrag(ref center, ref radius);
				AuthorShared.Scene.matrix = matrix4x4;
				return flag;
			}
			case HitShapeKind.Capsule:
			{
				flag = flag | AuthorShared.Scene.CapsuleDrag(ref center, ref radius, ref height, ref capsuleAxis);
				AuthorShared.Scene.matrix = matrix4x4;
				return flag;
			}
			case HitShapeKind.Line:
			{
				AuthorShared.Scene.matrix = matrix4x4;
				return flag;
			}
			case HitShapeKind.Box:
			{
				flag = flag | AuthorShared.Scene.BoxDrag(ref center, ref size);
				AuthorShared.Scene.matrix = matrix4x4;
				return flag;
			}
			default:
			{
				AuthorShared.Scene.matrix = matrix4x4;
				return flag;
			}
		}
	}

	private void DrawGiz(Transform bone, bool mirrored)
	{
		if (Event.current.shift && bone)
		{
			Gizmos.matrix = bone.localToWorldMatrix;
			switch (this.kind)
			{
				case HitShapeKind.Sphere:
				{
					Gizmos.DrawWireSphere(this.GetCenter(mirrored), this.radius);
					break;
				}
				case HitShapeKind.Capsule:
				{
					Gizmos2.DrawWireCapsule(this.GetCenter(mirrored), this.radius, this.height, this.capsuleAxis);
					break;
				}
				case HitShapeKind.Box:
				{
					Gizmos.DrawWireCube(this.GetCenter(mirrored), this.size);
					break;
				}
			}
		}
	}

	private void FigureOutDefaultBodyPart(Transform bone, ref BodyPart part)
	{
		if ((int)part == 0)
		{
			AuthorHull authorHull = base.creation as AuthorHull;
			if (authorHull)
			{
				authorHull.FigureOutDefaultBodyPart(ref bone, ref part, ref this.mirrored, ref this.mirroredBodyPart);
				Debug.Log(string.Format("[{0}:{1}][{2}:{3}]", new object[] { bone, (BodyPart)((int)part), this.mirrored, this.mirroredBodyPart }), this);
			}
		}
	}

	private Vector3 GetCenter(bool mirrored)
	{
		if (!mirrored)
		{
			return this.center;
		}
		return new Vector3((!this.mirrorX ? this.center.x : -this.center.x), (!this.mirrorY ? this.center.y : -this.center.y), (!this.mirrorZ ? this.center.z : -this.center.z));
	}

	private void OnDrawGizmos()
	{
		if (this.bone != this.mirrored)
		{
			Gizmos.color = AuthorChHit.mirroredGizmoColor;
			this.DrawGiz(this.mirrored, true);
		}
		Gizmos.color = AuthorChHit.boneGizmoColor;
		this.DrawGiz(this.bone, false);
	}

	internal void OnJointDestroy(AuthorChJoint joint)
	{
		if (this.myJoints == null)
		{
			return;
		}
		int num = Array.IndexOf<AuthorChJoint>(this.myJoints, joint);
		if (num != -1)
		{
			for (int i = num; i < (int)this.myJoints.Length - 1; i++)
			{
				this.myJoints[i] = this.myJoints[i + 1];
			}
			Array.Resize<AuthorChJoint>(ref this.myJoints, (int)this.myJoints.Length - 1);
		}
	}

	protected override void OnPeiceDestroy()
	{
		try
		{
			if (this.myJoints != null)
			{
				AuthorChJoint[] authorChJointArray = this.myJoints;
				this.myJoints = null;
				AuthorChJoint[] authorChJointArray1 = authorChJointArray;
				for (int i = 0; i < (int)authorChJointArray1.Length; i++)
				{
					AuthorChJoint authorChJoint = authorChJointArray1[i];
					if (authorChJoint)
					{
						UnityEngine.Object.Destroy(authorChJoint);
					}
				}
			}
		}
		finally
		{
			base.OnPeiceDestroy();
		}
	}

	protected override void OnRegistered()
	{
		int num;
		string str = base.peiceID;
		if (str != null)
		{
			if (AuthorChHit.<>f__switch$map1 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(3)
				{
					{ "Sphere", 0 },
					{ "Box", 1 },
					{ "Capsule", 2 }
				};
				AuthorChHit.<>f__switch$map1 = strs;
			}
			if (AuthorChHit.<>f__switch$map1.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						this.kind = HitShapeKind.Sphere;
						break;
					}
					case 1:
					{
						this.kind = HitShapeKind.Box;
						break;
					}
					case 2:
					{
						this.kind = HitShapeKind.Capsule;
						break;
					}
				}
			}
		}
		base.OnRegistered();
	}

	public override bool OnSceneView()
	{
		Vector3 vector3 = new Vector3();
		float single;
		float single1;
		float single2;
		bool flag = base.OnSceneView();
		flag = flag | this.DoTransformHandles(this.bone, ref this.center, ref this.size, ref this.radius, ref this.height, ref this.capsuleAxis);
		if (this.mirrored && this.mirrored != this.bone)
		{
			vector3.x = (!this.mirrorX ? this.center.x : -this.center.x);
			vector3.y = (!this.mirrorY ? this.center.y : -this.center.y);
			vector3.z = (!this.mirrorZ ? this.center.z : -this.center.z);
			if (this.DoTransformHandles(this.mirrored, ref vector3, ref this.size, ref this.radius, ref this.height, ref this.capsuleAxis))
			{
				single = (!this.mirrorX ? vector3.x : -vector3.x);
				this.center.x = single;
				single1 = (!this.mirrorY ? vector3.y : -vector3.y);
				this.center.y = single1;
				single2 = (!this.mirrorZ ? vector3.z : -vector3.z);
				this.center.z = single2;
				flag = true;
			}
		}
		return flag;
	}

	public override bool PeiceInspectorGUI()
	{
		bool flag = base.PeiceInspectorGUI();
		string str = base.peiceID;
		if (AuthorShared.StringField("Title", ref str, new GUILayoutOption[0]))
		{
			base.peiceID = str;
			flag = true;
		}
		bool flag1 = (!this.mirrored ? false : this.mirrored != this.bone);
		bool flag2 = this.bone;
		BodyPart bodyPart = this.bodyPart;
		if (AuthorShared.ObjectField<Transform>("Bone", ref this.bone, AuthorShared.ObjectFieldFlags.AllowScene | AuthorShared.ObjectFieldFlags.Model | AuthorShared.ObjectFieldFlags.Instance, new GUILayoutOption[0]))
		{
			if (!flag2)
			{
				this.FigureOutDefaultBodyPart(this.bone, ref bodyPart);
			}
			flag = true;
		}
		BodyPart bodyPart1 = this.mirroredBodyPart;
		if (flag2)
		{
			bodyPart = (BodyPart)AuthorShared.EnumField("Body Part", bodyPart, new GUILayoutOption[0]);
		}
		GUI.Box(AuthorShared.BeginVertical(new GUILayoutOption[0]), GUIContent.none);
		flag = flag | AuthorShared.ObjectField<Transform>("Mirrored Bone", ref this.mirrored, AuthorShared.ObjectFieldFlags.AllowScene | AuthorShared.ObjectFieldFlags.Model | AuthorShared.ObjectFieldFlags.Instance, new GUILayoutOption[0]);
		if (flag1)
		{
			bodyPart1 = (BodyPart)AuthorShared.EnumField("Body Part", bodyPart1, new GUILayoutOption[0]);
			AuthorShared.BeginHorizontal(new GUILayoutOption[0]);
			bool flag3 = GUILayout.Toggle(this.mirrorX, "Mirror X", new GUILayoutOption[0]);
			bool flag4 = GUILayout.Toggle(this.mirrorY, "Mirror Y", new GUILayoutOption[0]);
			bool flag5 = GUILayout.Toggle(this.mirrorZ, "Mirror Z", new GUILayoutOption[0]);
			AuthorShared.EndHorizontal();
			if (flag3 != this.mirrorX || flag4 != this.mirrorY || flag5 != this.mirrorZ)
			{
				this.mirrorX = flag3;
				this.mirrorY = flag4;
				this.mirrorZ = flag5;
				flag = true;
			}
		}
		AuthorShared.EndVertical();
		Vector3 vector3 = this.center;
		float single = this.radius;
		float single1 = this.height;
		Vector3 vector31 = this.size;
		int num = this.capsuleAxis;
		AuthorShared.BeginSubSection("Shape", new GUILayoutOption[0]);
		HitShapeKind hitShapeKind = (HitShapeKind)AuthorShared.EnumField("Kind", this.kind, new GUILayoutOption[0]);
		switch (this.kind)
		{
			case HitShapeKind.Sphere:
			{
				vector3 = AuthorShared.Vector3Field("Center", this.center, new GUILayoutOption[0]);
				single = Mathf.Max(AuthorShared.FloatField("Radius", this.radius, new GUILayoutOption[0]), 0.001f);
				goto case HitShapeKind.Line;
			}
			case HitShapeKind.Capsule:
			{
				vector3 = AuthorShared.Vector3Field("Center", this.center, new GUILayoutOption[0]);
				single = Mathf.Max(AuthorShared.FloatField("Radius", this.radius, new GUILayoutOption[0]), 0.001f);
				single1 = Mathf.Max(AuthorShared.FloatField("Height", this.height, new GUILayoutOption[0]), 0.001f);
				num = Mathf.Clamp(AuthorShared.IntField("Height Axis", this.capsuleAxis, new GUILayoutOption[0]), 0, 2);
				goto case HitShapeKind.Line;
			}
			case HitShapeKind.Line:
			{
				AuthorShared.EndSubSection();
				AuthorShared.BeginSubSection("Rigidbody", new GUILayoutOption[0]);
				float single2 = Mathf.Max(AuthorShared.FloatField("Mass", this.mass, new GUILayoutOption[0]), 0.001f);
				float single3 = Mathf.Max(AuthorShared.FloatField("Drag", this.drag, new GUILayoutOption[0]), 0f);
				float single4 = Mathf.Max(AuthorShared.FloatField("Angular Drag", this.angularDrag, new GUILayoutOption[0]), 0f);
				AuthorShared.EndSubSection();
				AuthorShared.BeginSubSection("Hit Box", new GUILayoutOption[0]);
				int num1 = this.hitPriority;
				float single5 = this.damageMultiplier;
				if (flag1 || flag2)
				{
					num1 = AuthorShared.IntField("Hit Priority", num1, new GUILayoutOption[0]);
					single5 = AuthorShared.FloatField("Damage Mult.", single5, new GUILayoutOption[0]);
				}
				AuthorShared.EndSubSection();
				bool flag6 = GUILayout.Button("Add Joint", new GUILayoutOption[0]);
				if (Event.current.type == EventType.Repaint)
				{
					this.lastPopupRect = GUILayoutUtility.GetLastRect();
				}
				if (flag6)
				{
					AuthorShared.CustomMenu(this.lastPopupRect, AuthorChHit.JointMenu.options, 0, new AuthorShared.CustomMenuProc(AuthorChHit.JointMenu.Callback), this);
				}
				if (hitShapeKind != this.kind || vector3 != this.center || vector31 != this.size || single != this.radius || single1 != this.height || num != this.capsuleAxis || single2 != this.mass || single3 != this.drag || single4 != this.angularDrag || bodyPart != this.bodyPart || bodyPart1 != this.mirroredBodyPart || this.hitPriority != num1 || single5 != this.damageMultiplier)
				{
					flag = true;
					this.kind = hitShapeKind;
					this.center = vector3;
					this.size = vector31;
					this.radius = single;
					this.height = single1;
					this.capsuleAxis = num;
					this.mass = single2;
					this.drag = single3;
					this.angularDrag = single4;
					this.bodyPart = bodyPart;
					this.mirroredBodyPart = bodyPart1;
					this.hitPriority = num1;
					this.damageMultiplier = single5;
				}
				return flag;
			}
			case HitShapeKind.Box:
			{
				vector3 = AuthorShared.Vector3Field("Center", this.center, new GUILayoutOption[0]);
				vector31 = AuthorShared.Vector3Field("Size", this.size, new GUILayoutOption[0]);
				goto case HitShapeKind.Line;
			}
			default:
			{
				goto case HitShapeKind.Line;
			}
		}
	}

	public override void SaveJsonProperties(JSONStream stream)
	{
		base.SaveJsonProperties(stream);
		stream.WriteText("bone", base.FromRootBonePath(this.bone));
		stream.WriteEnum("bonepart", this.bodyPart);
		stream.WriteBoolean("mirror", this.isMirror);
		stream.WriteText("mirrorbone", base.FromRootBonePath(this.mirrored));
		stream.WriteEnum("mirrorbonepart", this.mirroredBodyPart);
		stream.WriteArrayStart("mirrorboneflip");
		stream.WriteBoolean(this.mirrorX);
		stream.WriteBoolean(this.mirrorY);
		stream.WriteBoolean(this.mirrorZ);
		stream.WriteArrayEnd();
		stream.WriteEnum("kind", this.kind);
		stream.WriteVector3("center", this.center);
		stream.WriteVector3("size", this.size);
		stream.WriteNumber("radius", this.radius);
		stream.WriteNumber("height", this.height);
		stream.WriteInteger("capsuleaxis", this.capsuleAxis);
		stream.WriteNumber("damagemul", this.damageMultiplier);
		stream.WriteInteger("hitpriority", this.hitPriority);
		stream.WriteNumber("mass", this.mass);
		stream.WriteNumber("drag", this.drag);
		stream.WriteNumber("adrag", this.angularDrag);
	}

	private static class JointMenu
	{
		public readonly static GUIContent[] options;

		static JointMenu()
		{
			AuthorChHit.JointMenu.options = new GUIContent[] { new GUIContent("Nevermind"), new GUIContent("Add Hinge Joint"), new GUIContent("Add Character Joint"), new GUIContent("Add Fixed Joint"), new GUIContent("Add Spring Joint") };
		}

		public static void Callback(object userData, string[] options, int selected)
		{
			AuthorChHit authorChHit = userData as AuthorChHit;
			switch (selected)
			{
				case 1:
				{
					authorChHit.AddHingeJoint();
					break;
				}
				case 2:
				{
					authorChHit.AddCharacterJoint();
					break;
				}
				case 3:
				{
					authorChHit.AddFixedJoint();
					break;
				}
				case 4:
				{
					authorChHit.AddSpringJoint();
					break;
				}
			}
		}
	}

	public struct Rep
	{
		public AuthorChHit hit;

		public Transform bone;

		public bool mirrored;

		public bool flipX;

		public bool flipY;

		public bool flipZ;

		public Vector3 center;

		public Vector3 size;

		public float radius;

		public float height;

		public int capsuleAxis;

		public bool valid;

		public string path
		{
			get
			{
				if (!this.valid)
				{
					return null;
				}
				return AuthorShared.CalculatePath(this.bone, this.bone.root);
			}
		}

		public Vector3 AxisFlip(Vector3 v)
		{
			if (this.flipX == this.mirrored)
			{
				v.x = -v.x;
			}
			if (this.flipY == this.mirrored)
			{
				v.y = -v.y;
			}
			if (this.flipZ == this.mirrored)
			{
				v.z = -v.z;
			}
			return v;
		}

		public Vector3 Flip(Vector3 v)
		{
			if (this.flipX)
			{
				v.x = -v.x;
			}
			if (this.flipY)
			{
				v.y = -v.y;
			}
			if (this.flipZ)
			{
				v.z = -v.z;
			}
			return v;
		}
	}
}