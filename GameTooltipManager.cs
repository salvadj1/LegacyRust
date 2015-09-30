using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTooltipManager : MonoBehaviour
{
	public static GameTooltipManager Singleton;

	public GameObject tooltipPrefab;

	protected List<GameTooltipManager.TooltipContainer> tooltips = new List<GameTooltipManager.TooltipContainer>();

	static GameTooltipManager()
	{
	}

	public GameTooltipManager()
	{
	}

	protected GameTooltipManager.TooltipContainer GetTipContainer(GameObject obj)
	{
		GameTooltipManager.TooltipContainer tooltipContainer;
		int num = Time.frameCount - 3;
		foreach (GameTooltipManager.TooltipContainer tooltip in this.tooltips)
		{
			if (tooltip.lastSeen >= num)
			{
				if (tooltip.target != obj)
				{
					continue;
				}
				tooltipContainer = tooltip;
				return tooltipContainer;
			}
		}
		List<GameTooltipManager.TooltipContainer>.Enumerator enumerator = this.tooltips.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameTooltipManager.TooltipContainer current = enumerator.Current;
				if (current.target != null)
				{
					if (current.lastSeen >= num)
					{
						continue;
					}
					tooltipContainer = current;
					return tooltipContainer;
				}
				else
				{
					tooltipContainer = current;
					return tooltipContainer;
				}
			}
			return null;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return tooltipContainer;
	}

	private void Start()
	{
		GameTooltipManager.Singleton = this;
		for (int i = 0; i < 16; i++)
		{
			GameTooltipManager.TooltipContainer tooltipContainer = new GameTooltipManager.TooltipContainer();
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.tooltipPrefab);
			gameObject.transform.parent = base.transform;
			tooltipContainer.tooltip = gameObject.GetComponent<dfControl>();
			tooltipContainer.tooltip_label = gameObject.GetComponent<dfLabel>();
			tooltipContainer.lastSeen = 0;
			this.tooltips.Add(tooltipContainer);
		}
	}

	private void Update()
	{
		float single = (float)(Time.frameCount - 3);
		foreach (GameTooltipManager.TooltipContainer tooltip in this.tooltips)
		{
			if ((float)tooltip.lastSeen <= single)
			{
				if (tooltip.tooltip.IsVisible)
				{
					tooltip.tooltip.Hide();
				}
			}
		}
	}

	public void UpdateTip(GameObject obj, string text, Vector3 vPosition, Color color, float alpha, float fscale)
	{
		GameTooltipManager.TooltipContainer tipContainer = this.GetTipContainer(obj);
		if (tipContainer == null)
		{
			return;
		}
		if (!tipContainer.tooltip.IsVisible)
		{
			tipContainer.tooltip.Show();
		}
		dfGUIManager manager = tipContainer.tooltip.GetManager();
		Vector2 screenSize = manager.GetScreenSize();
		Camera renderCamera = manager.RenderCamera;
		Camera camera = Camera.main;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(vPosition);
		screenPoint.x = screenSize.x * (screenPoint.x / camera.pixelWidth);
		screenPoint.y = screenSize.y * (screenPoint.y / camera.pixelHeight);
		screenPoint = manager.ScreenToGui(screenPoint);
		float single = screenPoint.x;
		float width = tipContainer.tooltip.Width / 2f;
		Vector3 vector3 = tipContainer.tooltip.transform.localScale;
		screenPoint.x = single - width * vector3.x;
		float single1 = screenPoint.y;
		float height = tipContainer.tooltip.Height;
		Vector3 vector31 = tipContainer.tooltip.transform.localScale;
		screenPoint.y = single1 - height * vector31.y;
		tipContainer.tooltip.RelativePosition = screenPoint;
		tipContainer.tooltip_label.Text = text;
		tipContainer.tooltip_label.Color = color;
		tipContainer.tooltip.Opacity = alpha;
		tipContainer.lastSeen = Time.frameCount;
		tipContainer.target = obj;
		tipContainer.tooltip.transform.localScale = new Vector3(fscale, fscale, fscale);
	}

	protected class TooltipContainer
	{
		public GameObject target;

		public dfControl tooltip;

		public dfLabel tooltip_label;

		public int lastSeen;

		public TooltipContainer()
		{
		}
	}
}