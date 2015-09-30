using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class NGUITools
{
	public const float kMinimumAlpha = 0.00196078443f;

	public const float kMaximumNegativeAlpha = -0.00196078443f;

	public const string kFormattingOffDisableSymbol = "[«]";

	public const string kFormattingOffEnableSymbol = "[»]";

	public const char kFormattingOffDisableCharacter = '«';

	public const char kFormattingOffEnableCharacter = '»';

	private static AudioListener mListener;

	private static bool mLoaded;

	private static float mGlobalVolume;

	private readonly static string[] kFormattingOffSymbols;

	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	static NGUITools()
	{
		NGUITools.mLoaded = false;
		NGUITools.mGlobalVolume = 1f;
		NGUITools.kFormattingOffSymbols = new string[] { "[»]", "[«]" };
	}

	private static void Activate(Transform t)
	{
		t.gameObject.SetActive(true);
	}

	public static GameObject AddChild(GameObject parent)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transforms = gameObject.transform;
			transforms.parent = parent.transform;
			transforms.localPosition = Vector3.zero;
			transforms.localRotation = Quaternion.identity;
			transforms.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transforms = gameObject.transform;
			transforms.parent = parent.transform;
			transforms.localPosition = Vector3.zero;
			transforms.localRotation = Quaternion.identity;
			transforms.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static T AddChild<T>(GameObject parent)
	where T : Component
	{
		GameObject name = NGUITools.AddChild(parent);
		name.name = NGUITools.GetName<T>();
		return name.AddComponent<T>();
	}

	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UIAtlas.Sprite sprite;
		UISprite uISprite;
		if (atlas == null)
		{
			sprite = null;
		}
		else
		{
			sprite = atlas.GetSprite(spriteName);
		}
		UIAtlas.Sprite sprite1 = sprite;
		if (sprite1 == null || sprite1.inner == sprite1.outer)
		{
			uISprite = NGUITools.AddWidget<UISprite>(go);
		}
		else
		{
			uISprite = NGUITools.AddWidget<UISlicedSprite>(go);
		}
		UISprite uISprite1 = uISprite;
		uISprite1.atlas = atlas;
		uISprite1.spriteName = spriteName;
		return uISprite1;
	}

	public static T AddWidget<T>(GameObject go)
	where T : UIWidget
	{
		int num = NGUITools.CalculateNextDepth(go);
		T t = NGUITools.AddChild<T>(go);
		t.depth = num;
		Transform vector3 = t.transform;
		vector3.localPosition = Vector3.zero;
		vector3.localRotation = Quaternion.identity;
		vector3.localScale = new Vector3(100f, 100f, 1f);
		t.gameObject.layer = go.layer;
		return t;
	}

	[Obsolete("Use AddWidgetHotSpot")]
	public static BoxCollider AddWidgetCollider(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		Collider component = go.GetComponent<Collider>();
		BoxCollider vector3 = component as BoxCollider;
		if (vector3 == null)
		{
			if (component != null)
			{
				if (!Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(component);
				}
				else
				{
					UnityEngine.Object.Destroy(component);
				}
			}
			vector3 = go.AddComponent<BoxCollider>();
		}
		int num = NGUITools.CalculateNextDepth(go);
		AABBox aABBox = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
		vector3.isTrigger = true;
		vector3.center = aABBox.center + (Vector3.back * ((float)num * 0.25f));
		float single = aABBox.size.x;
		Vector3 vector31 = aABBox.size;
		vector3.size = new Vector3(single, vector31.y, 0f);
		return vector3;
	}

	public static UIHotSpot AddWidgetHotSpot(GameObject go)
	{
		int num;
		AABBox aABBox;
		if (go == null)
		{
			return null;
		}
		Collider collider = go.collider;
		if (collider)
		{
			UIHotSpot hotSpot = NGUITools.ColliderToHotSpot(collider, true);
			if (!hotSpot)
			{
				return null;
			}
			return hotSpot;
		}
		UIHotSpot component = go.GetComponent<UIHotSpot>();
		if (component)
		{
			if (component.isRect)
			{
				UIRectHotSpot uIRectHotSpot = component.asRect;
				num = NGUITools.CalculateNextDepth(go);
				aABBox = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
				uIRectHotSpot.size = aABBox.size;
				uIRectHotSpot.center = aABBox.center + (Vector3.back * ((float)num * 0.25f));
				return uIRectHotSpot;
			}
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
			else
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		num = NGUITools.CalculateNextDepth(go);
		aABBox = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
		UIRectHotSpot uIRectHotSpot1 = go.AddComponent<UIRectHotSpot>();
		uIRectHotSpot1.size = aABBox.size;
		uIRectHotSpot1.center = aABBox.center + (Vector3.back * ((float)num * 0.25f));
		return uIRectHotSpot1;
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] gameObjectArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int num = 0;
		int length = (int)gameObjectArray.Length;
		while (num < length)
		{
			gameObjectArray[num].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			num++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] gameObjectArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int num = 0;
		int length = (int)gameObjectArray.Length;
		while (num < length)
		{
			gameObjectArray[num].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			num++;
		}
	}

	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int num1 = 0;
		int length = (int)componentsInChildren.Length;
		while (num1 < length)
		{
			num = Mathf.Max(num, componentsInChildren[num1].depth);
			num1++;
		}
		return num + 1;
	}

	private static void ColliderDestroy(Collider component)
	{
		if (!Application.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(component);
		}
		else
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	private static UIHotSpot ColliderToHotSpot(BoxCollider collider, bool nullChecked)
	{
		if (!nullChecked && !collider)
		{
			return null;
		}
		Vector3 vector3 = collider.center;
		Vector3 vector31 = collider.size;
		GameObject gameObject = collider.gameObject;
		bool flag = collider.enabled;
		NGUITools.ColliderDestroy(collider);
		if (vector31.z > 0.001f)
		{
			UIBoxHotSpot uIBoxHotSpot = gameObject.AddComponent<UIBoxHotSpot>();
			uIBoxHotSpot.center = vector3;
			uIBoxHotSpot.size = vector31;
			uIBoxHotSpot.enabled = flag;
			return uIBoxHotSpot;
		}
		UIRectHotSpot uIRectHotSpot = gameObject.AddComponent<UIRectHotSpot>();
		uIRectHotSpot.center = vector3;
		uIRectHotSpot.size = vector31;
		uIRectHotSpot.enabled = flag;
		return uIRectHotSpot;
	}

	public static UIHotSpot ColliderToHotSpot(BoxCollider collider)
	{
		return NGUITools.ColliderToHotSpot(collider, false);
	}

	private static UIHotSpot ColliderToHotSpot(Collider collider, bool nullChecked)
	{
		Bounds bound;
		if (!nullChecked && !collider)
		{
			return null;
		}
		if (collider is BoxCollider)
		{
			return NGUITools.ColliderToHotSpot((BoxCollider)collider);
		}
		if (collider is SphereCollider)
		{
			return NGUITools.ColliderToHotSpot((SphereCollider)collider);
		}
		if (collider is TerrainCollider)
		{
			Debug.Log("Sorry not going to convert a terrain collider.. that sounds destructive.", collider);
			return null;
		}
		Bounds bound1 = collider.bounds;
		Matrix4x4 matrix4x4 = collider.transform.worldToLocalMatrix;
		AABBox.Transform3x4(ref bound1, ref matrix4x4, out bound);
		bool flag = collider.enabled;
		GameObject gameObject = collider.gameObject;
		NGUITools.ColliderDestroy(collider);
		Vector3 vector3 = bound.size;
		if (vector3.z > 0.001f)
		{
			UIBoxHotSpot uIBoxHotSpot = gameObject.AddComponent<UIBoxHotSpot>();
			uIBoxHotSpot.size = vector3;
			uIBoxHotSpot.center = bound.center;
			uIBoxHotSpot.enabled = flag;
			return uIBoxHotSpot;
		}
		UIRectHotSpot uIRectHotSpot = gameObject.AddComponent<UIRectHotSpot>();
		uIRectHotSpot.size = vector3;
		uIRectHotSpot.center = bound.center;
		uIRectHotSpot.enabled = flag;
		return uIRectHotSpot;
	}

	public static UIHotSpot ColliderToHotSpot(Collider collider)
	{
		return NGUITools.ColliderToHotSpot(collider, false);
	}

	private static UIBoxHotSpot ColliderToHotSpotBox(BoxCollider collider, bool nullChecked)
	{
		if (!nullChecked && !collider)
		{
			return null;
		}
		Vector3 vector3 = collider.center;
		Vector3 vector31 = collider.size;
		GameObject gameObject = collider.gameObject;
		bool flag = collider.enabled;
		NGUITools.ColliderDestroy(collider);
		UIBoxHotSpot uIBoxHotSpot = gameObject.AddComponent<UIBoxHotSpot>();
		uIBoxHotSpot.center = vector3;
		uIBoxHotSpot.size = vector31;
		uIBoxHotSpot.enabled = flag;
		return uIBoxHotSpot;
	}

	public static UIBoxHotSpot ColliderToHotSpotBox(BoxCollider collider)
	{
		return NGUITools.ColliderToHotSpotBox(collider, false);
	}

	private static UIRectHotSpot ColliderToHotSpotRect(BoxCollider collider, bool nullChecked)
	{
		if (!nullChecked && !collider)
		{
			return null;
		}
		Vector3 vector3 = collider.center;
		Vector2 vector2 = collider.size;
		GameObject gameObject = collider.gameObject;
		bool flag = collider.enabled;
		NGUITools.ColliderDestroy(collider);
		UIRectHotSpot uIRectHotSpot = gameObject.AddComponent<UIRectHotSpot>();
		uIRectHotSpot.center = vector3;
		uIRectHotSpot.size = vector2;
		uIRectHotSpot.enabled = flag;
		return uIRectHotSpot;
	}

	public static UIRectHotSpot ColliderToHotSpotRect(BoxCollider collider)
	{
		return NGUITools.ColliderToHotSpotRect(collider, false);
	}

	private static void Deactivate(Transform t)
	{
		t.gameObject.SetActive(false);
	}

	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (!Application.isEditor)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	public static string EncodeColor(Color c)
	{
		int num = 16777215 & NGUIMath.ColorToInt(c) >> 8;
		return num.ToString("X6");
	}

	public static T[] FindActive<T>()
	where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	}

	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << (layer & 31);
		Camera[] cameraArray = NGUITools.FindActive<Camera>();
		int num1 = 0;
		int length = (int)cameraArray.Length;
		while (num1 < length)
		{
			Camera camera = cameraArray[num1];
			if ((camera.cullingMask & num) != 0)
			{
				return camera;
			}
			num1++;
		}
		return null;
	}

	public static T FindInParents<T>(GameObject go)
	where T : Component
	{
		if (go == null)
		{
			return (T)null;
		}
		object component = go.GetComponent<T>();
		if (component == null)
		{
			for (Transform i = go.transform.parent; i != null && component == null; i = i.parent)
			{
				component = i.gameObject.GetComponent<T>();
			}
		}
		return (T)component;
	}

	public static bool GetAllowClick(MonoBehaviour self, out bool possible)
	{
		Collider collider = self.collider;
		if (collider)
		{
			possible = true;
			return collider.enabled;
		}
		UIHotSpot component = self.GetComponent<UIHotSpot>();
		if (!component)
		{
			possible = false;
			return false;
		}
		possible = true;
		return component.enabled;
	}

	public static bool GetAllowClick(MonoBehaviour self)
	{
		bool flag;
		return NGUITools.GetAllowClick(self, out flag);
	}

	public static bool GetCentroid(Component cell, out Vector3 centroid)
	{
		if (!(cell is Collider))
		{
			if (!(cell is UIHotSpot))
			{
				UIHotSpot component = cell.GetComponent<UIHotSpot>();
				if (component)
				{
					centroid = component.worldCenter;
					return true;
				}
				Collider collider = cell.collider;
				if (!collider)
				{
					centroid = Vector3.zero;
					return false;
				}
				centroid = collider.bounds.center;
				return true;
			}
			centroid = ((UIHotSpot)cell).worldCenter;
		}
		else
		{
			centroid = ((Collider)cell).bounds.center;
		}
		return true;
	}

	public static string GetHierarchy(GameObject obj)
	{
		string str = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			str = string.Concat(obj.name, "/", str);
		}
		return string.Concat("\"", str, "\"");
	}

	public static string GetName<T>()
	where T : Component
	{
		string str = typeof(T).ToString();
		if (str.StartsWith("UI"))
		{
			str = str.Substring(2);
		}
		else if (str.StartsWith("UnityEngine."))
		{
			str = str.Substring(12);
		}
		return str;
	}

	public static TComponent GetOrAddComponent<TComponent>(GameObject gameObject)
	where TComponent : Component
	{
		TComponent tComponent = NGUITools.QuickGet<TComponent>(gameObject);
		return (!tComponent ? gameObject.AddComponent<TComponent>() : tComponent);
	}

	public static TComponent GetOrAddComponent<TComponent>(Component component)
	where TComponent : Component
	{
		if (component is TComponent)
		{
			return (TComponent)component;
		}
		return NGUITools.GetOrAddComponent<TComponent>(component.gameObject);
	}

	public static bool GetOrAddComponent<TComponent>(GameObject gameObject, ref TComponent value)
	where TComponent : Component
	{
		TComponent tComponent;
		if (!value)
		{
			TComponent orAddComponent = NGUITools.GetOrAddComponent<TComponent>(gameObject);
			TComponent tComponent1 = orAddComponent;
			value = orAddComponent;
			tComponent = tComponent1;
		}
		else
		{
			tComponent = value;
		}
		return tComponent;
	}

	public static bool GetOrAddComponent<TComponent>(Component component, ref TComponent value)
	where TComponent : Component
	{
		TComponent tComponent;
		if (!value)
		{
			TComponent orAddComponent = NGUITools.GetOrAddComponent<TComponent>(component);
			TComponent tComponent1 = orAddComponent;
			value = orAddComponent;
			tComponent = tComponent1;
		}
		else
		{
			tComponent = value;
		}
		return tComponent;
	}

	public static bool HasMeansOfClicking(Component self)
	{
		bool component;
		if (self.collider)
		{
			component = true;
		}
		else
		{
			component = self.GetComponent<UIHotSpot>();
		}
		return component;
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
			int num = 0;
			int num1 = t.childCount;
			while (num < num1)
			{
				NGUITools.MakePixelPerfect(t.GetChild(num));
				num++;
			}
		}
		else
		{
			component.MakePixelPerfect();
		}
	}

	public static WWW OpenURL(string url)
	{
		WWW wWW = null;
		try
		{
			wWW = new WWW(url);
		}
		catch (Exception exception)
		{
			Debug.LogError(exception.Message);
		}
		return wWW;
	}

	public static Color ParseColor(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text[offset]) << 4 | NGUIMath.HexToDecimal(text[offset + 1]);
		int num1 = NGUIMath.HexToDecimal(text[offset + 2]) << 4 | NGUIMath.HexToDecimal(text[offset + 3]);
		int num2 = NGUIMath.HexToDecimal(text[offset + 4]) << 4 | NGUIMath.HexToDecimal(text[offset + 5]);
		float single = 0.003921569f;
		return new Color(single * (float)num, single * (float)num1, single * (float)num2);
	}

	public static int ParseSymbol(string text, int index, List<Color> colors, ref int symbolSkipCount)
	{
		int num;
		int length = text.Length;
		if (index + 2 < length)
		{
			if (text[index + 2] != ']')
			{
				if (index + 7 < length && text[index + 7] == ']' && symbolSkipCount == 0)
				{
					if (colors != null)
					{
						Color color = NGUITools.ParseColor(text, index + 1);
						Color item = colors[colors.Count - 1];
						color.a = item.a;
						colors.Add(color);
					}
					return 8;
				}
			}
			else if (text[index + 1] == '-')
			{
				if (symbolSkipCount == 0)
				{
					if (colors != null && colors.Count > 1)
					{
						colors.RemoveAt(colors.Count - 1);
					}
					return 3;
				}
			}
			else if (text[index + 1] == '»')
			{
				int num1 = symbolSkipCount;
				num = num1;
				symbolSkipCount = num1 + 1;
				if (num == 0)
				{
					return 3;
				}
			}
			else if (text[index + 1] == '«')
			{
				int num2 = symbolSkipCount - 1;
				num = num2;
				symbolSkipCount = num2;
				if (num == 0)
				{
					return 3;
				}
			}
		}
		return 0;
	}

	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		volume = volume * NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null)
			{
				NGUITools.mListener = UnityEngine.Object.FindObjectOfType(typeof(AudioListener)) as AudioListener;
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera;
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null)
			{
				AudioSource audioSource = NGUITools.mListener.audio;
				if (audioSource == null)
				{
					audioSource = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
				}
				audioSource.pitch = pitch;
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	public static TComponent QuickGet<TComponent>(GameObject gameObject)
	where TComponent : Component
	{
		switch (NGUITools.SG<TComponent>.V)
		{
			case NGUITools.SlipGate.Renderer:
			{
				return (TComponent)((object)gameObject.renderer as TComponent);
			}
			case NGUITools.SlipGate.Collider:
			{
				return (TComponent)((object)gameObject.collider as TComponent);
			}
			case NGUITools.SlipGate.Behaviour:
			{
				return gameObject.GetComponent<TComponent>();
			}
			case NGUITools.SlipGate.Transform:
			{
				return (TComponent)((object)gameObject.transform as TComponent);
			}
			default:
			{
				return gameObject.GetComponent<TComponent>();
			}
		}
	}

	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	[Obsolete("Use UIAtlas.replacement instead")]
	public static void ReplaceAtlas(UIAtlas before, UIAtlas after)
	{
		UISprite[] uISpriteArray = NGUITools.FindActive<UISprite>();
		int num = 0;
		int length = (int)uISpriteArray.Length;
		while (num < length)
		{
			UISprite uISprite = uISpriteArray[num];
			if (uISprite.atlas == before)
			{
				uISprite.atlas = after;
			}
			num++;
		}
		UILabel[] uILabelArray = NGUITools.FindActive<UILabel>();
		int num1 = 0;
		int length1 = (int)uILabelArray.Length;
		while (num1 < length1)
		{
			UILabel uILabel = uILabelArray[num1];
			if (uILabel.font != null && uILabel.font.atlas == before)
			{
				uILabel.font.atlas = after;
			}
			num1++;
		}
	}

	[Obsolete("Use UIFont.replacement instead")]
	public static void ReplaceFont(UIFont before, UIFont after)
	{
		UILabel[] uILabelArray = NGUITools.FindActive<UILabel>();
		int num = 0;
		int length = (int)uILabelArray.Length;
		while (num < length)
		{
			UILabel uILabel = uILabelArray[num];
			if (uILabel.font == before)
			{
				uILabel.font = after;
			}
			num++;
		}
	}

	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	public static void SetActive(GameObject go, bool state)
	{
		if (!state)
		{
			NGUITools.Deactivate(go.transform);
		}
		else
		{
			NGUITools.Activate(go.transform);
		}
	}

	public static bool SetAllowClick(Component self, bool allow)
	{
		Collider collider = self.collider;
		if (collider)
		{
			collider.enabled = allow;
			return true;
		}
		UIHotSpot component = self.GetComponent<UIHotSpot>();
		if (!component)
		{
			return false;
		}
		component.enabled = allow;
		return true;
	}

	public static void SetAllowClickChildren(GameObject mChild, bool par1)
	{
		Collider[] componentsInChildren = mChild.GetComponentsInChildren<Collider>();
		int num = 0;
		int length = (int)componentsInChildren.Length;
		while (num < length)
		{
			componentsInChildren[num].enabled = false;
			num++;
		}
		UIHotSpot[] uIHotSpotArray = mChild.GetComponentsInChildren<UIHotSpot>();
		int num1 = 0;
		int length1 = (int)uIHotSpotArray.Length;
		while (num1 < length1)
		{
			uIHotSpotArray[num1].enabled = false;
			num1++;
		}
	}

	public static string StripSymbols(string text)
	{
		if (text != null)
		{
			text = text.Replace("\\n", "\n");
			int num = 0;
			int num1 = 0;
			int length = text.Length;
			while (num1 < length)
			{
				if (text[num1] == '[')
				{
					int num2 = NGUITools.ParseSymbol(text, num1, null, ref num);
					if (num2 > 0)
					{
						text = text.Remove(num1, num2);
						length = text.Length;
						continue;
					}
				}
				num1++;
			}
		}
		return text;
	}

	public static string UnformattedString(string str)
	{
		int num = str.IndexOf("[»]");
		int num1 = str.IndexOf("[«]");
		if (num == -1)
		{
			if (num1 == -1)
			{
				return string.Concat("[»]", str, "[«]");
			}
			int num2 = 1;
			while (true)
			{
				int num3 = num1 + 1;
				num1 = num3;
				if (num3 >= str.Length)
				{
					break;
				}
				num1 = str.IndexOf("[«]", num1);
				if (num1 != -1)
				{
					num2++;
				}
				else
				{
					break;
				}
			}
			if (num2 == 1)
			{
				return string.Concat("[»]", "[»]", str, "[«]");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[»]");
			while (true)
			{
				int num4 = num2;
				num2 = num4 - 1;
				if (num4 <= 0)
				{
					break;
				}
				stringBuilder.Append("[»]");
			}
			stringBuilder.Append(str);
			stringBuilder.Append("[«]");
			return stringBuilder.ToString();
		}
		if (num1 == -1)
		{
			int num5 = 1;
			while (true)
			{
				int num6 = num + 1;
				num = num6;
				if (num6 >= str.Length)
				{
					break;
				}
				num = str.IndexOf("[«]", num);
				if (num != -1)
				{
					num5++;
				}
				else
				{
					break;
				}
			}
			if (num5 == 1)
			{
				return string.Concat("[»]", str, "[«]", "[«]");
			}
			StringBuilder stringBuilder1 = new StringBuilder();
			stringBuilder1.Append("[»]");
			stringBuilder1.Append(str);
			while (true)
			{
				int num7 = num5;
				num5 = num7 - 1;
				if (num7 <= 0)
				{
					break;
				}
				stringBuilder1.Append("[«]");
			}
			stringBuilder1.Append("[«]");
			return stringBuilder1.ToString();
		}
		List<int> nums = new List<int>();
		List<bool> flags = new List<bool>();
		nums.Add(num);
		nums.Add(num1);
		flags.Add(true);
		flags.Add(false);
		while (true)
		{
			int num8 = num + 1;
			num = num8;
			if (num8 >= str.Length)
			{
				break;
			}
			num = str.IndexOf("[«]", num);
			if (num != -1)
			{
				nums.Add(num);
				flags.Add(true);
			}
			else
			{
				break;
			}
		}
		while (true)
		{
			int num9 = num1 + 1;
			num1 = num9;
			if (num9 >= str.Length)
			{
				break;
			}
			num1 = str.IndexOf("[«]", num1);
			if (num1 != -1)
			{
				nums.Add(num1);
				flags.Add(false);
			}
			else
			{
				break;
			}
		}
		bool[] array = flags.ToArray();
		Array.Sort<int, bool>(nums.ToArray(), array);
		int num10 = 0;
		int num11 = 0;
		for (int i = 0; i < (int)array.Length; i++)
		{
			if (!array[i])
			{
				num11++;
				while (true)
				{
					int num12 = i + 1;
					i = num12;
					if (num12 >= (int)array.Length)
					{
						goto Label0;
					}
					if (!array[i])
					{
						num11++;
					}
					else
					{
						int num13 = num11 - 1;
						num11 = num13;
						if (num13 == 0)
						{
							break;
						}
					}
				}
			}
			else
			{
				num10++;
				while (true)
				{
					int num14 = i + 1;
					i = num14;
					if (num14 >= (int)array.Length)
					{
						break;
					}
					if (!array[i])
					{
						int num15 = num10 - 1;
						num10 = num15;
						if (num15 == 0)
						{
							break;
						}
					}
					else
					{
						num10++;
					}
				}
			}
		Label0:
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append("[»]");
		for (int j = 0; j < num11; j++)
		{
			stringBuilder2.Append("[»]");
		}
		stringBuilder2.Append(str);
		for (int k = 0; k < num10; k++)
		{
			stringBuilder2.Append("[«]");
		}
		stringBuilder2.Append("[«]");
		return stringBuilder2.ToString();
	}

	public static bool ZeroAlpha(float alpha)
	{
		return (alpha >= 0f ? alpha < 0.00196078443f : alpha > -0.00196078443f);
	}

	private static class SG<T>
	where T : Component
	{
		public readonly static NGUITools.SlipGate V;

		static SG()
		{
			if (typeof(Renderer).IsAssignableFrom(typeof(T)))
			{
				NGUITools.SG<T>.V = NGUITools.SlipGate.Renderer;
			}
			else if (typeof(Collider).IsAssignableFrom(typeof(T)))
			{
				NGUITools.SG<T>.V = NGUITools.SlipGate.Collider;
			}
			else if (typeof(Behaviour).IsAssignableFrom(typeof(T)))
			{
				NGUITools.SG<T>.V = NGUITools.SlipGate.Behaviour;
			}
			else if (!typeof(Transform).IsAssignableFrom(typeof(T)))
			{
				NGUITools.SG<T>.V = NGUITools.SlipGate.Component;
			}
			else
			{
				NGUITools.SG<T>.V = NGUITools.SlipGate.Transform;
			}
		}
	}

	private enum SlipGate
	{
		Renderer,
		Collider,
		Behaviour,
		Transform,
		Component
	}
}