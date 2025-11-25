using Terraria.Audio;

namespace Everglow.Yggdrasil.Common.Elevator;

public class YggdrasilElevator : Commons.Templates.Furniture.Elevator.Elevator
{
	public bool LampOn = false;

	public override Color MapColor => new Color(122, 91, 79);

	public override string ElevatorCableTexture => ModAsset.Rope_Mod;

	public override string ElevatorTexture => ModAsset.SkyTreeLift_Mod;

	public override int ElevatorCableJointOffset => 125;

	public override void SetDefaults()
	{
		Size = new Vector2(96, 16);
	}

	public override bool PreDrawElevator(Color lightColor)
	{
		if (LampOn)
		{
			Lighting.AddLight((int)(Position.X / 16f) + 1, (int)(Position.Y / 16f) - 3, 1f, 0.8f, 0f);
		}

		return true;
	}

	public override bool PreDrawElevatorCable(Color lightColor)
	{
		Texture2D frame = LampOn
			? ModAsset.SkyTreeLiftShellLightOn.Value
			: ModAsset.SkyTreeLiftShellLightOff.Value;
		Main.spriteBatch.Draw(frame, Box.Center - Main.screenPosition + new Vector2(0, -46), null, lightColor, 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);

		var lampGlowColor = new Color(255, 255, 255, 0);
		// var lampGlowTexture = ;
		// Main.spriteBatch.Draw(lampGlowTexture, Box.Center - Main.screenPosition + new Vector2(0, -46), null, lampGlowColor, 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);

		Texture2D liftRopeTop = ModAsset.SkyTreeLiftRope.Value;
		Main.spriteBatch.Draw(liftRopeTop, Box.Center - Main.screenPosition + new Vector2(0, -110), null, lightColor, 0, new Vector2(48, 15), 1, SpriteEffects.None, 0);

		Vector2 ButtomPosition = new Vector2(-11, -33) + Box.Center;
		if ((Main.MouseWorld - ButtomPosition).Length() < 20)
		{
			if (Main.SmartCursorIsUsed)
			{
				Texture2D LiftButtomHighLight = ModAsset.SkyTreeLiftShellMiddleButtom.Value;
				if (LampOn)
				{
					LiftButtomHighLight = ModAsset.SkyTreeLiftShellMiddleButtom.Value;
				}

				Main.spriteBatch.Draw(LiftButtomHighLight, Box.Center - Main.screenPosition + new Vector2(0, -46), null, Color.White, 0, LiftButtomHighLight.Size() * 0.5f, 1, SpriteEffects.None, 0);
			}
			if (Main.mouseRight && Main.mouseRightRelease)
			{
				SoundEngine.PlaySound(SoundID.Unlock, ButtomPosition);
				LampOn = !LampOn;
			}
		}

		return true;
	}
}