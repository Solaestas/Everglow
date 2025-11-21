using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class FurnaceTrapDoor_Open : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileSolid[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = false;
		MinPick = int.MaxValue;
		DustType = ModContent.DustType<Heatproof_Furniture_Dust>();

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Width = 8;

		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new Point16(4, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(84, 84, 84));
	}

	public override bool CanExplode(int i, int j) => false;

	public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

	public override void MouseOver(int i, int j)
	{
		Player player = Main.LocalPlayer;
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<Items.Placeables.FurnaceTrapDoor_Item>();
		base.MouseOver(i, j);
	}

	public override bool RightClick(int i, int j)
	{
		return base.RightClick(i, j);
	}

	public void CloseTrapDoor(int i, int j)
	{
		Tile thisTile = TileUtils.SafeGetTile(i, j);
		int startX = i - thisTile.TileFrameX / 18;
		int startY = j - thisTile.TileFrameY / 18;
		var firstTile = TileUtils.SafeGetTile(startX, startY);
		if (firstTile.TileType == ModContent.TileType<FurnaceTrapDoor_Open>())
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					var checkTile = TileUtils.SafeGetTile(startX + x, startY + y);
					checkTile.TileType = (ushort)ModContent.TileType<FurnaceTrapDoor>();
					checkTile.TileFrameX = (short)(x * 18);
					checkTile.TileFrameY = (short)(y * 18);
				}
			}
		}
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		var tile = TileUtils.SafeGetTile(i, j);
		var pos = new Point(i - tile.TileFrameX / 18 + 4, j - tile.TileFrameY / 18).ToWorldCoordinates();
		if((Main.LocalPlayer.Center - pos).Length() > 300)
		{
			CloseTrapDoor(i, j);
		}
		base.NearbyEffects(i, j, closer);
	}
}