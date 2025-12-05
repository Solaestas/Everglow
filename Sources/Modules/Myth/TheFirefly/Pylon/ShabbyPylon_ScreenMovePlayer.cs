using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Pylon;

public class ShabbyPylon_ScreenMovePlayer : ModPlayer
{
	public int AnimationTimer = 0;
	public const float MaxTime = 600f;

	public override void ModifyScreenPosition()
	{
		if (PylonSystem.Instance.firstEnableAnimation)
		{
			var firefly = TileEntity.ByPosition.FirstOrDefault(pair => pair.Value is ShabbyPylonTileEntity);
			if (firefly.Value != default(TileEntity))
			{
				var target = firefly.Key.ToWorldCoordinates() - Main.ScreenSize.ToVector2() / 2;
				AnimationTimer++;
				float Value = (1 - MathF.Cos(AnimationTimer / 60f * MathF.PI)) / 2f;
				if (AnimationTimer >= 60 && AnimationTimer < 540)
				{
					Value = 1;
				}

				if (AnimationTimer >= 540)
				{
					Value = (1 + MathF.Cos((AnimationTimer - 540) / 60f * MathF.PI)) / 2f;
				}

				Main.screenPosition = Value.Lerp(Main.screenPosition, target);
				if (AnimationTimer >= MaxTime)
				{
					PylonSystem.Instance.firstEnableAnimation = false;
				}

				Player.immune = true;
				Player.immuneTime = 2;
			}
		}
	}
}