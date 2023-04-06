using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Pylon;

internal class FireflyPylonTileEntity : TEModdedPylon
{
}

public abstract class BaseModPylon<T> : ModPylon where T : TEModdedPylon
{
	public const int DefaultVerticalOffset = -12;

	public Asset<Texture2D> crystalTexture;
	public Asset<Texture2D> crystalHighlightTexture;
	public Asset<Texture2D> mapIcon;
	public virtual int CrystalVerticalFrameCount => 8;
	public virtual int CrystalFrameHeight => 64;
	public virtual string MapEntryNameKey => null;
	public virtual int DropItemType => 0;

	public override void Load()
	{
		crystalTexture = ModContent.Request<Texture2D>(Texture + "_Crystal");
		crystalHighlightTexture = ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Pylon/CommonPylon_CrystalHighlight");
		mapIcon = ModContent.Request<Texture2D>(Texture + "_MapIcon");
	}

	public override void SetStaticDefaults()
	{
		Main.tileLighted[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.DrawYOffset = 0;
		TileObjectData.newTile.StyleHorizontal = true;

		TEModdedPylon moddedPylon = ModContent.GetInstance<T>();
		TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);
		TileObjectData.addTile(Type);

		TileID.Sets.InteractibleByNPCs[Type] = true;
		TileID.Sets.PreventsSandfall[Type] = true;
		AddToArray(ref TileID.Sets.CountsAsPylon);
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		ModContent.GetInstance<T>().Kill(i, j);
		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 2, 3, DropItemType);
	}

	public override void MouseOver(int i, int j)
	{
		Main.LocalPlayer.cursorItemIconID = DropItemType;
		Main.LocalPlayer.cursorItemIconEnabled = true;
	}

	public static void DrawModPylon(SpriteBatch spriteBatch, int i, int j, Asset<Texture2D> crystalTexture, Asset<Texture2D> crystalHighlightTexture, Vector2 crystalOffset, Color pylonShadowColor, Color dustColor, int dustChanceDenominator, int crystalVerticalFrameCount, bool animation = true, int dustType = 43)
	{
		// Gets offscreen vector for different lighting modes
		var offscreenVector = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
			offscreenVector = Vector2.Zero;

		// Double check that the tile exists
		var point = new Point(i, j);
		Tile tile = Main.tile[point.X, point.Y];
		if (tile == null || !tile.HasTile)
			return;

		var tileData = TileObjectData.GetTileData(tile);

		// Calculate frame based on vanilla counters in order to line up the animation
		int frameY = animation ? Main.tileFrameCounter[TileID.TeleportationPylon] / crystalVerticalFrameCount : 0;

		// Frame our modded crystal sheet accordingly for proper drawing
		Rectangle crystalFrame = crystalTexture.Frame(1, crystalVerticalFrameCount, 0, frameY);
		Rectangle smartCursorGlowFrame = crystalHighlightTexture.Frame(1, crystalVerticalFrameCount, 0, frameY);
		// I have no idea what is happening here; but it fixes the frame bleed issue. All I know is that the vertical sinusoidal motion has something to with it.
		// If anyone else has a clue as to why, please do tell. - MutantWafflez
		crystalFrame.Height -= 1;
		smartCursorGlowFrame.Height -= 1;

		// Calculate positional variables for actually drawing the crystal
		Vector2 origin = crystalFrame.Size() / 2f;
		var tileOrigin = new Vector2(tileData.CoordinateFullWidth / 2f, tileData.CoordinateFullHeight / 2f);
		Vector2 crystalPosition = point.ToWorldCoordinates(tileOrigin.X - 2f, tileOrigin.Y) + crystalOffset;

		// Calculate additional drawing positions with a sine wave movement
		float sinusoidalOffset = animation ? (float)Math.Sin(Main.GlobalTimeWrappedHourly * (Math.PI * 2) / 5) : 0;
		Vector2 drawingPosition = crystalPosition + offscreenVector + new Vector2(0f, sinusoidalOffset * 4f);

		// Do dust drawing
		if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)) && Main.rand.NextBool(dustChanceDenominator))
		{
			Rectangle dustBox = Utils.CenteredRectangle(crystalPosition, crystalFrame.Size());
			int numForDust = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, dustType, 0f, 0f, 254, dustColor, 0.5f);
			Dust obj = Main.dust[numForDust];
			obj.velocity *= 0.1f;
			Main.dust[numForDust].velocity.Y -= 0.2f;
		}

		// Get color value and draw the the crystal
		Color color = Lighting.GetColor(point.X, point.Y);
		color = Color.Lerp(color, Color.White, 0.8f);
		spriteBatch.Draw(crystalTexture.Value, drawingPosition - Main.screenPosition, crystalFrame, color * 0.7f, 0f, origin, 1f, SpriteEffects.None, 0f);

		// Draw the shadow effect for the crystal
		float scale = animation ? (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 1f) * 0.2f + 0.8f : 0.8f;
		Color shadowColor = pylonShadowColor * scale;
		for (float shadowPos = 0f; shadowPos < 1f; shadowPos += 1f / 6f)
		{
			spriteBatch.Draw(crystalTexture.Value, drawingPosition - Main.screenPosition + ((float)Math.PI * 2f * shadowPos).ToRotationVector2() * (6f + sinusoidalOffset * 2f), crystalFrame, shadowColor, 0f, origin, 1f, SpriteEffects.None, 0f);
		}

		// Interpret smart cursor outline color & draw it
		int selectionLevel = 0;
		if (Main.InSmartCursorHighlightArea(point.X, point.Y, out bool actuallySelected))
		{
			selectionLevel = 1;
			if (actuallySelected)
				selectionLevel = 2;
		}

		if (selectionLevel == 0)
			return;

		int averageBrightness = (color.R + color.G + color.B) / 3;

		if (averageBrightness <= 10)
			return;

		Color selectionGlowColor = Colors.GetSelectionGlowColor(selectionLevel == 2, averageBrightness);
		spriteBatch.Draw(crystalHighlightTexture.Value, drawingPosition - Main.screenPosition, smartCursorGlowFrame, selectionGlowColor, 0f, origin, 1f, SpriteEffects.None, 0f);
	}
}

internal class FireflyPylon : BaseModPylon<FireflyPylonTileEntity>
{
	public override int DropItemType => ModContent.ItemType<FireflyPylonItem>();

	public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
	{
		DrawModPylon(spriteBatch, i, j, crystalTexture, crystalHighlightTexture, new Vector2(0, DefaultVerticalOffset), new Color(5, 0, 55, 30), new Color(255, 0, 155, 20), 4, CrystalVerticalFrameCount, true, ModContent.DustType<FireflyPylonDust>());
	}

	public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
	{
		if (!PylonSystem.Instance.shabbyPylonEnable)
			return;

		bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
		DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.Everglow.Myth.ItemName.FireflyPylonItem", ref mouseOverText);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Texture2D tex = MythContent.QuickTexture("TheFirefly/Pylon/FireflyPylonGlow");

		spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);
	}
}

internal class FireflyPylonItem : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<FireflyPylon>());
	}
}