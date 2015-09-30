using Facepunch.Progress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class TreeUnpack : ThrottledTask, IProgress
{
	[SerializeField]
	private TreeUnpackGroup[] unpackGroups;

	private IEnumerator<Mesh> meshEnumerator;

	private IEnumerator<TreeUnpackGroup> groupEnumerator;

	private TreeUnpackGroup currentGroup;

	private Mesh currentMesh;

	[NonSerialized]
	private int totalTrees;

	[NonSerialized]
	private int currentTreeIndex;

	public float progress
	{
		get
		{
			return (this.totalTrees <= 0 ? 1f : (float)this.currentTreeIndex / (float)this.totalTrees);
		}
	}

	public TreeUnpack()
	{
	}

	private new void Awake()
	{
		base.Awake();
		base.StartCoroutine("DoUnpack");
	}

	[DebuggerHidden]
	private IEnumerator DoUnpack()
	{
		TreeUnpack.<DoUnpack>c__IteratorD variable = null;
		return variable;
	}

	private bool MoveNext()
	{
		if (this.meshEnumerator != null)
		{
			while (this.meshEnumerator.MoveNext())
			{
				TreeUnpack treeUnpack = this;
				treeUnpack.currentTreeIndex = treeUnpack.currentTreeIndex + 1;
				this.currentMesh = this.meshEnumerator.Current;
				if (!this.currentMesh)
				{
					continue;
				}
				return true;
			}
		}
		if (!this.groupEnumerator.MoveNext())
		{
			return false;
		}
		this.currentGroup = this.groupEnumerator.Current;
		this.meshEnumerator = ((IEnumerable<Mesh>)this.currentGroup.meshes).GetEnumerator();
		return this.MoveNext();
	}
}