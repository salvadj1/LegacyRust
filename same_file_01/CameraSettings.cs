using System;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
	public CameraSettings.ViewDistanceLayer[] ViewDistanceLayers;

	public CameraSettings()
	{
	}

	protected void Awake()
	{
		GameEvent.QualitySettingsRefresh += new GameEvent.OnGenericEvent(this.RefreshSettings);
	}

	protected void OnDestroy()
	{
		GameEvent.QualitySettingsRefresh -= new GameEvent.OnGenericEvent(this.RefreshSettings);
	}

	private void RefreshSettings()
	{
		CameraLayerDepths component = base.GetComponent<CameraLayerDepths>();
		if (component)
		{
			CameraSettings.ViewDistanceLayer[] viewDistanceLayers = this.ViewDistanceLayers;
			for (int i = 0; i < (int)viewDistanceLayers.Length; i++)
			{
				CameraSettings.ViewDistanceLayer viewDistanceLayer = viewDistanceLayers[i];
				component[viewDistanceLayer.Index] = viewDistanceLayer.MinimumValue + render.distance * viewDistanceLayer.Range;
			}
		}
	}

	[Serializable]
	public class ViewDistanceLayer
	{
		public string Name;

		public float MinimumValue;

		public float MaximumValue;

		public int Index
		{
			get
			{
				return LayerMask.NameToLayer(this.Name);
			}
		}

		public float Range
		{
			get
			{
				return this.MaximumValue - this.MinimumValue;
			}
		}

		public ViewDistanceLayer()
		{
		}
	}
}