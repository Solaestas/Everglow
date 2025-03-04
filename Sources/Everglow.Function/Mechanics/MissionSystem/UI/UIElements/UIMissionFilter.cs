using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionFilter : BaseElement
{
	private float outerRotation = 0;

	private float innerRotation = 0;

	/// <summary>
	/// Outer click position
	/// </summary>
	private Vector2? outerMouseClickPosition;

	/// <summary>
	/// Inner click position
	/// </summary>
	private Vector2? innerMouseClickPosition;

	/// <summary>
	/// The initial rotation of outer ring
	/// </summary>
	private float outerMouseClickRotation = 0;

	/// <summary>
	/// The initial rotation of inner ring
	/// </summary>
	private float innerMouseClickRotation = 0;

	/// <summary>
	/// If the outer ring is held
	/// </summary>
	private bool outerHolding = false;

	/// <summary>
	/// If the inner ring is held
	/// </summary>
	private bool innerHolding = false;

	public static List<PoolType?> PoolTypeList => [null, .. Enum.GetValues<PoolType>()];

	public static List<MissionType?> MissionTypeList => [null, .. Enum.GetValues<MissionType>()];

	public PoolType? PoolTypeValue { get; private set; }

	public MissionType? MissionTypeValue { get; private set; }

	public override void OnInitialization()
	{
		base.OnInitialization();

		var scale = MissionContainer.Scale;

		Info.Width.SetValue(350 * scale);
		Info.Height.SetValue(350 * scale);

		Events.OnMouseHover += Events_OnMouseHover;
	}

	private void Events_OnMouseHover(BaseElement baseElement)
	{
		if (!(Main.mouseLeft && !Main.mouseLeftRelease))
		{
			innerHolding = false;
			outerHolding = false;
		}

		var scale = MissionContainer.Scale;

		var mouseClicking = Main.mouseLeft && !Main.mouseLeftRelease;
		var distanceToCenter = Main.MouseScreen.Distance(HitBox.Center.ToVector2());

		if (((DistanceWithinOuterRing(distanceToCenter) && mouseClicking) || outerHolding) && !innerHolding)
		{
			OuterOnHover();
		}
		else
		{
			OuterOnOut();
		}

		if (((DistanceWithinInnerRing(distanceToCenter) && mouseClicking) || innerHolding) && !outerHolding)
		{
			InnerOnHover();
		}
		else
		{
			InnerOnOut();
		}
	}

	private static bool DistanceWithinInnerRing(float distance)
	{
		var innerRaidus1 = 229 / 2f;
		innerRaidus1 *= MissionContainer.Scale;
		var innerRadius2 = 229 / 2f - 42;
		innerRadius2 *= MissionContainer.Scale;
		return distance < innerRaidus1 && distance > innerRadius2;
	}

	private static bool DistanceWithinOuterRing(float distance)
	{
		var outerRadius1 = 337 / 2f;
		outerRadius1 *= MissionContainer.Scale;
		var outerRadius2 = 337 / 2f - 48;
		outerRadius2 *= MissionContainer.Scale;
		return distance < outerRadius1 && distance > outerRadius2;
	}

	private void OuterOnOut()
	{
		outerMouseClickPosition = null;
	}

	private void OuterOnHover()
	{
		if (outerMouseClickPosition == null)
		{
			outerMouseClickPosition = Main.MouseScreen;
			outerMouseClickRotation = outerRotation;
			outerHolding = true;
		}

		var enterAngle = HitBox.Center.ToVector2().AngleTo(outerMouseClickPosition.Value);
		var currentAngle = HitBox.Center.ToVector2().AngleTo(Main.MouseScreen);
		outerRotation = outerMouseClickRotation + currentAngle - enterAngle;
	}

	private void InnerOnOut()
	{
		innerMouseClickPosition = null;
	}

	private void InnerOnHover()
	{
		if (innerMouseClickPosition == null)
		{
			innerMouseClickPosition = Main.MouseScreen;
			innerMouseClickRotation = innerRotation;
			innerHolding = true;
		}

		var enterAngle = HitBox.Center.ToVector2().AngleTo(innerMouseClickPosition.Value);
		var currentAngle = HitBox.Center.ToVector2().AngleTo(Main.MouseScreen);
		innerRotation = innerMouseClickRotation + currentAngle - enterAngle;
	}

	public override void Update(GameTime gt)
	{
		// Update mission type
		var missionType = RotationToMissionType(outerRotation);
		if (MissionTypeValue != missionType)
		{
			MissionTypeValue = missionType;
			MissionManager.NeedRefresh = true;
		}

		// Update pool type
		var poolType = RotationToPoolType(innerRotation);
		if (PoolTypeValue != poolType)
		{
			PoolTypeValue = poolType;
			MissionManager.NeedRefresh = true;
		}

		if (!innerHolding)
		{
			var num = PoolTypeList.Count;
			var unit = MathHelper.TwoPi / num;
			var exceed = innerRotation % unit;
			if (exceed > unit / 2)
			{
				var delta = unit - exceed;
				if (delta > 0.01f)
				{
					innerRotation += delta / 10;
				}
				else
				{
					innerRotation += delta;
				}
			}
			else
			{
				var delta = exceed;
				if (delta > 0.01f)
				{
					innerRotation -= delta / 10;
				}
				else
				{
					innerRotation -= delta;
				}
			}
		}

		if (!outerHolding)
		{
			var num = MissionTypeList.Count;
			var unit = MathHelper.TwoPi / num;
			var exceed = outerRotation % unit;
			if (exceed > unit / 2)
			{
				var delta = unit - exceed;
				if (delta > 0.01f)
				{
					outerRotation += delta / 10;
				}
				else
				{
					outerRotation += delta;
				}
			}
			else
			{
				var delta = exceed;
				if (delta > 0.01f)
				{
					outerRotation -= delta / 10;
				}
				else
				{
					outerRotation -= delta;
				}
			}
		}
	}

	public static MissionType? RotationToMissionType(float rotation)
	{
		var unit = MathHelper.TwoPi / MissionTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = Math.Round(standard / unit) % MissionTypeList.Count;
		return index switch
		{
			0 => null,
			1 => MissionType.None,
			2 => MissionType.MainStory,
			3 => MissionType.SideStory,
			4 => MissionType.Legendary,
			5 => MissionType.Achievement,
			6 => MissionType.Daily,
			7 => MissionType.Challenge,
			_ => null,
		};
	}

	public static PoolType? RotationToPoolType(float rotation)
	{
		var unit = MathHelper.TwoPi / PoolTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = Math.Round(standard / unit) % PoolTypeList.Count;
		return index switch
		{
			0 => null,
			1 => PoolType.Accepted,
			2 => PoolType.Available,
			3 => PoolType.Failed,
			4 => PoolType.Overdue,
			5 => PoolType.Completed,
			_ => null,
		};
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		var drawPos = new Vector2(Info.HitBox.X + Info.HitBox.Width / 2, Info.HitBox.Y + Info.HitBox.Height / 2);
		var scale = MissionContainer.Scale;

		var typeTexture = ModAsset.MissionClassificationMarbleRing.Value;
		sb.Draw(typeTexture, drawPos, null, Color.White, outerRotation, typeTexture.Size() / 2, scale, SpriteEffects.None, 0);

		var typeGlow = ModAsset.GoldRingTexture.Value;
		sb.Draw(typeGlow, drawPos, null, Color.White, outerRotation, typeGlow.Size() / 2, scale, SpriteEffects.None, 0);

		var statusFilter = ModAsset.MissionDurationMarbleRing.Value;
		sb.Draw(statusFilter, drawPos, null, Color.White, innerRotation, statusFilter.Size() / 2, scale, SpriteEffects.None, 0);
	}

	protected override void DrawChildren(SpriteBatch sb)
	{
		base.DrawChildren(sb);
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		base.DrawSelf(sb);
	}
}