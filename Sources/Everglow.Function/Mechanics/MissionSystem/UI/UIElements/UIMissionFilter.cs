using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionFilter : BaseElement
{
	private const int MouseHoldFramesLimitForAutoRotation = 8;
	private const float RotationSnapThreshold = 0.01f;

	// Outer ring rotation state
	private float _outerRotation;
	private Vector2? _outerMouseDownPosition;
	private float _outerMouseClickRotation;
	private bool _outerHeld;
	private int _outerClickTimer;
	private float? _outerClickTargetRotation;

	// Inner ring rotation state
	private float _innerRotation;
	private Vector2? _innerMouseDownPosition;
	private float _innerMouseClickRotation;
	private bool _innerHeld;
	private int _innerClickTimer;
	private float? _innerClickTargetRotation;

	/// <summary>
	/// Available pool type selections (null represents "All" option)
	/// </summary>
	private static List<PoolType?> PoolTypeList { get; } = [null, .. Enum.GetValues<PoolType>()];

	/// <summary>
	/// Available mission type selections (null represents "All" option)
	/// </summary>
	private static List<MissionType?> MissionTypeList { get; } = [null, .. Enum.GetValues<MissionType>()];

	/// <summary>
	/// Currently selected pool type filter
	/// </summary>
	public PoolType? PoolTypeValue { get; private set; }

	/// <summary>
	/// Currently selected mission type filter
	/// </summary>
	public MissionType? MissionTypeValue { get; private set; }

	private float MouseRotation => HitBox.Center.ToVector2().AngleTo(Main.MouseScreen);

	#region Statics

	private static MissionType? RotationToMissionType(float rotation)
	{
		var unit = MathHelper.TwoPi / MissionTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = (int)Math.Round(standard / unit) % MissionTypeList.Count;
		return MissionTypeList[index];
	}

	private static PoolType? RotationToPoolType(float rotation)
	{
		var unit = MathHelper.TwoPi / PoolTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = (int)Math.Round(standard / unit) % PoolTypeList.Count;
		return PoolTypeList[index];
	}

	private static float MissionTypeToRotation(MissionType? type) => MissionTypeList.IndexOf(type) * MathHelper.TwoPi / MissionTypeList.Count;

	private static float PoolTypeToRotation(PoolType? type) => PoolTypeList.IndexOf(type) * MathHelper.TwoPi / PoolTypeList.Count;

	private static bool DistanceWithinInnerRing(float distance)
	{
		var innerRadius1 = 229 / 2f;
		innerRadius1 *= MissionContainer.Scale;
		var innerRadius2 = 229 / 2f - 42;
		innerRadius2 *= MissionContainer.Scale;
		return distance < innerRadius1 && distance > innerRadius2;
	}

	private static bool DistanceWithinOuterRing(float distance)
	{
		var outerRadius1 = 337 / 2f;
		outerRadius1 *= MissionContainer.Scale;
		var outerRadius2 = 337 / 2f - 48;
		outerRadius2 *= MissionContainer.Scale;
		return distance < outerRadius1 && distance > outerRadius2;
	}

	private static float GetNaturalRotation(float targetRotation, float originRotation)
	{
		var rotationDiff = (targetRotation - originRotation) % MathHelper.TwoPi;
		if (MathF.Abs(rotationDiff) > MathF.PI)
		{
			if (rotationDiff > 0)
			{
				rotationDiff -= MathHelper.TwoPi;
			}
			else if (rotationDiff < 0)
			{
				rotationDiff += MathHelper.TwoPi;
			}
		}

		return rotationDiff;
	}

	private static float CalculateNearestSnapRotation(float currentRotation, float optionCount)
	{
		if (optionCount <= 0)
		{
			throw new InvalidDataException();
		}

		var unit = MathHelper.TwoPi / optionCount;
		var diff = currentRotation % MathHelper.TwoPi % unit;

		if (diff > 0)
		{
			if (diff > unit / 2)
			{
				diff = unit - diff;
			}
			else
			{
				diff = -diff;
			}
		}
		else if (diff < 0)
		{
			if (diff < -unit / 2)
			{
				diff = -unit - diff;
			}
			else
			{
				diff = -diff;
			}
		}
		return currentRotation + diff;
	}

	#endregion

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
			_innerHeld = false;
			_outerHeld = false;
		}

		var scale = MissionContainer.Scale;

		var mouseClicking = Main.mouseLeft && !Main.mouseLeftRelease;
		var distanceToCenter = Main.MouseScreen.Distance(HitBox.Center.ToVector2());

		if (((DistanceWithinOuterRing(distanceToCenter) && mouseClicking) || _outerHeld) && !_innerHeld)
		{
			OuterOnHover();
		}
		else
		{
			OuterOnOut();
		}

		if (((DistanceWithinInnerRing(distanceToCenter) && mouseClicking) || _innerHeld) && !_outerHeld)
		{
			InnerOnHover();
		}
		else
		{
			InnerOnOut();
		}
	}

	private void OuterOnOut()
	{
		_outerMouseDownPosition = null;
	}

	private void InnerOnOut()
	{
		_innerMouseDownPosition = null;
	}

	private void OuterOnHover()
	{
		if (_outerMouseDownPosition == null)
		{
			_outerMouseDownPosition = Main.MouseScreen;
			_outerMouseClickRotation = _outerRotation;
			_outerHeld = true;
		}

		var enterAngle = HitBox.Center.ToVector2().AngleTo(_outerMouseDownPosition.Value);
		_outerRotation = _outerMouseClickRotation + MouseRotation - enterAngle;
	}

	private void InnerOnHover()
	{
		if (_innerMouseDownPosition == null)
		{
			_innerMouseDownPosition = Main.MouseScreen;
			_innerMouseClickRotation = _innerRotation;
			_innerHeld = true;
		}

		var enterAngle = HitBox.Center.ToVector2().AngleTo(_innerMouseDownPosition.Value);
		_innerRotation = _innerMouseClickRotation + MouseRotation - enterAngle;
	}

	private void OuterRotatedTo(float rotation)
	{
		var rotationDiff = GetNaturalRotation(rotation, _outerRotation);

		if (MathF.Abs(rotationDiff) > RotationSnapThreshold)
		{
			_outerRotation += rotationDiff / 10;
		}
		else
		{
			_outerRotation += rotationDiff;
		}
	}

	private void InnerRotatedTo(float rotation)
	{
		var rotationDiff = GetNaturalRotation(rotation, _innerRotation);

		if (MathF.Abs(rotationDiff) > RotationSnapThreshold)
		{
			_innerRotation += rotationDiff / 10;
		}
		else
		{
			_innerRotation += rotationDiff;
		}
	}

	public override void Update(GameTime gt)
	{
		// Update mission type
		var missionType = RotationToMissionType(_outerRotation);
		if (MissionTypeValue != missionType)
		{
			MissionTypeValue = missionType;
			MissionManager.NeedRefresh = true;
		}

		// Update pool type
		var poolType = RotationToPoolType(_innerRotation);
		if (PoolTypeValue != poolType)
		{
			PoolTypeValue = poolType;
			MissionManager.NeedRefresh = true;
		}

		ManageAutoRotation();
	}

	private void ManageAutoRotation()
	{
		// Inner spin
		if (_innerHeld)
		{
			_innerClickTimer++;
			_innerClickTargetRotation = null;
		}
		else
		{
			if (_innerClickTimer != 0 && _innerClickTimer < MouseHoldFramesLimitForAutoRotation)
			{
				var clickedPoolType = RotationToPoolType(MathHelper.Pi - MouseRotation + _innerRotation);
				_innerClickTargetRotation = PoolTypeToRotation(clickedPoolType);
			}

			_innerClickTimer = 0;

			// If there's no click target rotation, then fix the rotation to nearest snap.
			_innerClickTargetRotation ??= CalculateNearestSnapRotation(_innerRotation, PoolTypeList.Count);

			InnerRotatedTo(_innerClickTargetRotation.Value);
		}

		// Outer spin
		if (_outerHeld)
		{
			_outerClickTimer++;
			_outerClickTargetRotation = null;
		}
		else
		{
			if (_outerClickTimer != 0 && _outerClickTimer < MouseHoldFramesLimitForAutoRotation)
			{
				var clickedMissionType = RotationToMissionType(MathHelper.Pi - MouseRotation + _outerRotation);
				_outerClickTargetRotation = MissionTypeToRotation(clickedMissionType);
			}

			_outerClickTimer = 0;

			// If there's no click target rotation, then fix the rotation to nearest snap.
			_outerClickTargetRotation ??= CalculateNearestSnapRotation(_outerRotation, MissionTypeList.Count);

			OuterRotatedTo(_outerClickTargetRotation.Value);
		}
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		var drawPos = new Vector2(Info.HitBox.X + Info.HitBox.Width / 2, Info.HitBox.Y + Info.HitBox.Height / 2);
		var scale = MissionContainer.Scale;

		var typeTexture = ModAsset.MissionClassificationMarbleRing.Value;
		sb.Draw(typeTexture, drawPos, null, Color.White, _outerRotation, typeTexture.Size() / 2, scale, SpriteEffects.None, 0);

		var typeGlow = ModAsset.GoldRingTexture.Value;
		sb.Draw(typeGlow, drawPos, null, Color.White, _outerRotation, typeGlow.Size() / 2, scale, SpriteEffects.None, 0);

		var statusFilter = ModAsset.MissionDurationMarbleRing.Value;
		sb.Draw(statusFilter, drawPos, null, Color.White, _innerRotation, statusFilter.Size() / 2, scale, SpriteEffects.None, 0);
	}
}