using NGUI.Meshing;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Draw Call")]
[ExecuteInEditMode]
public class UIDrawCall : MonoBehaviour
{
	private Transform mTrans;

	private UIMaterial mSharedMat;

	private Mesh mMesh0;

	private Mesh mMesh1;

	private MeshFilter mFilter;

	private MeshRenderer mRen;

	private UIDrawCall.Clipping mClipping;

	private Vector4 mClipRange;

	private Vector2 mClipSoft;

	private UIMaterial mDepthMat;

	private int[] mIndices;

	private Vector3[] mVerts;

	private Vector2[] mUVs;

	private Color[] mColors;

	private UIDrawCall mNext;

	private UIDrawCall mPrev;

	private bool mHasNext;

	private bool mHasPrev;

	private UIPanelMaterialPropertyBlock mPanelPropertyBlock;

	private MaterialPropertyBlock mBlock;

	private bool mDepthPass;

	private bool mReset = true;

	private bool mEven = true;

	private static Material[] materialBuffer2;

	private static Material[] materialBuffer1;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mClipping = value;
				this.mReset = true;
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			this.mClipRange = value;
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoft;
		}
		set
		{
			this.mClipSoft = value;
		}
	}

	public bool depthPass
	{
		get
		{
			return this.mDepthPass;
		}
		set
		{
			if (this.mDepthPass != value)
			{
				this.mDepthPass = value;
				this.mReset = true;
			}
		}
	}

	public UIMaterial material
	{
		get
		{
			return this.mSharedMat;
		}
		set
		{
			this.mSharedMat = value;
		}
	}

	public UIPanelMaterialPropertyBlock panelPropertyBlock
	{
		get
		{
			return this.mPanelPropertyBlock;
		}
		set
		{
			this.mPanelPropertyBlock = value;
		}
	}

	public int triangles
	{
		get
		{
			Mesh mesh = (!this.mEven ? this.mMesh1 : this.mMesh0);
			return (mesh == null ? 0 : mesh.vertexCount >> 1);
		}
	}

	static UIDrawCall()
	{
		UIDrawCall.materialBuffer2 = new Material[2];
		UIDrawCall.materialBuffer1 = new Material[1];
	}

	public UIDrawCall()
	{
	}

	private Mesh GetMesh(ref bool rebuildIndices, int vertexCount)
	{
		this.mEven = !this.mEven;
		if (this.mEven)
		{
			if (this.mMesh0 == null)
			{
				this.mMesh0 = new Mesh()
				{
					hideFlags = HideFlags.DontSave
				};
				rebuildIndices = true;
			}
			else if (rebuildIndices || this.mMesh0.vertexCount != vertexCount)
			{
				rebuildIndices = true;
				this.mMesh0.Clear();
			}
			return this.mMesh0;
		}
		if (this.mMesh1 == null)
		{
			this.mMesh1 = new Mesh()
			{
				hideFlags = HideFlags.DontSave
			};
			rebuildIndices = true;
		}
		else if (rebuildIndices || this.mMesh1.vertexCount != vertexCount)
		{
			rebuildIndices = true;
			this.mMesh1.Clear();
		}
		return this.mMesh1;
	}

	internal void LinkedList__Insert(ref UIDrawCall list)
	{
		this.mHasPrev = false;
		this.mHasNext = list;
		this.mNext = list;
		this.mPrev = null;
		if (this.mHasNext)
		{
			list.mHasPrev = true;
			list.mPrev = this;
		}
		list = this;
	}

	internal void LinkedList__Remove()
	{
		if (this.mHasPrev)
		{
			this.mPrev.mHasNext = this.mHasNext;
			this.mPrev.mNext = this.mNext;
		}
		if (this.mHasNext)
		{
			this.mNext.mHasPrev = this.mHasPrev;
			this.mNext.mPrev = this.mPrev;
		}
		int num = 0;
		bool flag = (bool)num;
		this.mHasPrev = (bool)num;
		this.mHasNext = flag;
		object obj = null;
		UIDrawCall uIDrawCall = (UIDrawCall)obj;
		this.mPrev = (UIDrawCall)obj;
		this.mNext = uIDrawCall;
	}

	private void OnDestroy()
	{
		NGUITools.DestroyImmediate(this.mMesh0);
		NGUITools.DestroyImmediate(this.mMesh1);
		NGUITools.DestroyImmediate(this.mDepthMat);
	}

	private void OnWillRenderObject()
	{
		Vector4 vector4 = new Vector4();
		Vector4 vector41 = new Vector4();
		if (this.mReset)
		{
			this.mReset = false;
			this.UpdateMaterials();
		}
		if (this.mBlock != null)
		{
			this.mBlock.Clear();
		}
		else
		{
			this.mBlock = new MaterialPropertyBlock();
		}
		if (this.mPanelPropertyBlock != null)
		{
			this.mPanelPropertyBlock.AddToMaterialPropertyBlock(this.mBlock);
		}
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			vector4.z = -this.mClipRange.x / this.mClipRange.z;
			vector4.w = -this.mClipRange.y / this.mClipRange.w;
			vector4.x = 1f / this.mClipRange.z;
			vector4.y = 1f / this.mClipRange.w;
			this.mBlock.AddVector(UIDrawCall.FastProperties.kProp_ClippingRegion, vector4);
			if (this.mClipSoft.x <= 0f)
			{
				vector41.x = 1000f;
			}
			else
			{
				vector41.x = this.mClipRange.z / this.mClipSoft.x;
			}
			if (this.mClipSoft.y <= 0f)
			{
				vector41.y = 1000f;
			}
			else
			{
				vector41.y = this.mClipRange.w / this.mClipSoft.y;
			}
			float single = 0f;
			float single1 = single;
			vector41.w = single;
			vector41.z = single1;
			this.mBlock.AddVector(UIDrawCall.FastProperties.kProp_ClipSharpness, vector41);
		}
		base.renderer.SetPropertyBlock(this.mBlock);
	}

	public void Set(MeshBuffer m)
	{
		if (this.mFilter == null)
		{
			this.mFilter = base.gameObject.GetComponent<MeshFilter>();
		}
		if (this.mFilter == null)
		{
			this.mFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (this.mRen == null)
		{
			this.mRen = base.gameObject.GetComponent<MeshRenderer>();
		}
		if (this.mRen == null)
		{
			this.mRen = base.gameObject.AddComponent<MeshRenderer>();
			this.UpdateMaterials();
		}
		if (m.vSize >= 65000)
		{
			if (this.mFilter.mesh != null)
			{
				this.mFilter.mesh.Clear();
			}
			Debug.LogError(string.Concat("Too many vertices on one panel: ", m.vSize));
		}
		else
		{
			bool flag = m.ExtractMeshBuffers(ref this.mVerts, ref this.mUVs, ref this.mColors, ref this.mIndices);
			Mesh mesh = this.GetMesh(ref flag, m.vSize);
			mesh.vertices = this.mVerts;
			mesh.uv = this.mUVs;
			mesh.colors = this.mColors;
			mesh.triangles = this.mIndices;
			mesh.RecalculateBounds();
			this.mFilter.mesh = mesh;
		}
	}

	private void UpdateMaterials()
	{
		if (this.mDepthPass)
		{
			if (this.mDepthMat == null)
			{
				Material material = new Material(Shader.Find("Depth"))
				{
					hideFlags = HideFlags.DontSave,
					mainTexture = this.mSharedMat.mainTexture
				};
				this.mDepthMat = UIMaterial.Create(material, true, this.mClipping);
			}
		}
		else if (this.mDepthMat != null)
		{
			NGUITools.Destroy(this.mDepthMat);
			this.mDepthMat = null;
		}
		Material item = this.mSharedMat[this.mClipping];
		if (this.mDepthMat != null)
		{
			UIDrawCall.materialBuffer2[0] = this.mDepthMat[this.mClipping];
			UIDrawCall.materialBuffer2[1] = item;
			this.mRen.sharedMaterials = UIDrawCall.materialBuffer2;
			object obj = null;
			Material material1 = (Material)obj;
			UIDrawCall.materialBuffer2[1] = (Material)obj;
			UIDrawCall.materialBuffer2[0] = material1;
		}
		else if (this.mRen.sharedMaterial != item)
		{
			UIDrawCall.materialBuffer1[0] = item;
			this.mRen.sharedMaterials = UIDrawCall.materialBuffer1;
			UIDrawCall.materialBuffer1[0] = null;
		}
	}

	public enum Clipping
	{
		None,
		HardClip,
		AlphaClip,
		SoftClip
	}

	private static class FastProperties
	{
		public readonly static int kProp_ClippingRegion;

		public readonly static int kProp_ClipSharpness;

		static FastProperties()
		{
			UIDrawCall.FastProperties.kProp_ClippingRegion = Shader.PropertyToID("_MainTex_ST");
			UIDrawCall.FastProperties.kProp_ClipSharpness = Shader.PropertyToID("_ClipSharpness");
		}
	}

	public struct Iterator
	{
		public UIDrawCall Current;

		public bool Has;

		public UIDrawCall.Iterator Next
		{
			get
			{
				UIDrawCall.Iterator current = new UIDrawCall.Iterator();
				if (!this.Has)
				{
					return new UIDrawCall.Iterator();
				}
				current.Has = this.Current.mHasNext;
				current.Current = this.Current.mNext;
				return current;
			}
		}

		public UIDrawCall.Iterator Prev
		{
			get
			{
				UIDrawCall.Iterator current = new UIDrawCall.Iterator();
				if (!this.Has)
				{
					return new UIDrawCall.Iterator();
				}
				current.Has = this.Current.mHasPrev;
				current.Current = this.Current.mPrev;
				return current;
			}
		}

		public static explicit operator Iterator(UIDrawCall call)
		{
			UIDrawCall.Iterator iterator = new UIDrawCall.Iterator();
			iterator.Has = call;
			if (!iterator.Has)
			{
				iterator.Current = null;
			}
			else
			{
				iterator.Current = call;
			}
			return iterator;
		}
	}
}