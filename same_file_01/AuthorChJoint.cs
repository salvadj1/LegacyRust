using System;
using UnityEngine;

public class AuthorChJoint : AuthorPeice
{
	[SerializeField]
	private AuthorChHit self;

	[SerializeField]
	private AuthorChHit connect;

	[SerializeField]
	private AuthorChJoint.Kind kind;

	[SerializeField]
	private bool reverseLink;

	[SerializeField]
	private Vector3 anchor;

	[SerializeField]
	private Vector3 axis = Vector3.up;

	[SerializeField]
	private Vector3 swingAxis = Vector3.forward;

	[SerializeField]
	private float twistL_limit = -20f;

	[SerializeField]
	private float twistL_bounce;

	[SerializeField]
	private float twistL_dampler;

	[SerializeField]
	private float twistL_spring;

	[SerializeField]
	private float twistH_limit = 70f;

	[SerializeField]
	private float twistH_bounce;

	[SerializeField]
	private float twistH_dampler;

	[SerializeField]
	private float twistH_spring;

	[SerializeField]
	private float swing1_limit = 20f;

	[SerializeField]
	private float swing1_bounce;

	[SerializeField]
	private float swing1_dampler;

	[SerializeField]
	private float swing1_spring;

	[SerializeField]
	private float swing2_limit = 20f;

	[SerializeField]
	private float swing2_bounce;

	[SerializeField]
	private float swing2_dampler;

	[SerializeField]
	private float swing2_spring;

	[SerializeField]
	private float h_limit_min;

	[SerializeField]
	private float h_limit_max;

	[SerializeField]
	private float h_limit_minb;

	[SerializeField]
	private float h_limit_maxb;

	[SerializeField]
	private float h_spring_s;

	[SerializeField]
	private float h_spring_d;

	[SerializeField]
	private float h_spring_t;

	[SerializeField]
	private float h_motor_f;

	[SerializeField]
	private float h_motor_v;

	[SerializeField]
	private bool h_motor_s;

	[SerializeField]
	private float spring_spring;

	[SerializeField]
	private float spring_min;

	[SerializeField]
	private float spring_max;

	[SerializeField]
	private float spring_damper;

	[SerializeField]
	private bool useLimit;

	[SerializeField]
	private bool useSpring;

	[SerializeField]
	private bool useMotor;

	[SerializeField]
	private float limitOffset;

	[SerializeField]
	private float twistOffset;

	[SerializeField]
	private float swingOffset1;

	[SerializeField]
	private float swingOffset2;

	[SerializeField]
	private float breakForce = Single.PositiveInfinity;

	[SerializeField]
	private float breakTorque = Single.PositiveInfinity;

	private readonly static Color twistColor;

	private readonly static Color swing1Color;

	private readonly static Color swing2Color;

	private SoftJointLimit highTwist
	{
		get
		{
			SoftJointLimit softJointLimit = new SoftJointLimit()
			{
				limit = this.twistH_limit,
				damper = this.twistH_dampler,
				spring = this.twistH_spring,
				bounciness = this.twistH_bounce
			};
			return softJointLimit;
		}
		set
		{
			this.twistH_limit = value.limit;
			this.twistH_dampler = value.damper;
			this.twistH_spring = value.spring;
			this.twistH_bounce = value.bounciness;
		}
	}

	private JointLimits limit
	{
		get
		{
			JointLimits jointLimit = new JointLimits()
			{
				min = this.h_limit_min,
				max = this.h_limit_max,
				minBounce = this.h_limit_minb,
				maxBounce = this.h_limit_maxb
			};
			return jointLimit;
		}
		set
		{
			this.h_limit_min = value.min;
			this.h_limit_max = value.max;
			this.h_limit_minb = value.minBounce;
			this.h_limit_maxb = value.maxBounce;
		}
	}

	private SoftJointLimit lowTwist
	{
		get
		{
			SoftJointLimit softJointLimit = new SoftJointLimit()
			{
				limit = this.twistL_limit,
				damper = this.twistL_dampler,
				spring = this.twistL_spring,
				bounciness = this.twistL_bounce
			};
			return softJointLimit;
		}
		set
		{
			this.twistL_limit = value.limit;
			this.twistL_dampler = value.damper;
			this.twistL_spring = value.spring;
			this.twistL_bounce = value.bounciness;
		}
	}

	private JointMotor motor
	{
		get
		{
			JointMotor jointMotor = new JointMotor()
			{
				force = this.h_motor_f,
				targetVelocity = this.h_motor_v,
				freeSpin = this.h_motor_s
			};
			return jointMotor;
		}
		set
		{
			this.h_motor_f = value.force;
			this.h_motor_v = value.targetVelocity;
			this.h_motor_s = value.freeSpin;
		}
	}

	private JointSpring spring
	{
		get
		{
			JointSpring jointSpring = new JointSpring()
			{
				spring = this.h_spring_s,
				damper = this.h_spring_d,
				targetPosition = this.h_spring_t
			};
			return jointSpring;
		}
		set
		{
			this.h_spring_s = value.spring;
			this.h_spring_d = value.damper;
			this.h_spring_t = value.targetPosition;
		}
	}

	private SoftJointLimit swing1
	{
		get
		{
			SoftJointLimit softJointLimit = new SoftJointLimit()
			{
				limit = this.swing1_limit,
				damper = this.swing1_dampler,
				spring = this.swing1_spring,
				bounciness = this.swing1_bounce
			};
			return softJointLimit;
		}
		set
		{
			this.swing1_limit = value.limit;
			this.swing1_dampler = value.damper;
			this.swing1_spring = value.spring;
			this.swing1_bounce = value.bounciness;
		}
	}

	private SoftJointLimit swing2
	{
		get
		{
			SoftJointLimit softJointLimit = new SoftJointLimit()
			{
				limit = this.swing2_limit,
				damper = this.swing2_dampler,
				spring = this.swing2_spring,
				bounciness = this.swing2_bounce
			};
			return softJointLimit;
		}
		set
		{
			this.swing2_limit = value.limit;
			this.swing2_dampler = value.damper;
			this.swing2_spring = value.spring;
			this.swing2_bounce = value.bounciness;
		}
	}

	static AuthorChJoint()
	{
		AuthorChJoint.twistColor = new Color(1f, 1f, 0.4f, 0.8f);
		AuthorChJoint.swing1Color = new Color(1f, 0.4f, 1f, 0.8f);
		AuthorChJoint.swing2Color = new Color(0.4f, 1f, 1f, 0.8f);
	}

	public AuthorChJoint()
	{
	}

	public Joint AddJoint(Transform root, ref AuthorChHit.Rep self)
	{
		Joint joint;
		switch (this.kind)
		{
			case AuthorChJoint.Kind.Hinge:
			{
				HingeJoint hingeJoint = this.CreateJoint<HingeJoint>(root, ref self);
				hingeJoint.limits = this.limit;
				hingeJoint.useLimits = this.useLimit;
				hingeJoint.motor = this.motor;
				hingeJoint.useMotor = this.useMotor;
				hingeJoint.spring = this.spring;
				hingeJoint.useSpring = this.useSpring;
				joint = hingeJoint;
				break;
			}
			case AuthorChJoint.Kind.Character:
			{
				CharacterJoint characterJoint = this.CreateJoint<CharacterJoint>(root, ref self);
				characterJoint.swingAxis = self.AxisFlip(this.swingAxis);
				characterJoint.lowTwistLimit = this.lowTwist;
				characterJoint.highTwistLimit = this.highTwist;
				characterJoint.lowTwistLimit = this.lowTwist;
				characterJoint.swing1Limit = this.swing1;
				characterJoint.swing2Limit = this.swing2;
				joint = characterJoint;
				break;
			}
			case AuthorChJoint.Kind.Fixed:
			{
				joint = this.CreateJoint<FixedJoint>(root, ref self);
				break;
			}
			case AuthorChJoint.Kind.Spring:
			{
				SpringJoint springSpring = this.CreateJoint<SpringJoint>(root, ref self);
				springSpring.spring = this.spring_spring;
				springSpring.damper = this.spring_damper;
				springSpring.minDistance = this.spring_min;
				springSpring.maxDistance = this.spring_max;
				joint = springSpring;
				break;
			}
			default:
			{
				return null;
			}
		}
		return joint;
	}

	private TJoint ConfigJoint<TJoint>(TJoint joint, Transform root, ref AuthorChHit.Rep self)
	where TJoint : Joint
	{
		this.ConfigureJointShared(joint, root, ref self);
		return joint;
	}

	private void ConfigureJointShared(Joint joint, Transform root, ref AuthorChHit.Rep self)
	{
		AuthorChHit.Rep rep;
		if (this.connect)
		{
			if (!self.mirrored)
			{
				rep = this.connect.primary;
				if (!rep.valid)
				{
					rep = this.connect.secondary;
				}
			}
			else
			{
				rep = this.connect.secondary;
				if (!rep.valid)
				{
					rep = this.connect.primary;
				}
			}
			if (!rep.valid)
			{
				Debug.LogWarning("No means of making/getting rigidbody", this.connect);
			}
			else
			{
				Transform transforms = root.FindChild(rep.path);
				Rigidbody rigidbody = transforms.rigidbody;
				if (!rigidbody)
				{
					rigidbody = transforms.gameObject.AddComponent<Rigidbody>();
				}
				joint.connectedBody = rigidbody;
			}
		}
		joint.anchor = self.Flip(this.anchor);
		joint.axis = self.AxisFlip(this.axis);
		joint.breakForce = this.breakForce;
		joint.breakTorque = this.breakTorque;
	}

	private TJoint CreateJoint<TJoint>(Transform root, ref AuthorChHit.Rep self)
	where TJoint : Joint
	{
		return this.ConfigJoint<TJoint>(self.bone.gameObject.AddComponent<TJoint>(), root, ref self);
	}

	private bool DoTransformHandles(ref AuthorChHit.Rep self, ref AuthorChHit.Rep connect)
	{
		if (!self.valid)
		{
			return false;
		}
		Vector3 vector3 = self.Flip(this.anchor);
		Vector3 vector31 = self.AxisFlip(this.axis);
		Vector3 vector32 = self.AxisFlip(this.swingAxis);
		Matrix4x4 matrix4x4 = AuthorShared.Scene.matrix;
		if (connect.valid)
		{
			AuthorShared.Scene.matrix = connect.bone.localToWorldMatrix;
			Color color = AuthorShared.Scene.color;
			AuthorShared.Scene.color = color * new Color(1f, 1f, 1f, 0.4f);
			Vector3 vector33 = connect.bone.InverseTransformPoint(self.bone.position);
			if (vector33 != Vector3.zero)
			{
				AuthorShared.Scene.DrawBone(Vector3.zero, Quaternion.LookRotation(vector33), vector33.magnitude, 0.02f, new Vector3(0.05f, 0.05f, 0.5f));
			}
			AuthorShared.Scene.color = color;
		}
		AuthorShared.Scene.matrix = self.bone.localToWorldMatrix;
		bool flag = false;
		if (AuthorShared.Scene.PivotDrag(ref vector3, ref vector31))
		{
			flag = true;
			this.anchor = self.Flip(vector3);
			this.axis = self.AxisFlip(vector31);
		}
		AuthorChJoint.Kind kind = this.kind;
		if (kind != AuthorChJoint.Kind.Hinge)
		{
			if (kind == AuthorChJoint.Kind.Character)
			{
				Color color1 = AuthorShared.Scene.color;
				AuthorShared.Scene.color = color1 * AuthorChJoint.twistColor;
				SoftJointLimit softJointLimit = this.lowTwist;
				SoftJointLimit softJointLimit1 = this.highTwist;
				if (AuthorShared.Scene.LimitDrag(vector3, vector31, ref this.twistOffset, ref softJointLimit, ref softJointLimit1))
				{
					flag = true;
					this.lowTwist = softJointLimit;
					this.highTwist = softJointLimit1;
				}
				AuthorShared.Scene.color = color1 * AuthorChJoint.swing1Color;
				softJointLimit = this.swing1;
				if (AuthorShared.Scene.LimitDrag(vector3, vector32, ref this.swingOffset1, ref softJointLimit))
				{
					flag = true;
					this.swing1 = softJointLimit;
				}
				AuthorShared.Scene.color = color1 * AuthorChJoint.swing2Color;
				softJointLimit = this.swing2;
				if (AuthorShared.Scene.LimitDrag(vector3, Vector3.Cross(vector32, vector31), ref this.swingOffset2, ref softJointLimit))
				{
					flag = true;
					this.swing2 = softJointLimit;
				}
				AuthorShared.Scene.color = color1;
			}
		}
		else if (this.useLimit)
		{
			JointLimits jointLimit = this.limit;
			if (AuthorShared.Scene.LimitDrag(vector3, vector31, ref this.limitOffset, ref jointLimit))
			{
				flag = true;
				this.limit = jointLimit;
			}
		}
		AuthorShared.Scene.matrix = matrix4x4;
		return flag;
	}

	private static bool Field(AuthorShared.Content content, ref JointLimits limits, ref bool use, ref float offset)
	{
		GUI.Box(AuthorShared.BeginVertical(new GUILayoutOption[0]), GUIContent.none);
		AuthorShared.PrefixLabel(content);
		bool flag = use;
		bool flag1 = AuthorShared.Change(ref use, GUILayout.Toggle(use, "Use", new GUILayoutOption[0]));
		if (flag)
		{
			float single = limits.min;
			float single1 = limits.max;
			float single2 = limits.minBounce;
			float single3 = limits.maxBounce;
			flag1 = flag1 | AuthorShared.FloatField("Min", ref single, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.FloatField("Max", ref single1, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.FloatField("Min bounciness", ref single2, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.FloatField("Max bounciness", ref single3, new GUILayoutOption[0]);
			if (use && flag1)
			{
				limits.min = single;
				limits.max = single1;
				limits.minBounce = single2;
				limits.maxBounce = single3;
			}
			Color color = GUI.contentColor;
			GUI.contentColor = color * new Color(1f, 1f, 1f, 0.3f);
			flag1 = flag1 | AuthorShared.FloatField("Offset(visual only)", ref offset, new GUILayoutOption[0]);
			GUI.contentColor = color;
		}
		AuthorShared.EndVertical();
		return flag1;
	}

	private static bool Field(AuthorShared.Content content, ref JointMotor motor, ref bool use)
	{
		GUI.Box(AuthorShared.BeginVertical(new GUILayoutOption[0]), GUIContent.none);
		AuthorShared.PrefixLabel(content);
		bool flag = use;
		bool flag1 = AuthorShared.Change(ref use, GUILayout.Toggle(use, "Use", new GUILayoutOption[0]));
		if (flag)
		{
			float single = motor.force;
			float single1 = motor.targetVelocity;
			bool flag2 = motor.freeSpin;
			flag1 = flag1 | AuthorShared.FloatField("Force", ref single, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.FloatField("Target Velocity", ref single1, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.Change(ref flag2, GUILayout.Toggle(flag2, "Free Spin", new GUILayoutOption[0]));
			if (use && flag1)
			{
				motor.force = single;
				motor.targetVelocity = single1;
				motor.freeSpin = flag2;
			}
		}
		AuthorShared.EndVertical();
		return flag1;
	}

	private static bool Field(AuthorShared.Content content, ref JointSpring spring, ref bool use)
	{
		GUI.Box(AuthorShared.BeginVertical(new GUILayoutOption[0]), GUIContent.none);
		AuthorShared.PrefixLabel(content);
		bool flag = use;
		bool flag1 = AuthorShared.Change(ref use, GUILayout.Toggle(use, "Use", new GUILayoutOption[0]));
		if (flag)
		{
			float single = spring.spring;
			float single1 = spring.targetPosition;
			float single2 = spring.damper;
			flag1 = flag1 | AuthorShared.FloatField("Spring Force", ref single, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.FloatField("Target Position", ref single1, new GUILayoutOption[0]);
			flag1 = flag1 | AuthorShared.FloatField("Damper", ref single2, new GUILayoutOption[0]);
			if (use && flag1)
			{
				spring.spring = single;
				spring.targetPosition = single1;
				spring.damper = single2;
			}
		}
		AuthorShared.EndVertical();
		return flag1;
	}

	private static bool Field(AuthorShared.Content content, ref SoftJointLimit limits, ref float offset)
	{
		GUI.Box(AuthorShared.BeginVertical(new GUILayoutOption[0]), GUIContent.none);
		AuthorShared.PrefixLabel(content);
		float single = limits.limit;
		float single1 = limits.spring;
		float single2 = limits.damper;
		float single3 = limits.bounciness;
		bool flag = AuthorShared.FloatField("Limit", ref single, new GUILayoutOption[0]);
		flag = flag | AuthorShared.FloatField("Spring", ref single1, new GUILayoutOption[0]);
		flag = flag | AuthorShared.FloatField("Damper", ref single2, new GUILayoutOption[0]);
		flag = flag | AuthorShared.FloatField("Bounciness", ref single3, new GUILayoutOption[0]);
		if (flag)
		{
			limits.limit = single;
			limits.spring = single1;
			limits.damper = single2;
			limits.bounciness = single3;
		}
		Color color = GUI.contentColor;
		GUI.contentColor = color * new Color(1f, 1f, 1f, 0.3f);
		flag = flag | AuthorShared.FloatField("Offset(visual only)", ref offset, new GUILayoutOption[0]);
		GUI.contentColor = color;
		AuthorShared.EndVertical();
		return flag;
	}

	internal void InitializeFromOwner(AuthorChHit self, AuthorChJoint.Kind kind)
	{
		this.self = self;
		this.kind = kind;
		AuthorShared.SetDirty(this);
	}

	protected override void OnPeiceDestroy()
	{
		try
		{
			if (this.self)
			{
				this.self.OnJointDestroy(this);
			}
		}
		finally
		{
			base.OnPeiceDestroy();
		}
	}

	public override bool OnSceneView()
	{
		bool flag = base.OnSceneView();
		AuthorChHit.Rep rep = this.self.primary;
		AuthorChHit.Rep rep1 = this.self.secondary;
		AuthorChHit.Rep rep2 = new AuthorChHit.Rep();
		AuthorChHit.Rep rep3 = new AuthorChHit.Rep();
		if (this.connect)
		{
			rep2 = this.connect.primary;
			rep3 = this.connect.secondary;
			if (!rep3.valid)
			{
				rep3 = rep2;
			}
		}
		if (rep.valid)
		{
			flag = flag | this.DoTransformHandles(ref rep, ref rep2);
		}
		if (rep1.valid)
		{
			flag = flag | this.DoTransformHandles(ref rep1, ref rep3);
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
		AuthorShared.EnumField("Kind", this.kind, new GUILayoutOption[0]);
		AuthorShared.PrefixLabel("Self");
		if (GUILayout.Button(AuthorShared.ObjectContent<AuthorChHit>(this.self, typeof(AuthorChHit)), new GUILayoutOption[0]))
		{
			AuthorShared.PingObject(this.self);
		}
		flag = flag | AuthorShared.PeiceField<AuthorChHit>("Connected", this, ref this.connect, typeof(AuthorChHit), GUI.skin.button, new GUILayoutOption[0]);
		flag = flag | AuthorShared.Toggle("Reverse Link", ref this.reverseLink, new GUILayoutOption[0]);
		flag = flag | AuthorShared.Vector3Field("Anchor", ref this.anchor, new GUILayoutOption[0]);
		flag = flag | AuthorShared.Vector3Field("Axis", ref this.axis, new GUILayoutOption[0]);
		AuthorChJoint.Kind kind = this.kind;
		if (kind == AuthorChJoint.Kind.Hinge)
		{
			JointLimits jointLimit = this.limit;
			if (AuthorChJoint.Field("Limits", ref jointLimit, ref this.useLimit, ref this.limitOffset))
			{
				flag = true;
				this.limit = jointLimit;
			}
		}
		else if (kind == AuthorChJoint.Kind.Character)
		{
			flag = flag | AuthorShared.Vector3Field("Swing Axis", ref this.swingAxis, new GUILayoutOption[0]);
			SoftJointLimit softJointLimit = this.lowTwist;
			if (AuthorChJoint.Field("Low Twist", ref softJointLimit, ref this.twistOffset))
			{
				flag = true;
				this.lowTwist = softJointLimit;
			}
			softJointLimit = this.highTwist;
			if (AuthorChJoint.Field("High Twist", ref softJointLimit, ref this.twistOffset))
			{
				flag = true;
				this.highTwist = softJointLimit;
			}
			softJointLimit = this.swing1;
			if (AuthorChJoint.Field("Swing 1", ref softJointLimit, ref this.swingOffset1))
			{
				flag = true;
				this.swing1 = softJointLimit;
			}
			softJointLimit = this.swing2;
			if (AuthorChJoint.Field("Swing 2", ref softJointLimit, ref this.swingOffset2))
			{
				flag = true;
				this.swing2 = softJointLimit;
			}
		}
		flag = flag | AuthorShared.FloatField("Break Force", ref this.breakForce, new GUILayoutOption[0]);
		flag = flag | AuthorShared.FloatField("Break Torque", ref this.breakTorque, new GUILayoutOption[0]);
		return flag;
	}

	public enum Kind
	{
		None,
		Hinge,
		Character,
		Fixed,
		Spring
	}
}