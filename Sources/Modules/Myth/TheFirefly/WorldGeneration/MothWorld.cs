using SubworldLibrary;
using Terraria.WorldBuilding;

namespace Everglow.Myth.TheFirefly.WorldGeneration;

internal class MothWorld : Subworld
{
	public override int Width => 800;

	public override int Height => 600;

	public override bool NormalUpdates => true;

	public override bool ShouldSave => false;

	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new MothLand.MothLandGenPass(),
	};

	public override void DrawMenu(GameTime gameTime)
	{
		Texture2D MenuUG2SBG = ModAsset.FireflyUnderground1Screen4K.Value;
		Vector2 zero = Vector2.Zero;
		float uG1Width = Main.screenWidth / (float)MenuUG2SBG.Width;
		float uG1Height = Main.screenHeight / (float)MenuUG2SBG.Height;
		if (uG1Width != uG1Height)
		{
			if (uG1Height > uG1Width)
			{
				uG1Width = uG1Height;
				zero.X -= (MenuUG2SBG.Width * uG1Width - Main.screenWidth) * 0.5f;
			}
			else
			{
				zero.Y -= (MenuUG2SBG.Height * uG1Width - Main.screenHeight) * 0.5f;
			}
		}
		Main.spriteBatch.Draw(MenuUG2SBG, zero, null, Color.White, 0f, Vector2.Zero, uG1Width, 0, 0f);
		base.DrawMenu(gameTime);
	}

	// public override bool ChangeAudio()
	// TODO: MothBiomeOld should play when entering and exiting the firefly subworld but MothBiome should play while inside the subworld.
	// {
	// Main.newMusic = MythContent.QuickMusic("MothBiomeOld");
	// if (SubworldSystem.IsActive<MothWorld>())
	// {
	// Main.newMusic = MythContent.QuickMusic("MothBiome");
	// return true;
	// }
	// return false;
	// }
	public override void OnEnter()
	{
		SubworldSystem.hideUnderworld = true;
	}

	public override void OnLoad()
	{
		Main.worldSurface = 20;
		Main.rockLayer = 150;
		GenVars.waterLine = 50;
	}
}