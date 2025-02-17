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
		//CloseTrapDoor(i, j);
		return base.RightClick(i, j);
	}

	public void CloseTrapDoor(int i, int j)
	{
	}
}