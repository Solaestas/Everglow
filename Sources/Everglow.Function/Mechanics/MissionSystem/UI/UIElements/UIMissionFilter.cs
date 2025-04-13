using Everglow.Commons.DataStructures;
using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Utilities;

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
	private float? _outerHoverTargetRotation;

	// Inner ring rotation state
	private float _innerRotation;
	private Vector2? _innerMouseDownPosition;
	private float _innerMouseClickRotation;
	private bool _innerHeld;
	private int _innerClickTimer;
	private float? _innerClickTargetRotation;
	private float? _innerHoverTargetRotation;

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

	private static MissionType? RotationToMissionType(float rotation)
	{
		var unit = MathHelper.TwoPi / MissionTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = (int)Math.Round(standard / unit) % MissionTypeList.Count;
		return MissionTypeList[index];
	}

	private static MissionType? RotationToMissionTypeCheckGemMisalignment(float rotation)
	{
		var unit = MathHelper.TwoPi / MissionTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = (int)Math.Round(standard / unit) % MissionTypeList.Count;
		float angularMisalignment = MathF.Abs((standard + unit * 0.5f) % unit - unit * 0.5f);

		// TODO: A new kind of MissionType, only trigger when laser can't pass a gem in the turntable.
		// PoolType as well.
		if (angularMisalignment > 0.05f)
		{
			index = 1;
		}
		return MissionTypeList[index];
	}

	private static PoolType? RotationToPoolType(float rotation)
	{
		var unit = MathHelper.TwoPi / PoolTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = (int)Math.Round(standard / unit) % PoolTypeList.Count;
		return PoolTypeList[index];
	}

	private static PoolType? RotationToPoolTypeCheckGemMisalignment(float rotation)
	{
		var unit = MathHelper.TwoPi / PoolTypeList.Count;
		var standard = ((rotation % MathHelper.TwoPi) + MathHelper.TwoPi) % MathHelper.TwoPi;
		var index = (int)Math.Round(standard / unit) % PoolTypeList.Count;
		float angularMisalignment = MathF.Abs((standard + unit * 0.5f) % unit - unit * 0.5f);

		// TODO: A new kind of PoolType, only trigger when laser can't pass a gem in the turntable.
		// if (angularMisalignment > 0.05f)
		// {
		// index = 1;
		// }
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

	/// <summary>
	/// Calculate the nearest snap rotation based on the current rotation and the count of options.
	/// </summary>
	/// <param name="currentRotation"></param>
	/// <param name="optionCount"></param>
	/// <returns></returns>
	/// <exception cref="InvalidDataException"></exception>
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

	public override void OnInitialization()
	{
		base.OnInitialization();

		var scale = MissionContainer.Scale;

		Info.Width.SetValue(350 * scale);
		Info.Height.SetValue(350 * scale);

		Events.OnMouseHover += ManageHeldLogic;
	}

	public override void Update(GameTime gt)
	{
		// Update mission type
		var missionType = RotationToMissionTypeCheckGemMisalignment(_outerRotation);
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

		var distanceToCenter = Main.MouseScreen.Distance(HitBox.Center.ToVector2());

		// Outer ring hover logic: show mission type text and highlight the selected mission type
		if (DistanceWithinOuterRing(distanceToCenter))
		{
			var hoverMissionType = RotationToMissionType(MathHelper.Pi - MouseRotation + _outerRotation);
			_outerHoverTargetRotation = MissionTypeToRotation(hoverMissionType);
			MissionContainer.Instance.MouseText = hoverMissionType?.ToString() ?? "All";
		}
		else
		{
			_outerHoverTargetRotation = null;
		}

		// Inner ring hover logic: show mission type text and highlight the selected mission type
		if (DistanceWithinInnerRing(distanceToCenter))
		{
			var hoverPoolType = RotationToPoolType(MathHelper.Pi - MouseRotation + _innerRotation);
			_innerHoverTargetRotation = PoolTypeToRotation(hoverPoolType);
			MissionContainer.Instance.MouseText = hoverPoolType?.ToString() ?? "All";
		}
		else
		{
			_innerHoverTargetRotation = null;
		}
	}

	/// <summary>
	/// Manage the ring held logic: rotate the inner and outer ring with mouse movement when held.
	/// </summary>
	/// <param name="baseElement"></param>
	private void ManageHeldLogic(BaseElement baseElement)
	{
		// Reset held state when mouse is not clicking
		var mouseLeftClicking = Main.mouseLeft && !Main.mouseLeftRelease;
		if (!mouseLeftClicking)
		{
			_innerHeld = false;
			_outerHeld = false;
		}

		var distanceToCenter = Main.MouseScreen.Distance(HitBox.Center.ToVector2());

		// Outer ring held logic
		if (((DistanceWithinOuterRing(distanceToCenter) && mouseLeftClicking) || _outerHeld) && !_innerHeld)
		{
			RotateRingWithMouse(ref _outerMouseDownPosition, ref _outerMouseClickRotation, ref _outerRotation, ref _outerHeld);
		}
		else
		{
			// Outer on release
			ResetHeldState(ref _outerMouseDownPosition);
		}

		// Inner ring held logic
		if (((DistanceWithinInnerRing(distanceToCenter) && mouseLeftClicking) || _innerHeld) && !_outerHeld)
		{
			RotateRingWithMouse(ref _innerMouseDownPosition, ref _innerMouseClickRotation, ref _innerRotation, ref _innerHeld);
		}
		else
		{
			// Inner on release
			ResetHeldState(ref _innerMouseDownPosition);
		}
	}

	/// <summary>
	/// Clear the ring mouse held state when released.
	/// </summary>
	/// <param name="mouseDownPosition"></param>
	private void ResetHeldState(ref Vector2? mouseDownPosition)
	{
		mouseDownPosition = null;
	}

	/// <summary>
	/// Rotate the ring with mouse movement when held.
	/// </summary>
	/// <param name="mouseDownPosition"></param>
	/// <param name="mouseClickRotation"></param>
	/// <param name="rotation"></param>
	/// <param name="held"></param>
	private void RotateRingWithMouse(ref Vector2? mouseDownPosition, ref float mouseClickRotation, ref float rotation, ref bool held)
	{
		if (mouseDownPosition == null)
		{
			mouseDownPosition = Main.MouseScreen;
			mouseClickRotation = rotation;
			held = true;
		}

		var enterAngle = HitBox.Center.ToVector2().AngleTo(mouseDownPosition.Value);
		rotation = mouseClickRotation + MouseRotation - enterAngle;
	}

	/// <summary>
	/// Rotate the ring to the target rotation.
	/// </summary>
	/// <param name="targetRotation"></param>
	/// <param name="currentRotation"></param>
	private void RotateRingTo(float targetRotation, ref float currentRotation)
	{
		var rotationDiff = GetNaturalRotation(targetRotation, currentRotation);

		if (MathF.Abs(rotationDiff) > RotationSnapThreshold)
		{
			currentRotation += rotationDiff / 10;
		}
		else
		{
			currentRotation += rotationDiff;
		}
	}

	/// <summary>
	/// Manage auto-rotation of two rings.
	/// <para/>The auto-rotation is consist of two part: click-to-select and fix rotation.
	/// </summary>
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

			RotateRingTo(_innerClickTargetRotation.Value, ref _innerRotation);
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

			RotateRingTo(_outerClickTargetRotation.Value, ref _outerRotation);
		}
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		var drawPos = new Vector2(Info.HitBox.X + Info.HitBox.Width / 2, Info.HitBox.Y + Info.HitBox.Height / 2);
		var scale = MissionContainer.Scale;

		var typeTexture = ModAsset.MissionClassificationMarbleRing.Value;
		sb.Draw(typeTexture, drawPos, null, Color.White, _outerRotation, typeTexture.Size() / 2, scale, SpriteEffects.None, 0);

		SpriteBatchState sBS = GraphicsUtils.GetState(sb).Value;
		sb.End();
		sb.Begin(sBS);
		sb.sortMode = SpriteSortMode.Immediate;
		sb.blendState = BlendState.AlphaBlend;
		Effect goldShader = ModAsset.GoldenReflection.Value;
		goldShader.Parameters["sv_Pos_Y"].SetValue(drawPos.Y);
		goldShader.Parameters["uSize"].SetValue(0.006f);
		goldShader.Parameters["uHeatMap"].SetValue(ModAsset.GoldenHue.Value);
		goldShader.CurrentTechnique.Passes[0].Apply();
		var typeGlow = ModAsset.GoldRingTexture.Value;
		sb.Draw(typeGlow, drawPos, null, Color.White, _outerRotation, typeGlow.Size() / 2, scale, SpriteEffects.None, 0);
		sb.End();
		sb.Begin(sBS);

		var statusFilter = ModAsset.MissionDurationMarbleRing.Value;
		sb.Draw(statusFilter, drawPos, null, Color.White, _innerRotation, statusFilter.Size() / 2, scale, SpriteEffects.None, 0);

		if (_innerHoverTargetRotation != null)
		{
			var statusFilter_Selected = ModAsset.MissionDurationMarbleRing_Seleted.Value;
			sb.Draw(statusFilter_Selected, drawPos, null, Color.White, _innerRotation - _innerHoverTargetRotation.Value, statusFilter_Selected.Size() / 2, scale, SpriteEffects.None, 0);
		}
		if (_outerHoverTargetRotation != null)
		{
			var typeTexture_Selected = ModAsset.MissionClassificationMarbleRing_Selected.Value;
			sb.Draw(typeTexture_Selected, drawPos, null, Color.White, _outerRotation - _outerHoverTargetRotation.Value, typeTexture_Selected.Size() / 2, scale, SpriteEffects.None, 0);
		}
	}
}