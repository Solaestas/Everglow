using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodBed : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
		TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
		TileID.Sets.IsValidSpawnPoint[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair); // Beds count as chairs for the purpose of suitable room creation

		DustType = ModContent.DustType<BlueGlow>();
		AdjTiles = new int[] { TileID.Beds };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); // this style already takes care of direction for us
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
	{
		return true;
	}

	public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
	{
		width = 2;
		height = 2;
	}

	public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
	{
		// Default values match the regular vanilla bed
		// You might need to mess with the info here if your bed is not a typical 4x2 tile
		info.VisualOffset.Y += 4f; // Move player down a notch because the bed is not as high as a regular bed
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 1;
	}
	public override bool RightClick(int i, int j)
	{
		return FurnitureUtils.BedRightClick(i, j);
	}

	public override void MouseOver(int i, int j)
	{
		FurnitureUtils.BedMouseOver<Items.Furnitures.GlowWoodBed>(i, j);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodBedGlow");
		spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.8f, 0.8f, 0.8f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

		if (tile.TileFrameX == 54 && tile.TileFrameY == 0)
		{
			Color cTile = Lighting.GetColor(i, j);
			tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodBedExtra");
			spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 - 4) - Main.screenPosition + zero, new Rectangle(0, 0, 20, 20), cTile, 0, new Vector2(0), 1, SpriteEffects.None, 0);
			//var tileTarget = Main.tile[Player.tileTargetX, Player.tileTargetY];
			//if (tileTarget.TileFrameX == tile.TileFrameX + (Player.tileTargetX - i) * 18)
			//{
			//    if (tileTarget.TileFrameY == tile.TileFrameY + (Player.tileTargetY - j) * 18)
			//    {
			//        if (Main.SmartCursorIsUsed)
			//        {
			//            tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodBedExtra");
			//            Color drawC = Color.Wheat;
			//            if (Player.tileTargetX - i <= -2)
			//            {
			//                drawC = Color.Gray;
			//            }
			//            spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 - 4) - Main.screenPosition + zero, new Rectangle(0, 22, 20, 20), drawC, 0, new Vector2(0), 1, SpriteEffects.None, 0);
			//        }
			//    }
			//}
		}

		if (tile.TileFrameX == 72 && tile.TileFrameY == 0)
		{
			Color cTile = Lighting.GetColor(i, j);
			tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodBedExtra");
			spriteBatch.Draw(tex, new Vector2(i * 16 - 4, j * 16 - 4) - Main.screenPosition + zero, new Rectangle(22, 0, 20, 20), cTile, 0, new Vector2(0), 1, SpriteEffects.None, 0);
		}
	}
}