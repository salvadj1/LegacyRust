using System;
using UnityEngine;

public class LocalDamageDisplay : IDLocalCharacterAddon
{
	private const int SHOW_DAMAGE_OVERLAY = 1;

	private const int SHOW_IMPACT_OVERLAY = 2;

	private const int kDamageOverlayIndex = 0;

	private const int kImpactOverlayIndex = 1;

	private const int kDamageOverlayPass = 3;

	private const int kImpactOverlayPass = 1;

	private const int mode_count = 2;

	protected const IDLocalCharacterAddon.AddonFlags kRequiredAddonFlags = IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake;

	[NonSerialized]
	public Texture2D damageOverlay;

	[NonSerialized]
	public Texture2D damageOverlay2;

	[NonSerialized]
	public float lastHealthPercent = 1f;

	[NonSerialized]
	public BobEffect takeDamageBob;

	[NonSerialized]
	public BobEffect meleeBob;

	[NonSerialized]
	public float lastTakeDamageTime;

	private int lastShowFlags;

	private static bool adminObjectShow;

	private static int mode;

	public LocalDamageDisplay() : this(IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake)
	{
	}

	protected LocalDamageDisplay(IDLocalCharacterAddon.AddonFlags addonFlags) : base((IDLocalCharacterAddon.AddonFlags)((byte)(addonFlags | IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake)))
	{
	}

	private static void DrawLabel(Vector3 point, string label)
	{
		Vector3? nullable = CameraFX.World2Screen(point);
		if (nullable.HasValue)
		{
			Vector3 value = nullable.Value;
			if (value.z > 0f)
			{
				Vector2 gUIPoint = GUIUtility.ScreenToGUIPoint(value);
				gUIPoint.y = (float)Screen.height - (gUIPoint.y + 1f);
				GUI.color = Color.white;
				GUI.Label(new Rect(gUIPoint.x - 64f, gUIPoint.y - 12f, 128f, 24f), label);
			}
		}
	}

	public void Hurt(float percent, GameObject attacker)
	{
		bool component;
		if (percent < 0.05f)
		{
			return;
		}
		this.lastTakeDamageTime = Time.time;
		if (CameraMount.current == null)
		{
			return;
		}
		HeadBob headBob = CameraMount.current.GetComponent<HeadBob>();
		if (headBob == null)
		{
			Debug.Log("no camera headbob");
		}
		if (headBob)
		{
			if (!attacker)
			{
				component = false;
			}
			else
			{
				Controllable controllable = attacker.GetComponent<Controllable>();
				component = (!controllable ? false : controllable.npcName == "zombie");
				if (!component)
				{
					component = attacker.GetComponent<BasicWildLifeAI>() != null;
				}
			}
			headBob.AddEffect((!component ? this.takeDamageBob : this.meleeBob));
		}
	}

	private void LateUpdate()
	{
		float single;
		float single1;
		GameFullscreen instance = ImageEffectManager.GetInstance<GameFullscreen>();
		int num = this.UpdateFadeValues(out single, out single1);
		int num1 = num ^ this.lastShowFlags;
		this.lastShowFlags = num;
		if (num1 != 0)
		{
			if ((num1 & 1) == 1)
			{
				if ((num & 1) != 1)
				{
					instance.overlays[0].texture = null;
				}
				else
				{
					instance.overlays[0].texture = this.damageOverlay;
					instance.overlays[0].pass = 3;
				}
			}
			if ((num1 & 2) == 2)
			{
				if ((num & 2) != 2)
				{
					instance.overlays[1].texture = null;
				}
				else
				{
					instance.overlays[1].texture = this.damageOverlay2;
					instance.overlays[1].pass = 3;
				}
			}
		}
		if ((num & 1) == 1)
		{
			instance.overlays[0].alpha = single;
		}
		if ((num & 2) == 2)
		{
			instance.overlays[1].alpha = single1;
		}
	}

	protected override void OnAddonAwake()
	{
		CharacterOverlayTrait trait = base.GetTrait<CharacterOverlayTrait>();
		this.damageOverlay = trait.damageOverlay;
		this.damageOverlay2 = trait.damageOverlay2;
		this.takeDamageBob = trait.takeDamageBob as BobEffect;
		this.meleeBob = trait.meleeBob as BobEffect;
	}

	private void OnDisable()
	{
		GameFullscreen instance = ImageEffectManager.GetInstance<GameFullscreen>();
		int num = this.lastShowFlags;
		this.lastShowFlags = 0;
		if ((num & 1) == 1)
		{
			instance.overlays[0].texture = null;
		}
		if ((num & 2) == 2)
		{
			instance.overlays[1].texture = null;
		}
	}

	private void OnGUI()
	{
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		if (LocalDamageDisplay.adminObjectShow)
		{
			GUI.color = Color.white;
			GUI.Box(new Rect(5f, 5f, 128f, 24f), (LocalDamageDisplay.mode != 0 ? "showing selection" : "showing characters"));
			if (LocalDamageDisplay.mode == 0)
			{
				UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(Character));
				for (int i = 0; i < (int)objArray.Length; i++)
				{
					UnityEngine.Object obj = objArray[i];
					if (obj)
					{
						Character character = (Character)obj;
						if (character.gameObject != this)
						{
							LocalDamageDisplay.DrawLabel(character.origin, character.name);
						}
					}
				}
			}
		}
	}

	public void SetNewHealthPercent(float newHealthPercent, GameObject attacker)
	{
		if (newHealthPercent < this.lastHealthPercent)
		{
			this.Hurt(newHealthPercent, attacker);
		}
		this.lastHealthPercent = newHealthPercent;
	}

	private void Update()
	{
		if (DebugInput.GetKeyDown(KeyCode.O))
		{
			LocalDamageDisplay.adminObjectShow = !LocalDamageDisplay.adminObjectShow;
			if (!LocalDamageDisplay.adminObjectShow)
			{
				Debug.Log("hid object overlay", this);
			}
			else
			{
				Debug.Log("shown object overlay", this);
			}
		}
		if (LocalDamageDisplay.adminObjectShow && DebugInput.GetKeyDown(KeyCode.L))
		{
			LocalDamageDisplay.mode = (LocalDamageDisplay.mode + 1) % 2;
		}
	}

	private int UpdateFadeValues(out float alpha, out float impactAlpha)
	{
		alpha = 1f - this.lastHealthPercent;
		float single = Mathf.Abs(Mathf.Sin(Time.time * 6f));
		int num = 0;
		if (this.lastHealthPercent <= 0.6f && alpha > 0f)
		{
			num = num | 1;
			alpha = (alpha - 0.6f) * 2.5f * single;
		}
		impactAlpha = 1f - Mathf.Clamp01((Time.time - this.lastTakeDamageTime) / 0.5f);
		impactAlpha = impactAlpha * 1f;
		if (impactAlpha > 0f)
		{
			num = num | 2;
		}
		return num;
	}
}