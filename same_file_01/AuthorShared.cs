using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public abstract class AuthorShared : MonoBehaviour
{
	private readonly static GUIContent AuthorPeiceContent;

	private static Rect lastRect_popup;

	private readonly static AuthorShared.GenerateOptions authorPopupGenerate;

	static AuthorShared()
	{
		AuthorShared.AuthorPeiceContent = new GUIContent();
		AuthorShared.authorPopupGenerate = new AuthorShared.GenerateOptions(AuthorShared.AuthorPopupGenerate);
	}

	protected AuthorShared()
	{
	}

	public static TComponent AddComponent<TComponent>(GameObject target, string type)
	where TComponent : Component
	{
		Component component = target.AddComponent(type);
		if (!component)
		{
			UnityEngine.Debug.LogWarning(string.Concat("The string type \"", type, "\" evaluated to no component type. null returning"), target);
			return (TComponent)null;
		}
		if (component is TComponent)
		{
			return (TComponent)component;
		}
		UnityEngine.Debug.LogWarning(string.Concat(new string[] { "The string type \"", type, "\" is a component class but does not inherit \"", typeof(TComponent).AssemblyQualifiedName, "\"" }), target);
		UnityEngine.Object.DestroyImmediate(component);
		return (TComponent)null;
	}

	public static bool ArrayField<T>(AuthorShared.Content content, ref T[] array, AuthorShared.ArrayFieldFunctor<T> functor)
	{
		AuthorShared.BeginHorizontal(new GUILayoutOption[0]);
		int num = (array != null ? (int)array.Length : 0);
		AuthorShared.PrefixLabel(content);
		AuthorShared.BeginVertical(new GUILayoutOption[0]);
		AuthorShared.BeginHorizontal(new GUILayoutOption[0]);
		AuthorShared.Label("Size", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		AuthorShared.Content content1 = new AuthorShared.Content();
		int num1 = Mathf.Max(0, AuthorShared.IntField(content1, num, new GUILayoutOption[0]));
		AuthorShared.EndHorizontal();
		bool flag = num != num1;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				flag = flag | functor(ref array[i]);
			}
		}
		AuthorShared.EndVertical();
		AuthorShared.EndHorizontal();
		if (!flag)
		{
			return false;
		}
		Array.Resize<T>(ref array, num1);
		return true;
	}

	public static bool? Ask(string Question)
	{
		return null;
	}

	private static bool AuthorPopupGenerate(object arg, ref int selected, out GUIContent[] options, out Array array)
	{
		AuthorPeice authorPeice;
		options = null;
		array = null;
		AuthorShared.AuthorOptionGenerate authorOptionGenerate = (AuthorShared.AuthorOptionGenerate)arg;
		List<AuthorPeice> authorPeices = new List<AuthorPeice>(authorOptionGenerate.creation.EnumeratePeices(authorOptionGenerate.selectedOnly));
		int count = authorPeices.Count;
		if (count == 0)
		{
			return false;
		}
		if (authorOptionGenerate.type == null)
		{
			for (int i = 0; i < count; i++)
			{
				AuthorPeice item = authorPeices[i];
				authorPeice = item;
				if (!item)
				{
					int num = i;
					i = num - 1;
					authorPeices.RemoveAt(num);
					count--;
				}
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				AuthorPeice item1 = authorPeices[j];
				authorPeice = item1;
				if (!item1 || !authorOptionGenerate.type.IsAssignableFrom(authorPeice.GetType()))
				{
					int num1 = j;
					j = num1 - 1;
					authorPeices.RemoveAt(num1);
					count--;
				}
			}
		}
		if (count == 0)
		{
			return false;
		}
		if (!authorOptionGenerate.allowSelf && authorOptionGenerate.self)
		{
			if (!authorOptionGenerate.peice)
			{
				for (int k = 0; k < count; k++)
				{
					AuthorPeice authorPeice1 = authorPeices[k];
					authorPeice = authorPeice1;
					if (authorPeice1 == authorOptionGenerate.self)
					{
						int num2 = k;
						k = num2 - 1;
						authorPeices.RemoveAt(num2);
						count--;
					}
				}
			}
			else
			{
				for (int l = 0; l < count; l++)
				{
					AuthorPeice item2 = authorPeices[l];
					authorPeice = item2;
					if (item2 == authorOptionGenerate.self)
					{
						int num3 = l;
						l = num3 - 1;
						authorPeices.RemoveAt(num3);
						count--;
					}
					else if (authorPeice == authorOptionGenerate.peice)
					{
						int num4 = l;
						l = num4 + 1;
						selected = num4;
						while (l < count)
						{
							AuthorPeice authorPeice2 = authorPeices[l];
							authorPeice = authorPeice2;
							if (authorPeice2 == authorOptionGenerate.self)
							{
								int num5 = l;
								l = num5 - 1;
								authorPeices.RemoveAt(num5);
								count--;
							}
							l++;
						}
						break;
					}
				}
			}
		}
		else if (authorOptionGenerate.peice)
		{
			int num6 = 0;
			while (num6 < count)
			{
				AuthorPeice item3 = authorPeices[num6];
				authorPeice = item3;
				if (item3 != authorOptionGenerate.peice)
				{
					num6++;
				}
				else
				{
					selected = num6;
					break;
				}
			}
		}
		if (count == 0)
		{
			return false;
		}
		AuthorPeice[] authorPeiceArray = authorPeices.ToArray();
		authorPeices = null;
		options = new GUIContent[(int)authorPeiceArray.Length];
		for (int m = 0; m < (int)authorPeiceArray.Length; m++)
		{
			options[m] = new GUIContent(string.Format("{0:00}. {1} ({2})", m, authorPeiceArray[m].peiceID, authorPeiceArray[m].GetType().Name), authorPeiceArray[m].ToString());
		}
		array = authorPeiceArray;
		return true;
	}

	public static Rect BeginHorizontal(params GUILayoutOption[] options)
	{
		return new Rect(0f, 0f, 0f, 0f);
	}

	public static Rect BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
	{
		return new Rect(0f, 0f, 0f, 0f);
	}

	public static Vector2 BeginScrollView(Vector2 scroll, params GUILayoutOption[] options)
	{
		return scroll;
	}

	public static Rect BeginSubSection(AuthorShared.Content title, params GUILayoutOption[] options)
	{
		Color color = GUI.backgroundColor;
		GUI.backgroundColor = new Color(color.r, color.g, color.b, color.a * 0.4f);
		Rect rect = AuthorShared.BeginVertical(AuthorShared.Styles.subSection, new GUILayoutOption[0]);
		AuthorShared.Label(title, AuthorShared.Styles.subSectionTitle, new GUILayoutOption[0]);
		GUI.backgroundColor = color;
		return rect;
	}

	public static Rect BeginSubSection(AuthorShared.Content title, AuthorShared.Content infoContent, GUIStyle infoStyle, params GUILayoutOption[] options)
	{
		Rect rect = AuthorShared.BeginSubSection(title, options);
		if (infoContent.type != 0 && Event.current.type == EventType.Repaint)
		{
			if (infoContent.type != 1)
			{
				GUI.Label(GUILayoutUtility.GetLastRect(), infoContent.content, infoStyle);
			}
			else
			{
				GUI.Label(GUILayoutUtility.GetLastRect(), infoContent.text, infoStyle);
			}
		}
		return rect;
	}

	public static Rect BeginSubSection(AuthorShared.Content title, AuthorShared.Content infoContent, params GUILayoutOption[] options)
	{
		return AuthorShared.BeginSubSection(title, infoContent, AuthorShared.Styles.infoLabel, options);
	}

	public static Rect BeginVertical(params GUILayoutOption[] options)
	{
		return new Rect(0f, 0f, 0f, 0f);
	}

	public static Rect BeginVertical(GUIStyle style, params GUILayoutOption[] options)
	{
		return new Rect(0f, 0f, 0f, 0f);
	}

	public static bool Button(AuthorShared.Content content, GUIStyle style, params GUILayoutOption[] options)
	{
		int num = content.type;
		if (num == 1)
		{
			return GUILayout.Button(content.text, style, options);
		}
		if (num != 2)
		{
			return GUILayout.Button(GUIContent.none, style, options);
		}
		return GUILayout.Button(content.content, style, options);
	}

	public static bool Button(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUILayout.Button(image, style, options);
	}

	public static bool Button(AuthorShared.Content content, params GUILayoutOption[] options)
	{
		int num = content.type;
		if (num == 1)
		{
			return GUILayout.Button(content.text, options);
		}
		if (num != 2)
		{
			return GUILayout.Button(GUIContent.none, options);
		}
		return GUILayout.Button(content.content, options);
	}

	public static bool Button(Texture image, params GUILayoutOption[] options)
	{
		return GUILayout.Button(image, options);
	}

	public static string CalculatePath(Transform targetTransform, Transform root)
	{
		return targetTransform.name;
	}

	public static bool Change(ref int current, int incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref float current, float incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref bool current, bool incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref string current, string incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref Vector2 current, Vector2 incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref Vector3 current, Vector3 incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref Vector4 current, Vector4 incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change(ref Quaternion current, Quaternion incoming)
	{
		if (current == incoming)
		{
			return false;
		}
		current = incoming;
		return true;
	}

	public static bool Change<T>(ref T current, object incoming)
	where T : struct
	{
		bool flag;
		if (current.Equals(incoming))
		{
			return false;
		}
		T t = current;
		try
		{
			current = (T)incoming;
			flag = true;
		}
		catch
		{
			current = t;
			flag = false;
		}
		return flag;
	}

	public static void CustomMenu(Rect position, GUIContent[] options, int selected, AuthorShared.CustomMenuProc proc, object userData)
	{
		string[] strArrays = new string[(int)options.Length];
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			strArrays[i] = options[i].text;
		}
		proc(userData, strArrays, selected);
	}

	public static void EndHorizontal()
	{
	}

	public static void EndScrollView()
	{
	}

	public static void EndSubSection()
	{
		AuthorShared.EndVertical();
	}

	public static void EndVertical()
	{
	}

	public static Enum EnumField(AuthorShared.Content content, Enum value, GUIStyle style, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool EnumField<T>(AuthorShared.Content content, ref T value, GUIStyle style, params GUILayoutOption[] options)
	where T : struct
	{
		return AuthorShared.Change<T>(ref value, AuthorShared.EnumField(content, (Enum)Enum.ToObject(typeof(T), value), style, options));
	}

	public static Enum EnumField(AuthorShared.Content content, Enum value, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool EnumField<T>(AuthorShared.Content content, ref T value, params GUILayoutOption[] options)
	where T : struct
	{
		return AuthorShared.Change<T>(ref value, AuthorShared.EnumField(content, (Enum)Enum.ToObject(typeof(T), value), options));
	}

	public static bool Exists(AuthorShared.ObjectKind kind)
	{
		return kind >= AuthorShared.ObjectKind.LevelInstance;
	}

	public static GameObject FindPrefabRoot(GameObject prefab)
	{
		return prefab.transform.root.gameObject;
	}

	public static float FloatField(AuthorShared.Content content, float value, GUIStyle style, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool FloatField(AuthorShared.Content content, ref float value, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.FloatField(content, value, options));
	}

	public static float FloatField(AuthorShared.Content content, float value, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool FloatField(AuthorShared.Content content, ref float value, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.FloatField(content, value, style, options));
	}

	public static UnityEngine.Object[] GetAllSelectedObjects()
	{
		return new UnityEngine.Object[0];
	}

	public static string GetAssetPath(UnityEngine.Object obj)
	{
		return string.Empty;
	}

	private static Rect GetControlRect(bool hasLabel, float height, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUILayoutUtility.GetRect(0f, 100f, height, height, style, options);
	}

	public static AuthorShared.ObjectKind GetObjectKind(UnityEngine.Object value)
	{
		return AuthorShared.ObjectKind.Null;
	}

	public static Transform GetRootBone(GameObject go)
	{
		SkinnedMeshRenderer skinnedMeshRenderer;
		return AuthorShared.GetRootBone(go, out skinnedMeshRenderer);
	}

	public static Transform GetRootBone(GameObject go, out SkinnedMeshRenderer renderer)
	{
		if (!(go.renderer is SkinnedMeshRenderer))
		{
			renderer = null;
			foreach (Transform transforms in go.transform.ListDecendantsByDepth())
			{
				if (!(transforms.renderer is SkinnedMeshRenderer))
				{
					continue;
				}
				renderer = transforms.renderer as SkinnedMeshRenderer;
				break;
			}
			if (renderer == null)
			{
				return go.transform;
			}
		}
		else
		{
			renderer = go.renderer as SkinnedMeshRenderer;
		}
		return AuthorShared.GetRootBone(renderer);
	}

	public static Transform GetRootBone(Component co, out SkinnedMeshRenderer renderer)
	{
		if (!(co is SkinnedMeshRenderer))
		{
			return AuthorShared.GetRootBone(co.gameObject, out renderer);
		}
		renderer = co as SkinnedMeshRenderer;
		return AuthorShared.GetRootBone(renderer);
	}

	public static Transform GetRootBone(Component co)
	{
		if (co is SkinnedMeshRenderer)
		{
			return AuthorShared.GetRootBone(co as SkinnedMeshRenderer);
		}
		return AuthorShared.GetRootBone(co.gameObject);
	}

	public static Transform GetRootBone(SkinnedMeshRenderer renderer)
	{
		if (!renderer)
		{
			throw new ArgumentNullException("renderer");
		}
		return renderer.transform;
	}

	public static string GUIDToPath(string guid)
	{
		return string.Empty;
	}

	public static bool InAnimationMode()
	{
		return false;
	}

	public static T InstantiatePrefab<T>(T prefab)
	where T : Component
	{
		return (T)UnityEngine.Object.Instantiate(prefab);
	}

	public static GameObject InstantiatePrefab(GameObject prefab)
	{
		return (GameObject)UnityEngine.Object.Instantiate(prefab);
	}

	public static int IntField(AuthorShared.Content content, int value, GUIStyle style, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool IntField(AuthorShared.Content content, ref int value, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.IntField(content, value, style, options));
	}

	public static int IntField(AuthorShared.Content content, int value, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool IntField(AuthorShared.Content content, ref int value, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.IntField(content, value, options));
	}

	public static bool IsAsset(AuthorShared.ObjectKind kind)
	{
		int num = (int)kind;
		return (num < 0 ? false : (num & 1) == 1);
	}

	public static bool IsInstance(AuthorShared.ObjectKind kind)
	{
		int num = (int)kind;
		return (num < 0 ? false : (num & 1) == 0);
	}

	public static bool IsLevelInstance(AuthorShared.ObjectKind kind)
	{
		return (kind == AuthorShared.ObjectKind.LevelInstance || kind == AuthorShared.ObjectKind.MissingPrefabInstance || kind == AuthorShared.ObjectKind.PrefabInstance || kind == AuthorShared.ObjectKind.ModelInstance || kind == AuthorShared.ObjectKind.DisconnectedPrefabInstance ? true : kind == AuthorShared.ObjectKind.DisconnectedModelInstance);
	}

	public static bool IsModelAssetOrInstance(AuthorShared.ObjectKind kind)
	{
		return (kind == AuthorShared.ObjectKind.Model || kind == AuthorShared.ObjectKind.ModelInstance ? true : kind == AuthorShared.ObjectKind.DisconnectedModelInstance);
	}

	public static bool IsNonModelPrefabAssetOrInstance(AuthorShared.ObjectKind kind)
	{
		return (kind == AuthorShared.ObjectKind.Prefab || kind == AuthorShared.ObjectKind.PrefabInstance ? true : kind == AuthorShared.ObjectKind.DisconnectedPrefabInstance);
	}

	public static bool IsPrefabAssetOrInstance(AuthorShared.ObjectKind kind)
	{
		return (kind == AuthorShared.ObjectKind.Prefab || kind == AuthorShared.ObjectKind.Model || kind == AuthorShared.ObjectKind.PrefabInstance || kind == AuthorShared.ObjectKind.ModelInstance || kind == AuthorShared.ObjectKind.DisconnectedPrefabInstance ? true : kind == AuthorShared.ObjectKind.DisconnectedModelInstance);
	}

	public static bool IsScriptableObjectAssetOrInstance(AuthorShared.ObjectKind kind)
	{
		return (kind == AuthorShared.ObjectKind.ScriptableObject ? true : kind == AuthorShared.ObjectKind.ScriptableObjectInstance);
	}

	public static void Label(AuthorShared.Content content, GUIStyle style, params GUILayoutOption[] options)
	{
		int num = content.type;
		if (num == 1)
		{
			GUILayout.Label(content.text, style, options);
		}
		else if (num == 2)
		{
			GUILayout.Label(content.content, style, options);
		}
		else
		{
			GUILayout.Label(GUIContent.none, style, options);
		}
	}

	public static void Label(Texture content, GUIStyle style, params GUILayoutOption[] options)
	{
		GUILayout.Label(content, style, options);
	}

	public static void Label(AuthorShared.Content content, params GUILayoutOption[] options)
	{
		int num = content.type;
		if (num == 1)
		{
			GUILayout.Label(content.text, options);
		}
		else if (num == 2)
		{
			GUILayout.Label(content.content, options);
		}
		else
		{
			GUILayout.Label(GUIContent.none, options);
		}
	}

	public static void Label(Texture content, params GUILayoutOption[] options)
	{
		GUILayout.Label(content, options);
	}

	public static bool MatchPrefab(UnityEngine.Object a, UnityEngine.Object b)
	{
		if (!(a == b) && a && b)
		{
			return false;
		}
		return false;
	}

	public static AuthorShared.Content ObjectContent(UnityEngine.Object o, Type type)
	{
		return AuthorShared.ObjectContentR(o, type);
	}

	public static AuthorShared.Content ObjectContent(Type type)
	{
		return AuthorShared.ObjectContentR(null, type);
	}

	public static AuthorShared.Content ObjectContent<T>(T o, Type type)
	where T : UnityEngine.Object
	{
		object obj = o;
		Type type1 = type ?? typeof(T);
		return AuthorShared.ObjectContentR((UnityEngine.Object)obj, type1);
	}

	public static AuthorShared.Content ObjectContent<T>(T o)
	where T : UnityEngine.Object
	{
		return AuthorShared.ObjectContentR(o, typeof(T));
	}

	public static AuthorShared.Content ObjectContent<T>()
	where T : UnityEngine.Object
	{
		return AuthorShared.ObjectContentR(null, typeof(T));
	}

	private static AuthorShared.Content ObjectContentR(UnityEngine.Object o, Type type)
	{
		return GUIContent.none;
	}

	public static UnityEngine.Object ObjectField(AuthorShared.Content label, UnityEngine.Object value, Type type, bool allowScene, params GUILayoutOption[] options)
	{
		return value;
	}

	public static UnityEngine.Object ObjectField(AuthorShared.Content label, UnityEngine.Object value, Type type, AuthorShared.ObjectFieldFlags flags, params GUILayoutOption[] options)
	{
		UnityEngine.Object obj = AuthorShared.ObjectField(label, value, type, (flags & AuthorShared.ObjectFieldFlags.AllowScene) == AuthorShared.ObjectFieldFlags.AllowScene, options);
		return obj;
	}

	public static UnityEngine.Object ObjectField(UnityEngine.Object obj, Type type, AuthorShared.ObjectFieldFlags flags, params GUILayoutOption[] options)
	{
		AuthorShared.Content content = new AuthorShared.Content();
		return AuthorShared.ObjectField(content, obj, type, flags, options);
	}

	public static UnityEngine.Object ObjectField(UnityEngine.Object obj, Type type, params GUILayoutOption[] options)
	{
		AuthorShared.Content content = new AuthorShared.Content();
		return AuthorShared.ObjectField(content, obj, type, false, options);
	}

	public static bool ObjectField<T>(AuthorShared.Content content, ref T reference, AuthorShared.ObjectFieldFlags flags, params GUILayoutOption[] options)
	where T : UnityEngine.Object
	{
		return AuthorShared.ObjectField<T>(content, ref reference, typeof(T), flags, options);
	}

	public static bool ObjectField<T>(AuthorShared.Content content, ref T reference, Type type, AuthorShared.ObjectFieldFlags flags, params GUILayoutOption[] options)
	where T : UnityEngine.Object
	{
		AuthorShared.Content content1 = content;
		object obj = reference;
		Type type1 = type ?? typeof(T);
		UnityEngine.Object obj1 = AuthorShared.ObjectField(content1, (UnityEngine.Object)obj, type1, flags, options);
		if (!GUI.changed)
		{
			return false;
		}
		reference = (T)obj1;
		return true;
	}

	public static string PathToGUID(string path)
	{
		return string.Empty;
	}

	public static string PathToProjectPath(string path)
	{
		return path;
	}

	public static bool PeiceField<T>(AuthorShared.Content content, AuthorCreation self, ref T peice, Type type, GUIStyle style, params GUILayoutOption[] options)
	where T : AuthorPeice
	{
		return AuthorShared.PeiceFieldBase<T>(content, self, ref peice, type, true, style, options);
	}

	public static bool PeiceField<T>(AuthorShared.Content content, AuthorPeice self, ref T peice, Type type, bool allowSelf, GUIStyle style, params GUILayoutOption[] options)
	where T : AuthorPeice
	{
		return AuthorShared.PeiceFieldBase<T>(content, self, ref peice, type, allowSelf, style, options);
	}

	public static bool PeiceField<T>(AuthorShared.Content content, AuthorPeice self, ref T peice, Type type, GUIStyle style, params GUILayoutOption[] options)
	where T : AuthorPeice
	{
		return AuthorShared.PeiceFieldBase<T>(content, self, ref peice, type, false, style, options);
	}

	private static bool PeiceFieldBase<T>(AuthorShared.Content content, AuthorShared self, ref T peice, Type type, bool allowSelf, GUIStyle style, params GUILayoutOption[] options)
	where T : AuthorPeice
	{
		return false;
	}

	public static void PingObject(UnityEngine.Object o)
	{
	}

	public static void PingObject(int instanceID)
	{
	}

	public static int Popup(AuthorShared.Content content, int index, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(AuthorShared.Content content, ref int index, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(content, index, displayedOptions, style, options));
	}

	public static int Popup(AuthorShared.Content content, int index, GUIContent[] displayedOptions, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(AuthorShared.Content content, ref int index, GUIContent[] displayedOptions, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(content, index, displayedOptions, options));
	}

	public static int Popup(int index, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(ref int index, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(index, displayedOptions, style, options));
	}

	public static int Popup(int index, GUIContent[] displayedOptions, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(ref int index, GUIContent[] displayedOptions, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(index, displayedOptions, options));
	}

	public static int Popup(AuthorShared.Content content, int index, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(AuthorShared.Content content, ref int index, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(content, index, displayedOptions, style, options));
	}

	public static int Popup(AuthorShared.Content content, int index, string[] displayedOptions, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(AuthorShared.Content content, ref int index, string[] displayedOptions, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(content, index, displayedOptions, options));
	}

	public static int Popup(int index, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(ref int index, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(index, displayedOptions, style, options));
	}

	public static int Popup(int index, string[] displayedOptions, params GUILayoutOption[] options)
	{
		return index;
	}

	public static bool Popup(ref int index, string[] displayedOptions, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref index, AuthorShared.Popup(index, displayedOptions, options));
	}

	private static bool PopupImmediate<T>(AuthorShared.Content content, AuthorShared.GenerateOptions generateOptions, T args, GUIStyle style, GUILayoutOption[] options, out object value)
	{
		value = null;
		return false;
	}

	public static void PrefixLabel(AuthorShared.Content content)
	{
	}

	public static void PrefixLabel(AuthorShared.Content content, GUIStyle followingStyle)
	{
	}

	public static void PrefixLabel(AuthorShared.Content content, GUIStyle followingStyle, GUIStyle labelStyle)
	{
	}

	public static bool SelectionContains(UnityEngine.Object obj)
	{
		return false;
	}

	public static bool SelectionContains(int obj)
	{
		return false;
	}

	public static void SetActiveSelection(UnityEngine.Object o)
	{
	}

	public static void SetAllSelectedObjects(params UnityEngine.Object[] objects)
	{
	}

	public static void SetDirty(UnityEngine.Object obj)
	{
	}

	public static void SetSerializedProperty(UnityEngine.Object objSet, string propertyPath, UnityEngine.Object value)
	{
	}

	public static void StartAnimationMode(params UnityEngine.Object[] objects)
	{
	}

	public static void StopAnimationMode()
	{
	}

	public static string StringField(AuthorShared.Content content, string value, GUIStyle style, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool StringField(AuthorShared.Content content, ref string value, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.StringField(content, value, style, options));
	}

	public static string StringField(AuthorShared.Content content, string value, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool StringField(AuthorShared.Content content, ref string value, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.StringField(content, value, options));
	}

	public static bool Toggle(AuthorShared.Content content, bool state, GUIStyle style, params GUILayoutOption[] options)
	{
		int num = content.type;
		if (num == 1)
		{
			return GUILayout.Toggle(state, content.text, style, options);
		}
		if (num != 2)
		{
			return GUILayout.Toggle(state, GUIContent.none, style, options);
		}
		return GUILayout.Toggle(state, content.content, style, options);
	}

	public static bool Toggle(Texture image, bool state, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUILayout.Toggle(state, image, style, options);
	}

	public static bool Toggle(AuthorShared.Content content, bool state, params GUILayoutOption[] options)
	{
		int num = content.type;
		if (num == 1)
		{
			return GUILayout.Toggle(state, content.text, options);
		}
		if (num != 2)
		{
			return GUILayout.Toggle(state, GUIContent.none, options);
		}
		return GUILayout.Toggle(state, content.content, options);
	}

	public static bool Toggle(Texture image, bool state, params GUILayoutOption[] options)
	{
		return GUILayout.Toggle(state, image, options);
	}

	public static bool Toggle(AuthorShared.Content content, ref bool state, GUIStyle style, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref state, AuthorShared.Toggle(content, state, style, options));
	}

	public static bool Toggle(AuthorShared.Content content, ref bool state, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref state, AuthorShared.Toggle(content, state, options));
	}

	public static string TryPathToProjectPath(string path)
	{
		return path;
	}

	public static Vector3 Vector3Field(AuthorShared.Content content, Vector3 value, params GUILayoutOption[] options)
	{
		return value;
	}

	public static bool Vector3Field(AuthorShared.Content content, ref Vector3 value, params GUILayoutOption[] options)
	{
		return AuthorShared.Change(ref value, AuthorShared.Vector3Field(content, value, options));
	}

	private static bool VerifyArgs(AuthorShared.GenerateOptions generateOptions, GUIContent[] options, Array array)
	{
		return (options == null || array == null || (int)options.Length != array.Length ? 1 : (int)((int)options.Length == 0)) == 0;
	}

	public delegate bool ArrayFieldFunctor<T>(ref T value);

	protected class AttributeKeyValueList
	{
		private Dictionary<AuthTarg, ArrayList> dict;

		public AttributeKeyValueList(params object[] keysThenValues) : this((IEnumerable)keysThenValues)
		{
		}

		public AttributeKeyValueList(IEnumerable keysThenValues)
		{
			ArrayList arrayLists;
			this.dict = new Dictionary<AuthTarg, ArrayList>();
			AuthTarg? nullable = null;
			IEnumerator enumerator = null;
			try
			{
				enumerator = keysThenValues.GetEnumerator();
				if (enumerator != null)
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (current as AuthTarg != AuthTarg.HitBox)
						{
							nullable = new AuthTarg?((AuthTarg)((int)current));
						}
						else if (!nullable.HasValue || object.ReferenceEquals(current, null))
						{
							continue;
						}
						if (!this.dict.TryGetValue(nullable.Value, out arrayLists))
						{
							Dictionary<AuthTarg, ArrayList> authTargs = this.dict;
							AuthTarg value = nullable.Value;
							ArrayList arrayLists1 = new ArrayList();
							arrayLists = arrayLists1;
							authTargs[value] = arrayLists1;
						}
						arrayLists.Add(current);
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		[DebuggerHidden]
		private static IEnumerable<Component> GetComponentDown(GameObject go, Type type, Transform childSkip)
		{
			AuthorShared.AttributeKeyValueList.<GetComponentDown>c__Iterator0 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private static IEnumerable<Component> GetComponentDown(GameObject go, Type type)
		{
			AuthorShared.AttributeKeyValueList.<GetComponentDown>c__Iterator1 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private static IEnumerable<Component> GetComponentUp(GameObject go, Type type, bool andThenDown)
		{
			AuthorShared.AttributeKeyValueList.<GetComponentUp>c__Iterator2 variable = null;
			return variable;
		}

		public void Run(MonoBehaviour script)
		{
			if (this.dict.Count > 0)
			{
				AuthorShared.AttributeKeyValueList.TypeRunner.Exec(script, this);
			}
		}

		public void Run(GameObject go)
		{
			if (this.dict.Count > 0 && go)
			{
				MonoBehaviour[] componentsInChildren = go.GetComponentsInChildren<MonoBehaviour>(true);
				for (int i = 0; i < (int)componentsInChildren.Length; i++)
				{
					AuthorShared.AttributeKeyValueList.TypeRunner.Exec(componentsInChildren[i], this);
				}
			}
		}

		private static void RunInstance(MonoBehaviour instance, AuthorShared.AttributeKeyValueList.AuthField attribute, ArrayList args)
		{
			bool flag;
			object value = attribute.field.GetValue(instance);
			if (!(value is UnityEngine.Object))
			{
				flag = value != null;
			}
			else
			{
				flag = (UnityEngine.Object)value;
			}
			if (flag)
			{
				return;
			}
			Type fieldType = attribute.field.FieldType;
			bool flag1 = typeof(Component).IsAssignableFrom(fieldType);
			if (flag1 == (flag1 ? false : typeof(GameObject).IsAssignableFrom(fieldType)))
			{
				return;
			}
			if (AuthorShared.AttributeKeyValueList.Search(instance, attribute, args, flag1, ref value))
			{
				attribute.field.SetValue(instance, value);
			}
		}

		private static bool Search(MonoBehaviour instance, AuthorShared.AttributeKeyValueList.AuthField attribute, ArrayList args, bool isComponent, ref object value)
		{
			AuthOptions authOption = attribute.options & (AuthOptions.SearchDown | AuthOptions.SearchUp);
			bool flag = (int)authOption != 0;
			if ((!flag ? true : (attribute.options & AuthOptions.SearchInclusive) == AuthOptions.SearchInclusive) && AuthorShared.AttributeKeyValueList.SearchGameObject(instance.gameObject, attribute, args, isComponent, ref value))
			{
				return true;
			}
			if (flag)
			{
				if ((authOption & AuthOptions.SearchDown) == AuthOptions.SearchDown)
				{
					if ((attribute.options & (AuthOptions.SearchUp | AuthOptions.SearchReverse)) == (AuthOptions.SearchUp | AuthOptions.SearchReverse))
					{
						if (AuthorShared.AttributeKeyValueList.SearchGameObjectUp(instance.gameObject, attribute, args, isComponent, ref value))
						{
							return true;
						}
						authOption = authOption & (AuthOptions.SearchDown | AuthOptions.SearchInclusive | AuthOptions.SearchReverse);
					}
					if (AuthorShared.AttributeKeyValueList.SearchGameObjectDown(instance.gameObject, attribute, args, isComponent, ref value))
					{
						return true;
					}
				}
				if ((authOption & AuthOptions.SearchUp) == AuthOptions.SearchUp && AuthorShared.AttributeKeyValueList.SearchGameObjectUp(instance.gameObject, attribute, args, isComponent, ref value))
				{
					return true;
				}
			}
			return false;
		}

		private static bool SearchGameObject(GameObject self, AuthorShared.AttributeKeyValueList.AuthField attribute, ArrayList options, bool isComponent, ref object value)
		{
			Component component;
			bool flag;
			IEnumerator enumerator = options.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current is UnityEngine.Object)
					{
						UnityEngine.Object obj = (UnityEngine.Object)current;
						if (obj)
						{
							if (((int)attribute.options & 4) == 0 || !(obj.name != attribute.nameMask))
							{
								if (!isComponent)
								{
									if (!(obj is GameObject))
									{
										continue;
									}
									value = (GameObject)obj;
									flag = true;
									return flag;
								}
								else
								{
									if (obj is GameObject)
									{
										component = ((GameObject)obj).GetComponent(attribute.field.FieldType);
									}
									else if (attribute.field.FieldType.IsAssignableFrom(obj.GetType()))
									{
										component = (Component)obj;
									}
									else if (!(obj is Component))
									{
										continue;
									}
									else
									{
										component = ((Component)obj).GetComponent(attribute.field.FieldType);
									}
									if (component)
									{
										value = component;
										flag = true;
										return flag;
									}
								}
							}
						}
					}
				}
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			return flag;
		}

		private static bool SearchGameObjectDown(GameObject self, AuthorShared.AttributeKeyValueList.AuthField attribute, ArrayList options, bool isComponent, ref object value)
		{
			GameObject gameObject;
			bool flag;
			Type type = (!isComponent ? typeof(Transform) : attribute.field.FieldType);
			IEnumerator enumerator = options.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current is UnityEngine.Object)
					{
						UnityEngine.Object obj = (UnityEngine.Object)current;
						if (obj)
						{
							if (obj is GameObject)
							{
								gameObject = (GameObject)obj;
							}
							else if (!(obj is Component))
							{
								continue;
							}
							else
							{
								gameObject = ((Component)obj).gameObject;
							}
							IEnumerator<Component> enumerator1 = AuthorShared.AttributeKeyValueList.GetComponentDown(gameObject, type).GetEnumerator();
							try
							{
								while (enumerator1.MoveNext())
								{
									Component component = enumerator1.Current;
									if (((int)attribute.options & 4) == 0 || !(component.name != attribute.nameMask))
									{
										if (!isComponent)
										{
											GameObject gameObject1 = component.gameObject;
											if (!gameObject1)
											{
												continue;
											}
											value = gameObject1;
											flag = true;
											return flag;
										}
										else
										{
											value = component;
											flag = true;
											return flag;
										}
									}
								}
							}
							finally
							{
								if (enumerator1 == null)
								{
								}
								enumerator1.Dispose();
							}
						}
					}
				}
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			return flag;
		}

		private static bool SearchGameObjectUp(GameObject self, AuthorShared.AttributeKeyValueList.AuthField attribute, ArrayList options, bool isComponent, ref object value)
		{
			GameObject gameObject;
			bool flag;
			Type type = (!isComponent ? typeof(Transform) : attribute.field.FieldType);
			IEnumerator enumerator = options.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current is UnityEngine.Object)
					{
						UnityEngine.Object obj = (UnityEngine.Object)current;
						if (obj)
						{
							if (obj is GameObject)
							{
								gameObject = (GameObject)obj;
							}
							else if (!(obj is Component))
							{
								continue;
							}
							else
							{
								gameObject = ((Component)obj).gameObject;
							}
							IEnumerator<Component> enumerator1 = AuthorShared.AttributeKeyValueList.GetComponentUp(gameObject, type, false).GetEnumerator();
							try
							{
								while (enumerator1.MoveNext())
								{
									Component component = enumerator1.Current;
									if (((int)attribute.options & 4) == 0 || !(component.name != attribute.nameMask))
									{
										if (!isComponent)
										{
											GameObject gameObject1 = component.gameObject;
											if (!gameObject1)
											{
												continue;
											}
											value = gameObject1;
											flag = true;
											return flag;
										}
										else
										{
											value = component;
											flag = true;
											return flag;
										}
									}
								}
							}
							finally
							{
								if (enumerator1 == null)
								{
								}
								enumerator1.Dispose();
							}
						}
					}
				}
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			return flag;
		}

		private class AuthField
		{
			public FieldInfo field;

			public AuthOptions options;

			public string nameMask;

			public AuthField()
			{
			}
		}

		private static class TypeRunner
		{
			private readonly static Dictionary<Type, AuthorShared.AttributeKeyValueList.TypeRunnerPlatform> platforms;

			static TypeRunner()
			{
				AuthorShared.AttributeKeyValueList.TypeRunner.platforms = new Dictionary<Type, AuthorShared.AttributeKeyValueList.TypeRunnerPlatform>();
			}

			public static void Exec(MonoBehaviour monoBehaviour, AuthorShared.AttributeKeyValueList kv)
			{
				AuthorShared.AttributeKeyValueList.TypeRunnerPlatform typeRunnerPlatform;
				if (monoBehaviour)
				{
					Type type = monoBehaviour.GetType();
					if (type != typeof(MonoBehaviour))
					{
						if (!AuthorShared.AttributeKeyValueList.TypeRunner.platforms.TryGetValue(type, out typeRunnerPlatform))
						{
							AuthorShared.AttributeKeyValueList.TypeRunner.GeneratePlatform(type, out typeRunnerPlatform);
						}
						typeRunnerPlatform.Exec(monoBehaviour, kv);
					}
				}
			}

			private static void GeneratePlatform(Type type, out AuthorShared.AttributeKeyValueList.TypeRunnerPlatform platform)
			{
				if (type.BaseType == typeof(MonoBehaviour))
				{
					platform = null;
				}
				else if (!AuthorShared.AttributeKeyValueList.TypeRunner.platforms.TryGetValue(type.BaseType, out platform))
				{
					AuthorShared.AttributeKeyValueList.TypeRunner.GeneratePlatform(type.BaseType, out platform);
				}
				AuthorShared.AttributeKeyValueList.TypeRunnerExec value = (AuthorShared.AttributeKeyValueList.TypeRunnerExec)typeof(AuthorShared.AttributeKeyValueList.TypeRunner<>).MakeGenericType(new Type[] { type }).GetField("exec", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(null);
				if (value != null)
				{
					if (platform != null && platform.exec != null)
					{
						value += platform.exec;
					}
				}
				else if (platform != null)
				{
					value = platform.exec;
				}
				Dictionary<Type, AuthorShared.AttributeKeyValueList.TypeRunnerPlatform> types = AuthorShared.AttributeKeyValueList.TypeRunner.platforms;
				AuthorShared.AttributeKeyValueList.TypeRunnerPlatform typeRunnerPlatform = new AuthorShared.AttributeKeyValueList.TypeRunnerPlatform()
				{
					@base = platform,
					exec = value,
					hasBase = platform != null,
					hasDelegate = value != null,
					tested = true
				};
				AuthorShared.AttributeKeyValueList.TypeRunnerPlatform typeRunnerPlatform1 = typeRunnerPlatform;
				typeRunnerPlatform = typeRunnerPlatform1;
				platform = typeRunnerPlatform1;
				types[type] = typeRunnerPlatform;
			}

			public static bool TestAttribute<T>(FieldInfo field, out T[] attribs)
			where T : Attribute
			{
				if (Attribute.IsDefined(field, typeof(T)))
				{
					Attribute[] customAttributes = Attribute.GetCustomAttributes(field, typeof(T), false);
					if ((int)customAttributes.Length > 0)
					{
						attribs = new T[(int)customAttributes.Length];
						for (int i = 0; i < (int)customAttributes.Length; i++)
						{
							attribs[i] = (T)customAttributes[i];
						}
						return true;
					}
				}
				attribs = null;
				return false;
			}
		}

		private static class TypeRunner<T>
		where T : MonoBehaviour
		{
			private readonly static KeyValuePair<AuthTarg, AuthorShared.AttributeKeyValueList.AuthField>[] fields;

			private readonly static int fieldCount;

			private readonly static AuthorShared.AttributeKeyValueList.TypeRunnerExec exec;

			static TypeRunner()
			{
				PostAuthAttribute[] postAuthAttributeArray;
				bool flag;
				int num;
				FieldInfo[] fields = typeof(T).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				int length = (int)fields.Length;
				for (int i = 0; i < length; i++)
				{
					if (AuthorShared.AttributeKeyValueList.TypeRunner.TestAttribute<PostAuthAttribute>(fields[i], out postAuthAttributeArray))
					{
						List<KeyValuePair<AuthTarg, AuthorShared.AttributeKeyValueList.AuthField>> keyValuePairs = new List<KeyValuePair<AuthTarg, AuthorShared.AttributeKeyValueList.AuthField>>();
						do
						{
							flag = false;
							int num1 = 0;
							int length1 = (int)postAuthAttributeArray.Length;
							do
							{
								AuthTarg authTarg = postAuthAttributeArray[num1].target;
								AuthorShared.AttributeKeyValueList.AuthField authField = new AuthorShared.AttributeKeyValueList.AuthField()
								{
									field = fields[i],
									options = postAuthAttributeArray[num1].options,
									nameMask = postAuthAttributeArray[num1].nameMask
								};
								keyValuePairs.Add(new KeyValuePair<AuthTarg, AuthorShared.AttributeKeyValueList.AuthField>(authTarg, authField));
								num = num1 + 1;
								num1 = num;
							}
							while (num < length1);
							while (true)
							{
								int num2 = i + 1;
								i = num2;
								if (num2 >= length)
								{
									goto Label0;
								}
								bool flag1 = AuthorShared.AttributeKeyValueList.TypeRunner.TestAttribute<PostAuthAttribute>(fields[i], out postAuthAttributeArray);
								flag = flag1;
								if (flag1)
								{
									goto Label0;
								}
							}
						Label0:
						}
						while (flag);
						AuthorShared.AttributeKeyValueList.TypeRunner<T>.fields = keyValuePairs.ToArray();
						AuthorShared.AttributeKeyValueList.TypeRunner<T>.fieldCount = (int)AuthorShared.AttributeKeyValueList.TypeRunner<T>.fields.Length;
						AuthorShared.AttributeKeyValueList.TypeRunner<T>.exec = new AuthorShared.AttributeKeyValueList.TypeRunnerExec(AuthorShared.AttributeKeyValueList.TypeRunner<T>.Exec);
						return;
					}
				}
				AuthorShared.AttributeKeyValueList.TypeRunner<T>.exec = null;
				AuthorShared.AttributeKeyValueList.TypeRunner<T>.fieldCount = 0;
				AuthorShared.AttributeKeyValueList.TypeRunner<T>.fields = null;
			}

			private static void Exec(object instance, AuthorShared.AttributeKeyValueList list)
			{
				ArrayList arrayLists;
				MonoBehaviour monoBehaviour = (MonoBehaviour)instance;
				for (int i = 0; i < AuthorShared.AttributeKeyValueList.TypeRunner<T>.fieldCount; i++)
				{
					if (list.dict.TryGetValue(AuthorShared.AttributeKeyValueList.TypeRunner<T>.fields[i].Key, out arrayLists))
					{
						AuthorShared.AttributeKeyValueList.RunInstance(monoBehaviour, AuthorShared.AttributeKeyValueList.TypeRunner<T>.fields[i].Value, arrayLists);
					}
				}
			}
		}

		private delegate void TypeRunnerExec(object instance, AuthorShared.AttributeKeyValueList kv);

		private class TypeRunnerPlatform
		{
			public AuthorShared.AttributeKeyValueList.TypeRunnerExec exec;

			public AuthorShared.AttributeKeyValueList.TypeRunnerPlatform @base;

			public bool tested;

			public bool hasDelegate;

			public bool hasBase;

			public TypeRunnerPlatform()
			{
			}

			public void Exec(object instance, AuthorShared.AttributeKeyValueList kv)
			{
				if (this.hasBase)
				{
					this.@base.Exec(instance, kv);
				}
				if (this.hasDelegate)
				{
					this.exec(instance, kv);
				}
			}
		}
	}

	private struct AuthorOptionGenerate
	{
		public AuthorCreation creation;

		public AuthorShared self;

		public AuthorPeice peice;

		public Type type;

		public bool allowSelf;

		public bool selectedOnly;
	}

	public struct Content
	{
		public readonly int type;

		public readonly string text;

		public readonly GUIContent content;

		public Texture image
		{
			get
			{
				return (this.type != 2 ? GUIContent.none.image : this.content.image);
			}
		}

		public string tooltip
		{
			get
			{
				return (this.type != 2 ? GUIContent.none.tooltip : this.content.tooltip);
			}
		}

		private Content(GUIContent content)
		{
			this.content = content;
			this.text = (content ?? GUIContent.none).text;
			this.type = 2;
		}

		private Content(string text)
		{
			this.content = null;
			this.text = text;
			this.type = (text != null ? 1 : 0);
		}

		public static explicit operator String(AuthorShared.Content content)
		{
			return content.text;
		}

		public static bool operator @false(AuthorShared.Content content)
		{
			return content.type == 0;
		}

		public static implicit operator Content(GUIContent content)
		{
			return new AuthorShared.Content(content);
		}

		public static implicit operator Content(string content)
		{
			return new AuthorShared.Content(content);
		}

		public static implicit operator Content(bool show)
		{
			if (show)
			{
				return new AuthorShared.Content(GUIContent.none);
			}
			return new AuthorShared.Content();
		}

		public static implicit operator GUIContent(AuthorShared.Content content)
		{
			return AuthorShared.Content.g.GetOrTemp(content);
		}

		public static bool operator @true(AuthorShared.Content content)
		{
			return content.type != 0;
		}

		private static class g
		{
			public readonly static GUIContent noneCopy;

			public readonly static GUIContent[] bufContents;

			private static int bufPos;

			static g()
			{
				AuthorShared.Content.g.noneCopy = new GUIContent();
				AuthorShared.Content.g.bufContents = new GUIContent[] { new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent(), new GUIContent() };
				AuthorShared.Content.g.bufPos = 0;
			}

			public static GUIContent GetOrTemp(AuthorShared.Content content)
			{
				if (content.type == 2)
				{
					return content.content;
				}
				if (content.type != 1)
				{
					return AuthorShared.Content.g.noneCopy;
				}
				GUIContent gUIContent = AuthorShared.Content.g.bufContents[AuthorShared.Content.g.bufPos];
				int num = AuthorShared.Content.g.bufPos + 1;
				AuthorShared.Content.g.bufPos = num;
				if (num == (int)AuthorShared.Content.g.bufContents.Length)
				{
					AuthorShared.Content.g.bufPos = 0;
				}
				gUIContent.text = content.text;
				gUIContent.tooltip = AuthorShared.Content.g.noneCopy.tooltip;
				gUIContent.image = AuthorShared.Content.g.noneCopy.image;
				return gUIContent;
			}
		}
	}

	public delegate void CustomMenuProc(object userData, string[] options, int selected);

	private delegate bool GenerateOptions(object args, ref int selected, out GUIContent[] options, out Array values);

	private static class Hash
	{
		public readonly static int s_PopupHash;

		static Hash()
		{
			AuthorShared.Hash.s_PopupHash = "EditorPopup".GetHashCode();
		}
	}

	protected static class Icon
	{
		private static GUIContent _solo;

		private static GUIContent _delete;

		public static GUIContent delete
		{
			get
			{
				GUIContent gUIContent = AuthorShared.Icon._delete;
				if (gUIContent == null)
				{
					gUIContent = new GUIContent(AuthorShared.Icon.texDelete, "Delete");
					AuthorShared.Icon._delete = gUIContent;
				}
				return gUIContent;
			}
		}

		public static GUIContent solo
		{
			get
			{
				GUIContent gUIContent = AuthorShared.Icon._solo;
				if (gUIContent == null)
				{
					gUIContent = new GUIContent(AuthorShared.Icon.texSolo, "Solo Select");
					AuthorShared.Icon._solo = gUIContent;
				}
				return gUIContent;
			}
		}

		public static Texture texDelete
		{
			get
			{
				return null;
			}
		}

		public static Texture texSolo
		{
			get
			{
				return null;
			}
		}
	}

	public enum ObjectFieldFlags
	{
		AllowScene = 1,
		ForbidNull = 2,
		Prefab = 4,
		Model = 8,
		Instance = 16,
		NotPrefab = 32,
		NotModel = 64,
		NotInstance = 128,
		Asset = 256,
		Root = 512
	}

	public enum ObjectKind
	{
		Null = -2,
		LevelInstance = 0,
		Prefab = 1,
		Model = 2,
		PrefabInstance = 3,
		ModelInstance = 4,
		MissingPrefabInstance = 5,
		DisconnectedPrefabInstance = 6,
		ScriptableObject = 7,
		DisconnectedModelInstance = 8,
		OtherAsset = 9,
		OtherInstance = 10,
		ScriptableObjectInstance = 11
	}

	public enum PeiceAction
	{
		None,
		AddToSelection,
		RemoveFromSelection,
		SelectSolo,
		Delete,
		Dirty,
		Ping
	}

	public struct PeiceCommand
	{
		public AuthorPeice peice;

		public AuthorShared.PeiceAction action;
	}

	public struct PropMod
	{
		public UnityEngine.Object objectReference
		{
			get
			{
				return null;
			}
		}

		public string propertyPath
		{
			get
			{
				return string.Empty;
			}
		}

		public UnityEngine.Object target
		{
			get
			{
				return null;
			}
		}

		public string @value
		{
			get
			{
				return string.Empty;
			}
		}

		public static AuthorShared.PropMod[] Get(UnityEngine.Object o)
		{
			return new AuthorShared.PropMod[0];
		}

		public static AuthorShared.PropMod New()
		{
			return new AuthorShared.PropMod();
		}

		public static void Set(UnityEngine.Object o, AuthorShared.PropMod[] mod)
		{
		}
	}

	public static class Scene
	{
		private const int SHAPE_MESH = 0;

		private const int SHAPE_DISH = 1;

		private const int SHAPE_BONE = 2;

		private const int SHAPE_BOX = 3;

		private const int SHAPE_CAPSULE_X = 4;

		private const int SHAPE_CAPSULE_Y = 5;

		private const int SHAPE_CAPSULE_Z = 6;

		private const int SHAPE_SPHERE = 7;

		private const int kShapeCount = 8;

		private const string _ToolColor = "_Tc";

		private const string _Radius = "_Rv";

		private const string _Height = "_Hv";

		private const string _Sides = "_S3";

		private const string _LightScale = "_Lv";

		private const string _BoneParameters = "_B4";

		public static Color color
		{
			get
			{
				return Color.white;
			}
			set
			{
			}
		}

		public static bool lighting
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public static Matrix4x4 matrix
		{
			get
			{
				return Matrix4x4.identity;
			}
			set
			{
			}
		}

		public static bool BoxDrag(ref Vector3 center, ref Vector3 size)
		{
			return false;
		}

		private static float CapRadius(float radius, float height, int axis, int heightAxis)
		{
			if (heightAxis != axis)
			{
				return radius;
			}
			return radius + height / 2f;
		}

		public static bool CapsuleDrag(ref Vector3 center, ref float radius, ref float height, ref int heightAxis)
		{
			return false;
		}

		private static Vector3 Direction(int i)
		{
			Vector3 vector3;
			switch (i % 3)
			{
				case 0:
				{
					vector3 = (i / 3 % 2 * (i / 3 % 2) != 1 ? Vector3.right : Vector3.left);
					return vector3;
				}
				case 1:
				{
					return (i / 3 % 2 * (i / 3 % 2) != 1 ? Vector3.up : Vector3.down);
				}
				case 2:
				{
					return (i / 3 % 2 * (i / 3 % 2) != 1 ? Vector3.forward : Vector3.back);
				}
				default:
				{
					vector3 = (i / 3 % 2 * (i / 3 % 2) != 1 ? Vector3.right : Vector3.left);
					return vector3;
				}
			}
		}

		public static void DrawBone(Vector3 origin, Quaternion rot, float length, float backLength, Vector3 size)
		{
		}

		private static void DrawBoneNow(Vector3 origin, Quaternion forward, float length, float backLength, Vector3 size)
		{
		}

		public static void DrawBox(Vector3 center, Vector3 size)
		{
		}

		private static void DrawBoxNow(Vector3 center, Vector3 size)
		{
		}

		public static void DrawCapsule(Vector3 center, float radius, float height, int axis)
		{
		}

		private static void DrawCapsuleNow(Vector3 center, float radius, float height, int axis)
		{
		}

		public static void DrawSphere(Vector3 center, float radius)
		{
		}

		private static void DrawSphereNow(Vector3 center, float radius)
		{
		}

		public static float? GetAxialAngleDifference(Quaternion a, Quaternion b)
		{
			Vector3 vector3;
			Vector3 vector31;
			float single;
			float single1;
			a.ToAngleAxis(out single, out vector3);
			b.ToAngleAxis(out single1, out vector31);
			float single2 = Vector3.Dot(vector3, vector31);
			if (Mathf.Approximately(single2, 1f))
			{
				return new float?(Mathf.DeltaAngle(single, single1));
			}
			if (!Mathf.Approximately(single2, -1f))
			{
				return null;
			}
			return new float?(Mathf.DeltaAngle(single, -single1));
		}

		public static void GetUpAndRight(ref Vector3 forward, out Vector3 right, out Vector3 up)
		{
			forward.Normalize();
			float single = Vector3.Dot(forward, Vector3.up);
			if (single * single <= 0.809999943f)
			{
				right = Vector3.Cross(forward, Vector3.up);
				right.Normalize();
				up = Vector3.Cross(forward, right);
				up.Normalize();
			}
			else
			{
				if (forward.x * forward.x > forward.z * forward.z)
				{
					up = Vector3.Cross(forward, Vector3.forward);
				}
				else
				{
					up = Vector3.Cross(forward, Vector3.right);
				}
				up.Normalize();
				right = Vector3.Cross(forward, up);
				right.Normalize();
			}
			if (Vector3.Dot(Vector3.Cross(up, forward), right) < 0f)
			{
				right = -right;
			}
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref float min, ref float max)
		{
			float single = 0f;
			return (!AuthorShared.Scene.LimitDrag(anchor, axis, ref single, ref min, ref max) ? false : single == 0f);
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref float offset, ref float min, ref float max)
		{
			return false;
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref JointLimits limit)
		{
			float single = limit.min;
			float single1 = limit.max;
			if (!AuthorShared.Scene.LimitDrag(anchor, axis, ref single, ref single1))
			{
				return false;
			}
			limit.min = single;
			limit.max = single1;
			limit.min = single;
			return true;
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref float offset, ref JointLimits limit)
		{
			float single = limit.min;
			float single1 = limit.max;
			if (!AuthorShared.Scene.LimitDrag(anchor, axis, ref offset, ref single, ref single1))
			{
				return false;
			}
			limit.min = single;
			limit.max = single1;
			return true;
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref SoftJointLimit low, ref SoftJointLimit high)
		{
			float single = low.limit;
			float single1 = high.limit;
			if (AuthorShared.Scene.LimitDrag(anchor, axis, ref single, ref single1))
			{
				if (single != low.limit)
				{
					single = Mathf.Clamp(single, -180f, 180f);
					if (single == low.limit)
					{
						return false;
					}
					low.limit = single;
					return true;
				}
				if (single1 == high.limit)
				{
					return true;
				}
				single1 = Mathf.Clamp(single1, -180f, 180f);
				if (single1 != high.limit)
				{
					high.limit = single1;
					return true;
				}
			}
			return false;
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref SoftJointLimit bothWays)
		{
			float single = bothWays.limit;
			if (!AuthorShared.Scene.LimitDragBothWays(anchor, axis, ref single))
			{
				return false;
			}
			bothWays.limit = single;
			return true;
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref float offset, ref SoftJointLimit low, ref SoftJointLimit high)
		{
			float single = low.limit;
			float single1 = high.limit;
			if (AuthorShared.Scene.LimitDrag(anchor, axis, ref offset, ref single, ref single1))
			{
				if (single != low.limit)
				{
					single = Mathf.Clamp(single, -180f, 180f);
					if (single == low.limit)
					{
						return false;
					}
					low.limit = single;
					return true;
				}
				if (single1 == high.limit)
				{
					return true;
				}
				single1 = Mathf.Clamp(single1, -180f, 180f);
				if (single1 != high.limit)
				{
					high.limit = single1;
					return true;
				}
			}
			return false;
		}

		public static bool LimitDrag(Vector3 anchor, Vector3 axis, ref float offset, ref SoftJointLimit bothWays)
		{
			float single = bothWays.limit;
			if (!AuthorShared.Scene.LimitDragBothWays(anchor, axis, ref offset, ref single))
			{
				return false;
			}
			bothWays.limit = single;
			return true;
		}

		public static bool LimitDragBothWays(Vector3 anchor, Vector3 axis, ref float angle)
		{
			float single = 0f;
			return (!AuthorShared.Scene.LimitDragBothWays(anchor, axis, ref single, ref angle) ? false : single == 0f);
		}

		public static bool LimitDragBothWays(Vector3 anchor, Vector3 axis, ref float offset, ref float angle)
		{
			return false;
		}

		public static bool PivotDrag(ref Vector3 anchor, ref Vector3 axis)
		{
			return false;
		}

		public static bool PointDrag(ref Vector3 anchor)
		{
			return false;
		}

		public static bool PointDrag(ref Vector3 anchor, ref Vector3 axis)
		{
			return false;
		}

		public static bool SphereDrag(ref Vector3 center, ref float radius)
		{
			return false;
		}

		private static class Keyword
		{
			private const int BIT_STRINGS_LENGTH = 3;

			private readonly static string[] BIT_STRINGS;

			public readonly static string[][] SHAPE;

			static Keyword()
			{
				AuthorShared.Scene.Keyword.BIT_STRINGS = new string[] { "SBA", "SBB", "SBC" };
				AuthorShared.Scene.Keyword.SHAPE = new string[8][];
				for (int i = 0; i < 8; i++)
				{
					int num = 0;
					for (int j = 0; j < 3; j++)
					{
						if ((i & 1 << (j & 31)) == 1 << (j & 31))
						{
							num++;
						}
					}
					AuthorShared.Scene.Keyword.SHAPE[i] = new string[num];
					int num1 = 0;
					for (int k = 0; k < 3; k++)
					{
						if ((i & 1 << (k & 31)) == 1 << (k & 31))
						{
							int num2 = num1;
							num1 = num2 + 1;
							AuthorShared.Scene.Keyword.SHAPE[i][num2] = AuthorShared.Scene.Keyword.BIT_STRINGS[k];
						}
					}
				}
			}
		}
	}

	public static class Styles
	{
		private readonly static AuthorShared.Styles.StyleModFunctor rightAlignText;

		private readonly static AuthorShared.Styles.StyleModFunctor leftAlignText;

		private readonly static AuthorShared.Styles.StyleModFunctor centerAlignText;

		private readonly static AuthorShared.Styles.StyleModFunctor iconAbove;

		private static AuthorShared.Styles.StyleMod _peiceButtonLeft;

		private static AuthorShared.Styles.StyleMod _peiceButtonMid;

		private static AuthorShared.Styles.StyleMod _peiceButtonRight;

		private static AuthorShared.Styles.StyleMod _palletButton;

		private static AuthorShared.Styles.StyleMod _gradientOutline;

		private static AuthorShared.Styles.StyleMod _gradientInline;

		private static AuthorShared.Styles.StyleMod _gradientOutlineFill;

		private static AuthorShared.Styles.StyleMod _gradientInlineFill;

		private static AuthorShared.Styles.StyleMod _subSectionTitle;

		private static AuthorShared.Styles.StyleMod _infoLabel;

		public static GUIStyle boldLabel
		{
			get
			{
				return AuthorShared.Styles.label;
			}
		}

		public static GUIStyle box
		{
			get
			{
				return GUI.skin.box;
			}
		}

		public static GUIStyle button
		{
			get
			{
				return GUI.skin.button;
			}
		}

		public static GUIStyle gradientInline
		{
			get
			{
				return AuthorShared.Styles._gradientInline.GetStyle(AuthorShared.Styles.box);
			}
		}

		public static GUIStyle gradientInlineFill
		{
			get
			{
				return AuthorShared.Styles._gradientInlineFill.GetStyle(AuthorShared.Styles.box);
			}
		}

		public static GUIStyle gradientOutline
		{
			get
			{
				return AuthorShared.Styles._gradientOutline.GetStyle(AuthorShared.Styles.box);
			}
		}

		public static GUIStyle gradientOutlineFill
		{
			get
			{
				return AuthorShared.Styles._gradientOutlineFill.GetStyle(AuthorShared.Styles.box);
			}
		}

		public static GUIStyle infoLabel
		{
			get
			{
				return AuthorShared.Styles._infoLabel.GetStyle(AuthorShared.Styles.miniLabel);
			}
		}

		public static GUIStyle label
		{
			get
			{
				return GUI.skin.label;
			}
		}

		public static GUIStyle largeLabel
		{
			get
			{
				return AuthorShared.Styles.label;
			}
		}

		public static GUIStyle largeWhiteLabel
		{
			get
			{
				return AuthorShared.Styles.label;
			}
		}

		public static GUIStyle miniBoldLabel
		{
			get
			{
				return AuthorShared.Styles.label;
			}
		}

		public static GUIStyle miniButton
		{
			get
			{
				return AuthorShared.Styles.button;
			}
		}

		public static GUIStyle miniButtonLeft
		{
			get
			{
				return AuthorShared.Styles.button;
			}
		}

		public static GUIStyle miniButtonMid
		{
			get
			{
				return AuthorShared.Styles.button;
			}
		}

		public static GUIStyle miniButtonRight
		{
			get
			{
				return AuthorShared.Styles.button;
			}
		}

		public static GUIStyle miniLabel
		{
			get
			{
				return AuthorShared.Styles.label;
			}
		}

		public static GUIStyle palletButton
		{
			get
			{
				return AuthorShared.Styles._palletButton.GetStyle(AuthorShared.Styles.miniButton);
			}
		}

		public static GUIStyle peiceButtonLeft
		{
			get
			{
				return AuthorShared.Styles._peiceButtonLeft.GetStyle(AuthorShared.Styles.miniButtonLeft);
			}
		}

		public static GUIStyle peiceButtonMid
		{
			get
			{
				return AuthorShared.Styles._peiceButtonMid.GetStyle(AuthorShared.Styles.miniButtonMid);
			}
		}

		public static GUIStyle peiceButtonRight
		{
			get
			{
				return AuthorShared.Styles._peiceButtonRight.GetStyle(AuthorShared.Styles.miniButtonRight);
			}
		}

		public static GUIStyle subSection
		{
			get
			{
				return AuthorShared.Styles.gradientOutline;
			}
		}

		public static GUIStyle subSectionTitle
		{
			get
			{
				return AuthorShared.Styles._subSectionTitle.GetStyle(AuthorShared.Styles.box);
			}
		}

		static Styles()
		{
			AuthorShared.Styles.rightAlignText = new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.RightAlignText);
			AuthorShared.Styles.leftAlignText = new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.LeftAlignText);
			AuthorShared.Styles.centerAlignText = new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CenterAlignText);
			AuthorShared.Styles.iconAbove = new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.IconAbove);
			AuthorShared.Styles._peiceButtonLeft = new AuthorShared.Styles.StyleMod(AuthorShared.Styles.leftAlignText);
			AuthorShared.Styles._peiceButtonMid = new AuthorShared.Styles.StyleMod(AuthorShared.Styles.centerAlignText);
			AuthorShared.Styles._peiceButtonRight = new AuthorShared.Styles.StyleMod(AuthorShared.Styles.rightAlignText);
			AuthorShared.Styles._palletButton = new AuthorShared.Styles.StyleMod(AuthorShared.Styles.iconAbove);
			AuthorShared.Styles._gradientOutline = new AuthorShared.Styles.StyleMod(new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CreateGradientOutline));
			AuthorShared.Styles._gradientInline = new AuthorShared.Styles.StyleMod(new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CreateGradientInline));
			AuthorShared.Styles._gradientOutlineFill = new AuthorShared.Styles.StyleMod(new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CreateGradientOutlineFill));
			AuthorShared.Styles._gradientInlineFill = new AuthorShared.Styles.StyleMod(new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CreateGradientInlineFill));
			AuthorShared.Styles._subSectionTitle = new AuthorShared.Styles.StyleMod(new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CreateSubSectionTitleFill));
			AuthorShared.Styles._infoLabel = new AuthorShared.Styles.StyleMod(new AuthorShared.Styles.StyleModFunctor(AuthorShared.Styles.CreateInfoLabel));
		}

		private static void CenterAlignText(GUIStyle original, ref GUIStyle mod)
		{
			switch (original.alignment)
			{
				case TextAnchor.UpperLeft:
				case TextAnchor.UpperRight:
				{
					mod.alignment = TextAnchor.UpperCenter;
					return;
				}
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
				{
					return;
				}
				case TextAnchor.MiddleLeft:
				case TextAnchor.MiddleRight:
				{
					mod.alignment = TextAnchor.MiddleCenter;
					return;
				}
				case TextAnchor.LowerLeft:
				case TextAnchor.LowerRight:
				{
					mod.alignment = TextAnchor.LowerCenter;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static void CreateGradientInline(GUIStyle original, ref GUIStyle mod)
		{
			mod.border = new RectOffset(1, 1, 1, 1);
			mod.normal = new GUIStyleState()
			{
				background = (Texture2D)UnityEngine.Resources.LoadAssetAtPath("Assets/AuthorSuite/Editor Resources/Icons/GradientInline.png", typeof(Texture2D))
			};
		}

		private static void CreateGradientInlineFill(GUIStyle original, ref GUIStyle mod)
		{
			mod.border = new RectOffset(1, 1, 1, 1);
			mod.normal = new GUIStyleState()
			{
				background = (Texture2D)UnityEngine.Resources.LoadAssetAtPath("Assets/AuthorSuite/Editor Resources/Icons/GradientInlineFill.png", typeof(Texture2D))
			};
		}

		private static void CreateGradientOutline(GUIStyle original, ref GUIStyle mod)
		{
			mod.border = new RectOffset(1, 1, 1, 1);
			mod.normal = new GUIStyleState()
			{
				background = (Texture2D)UnityEngine.Resources.LoadAssetAtPath("Assets/AuthorSuite/Editor Resources/Icons/GradientOutline.png", typeof(Texture2D))
			};
		}

		private static void CreateGradientOutlineFill(GUIStyle original, ref GUIStyle mod)
		{
			mod.border = new RectOffset(1, 1, 1, 1);
			mod.normal = new GUIStyleState()
			{
				background = (Texture2D)UnityEngine.Resources.LoadAssetAtPath("Assets/AuthorSuite/Editor Resources/Icons/GradientOutlineFill.png", typeof(Texture2D))
			};
		}

		private static void CreateInfoLabel(GUIStyle original, ref GUIStyle mod)
		{
			mod.alignment = TextAnchor.LowerLeft;
			mod.normal.textColor = new Color(1f, 1f, 1f, 0.17f);
		}

		private static void CreateSubSectionTitleFill(GUIStyle original, ref GUIStyle mod)
		{
			AuthorShared.Styles.CreateGradientOutlineFill(original, ref mod);
			mod.alignment = TextAnchor.UpperRight;
			mod.font = AuthorShared.Styles.boldLabel.font;
			mod.normal.textColor = new Color(0.03f, 0.03f, 0.03f, 1f);
			mod.stretchWidth = true;
		}

		private static void IconAbove(GUIStyle original, ref GUIStyle mod)
		{
			mod.imagePosition = ImagePosition.ImageAbove;
		}

		private static void LeftAlignText(GUIStyle original, ref GUIStyle mod)
		{
			switch (original.alignment)
			{
				case TextAnchor.UpperCenter:
				case TextAnchor.UpperRight:
				{
					mod.alignment = TextAnchor.UpperLeft;
					return;
				}
				case TextAnchor.MiddleLeft:
				case TextAnchor.LowerLeft:
				{
					return;
				}
				case TextAnchor.MiddleCenter:
				case TextAnchor.MiddleRight:
				{
					mod.alignment = TextAnchor.MiddleLeft;
					return;
				}
				case TextAnchor.LowerCenter:
				case TextAnchor.LowerRight:
				{
					mod.alignment = TextAnchor.LowerLeft;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static void RightAlignText(GUIStyle original, ref GUIStyle mod)
		{
			switch (original.alignment)
			{
				case TextAnchor.UpperLeft:
				case TextAnchor.UpperCenter:
				{
					mod.alignment = TextAnchor.UpperRight;
					return;
				}
				case TextAnchor.UpperRight:
				case TextAnchor.MiddleRight:
				{
					return;
				}
				case TextAnchor.MiddleLeft:
				case TextAnchor.MiddleCenter:
				{
					mod.alignment = TextAnchor.MiddleRight;
					return;
				}
				case TextAnchor.LowerLeft:
				case TextAnchor.LowerCenter:
				{
					mod.alignment = TextAnchor.LowerRight;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private struct StyleMod
		{
			public readonly AuthorShared.Styles.StyleModFunctor functor;

			private GUIStyle original;

			private GUIStyle modified;

			public StyleMod(AuthorShared.Styles.StyleModFunctor functor)
			{
				this.functor = functor;
				object obj = null;
				GUIStyle gUIStyle = (GUIStyle)obj;
				this.modified = (GUIStyle)obj;
				this.original = gUIStyle;
			}

			public GUIStyle GetStyle(GUIStyle original)
			{
				if (original == null)
				{
					return null;
				}
				if (this.original != original)
				{
					this.original = original;
					this.modified = new GUIStyle(original);
					try
					{
						this.functor(original, ref this.modified);
						this.modified = this.modified ?? this.original;
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(exception);
					}
				}
				return this.modified;
			}
		}

		private delegate void StyleModFunctor(GUIStyle original, ref GUIStyle mod);
	}
}