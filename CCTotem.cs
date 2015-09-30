using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

public abstract class CCTotem : MonoBehaviour
{
	protected internal const int kMaxTotemicFiguresPerTotemPole = 8;

	protected internal const CollisionFlags kCF_Sides = CollisionFlags.Sides;

	protected internal const CollisionFlags kCF_Above = CollisionFlags.Above;

	protected internal const CollisionFlags kCF_Below = CollisionFlags.Below;

	protected internal const CollisionFlags kCF_None = CollisionFlags.None;

	internal abstract CCTotem.TotemicObject _Object
	{
		get;
	}

	protected internal CCTotem.TotemicObject totemicObject
	{
		get
		{
			return this._Object;
		}
	}

	internal CCTotem()
	{
	}

	private static void DestroyCCDesc(CCTotemPole ScriptOwner, ref CCDesc CCDesc)
	{
		if (!ScriptOwner)
		{
			CCDesc cCDesc = CCDesc;
			CCDesc = null;
			if (cCDesc)
			{
				UnityEngine.Object.Destroy(cCDesc.gameObject);
			}
		}
		else
		{
			ScriptOwner.DestroyCCDesc(ref CCDesc);
		}
	}

	private static string VS(Vector3 v)
	{
		return string.Format("[{0},{1},{2}]", v.x, v.y, v.z);
	}

	protected internal struct Configuration
	{
		public readonly CCTotem.Initialization totem;

		public readonly float totemMinHeight;

		public readonly float totemMaxHeight;

		public readonly float totemBottomBufferUnits;

		public readonly float figureSkinWidth;

		public readonly float figure2SkinWidth;

		public readonly float figureRadius;

		public readonly float figureSkinnedRadius;

		public readonly float figureDiameter;

		public readonly float figureSkinnedDiameter;

		public readonly float figureHeight;

		public readonly float figureSkinnedHeight;

		public readonly float figureSlideHeight;

		public readonly float figureFixedHeight;

		public readonly float poleTopBufferAmount;

		public readonly float poleBottomBufferAmount;

		public readonly float poleBottomBufferHeight;

		public readonly float poleBottomBufferUnitSize;

		public readonly float poleMostContractedHeightPossible;

		public readonly float poleMostExpandedHeightPossible;

		public readonly float poleContractedHeight;

		public readonly float poleContractedHeightFromMostContractedHeightPossible;

		public readonly float poleExpandedHeight;

		public readonly float poleExpandedHeightFromMostContractedHeightPossible;

		public readonly float poleFixedLength;

		public readonly float poleExpansionLength;

		public readonly int numRequiredTotemicFigures;

		public readonly int numSlidingTotemicFigures;

		public readonly Vector3 figureOriginOffsetBottom;

		public readonly Vector3 figureOriginOffsetTop;

		public readonly Vector3 figureOriginOffsetCenter;

		public Configuration(ref CCTotem.Initialization totem)
		{
			if (!totem.figurePrefab)
			{
				throw new ArgumentException("figurePrefab was missing", "totem");
			}
			this.totem = totem;
			this.totemMinHeight = totem.minHeight;
			this.totemMaxHeight = totem.maxHeight;
			this.totemBottomBufferUnits = totem.bottomBufferUnits;
			if (this.totemMinHeight >= this.totemMaxHeight)
			{
				throw new ArgumentException("maxHeight is less than or equal to minHeight", "totem");
			}
			if (Mathf.Approximately(this.totemBottomBufferUnits, 0f))
			{
				this.totemBottomBufferUnits = 0f;
			}
			else if (this.totemBottomBufferUnits < 0f)
			{
				throw new ArgumentException("bottomBufferPercent must not be less than zero", "totem");
			}
			CCDesc cCDesc = totem.figurePrefab;
			this.figureSkinWidth = cCDesc.skinWidth;
			this.figure2SkinWidth = this.figureSkinWidth + this.figureSkinWidth;
			this.figureRadius = cCDesc.radius;
			this.figureSkinnedRadius = this.figureRadius + this.figureSkinWidth;
			this.figureDiameter = this.figureRadius + this.figureRadius;
			this.figureSkinnedDiameter = this.figureSkinnedRadius + this.figureSkinnedRadius;
			this.figureHeight = cCDesc.height;
			if (this.figureHeight <= this.figureDiameter)
			{
				throw new ArgumentException("The CCDesc(CharacterController) Prefab is a sphere, not a capsule. Thus cannot be expanded on the totem pole", "totem");
			}
			this.figureSkinnedHeight = this.figureHeight + this.figure2SkinWidth;
			if (this.figureSkinnedHeight > this.totemMinHeight && !Mathf.Approximately(this.totemMinHeight, this.figureSkinnedHeight))
			{
				throw new ArgumentException("minHeight is too small. It must be at least the size of the CCDesc(CharacterController) prefab's [height+(skinWidth*2)]", "totem");
			}
			this.figureSlideHeight = this.figureSkinnedHeight - this.figureSkinnedDiameter;
			if (this.figureSlideHeight <= 0f)
			{
				throw new ArgumentException("The CCDesc(CharacterController) Prefab has limited height availability. Thus cannot be expanded on the totem pole", "totem");
			}
			this.figureFixedHeight = this.figureSkinnedHeight - this.figureSlideHeight;
			this.poleTopBufferAmount = this.figureSkinnedRadius;
			this.poleBottomBufferUnitSize = this.figureSlideHeight * 0.5f;
			this.poleBottomBufferAmount = this.poleBottomBufferUnitSize * this.totemBottomBufferUnits;
			if (this.poleBottomBufferAmount > this.figureSlideHeight)
			{
				if (!Mathf.Approximately(this.poleBottomBufferAmount, this.figureSlideHeight))
				{
					throw new ArgumentException("The bottomBuffer was too large and landed outside of sliding height area of the capsule", "totem");
				}
				this.poleBottomBufferAmount = this.figureSlideHeight;
				this.totemBottomBufferUnits = this.figureSlideHeight / this.poleBottomBufferUnitSize;
			}
			this.poleBottomBufferHeight = this.figureSkinnedRadius + this.poleBottomBufferAmount;
			this.poleMostContractedHeightPossible = this.figureSkinnedHeight + this.poleBottomBufferAmount;
			if (this.poleMostContractedHeightPossible > this.totemMinHeight)
			{
				if (!Mathf.Approximately(this.poleMostContractedHeightPossible, this.totemMinHeight))
				{
					throw new ArgumentException("bottomBufferPercent value is too high with the current setup, results in contracted height greater than totem.minHeight.", "totem");
				}
				this.totemMinHeight = this.poleMostContractedHeightPossible;
			}
			this.poleContractedHeight = Mathf.Max(this.poleMostContractedHeightPossible, this.totemMinHeight);
			this.poleContractedHeightFromMostContractedHeightPossible = this.poleContractedHeight - this.poleMostContractedHeightPossible;
			this.poleExpandedHeight = Mathf.Max(this.poleContractedHeight, this.totemMaxHeight);
			this.poleExpandedHeightFromMostContractedHeightPossible = this.poleExpandedHeight - this.poleMostContractedHeightPossible;
			if (Mathf.Approximately(this.poleContractedHeightFromMostContractedHeightPossible, this.poleExpandedHeightFromMostContractedHeightPossible))
			{
				throw new ArgumentException("minHeight and maxHeight were too close to eachother to provide reliable contraction/expansion calculations.", "totem");
			}
			if (this.poleContractedHeightFromMostContractedHeightPossible < 0f || this.poleExpandedHeightFromMostContractedHeightPossible < this.poleContractedHeightFromMostContractedHeightPossible)
			{
				throw new ArgumentException("Calculation error with current configuration.", "totem");
			}
			this.poleFixedLength = this.poleBottomBufferHeight + this.poleTopBufferAmount;
			this.poleExpansionLength = this.poleExpandedHeight - this.poleFixedLength;
			this.numSlidingTotemicFigures = Mathf.CeilToInt(this.poleExpansionLength / this.figureSlideHeight);
			if (this.numSlidingTotemicFigures < 1)
			{
				throw new ArgumentException("The current configuration of the CCTotem resulted in no need for more than one CCDesc(CharacterController), thus rejecting usage..", "totem");
			}
			this.poleMostExpandedHeightPossible = this.poleFixedLength + (float)this.numSlidingTotemicFigures * this.figureSlideHeight;
			this.numRequiredTotemicFigures = 1 + this.numSlidingTotemicFigures;
			if (this.numRequiredTotemicFigures > 8)
			{
				throw new ArgumentOutOfRangeException("totem", (object)this.numRequiredTotemicFigures, string.Concat("The current configuration of the CCTotem resulted in more than the max number of TotemicFigure's allowed :", 8));
			}
			Vector3 vector3 = cCDesc.center;
			this.figureOriginOffsetCenter = new Vector3(0f - vector3.x, 0f - vector3.y, 0f - vector3.z);
			this.figureOriginOffsetBottom = new Vector3(this.figureOriginOffsetCenter.x, 0f - (vector3.y - this.figureSkinnedHeight / 2f), this.figureOriginOffsetCenter.z);
			this.figureOriginOffsetTop = new Vector3(this.figureOriginOffsetCenter.x, 0f - (vector3.y + this.figureSkinnedHeight / 2f), this.figureOriginOffsetCenter.z);
		}

		public override string ToString()
		{
			return CCTotem.ToStringHelper<CCTotem.Configuration>.GetString(this);
		}
	}

	public delegate void ConfigurationBinder(bool Bind, CCDesc CCDesc, object Tag);

	protected internal struct Contraction
	{
		public readonly float Contracted;

		public readonly float Expanded;

		public readonly float Range;

		public readonly float InverseRange;

		private Contraction(float Contracted, float Expanded, float Range, float InverseRange)
		{
			this.Contracted = Contracted;
			this.Expanded = Expanded;
			this.Range = Range;
			this.InverseRange = InverseRange;
		}

		public static CCTotem.Contraction Define(float Contracted, float Expanded)
		{
			if (Mathf.Approximately(Contracted, Expanded))
			{
				throw new ArgumentOutOfRangeException("Contracted", "approximately equal to Expanded");
			}
			float expanded = Expanded - Contracted;
			return new CCTotem.Contraction(Contracted, Expanded, expanded, (float)(1 / (double)expanded));
		}

		public CCTotem.Expansion ExpansionForAmount(float Amount)
		{
			CCTotem.Expansion expansion;
			if (Amount <= 0f)
			{
				expansion = new CCTotem.Expansion(this.Contracted, 0f, 0f);
			}
			else if (Amount < this.Range)
			{
				float amount = Amount / this.Range;
				float contracted = this.Contracted + Amount;
				expansion = new CCTotem.Expansion(contracted, amount, Amount);
			}
			else
			{
				expansion = new CCTotem.Expansion(this.Expanded, 1f, this.Range);
			}
			return expansion;
		}

		public CCTotem.Expansion ExpansionForFraction(float FractionExpanded)
		{
			CCTotem.Expansion expansion;
			if (FractionExpanded <= 0f)
			{
				expansion = new CCTotem.Expansion(this.Contracted, 0f, 0f);
			}
			else if (FractionExpanded < 1f)
			{
				float fractionExpanded = FractionExpanded * this.Range;
				float contracted = this.Contracted + fractionExpanded;
				expansion = new CCTotem.Expansion(contracted, FractionExpanded, fractionExpanded);
			}
			else
			{
				expansion = new CCTotem.Expansion(this.Expanded, 1f, this.Range);
			}
			return expansion;
		}

		public CCTotem.Expansion ExpansionForValue(float Value)
		{
			CCTotem.Expansion expansion;
			if (Value <= this.Contracted)
			{
				expansion = new CCTotem.Expansion(this.Contracted, 0f, 0f);
			}
			else if (Value < this.Expanded)
			{
				float value = Value - this.Contracted;
				float range = value / this.Range;
				expansion = new CCTotem.Expansion(Value, range, value);
			}
			else
			{
				expansion = new CCTotem.Expansion(this.Expanded, 1f, this.Range);
			}
			return expansion;
		}

		public override string ToString()
		{
			return string.Format("{{Contracted={0},Expanded={1},Range={2},InverseRange={3}}}", new object[] { this.Contracted, this.Expanded, this.Range, this.InverseRange });
		}
	}

	protected internal struct Direction
	{
		public readonly bool Exists;

		public readonly CCTotem.TotemicFigure TotemicFigure;

		public static CCTotem.Direction None
		{
			get
			{
				return new CCTotem.Direction();
			}
		}

		public Direction(CCTotem.TotemicFigure TotemicFigure)
		{
			if (object.ReferenceEquals(TotemicFigure, null))
			{
				throw new ArgumentNullException("TotemicFigure");
			}
			this.TotemicFigure = TotemicFigure;
			this.Exists = true;
		}

		public override string ToString()
		{
			return (!this.Exists ? "{Does Not Exist}" : string.Format("{{TotemicFigure={0}}}", this.TotemicFigure));
		}
	}

	protected internal struct Ends<T>
	{
		public T Bottom;

		public T Top;

		public Ends(T Bottom, T Top)
		{
			this.Bottom = Bottom;
			this.Top = Top;
		}

		public override string ToString()
		{
			return string.Format("{{Bottom={0},Top={1}}}", this.Bottom, this.Top);
		}
	}

	protected internal struct Expansion
	{
		public readonly float Value;

		public readonly float FractionExpanded;

		public readonly float Amount;

		internal Expansion(float Value, float FractionExpanded, float Amount)
		{
			this.Value = Value;
			this.FractionExpanded = FractionExpanded;
			this.Amount = Amount;
		}

		public override string ToString()
		{
			return string.Format("{{Value={0},FractionExpanded={1},Amount={2}}}", this.Value, this.FractionExpanded, this.Amount);
		}
	}

	protected internal struct Initialization
	{
		public readonly CCTotemPole totemPole;

		public readonly CCDesc figurePrefab;

		public readonly float minHeight;

		public readonly float maxHeight;

		public readonly float initialHeight;

		public readonly float bottomBufferUnits;

		public readonly bool nonDefault;

		public Initialization(CCTotemPole totemPole, CCDesc figurePrefab, float minHeight, float maxHeight, float initialHeight, float bottomBufferUnits)
		{
			this.totemPole = totemPole;
			this.figurePrefab = figurePrefab;
			this.minHeight = minHeight;
			this.maxHeight = maxHeight;
			this.initialHeight = initialHeight;
			this.bottomBufferUnits = bottomBufferUnits;
			this.nonDefault = true;
		}

		public override string ToString()
		{
			return CCTotem.ToStringHelper<CCTotem.Initialization>.GetString(this);
		}
	}

	public struct MoveInfo
	{
		public readonly CollisionFlags CollisionFlags;

		public readonly CollisionFlags WorkingCollisionFlags;

		public readonly float WantedHeight;

		public readonly Vector3 BottomMovement;

		public readonly Vector3 TopMovement;

		public readonly CCTotem.PositionPlacement PositionPlacement;

		public MoveInfo(UnityEngine.CollisionFlags CollisionFlags, UnityEngine.CollisionFlags WorkingCollisionFlags, float WantedHeight, Vector3 BottomMovement, Vector3 TopMovement, CCTotem.PositionPlacement PositionPlacement)
		{
			this.CollisionFlags = CollisionFlags;
			this.WorkingCollisionFlags = WorkingCollisionFlags;
			this.WantedHeight = WantedHeight;
			this.BottomMovement = BottomMovement;
			this.TopMovement = TopMovement;
			this.PositionPlacement = PositionPlacement;
		}
	}

	public delegate void PositionBinder(ref CCTotem.PositionPlacement binding, object Tag);

	public struct PositionPlacement
	{
		public Vector3 bottom;

		public Vector3 top;

		public Vector3 colliderCenter;

		public float height;

		public float originalHeight;

		public Vector3 originalTop;

		public PositionPlacement(Vector3 Bottom, Vector3 Top, Vector3 ColliderPosition, float OriginalHeight)
		{
			this.bottom = Bottom;
			this.top = Top;
			this.colliderCenter = ColliderPosition;
			this.height = Top.y - Bottom.y;
			this.originalHeight = OriginalHeight;
			this.originalTop.x = Bottom.x;
			this.originalTop.y = Bottom.y + OriginalHeight;
			this.originalTop.z = Bottom.z;
		}
	}

	protected internal struct Route
	{
		public readonly CCTotem.Direction Up;

		public readonly CCTotem.Direction At;

		public readonly CCTotem.Direction Down;

		public IEnumerable<CCTotem.TotemicFigure> EnumerateDown
		{
			get
			{
				CCTotem.Route.<>c__Iterator2A variable = null;
				return variable;
			}
		}

		public IEnumerable<CCTotem.TotemicFigure> EnumerateDownInclusive
		{
			get
			{
				CCTotem.Route.<>c__Iterator29 variable = null;
				return variable;
			}
		}

		public IEnumerable<CCTotem.TotemicFigure> EnumerateUp
		{
			get
			{
				CCTotem.Route.<>c__Iterator28 variable = null;
				return variable;
			}
		}

		public IEnumerable<CCTotem.TotemicFigure> EnumerateUpInclusive
		{
			get
			{
				CCTotem.Route.<>c__Iterator27 variable = null;
				return variable;
			}
		}

		public Route(CCTotem.Direction Up, CCTotem.Direction At, CCTotem.Direction Down)
		{
			this.Up = Up;
			this.At = At;
			this.Down = Down;
		}

		public bool GetDown(out CCTotem.Route route)
		{
			if (!this.Down.Exists)
			{
				route = new CCTotem.Route(this.At, CCTotem.Direction.None, CCTotem.Direction.None);
				return false;
			}
			route = *(this.Down.TotemicFigure);
			return true;
		}

		public bool GetUp(out CCTotem.Route route)
		{
			if (!this.Up.Exists)
			{
				route = new CCTotem.Route(CCTotem.Direction.None, CCTotem.Direction.None, this.At);
				return false;
			}
			route = *(this.Up.TotemicFigure);
			return true;
		}

		public override string ToString()
		{
			return string.Format("{{Up={0},At={1},Down={2}}}", this.Up, this.At, this.Down);
		}
	}

	private static class ToStringHelper<T>
	where T : struct
	{
		private readonly static FieldInfo[] allFields;

		private readonly static object[] valueHolders;

		private readonly static string formatterString;

		static ToStringHelper()
		{
			CCTotem.ToStringHelper<T>.allFields = typeof(T).GetFields();
			CCTotem.ToStringHelper<T>.valueHolders = new object[(int)CCTotem.ToStringHelper<T>.allFields.Length];
			StringBuilder stringBuilder = new StringBuilder();
			using (StringWriter stringWriter = new StringWriter(stringBuilder))
			{
				stringWriter.Write("{{");
				bool flag = true;
				for (int i = 0; i < (int)CCTotem.ToStringHelper<T>.allFields.Length; i++)
				{
					if (!flag)
					{
						stringWriter.Write(", ");
					}
					else
					{
						flag = false;
					}
					stringWriter.Write("{0}={{{1}}}", CCTotem.ToStringHelper<T>.allFields[i].Name, i);
				}
				stringWriter.Write("}}");
			}
			CCTotem.ToStringHelper<T>.formatterString = stringBuilder.ToString();
		}

		public static string GetString(object boxed)
		{
			string str;
			try
			{
				for (int i = 0; i < (int)CCTotem.ToStringHelper<T>.allFields.Length; i++)
				{
					CCTotem.ToStringHelper<T>.valueHolders[i] = CCTotem.ToStringHelper<T>.allFields[i].GetValue(boxed);
				}
				str = string.Format(CCTotem.ToStringHelper<T>.formatterString, CCTotem.ToStringHelper<T>.valueHolders);
			}
			finally
			{
				for (int j = 0; j < (int)CCTotem.ToStringHelper<T>.allFields.Length; j++)
				{
					CCTotem.ToStringHelper<T>.valueHolders[j] = null;
				}
			}
			return str;
		}
	}

	public sealed class TotemicFigure : CCTotem.TotemicObject<CCTotemicFigure, CCTotem.TotemicFigure>
	{
		public CCDesc CCDesc;

		internal readonly CCTotem.TotemPole TotemPole;

		internal readonly int BottomUpIndex;

		internal readonly int TopDownIndex;

		internal readonly CollisionFlags CollisionFlagsMask;

		internal readonly CCTotem.Route TotemicRoute;

		internal readonly CCTotem.Contraction TotemContractionTop;

		internal readonly CCTotem.Contraction TotemContractionBottom;

		public Vector3 PreSweepBottom;

		public Vector3 PostSweepBottom;

		public Vector3 SweepMovement;

		public Vector3 BottomOrigin
		{
			get
			{
				return this.CCDesc.worldSkinnedBottom;
			}
		}

		public Vector3 CenterOrigin
		{
			get
			{
				return this.CCDesc.worldCenter;
			}
		}

		public Vector3 SlideBottomOrigin
		{
			get
			{
				return this.CCDesc.OffsetToWorld(this.CCDesc.center - new Vector3(0f, this.CCDesc.effectiveSkinnedHeight * 0.5f - this.CCDesc.skinnedRadius, 0f));
			}
		}

		public Vector3 SlideTopOrigin
		{
			get
			{
				return this.CCDesc.OffsetToWorld(this.CCDesc.center + new Vector3(0f, this.CCDesc.effectiveSkinnedHeight * 0.5f - this.CCDesc.skinnedRadius, 0f));
			}
		}

		public Vector3 TopOrigin
		{
			get
			{
				return this.CCDesc.worldSkinnedTop;
			}
		}

		[Obsolete("Infrastructure", true)]
		public TotemicFigure()
		{
			throw new NotSupportedException();
		}

		private TotemicFigure(CCTotem.TotemPole TotemPole, int BottomUpIndex)
		{
			this.TotemPole = TotemPole;
			this.BottomUpIndex = BottomUpIndex;
			this.TopDownIndex = this.TotemPole.Configuration.numRequiredTotemicFigures - (this.BottomUpIndex + 1);
			this.CollisionFlagsMask = CollisionFlags.Sides;
			if (this.BottomUpIndex == 0)
			{
				CCTotem.TotemicFigure collisionFlagsMask = this;
				collisionFlagsMask.CollisionFlagsMask = collisionFlagsMask.CollisionFlagsMask | CollisionFlags.Below;
			}
			if (this.TopDownIndex == 0)
			{
				CCTotem.TotemicFigure totemicFigure = this;
				totemicFigure.CollisionFlagsMask = totemicFigure.CollisionFlagsMask | CollisionFlags.Above;
			}
			this.TotemPole.TotemicFigures[this.BottomUpIndex] = this;
		}

		private TotemicFigure(CCTotem.Direction Down) : this(Down.TotemicFigure.TotemPole, Down.TotemicFigure.BottomUpIndex + 1)
		{
			CCTotem.Direction direction;
			float bottomUpIndex = (float)this.BottomUpIndex / (float)this.TotemPole.Configuration.numSlidingTotemicFigures;
			float single = (this.TotemPole.Configuration.numSlidingTotemicFigures != 1 ? (float)(this.BottomUpIndex - 1) / (float)(this.TotemPole.Configuration.numSlidingTotemicFigures - 1) : bottomUpIndex);
			float single1 = Mathf.Lerp(this.TotemPole.Configuration.poleBottomBufferAmount, this.TotemPole.Configuration.poleContractedHeight - this.TotemPole.Configuration.figureSkinnedHeight, single);
			float single2 = Mathf.Lerp(this.TotemPole.Configuration.poleBottomBufferAmount, this.TotemPole.Configuration.poleExpandedHeight - this.TotemPole.Configuration.figureSkinnedHeight, bottomUpIndex);
			this.TotemContractionBottom = CCTotem.Contraction.Define(single1, single2);
			this.TotemContractionTop = CCTotem.Contraction.Define(single1 + this.TotemPole.Configuration.figureSkinnedHeight, single2 + this.TotemPole.Configuration.figureSkinnedHeight);
			CCTotem.Direction direction1 = new CCTotem.Direction(this);
			direction = (this.BottomUpIndex >= this.TotemPole.Configuration.numRequiredTotemicFigures - 1 ? CCTotem.Direction.None : new CCTotem.Direction(new CCTotem.TotemicFigure(direction1)));
			this.TotemicRoute = new CCTotem.Route(Down, direction1, direction);
		}

		private TotemicFigure(CCTotem.TotemPole TotemPole) : this(TotemPole, 0)
		{
			CCTotem.Direction direction = new CCTotem.Direction(this);
			this.TotemicRoute = new CCTotem.Route(CCTotem.Direction.None, direction, new CCTotem.Direction(new CCTotem.TotemicFigure(direction)));
		}

		internal override void AssignedToScript(CCTotemicFigure Script)
		{
			this.Script = Script;
		}

		internal static CCTotem.Ends<CCTotem.TotemicFigure> CreateAllTotemicFigures(CCTotem.TotemPole TotemPole)
		{
			if (!object.ReferenceEquals(TotemPole.TotemicFigures[0], null))
			{
				throw new ArgumentException("The totem pole already has totemic figures", "TotemPole");
			}
			CCTotem.TotemicFigure totemicFigure = new CCTotem.TotemicFigure(TotemPole);
			CCTotem.TotemicFigure totemicFigures = TotemPole.TotemicFigures[TotemPole.Configuration.numRequiredTotemicFigures - 1];
			return new CCTotem.Ends<CCTotem.TotemicFigure>(totemicFigure, totemicFigures);
		}

		internal void Delete(CCTotemPole OwnerScript)
		{
			CCTotemicFigure script = this.Script;
			this.Script = null;
			if (script && object.ReferenceEquals(script.totemicObject, this))
			{
				script.totemicObject = null;
			}
			CCTotem.DestroyCCDesc(OwnerScript, ref this.CCDesc);
			if (script)
			{
				UnityEngine.Object.Destroy(script.gameObject);
			}
			if (object.ReferenceEquals(this.TotemPole.TotemicFigures[this.BottomUpIndex], this))
			{
				this.TotemPole.TotemicFigures[this.BottomUpIndex] = null;
			}
		}

		public CollisionFlags MoveSweep(Vector3 motion)
		{
			this.PreSweepBottom = this.BottomOrigin;
			CollisionFlags collisionFlag = this.MoveWorld(motion);
			this.PostSweepBottom = this.BottomOrigin;
			this.SweepMovement = this.PostSweepBottom - this.PreSweepBottom;
			return collisionFlag;
		}

		public CollisionFlags MoveWorld(Vector3 motion)
		{
			return this.CCDesc.Move(motion);
		}

		public CollisionFlags MoveWorldBottomTo(Vector3 targetBottom)
		{
			return this.MoveWorld(targetBottom - this.BottomOrigin);
		}

		public CollisionFlags MoveWorldTopTo(Vector3 targetTop)
		{
			return this.MoveWorld(targetTop - this.TopOrigin);
		}

		internal override void OnScriptDestroy(CCTotemicFigure Script)
		{
			if (object.ReferenceEquals(this.Script, Script))
			{
				this.Script = null;
				if (object.ReferenceEquals(Script.totemicObject, this))
				{
					Script.totemicObject = null;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("{{Index={0},ContractionBottom={1},ContractionTop={2},Script={3}}}", new object[] { this.BottomUpIndex, this.TotemContractionBottom, this.TotemContractionTop, this.Script });
		}
	}

	public abstract class TotemicObject
	{
		internal abstract CCTotem _Script
		{
			get;
		}

		protected internal CCTotem Script
		{
			get
			{
				return this._Script;
			}
		}

		internal TotemicObject()
		{
		}
	}

	public abstract class TotemicObject<CCTotemScript> : CCTotem.TotemicObject
	where CCTotemScript : CCTotem, new()
	{
		protected internal CCTotemScript Script;

		internal sealed override CCTotem _Script
		{
			get
			{
				return (object)this.Script;
			}
		}

		internal TotemicObject()
		{
		}
	}

	public abstract class TotemicObject<CCTotemScript, TTotemicObject> : CCTotem.TotemicObject<CCTotemScript>
	where CCTotemScript : CCTotem<TTotemicObject, CCTotemScript>, new()
	where TTotemicObject : CCTotem.TotemicObject<CCTotemScript, TTotemicObject>, new()
	{
		internal TotemicObject()
		{
		}

		internal abstract void AssignedToScript(CCTotemScript Script);

		internal abstract void OnScriptDestroy(CCTotemScript Script);
	}

	public sealed class TotemPole : CCTotem.TotemicObject<CCTotemPole, CCTotem.TotemPole>
	{
		private const int kCrouch_NotModified = 0;

		private const int kCrouch_MovingDown = -1;

		private const int kCrouch_MovingUp = 1;

		internal readonly CCTotem.Configuration Configuration;

		internal readonly CCTotem.TotemicFigure[] TotemicFigures;

		internal readonly CCTotem.Ends<CCTotem.TotemicFigure> TotemicFigureEnds;

		internal readonly CCTotem.Contraction Contraction;

		internal CCTotem.Ends<Vector3> Point;

		internal CCTotem.Expansion Expansion;

		internal CCDesc CCDesc;

		private bool grounded;

		private CCDesc CCDescOrPrefab
		{
			get
			{
				return (!this.CCDesc ? this.Configuration.totem.figurePrefab : this.CCDesc);
			}
		}

		public Vector3 center
		{
			get
			{
				return this.CCDescOrPrefab.center;
			}
		}

		public CollisionFlags collisionFlags
		{
			get
			{
				return (!this.CCDesc ? CollisionFlags.None : this.CCDesc.collisionFlags);
			}
		}

		public float height
		{
			get
			{
				return this.CCDescOrPrefab.height;
			}
		}

		public bool isGrounded
		{
			get
			{
				return this.grounded;
			}
		}

		public float radius
		{
			get
			{
				return this.CCDescOrPrefab.radius;
			}
		}

		public float slopeLimit
		{
			get
			{
				return this.CCDescOrPrefab.slopeLimit;
			}
		}

		public float stepOffset
		{
			get
			{
				return this.CCDescOrPrefab.stepOffset;
			}
		}

		public Vector3 velocity
		{
			get
			{
				return (!this.CCDesc ? Vector3.zero : this.CCDesc.velocity);
			}
		}

		[Obsolete("Infrastructure", true)]
		public TotemPole()
		{
			throw new NotSupportedException();
		}

		internal TotemPole(ref CCTotem.Configuration TotemConfiguration)
		{
			this.Configuration = TotemConfiguration;
			this.TotemicFigures = new CCTotem.TotemicFigure[8];
			this.TotemicFigureEnds = CCTotem.TotemicFigure.CreateAllTotemicFigures(this);
			this.Contraction = CCTotem.Contraction.Define(this.Configuration.poleContractedHeight, this.Configuration.poleExpandedHeight);
		}

		internal override void AssignedToScript(CCTotemPole Script)
		{
			this.Script = Script;
		}

		public void Create()
		{
			float configuration = this.Configuration.totem.initialHeight;
			this.Expansion = this.Contraction.ExpansionForValue(configuration);
			Vector3 script = this.Script.transform.position + this.Configuration.figureOriginOffsetBottom;
			this.CCDesc = this.InstantiateCCDesc(script, "__TotemPole");
			if (this.Script)
			{
				this.Script.ExecuteBinding(this.CCDesc, true);
			}
			for (int i = 0; i < this.Configuration.numRequiredTotemicFigures; i++)
			{
				this.InstantiateTotemicFigure(script, this.TotemicFigures[i]);
			}
			this.Point.Bottom = this.TotemicFigures[0].BottomOrigin;
			CCTotem.Ends<CCTotem.TotemicFigure> totemicFigureEnds = this.TotemicFigureEnds;
			this.Point.Top = totemicFigureEnds.Top.TopOrigin;
			this.CCDesc.ModifyHeight(this.Point.Top.y - this.Point.Bottom.y, false);
		}

		private void DeleteAllFiguresAndClearScript()
		{
			CCTotemPole script = this.Script;
			this.Script = null;
			for (int i = this.Configuration.numRequiredTotemicFigures - 1; i >= 0; i--)
			{
				CCTotem.TotemicFigure totemicFigures = this.TotemicFigures[i];
				if (!object.ReferenceEquals(totemicFigures, null))
				{
					if (totemicFigures.TotemPole != this)
					{
						this.TotemicFigures[i] = null;
					}
					else
					{
						this.TotemicFigures[i].Delete(script);
					}
				}
			}
			CCTotem.DestroyCCDesc(script, ref this.CCDesc);
			if (script && object.ReferenceEquals(script.totemicObject, this))
			{
				script.totemicObject = null;
			}
		}

		private CCDesc InstantiateCCDesc(Vector3 worldBottom, string name)
		{
			CCTotem.Initialization configuration = this.Configuration.totem;
			CCDesc cCDesc = (CCDesc)UnityEngine.Object.Instantiate(configuration.figurePrefab, worldBottom, Quaternion.identity);
			if (!string.IsNullOrEmpty(name))
			{
				cCDesc.name = name;
			}
			cCDesc.gameObject.hideFlags = HideFlags.NotEditable;
			cCDesc.detectCollisions = false;
			return cCDesc;
		}

		private CCTotemicFigure InstantiateTotemicFigure(Vector3 worldBottom, CCTotem.TotemicFigure target)
		{
			float single = worldBottom.y;
			CCTotem.Expansion expansion = target.TotemContractionBottom.ExpansionForFraction(this.Expansion.FractionExpanded);
			worldBottom.y = single + expansion.Value;
			target.CCDesc = this.InstantiateCCDesc(worldBottom, string.Format("__TotemicFigure{0}", target.BottomUpIndex));
			CCTotemicFigure cCTotemicFigure = target.CCDesc.gameObject.AddComponent<CCTotemicFigure>();
			cCTotemicFigure.AssignTotemicObject(target);
			if (this.Script)
			{
				this.Script.ExecuteBinding(target.CCDesc, true);
			}
			return cCTotemicFigure;
		}

		public CCTotem.MoveInfo Move(Vector3 motion, float height)
		{
			CCTotem.Expansion expansion = this.Contraction.ExpansionForValue(height);
			height = expansion.Value;
			CollisionFlags collisionFlagsMask = this.TotemicFigureEnds.Bottom.MoveSweep(motion) & this.TotemicFigureEnds.Bottom.CollisionFlagsMask;
			this.grounded = this.TotemicFigureEnds.Bottom.CCDesc.isGrounded;
			int num = 0;
			for (int i = this.Configuration.numRequiredTotemicFigures - 1; i >= 1; i--)
			{
				Vector3 sweepMovement = this.TotemicFigures[num].SweepMovement;
				collisionFlagsMask = collisionFlagsMask | this.TotemicFigures[i].MoveSweep(sweepMovement) & this.TotemicFigures[i].CollisionFlagsMask;
				num = i;
			}
			if (this.TotemicFigures[num].SweepMovement != this.TotemicFigures[0].SweepMovement)
			{
				Vector3 vector3 = this.TotemicFigures[num].SweepMovement;
				for (int j = 0; j < this.Configuration.numRequiredTotemicFigures; j++)
				{
					Vector3 sweepMovement1 = vector3 - this.TotemicFigures[j].SweepMovement;
					collisionFlagsMask = collisionFlagsMask | this.TotemicFigures[j].MoveSweep(sweepMovement1) & this.TotemicFigures[j].CollisionFlagsMask;
				}
			}
			this.Point.Bottom = this.TotemicFigures[0].BottomOrigin;
			CCTotem.Ends<CCTotem.TotemicFigure> totemicFigureEnds = this.TotemicFigureEnds;
			this.Point.Top = totemicFigureEnds.Top.TopOrigin;
			CCTotem.Contraction contraction = this.Contraction;
			this.Expansion = contraction.ExpansionForValue(this.Point.Top.y - this.Point.Bottom.y);
			if (this.Expansion.Value != expansion.Value)
			{
				Vector3 bottom = this.Point.Bottom + new Vector3(0f, expansion.Value, 0f);
				CollisionFlags collisionFlag = this.TotemicFigureEnds.Top.MoveWorldTopTo(bottom);
				CCTotem.Ends<CCTotem.TotemicFigure> end = this.TotemicFigureEnds;
				collisionFlagsMask = collisionFlagsMask | collisionFlag & end.Top.CollisionFlagsMask;
				Vector3 topOrigin = this.TotemicFigureEnds.Top.TopOrigin;
				CCTotem.Contraction contraction1 = this.Contraction;
				expansion = contraction1.ExpansionForValue(topOrigin.y - this.Point.Bottom.y);
				for (int k = this.Configuration.numRequiredTotemicFigures - 2; k > 0; k--)
				{
					CCTotem.TotemicFigure totemicFigures = this.TotemicFigures[k];
					Vector3 value = this.Point.Bottom;
					float single = value.y;
					CCTotem.Expansion expansion1 = totemicFigures.TotemContractionBottom.ExpansionForFraction(expansion.FractionExpanded);
					value.y = single + expansion1.Value;
					collisionFlagsMask = collisionFlagsMask | totemicFigures.MoveWorldBottomTo(value) & totemicFigures.CollisionFlagsMask;
				}
				CCTotem.Ends<CCTotem.TotemicFigure> totemicFigureEnds1 = this.TotemicFigureEnds;
				this.Point.Top = totemicFigureEnds1.Top.TopOrigin;
				this.Expansion = expansion;
			}
			float cCDesc = this.CCDesc.effectiveSkinnedHeight;
			Vector3 cCDesc1 = this.CCDesc.worldSkinnedBottom;
			Vector3 vector31 = this.CCDesc.worldSkinnedTop;
			Vector3 bottomOrigin = this.TotemicFigures[0].BottomOrigin - cCDesc1;
			this.CCDesc.ModifyHeight(this.Expansion.Value, false);
			CollisionFlags collisionFlag1 = this.CCDesc.Move(bottomOrigin);
			Vector3 cCDesc2 = this.CCDesc.worldSkinnedBottom;
			Vector3 vector32 = cCDesc2 - cCDesc1;
			if (bottomOrigin != vector32)
			{
				Vector3 vector33 = vector32 - bottomOrigin;
				for (int l = 0; l < this.Configuration.numRequiredTotemicFigures; l++)
				{
					collisionFlagsMask = collisionFlagsMask | this.TotemicFigures[l].MoveSweep(vector33) & this.TotemicFigures[l].CollisionFlagsMask;
				}
				this.Point.Bottom = this.TotemicFigures[0].BottomOrigin;
				CCTotem.Ends<CCTotem.TotemicFigure> end1 = this.TotemicFigureEnds;
				this.Point.Top = end1.Top.TopOrigin;
				CCTotem.Contraction contraction2 = this.Contraction;
				this.Expansion = contraction2.ExpansionForValue(this.Point.Top.y - this.Point.Bottom.y);
				this.CCDesc.ModifyHeight(this.Expansion.Value, false);
				cCDesc2 = this.CCDesc.worldSkinnedBottom;
				vector32 = cCDesc2 - cCDesc1;
			}
			Vector3 cCDesc3 = this.CCDesc.worldSkinnedTop;
			Vector3 vector34 = cCDesc3 - vector31;
			Vector3 cCDesc4 = this.CCDesc.transform.position;
			CCTotem.Configuration configuration = this.Configuration;
			CCTotem.PositionPlacement positionPlacement = new CCTotem.PositionPlacement(cCDesc2, cCDesc3, cCDesc4, configuration.poleExpandedHeight);
			return new CCTotem.MoveInfo(collisionFlag1, collisionFlagsMask, height, vector32, vector34, positionPlacement);
		}

		internal override void OnScriptDestroy(CCTotemPole Script)
		{
			if (object.ReferenceEquals(this.Script, Script))
			{
				this.DeleteAllFiguresAndClearScript();
			}
		}
	}
}