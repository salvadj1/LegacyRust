using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIMaterial : ScriptableObject
{
	private const UIDrawCall.Clipping kBeginClipping = UIDrawCall.Clipping.None;

	private const UIDrawCall.Clipping kEndClipping = 4;

	private const string hard = " (HardClip)";

	private const string alpha = " (AlphaClip)";

	private const string soft = " (SoftClip)";

	private Material key;

	private Material matNone;

	private Material matHardClip;

	private Material matAlphaClip;

	private Material matSoftClip;

	private Material matFirst;

	private int hashCode;

	private UIMaterial.ClippingFlags madeMats;

	public Material this[UIDrawCall.Clipping clipping]
	{
		get
		{
			UIMaterial.ClippingFlags clippingFlag = (UIMaterial.ClippingFlags)((int)UIDrawCall.Clipping.HardClip << (int)(clipping & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip)));
			if ((clippingFlag & this.madeMats) != clippingFlag)
			{
				return this.MakeMaterial(clipping);
			}
			switch (clipping)
			{
				case UIDrawCall.Clipping.None:
				{
					return this.matNone;
				}
				case UIDrawCall.Clipping.HardClip:
				{
					return this.matHardClip;
				}
				case UIDrawCall.Clipping.AlphaClip:
				{
					return this.matAlphaClip;
				}
				case UIDrawCall.Clipping.SoftClip:
				{
					return this.matSoftClip;
				}
			}
			throw new NotImplementedException();
		}
	}

	public Texture mainTexture
	{
		get
		{
			return ((int)this.madeMats != 0 ? this.matFirst.mainTexture : this.key.mainTexture);
		}
		set
		{
			if ((int)this.madeMats == 0)
			{
				this.MakeDefaultMaterial();
			}
			this.Set("_MainTex", value);
		}
	}

	public UIMaterial()
	{
	}

	public UIMaterial Clone()
	{
		Material material = new Material(this.key)
		{
			hideFlags = HideFlags.DontSave
		};
		return UIMaterial.Create(material, true);
	}

	public void CopyPropertiesFromMaterial(Material material)
	{
		if ((int)this.madeMats == 0)
		{
			if (material == this.key)
			{
				return;
			}
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).CopyPropertiesFromMaterial(material);
			}
		}
	}

	public void CopyPropertiesFromOriginal()
	{
		if ((int)this.madeMats != 0)
		{
			this.CopyPropertiesFromMaterial(this.key);
		}
	}

	public static UIMaterial Create(Material key)
	{
		UIMaterial uIMaterial;
		if (!key)
		{
			return null;
		}
		if (UIMaterial.g.keyedMaterials.TryGetValue(key, out uIMaterial))
		{
			return uIMaterial;
		}
		if (UIMaterial.g.generatedMaterials.TryGetValue(key, out uIMaterial))
		{
			return uIMaterial;
		}
		uIMaterial = ScriptableObject.CreateInstance<UIMaterial>();
		uIMaterial.key = key;
		int num = UIMaterial.g.hashCodeIterator + 1;
		UIMaterial.g.hashCodeIterator = num;
		uIMaterial.hashCode = num;
		if (uIMaterial.hashCode == 2147483647)
		{
			UIMaterial.g.hashCodeIterator = -2147483648;
		}
		UIMaterial.g.keyedMaterials.Add(key, uIMaterial);
		return uIMaterial;
	}

	public static UIMaterial Create(Material key, bool manageKeyDestruction, UIDrawCall.Clipping useAsClipping)
	{
		UIMaterial uIMaterial;
		if (!manageKeyDestruction)
		{
			return UIMaterial.Create(key);
		}
		if (!key)
		{
			return null;
		}
		if (UIMaterial.g.keyedMaterials.TryGetValue(key, out uIMaterial))
		{
			throw new InvalidOperationException("That material is registered and cannot be used with manageKeyDestruction");
		}
		if (UIMaterial.g.generatedMaterials.TryGetValue(key, out uIMaterial))
		{
			return uIMaterial;
		}
		uIMaterial = ScriptableObject.CreateInstance<UIMaterial>();
		uIMaterial.key = key;
		int num = UIMaterial.g.hashCodeIterator + 1;
		UIMaterial.g.hashCodeIterator = num;
		uIMaterial.hashCode = num;
		if (uIMaterial.hashCode == 2147483647)
		{
			UIMaterial.g.hashCodeIterator = -2147483648;
		}
		UIMaterial.g.generatedMaterials.Add(key, uIMaterial);
		uIMaterial.matFirst = key;
		switch (useAsClipping)
		{
			case UIDrawCall.Clipping.None:
			{
				uIMaterial.matNone = key;
				break;
			}
			case UIDrawCall.Clipping.HardClip:
			{
				uIMaterial.matHardClip = key;
				break;
			}
			case UIDrawCall.Clipping.AlphaClip:
			{
				uIMaterial.matAlphaClip = key;
				break;
			}
			case UIDrawCall.Clipping.SoftClip:
			{
				uIMaterial.matSoftClip = key;
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
		uIMaterial.madeMats = (UIMaterial.ClippingFlags)((int)UIDrawCall.Clipping.HardClip << (int)(useAsClipping & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip)));
		return uIMaterial;
	}

	public static UIMaterial Create(Material key, bool manageKeyDestruction)
	{
		return UIMaterial.Create(key, manageKeyDestruction, UIDrawCall.Clipping.None);
	}

	private static Material CreateMaterial(Shader shader)
	{
		return new Material(shader)
		{
			hideFlags = HideFlags.DontSave | HideFlags.NotEditable
		};
	}

	private Material FastGet(UIDrawCall.Clipping clipping)
	{
		switch (clipping)
		{
			case UIDrawCall.Clipping.None:
			{
				return this.matNone;
			}
			case UIDrawCall.Clipping.HardClip:
			{
				return this.matHardClip;
			}
			case UIDrawCall.Clipping.AlphaClip:
			{
				return this.matAlphaClip;
			}
			case UIDrawCall.Clipping.SoftClip:
			{
				return this.matSoftClip;
			}
		}
		throw new NotImplementedException();
	}

	private static Shader GetClippingShader(Shader original, UIDrawCall.Clipping clipping)
	{
		if (!original)
		{
			return null;
		}
		string str = original.name;
		switch (clipping)
		{
			case UIDrawCall.Clipping.None:
			{
				string str1 = str.Replace(" (HardClip)", string.Empty).Replace(" (AlphaClip)", string.Empty).Replace(" (SoftClip)", string.Empty);
				if (str1 == str)
				{
					return original;
				}
				str = str1;
				break;
			}
			case UIDrawCall.Clipping.HardClip:
			{
				if (!UIMaterial.ShaderNameDecor(ref str, " (AlphaClip)", " (SoftClip)", " (HardClip)"))
				{
					return original;
				}
				break;
			}
			case UIDrawCall.Clipping.AlphaClip:
			{
				if (!UIMaterial.ShaderNameDecor(ref str, " (SoftClip)", " (HardClip)", " (AlphaClip)"))
				{
					return original;
				}
				break;
			}
			case UIDrawCall.Clipping.SoftClip:
			{
				if (!UIMaterial.ShaderNameDecor(ref str, " (HardClip)", " (AlphaClip)", " (SoftClip)"))
				{
					return original;
				}
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
		Shader shader = Shader.Find(str);
		if (!shader)
		{
			throw new MissingReferenceException(string.Concat("Theres no shader named ", str));
		}
		return shader;
	}

	public sealed override int GetHashCode()
	{
		return this.hashCode;
	}

	public bool HasProperty(string property)
	{
		if ((int)this.madeMats == 0)
		{
			return this.key.HasProperty(property);
		}
		return this.matFirst.HasProperty(property);
	}

	private void MakeDefaultMaterial()
	{
		this.MakeMaterial(UIMaterial.ShaderClipping(this.key.shader.name));
	}

	private Material MakeMaterial(UIDrawCall.Clipping clipping)
	{
		Material material;
		Material material1;
		Shader clippingShader;
		Material material2;
		bool flag = (int)this.madeMats == 0;
		switch (clipping)
		{
			case UIDrawCall.Clipping.None:
			{
				clippingShader = this.key.shader;
				material1 = this.matNone;
				Material material3 = UIMaterial.CreateMaterial(clippingShader);
				material2 = material3;
				this.matNone = material3;
				material = material2;
				UIMaterial uIMaterial = this;
				uIMaterial.madeMats = uIMaterial.madeMats | UIMaterial.ClippingFlags.None;
				break;
			}
			case UIDrawCall.Clipping.HardClip:
			{
				clippingShader = UIMaterial.GetClippingShader(this.key.shader, UIDrawCall.Clipping.HardClip);
				material1 = this.matHardClip;
				Material material4 = UIMaterial.CreateMaterial(clippingShader);
				material2 = material4;
				this.matHardClip = material4;
				material = material2;
				UIMaterial uIMaterial1 = this;
				uIMaterial1.madeMats = uIMaterial1.madeMats | UIMaterial.ClippingFlags.HardClip;
				break;
			}
			case UIDrawCall.Clipping.AlphaClip:
			{
				clippingShader = UIMaterial.GetClippingShader(this.key.shader, UIDrawCall.Clipping.AlphaClip);
				material1 = this.matAlphaClip;
				Material material5 = UIMaterial.CreateMaterial(clippingShader);
				material2 = material5;
				this.matAlphaClip = material5;
				material = material2;
				UIMaterial uIMaterial2 = this;
				uIMaterial2.madeMats = uIMaterial2.madeMats | UIMaterial.ClippingFlags.AlphaClip;
				break;
			}
			case UIDrawCall.Clipping.SoftClip:
			{
				clippingShader = UIMaterial.GetClippingShader(this.key.shader, UIDrawCall.Clipping.SoftClip);
				material1 = this.matSoftClip;
				Material material6 = UIMaterial.CreateMaterial(clippingShader);
				material2 = material6;
				this.matSoftClip = material6;
				material = material2;
				UIMaterial uIMaterial3 = this;
				uIMaterial3.madeMats = uIMaterial3.madeMats | UIMaterial.ClippingFlags.SoftClip;
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
		UIMaterial.g.generatedMaterials.Add(material, this);
		if (!flag)
		{
			material.CopyPropertiesFromMaterial(this.matFirst);
		}
		else
		{
			this.matFirst = material;
			material.CopyPropertiesFromMaterial(this.key);
		}
		if (material1)
		{
			UnityEngine.Object.DestroyImmediate(material1);
		}
		return material;
	}

	private void OnDestroy()
	{
		if ((int)this.madeMats != 0)
		{
			for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
			{
				if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
				{
					Material material = this.FastGet(i);
					UIMaterial.g.generatedMaterials.Remove(material);
					UnityEngine.Object.DestroyImmediate(material);
				}
			}
		}
		UIMaterial.g.keyedMaterials.Remove(this.key);
		object obj = null;
		Material material1 = (Material)obj;
		this.key = (Material)obj;
		Material material2 = material1;
		material1 = material2;
		this.matAlphaClip = material2;
		Material material3 = material1;
		material1 = material3;
		this.matSoftClip = material3;
		Material material4 = material1;
		material1 = material4;
		this.matHardClip = material4;
		Material material5 = material1;
		material1 = material5;
		this.matFirst = material5;
		this.matNone = material1;
	}

	public static explicit operator UIMaterial(Material key)
	{
		return UIMaterial.Create(key);
	}

	public static explicit operator Material(UIMaterial uimat)
	{
		Material material;
		if (!uimat)
		{
			material = null;
		}
		else
		{
			material = uimat.key;
		}
		return material;
	}

	public void Set(string property, float value)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetFloat(property, value);
			}
		}
	}

	public void Set(string property, Vector2 value)
	{
		Vector4 vector4 = new Vector4();
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		vector4.x = value.x;
		vector4.y = value.y;
		float single = 0f;
		float single1 = single;
		vector4.w = single;
		vector4.z = single1;
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetVector(property, vector4);
			}
		}
	}

	public void Set(string property, Vector3 value)
	{
		Vector4 vector4 = new Vector4();
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		vector4.x = value.x;
		vector4.y = value.y;
		float single = 0f;
		float single1 = single;
		vector4.w = single;
		vector4.z = single1;
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetVector(property, vector4);
			}
		}
	}

	public void Set(string property, Vector4 value)
	{
		Vector4 vector4 = new Vector4();
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		vector4.x = value.x;
		vector4.y = value.y;
		float single = 0f;
		float single1 = single;
		vector4.w = single;
		vector4.z = single1;
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetVector(property, vector4);
			}
		}
	}

	public void Set(string property, Color color)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetColor(property, color);
			}
		}
	}

	public void Set(string property, Matrix4x4 mat)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetMatrix(property, mat);
			}
		}
	}

	public void Set(string property, Texture texture)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetTexture(property, texture);
			}
		}
	}

	public void SetPass(int pass)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetPass(pass);
			}
		}
	}

	public void SetTextureOffset(string property, Vector2 offset)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetTextureOffset(property, offset);
			}
		}
	}

	public void SetTextureScale(string property, Vector2 scale)
	{
		if ((int)this.madeMats == 0)
		{
			this.MakeDefaultMaterial();
		}
		for (UIDrawCall.Clipping i = UIDrawCall.Clipping.None; (int)i < 4; i = (UIDrawCall.Clipping)((int)i + (int)UIDrawCall.Clipping.HardClip))
		{
			if (((int)this.madeMats & (int)UIDrawCall.Clipping.HardClip << (int)(i & (UIDrawCall.Clipping.HardClip | UIDrawCall.Clipping.AlphaClip | UIDrawCall.Clipping.SoftClip))) != (int)UIDrawCall.Clipping.None)
			{
				this.FastGet(i).SetTextureScale(property, scale);
			}
		}
	}

	private static UIDrawCall.Clipping ShaderClipping(string shaderName)
	{
		if (shaderName.EndsWith(" (SoftClip)"))
		{
			return UIDrawCall.Clipping.SoftClip;
		}
		if (shaderName.EndsWith(" (HardClip)"))
		{
			return UIDrawCall.Clipping.HardClip;
		}
		if (shaderName.EndsWith(" (AlphaClip)"))
		{
			return UIDrawCall.Clipping.AlphaClip;
		}
		return UIDrawCall.Clipping.None;
	}

	private static bool ShaderNameDecor(ref string shaderName, string not1, string not2, string suffix)
	{
		string str = shaderName.Replace(not1, string.Empty).Replace(not2, string.Empty);
		if (str != shaderName)
		{
			if (!str.EndsWith(suffix))
			{
				shaderName = string.Concat(str, suffix);
			}
			return true;
		}
		if (shaderName.EndsWith(suffix))
		{
			return false;
		}
		shaderName = string.Concat(shaderName, suffix);
		return true;
	}

	public override string ToString()
	{
		return (!this.key ? "destroyed" : this.key.ToString());
	}

	private enum ClippingFlags
	{
		None = 1,
		HardClip = 2,
		AlphaClip = 4,
		SoftClip = 8
	}

	private static class g
	{
		public static int hashCodeIterator;

		public readonly static Dictionary<Material, UIMaterial> generatedMaterials;

		public readonly static Dictionary<Material, UIMaterial> keyedMaterials;

		static g()
		{
			UIMaterial.g.hashCodeIterator = -2147483648;
			UIMaterial.g.generatedMaterials = new Dictionary<Material, UIMaterial>();
			UIMaterial.g.keyedMaterials = new Dictionary<Material, UIMaterial>();
		}
	}
}