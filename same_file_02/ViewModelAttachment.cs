using System;
using UnityEngine;

public class ViewModelAttachment : MonoBehaviour
{
	[SerializeField]
	private SkinnedMeshRenderer[] renderers;

	[NonSerialized]
	private ViewModel boundViewModel;

	public ViewModel viewModel
	{
		get
		{
			return this.boundViewModel;
		}
		set
		{
			if (!object.ReferenceEquals(this.boundViewModel, value))
			{
				if (this.boundViewModel)
				{
					this.boundViewModel.RemoveRenderers(this.renderers);
					this.boundViewModel = null;
				}
				if (value)
				{
					this.boundViewModel = value;
					this.boundViewModel.AddRenderers(this.renderers);
				}
			}
		}
	}

	public ViewModelAttachment()
	{
	}

	private void OnDestroy()
	{
		if (this.boundViewModel)
		{
			this.boundViewModel.RemoveRenderers(this.renderers);
		}
	}
}