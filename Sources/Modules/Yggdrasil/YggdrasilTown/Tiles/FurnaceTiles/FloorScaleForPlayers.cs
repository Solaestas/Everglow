using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class FloorScaleForPlayers : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileSolid[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileBlendAll[Type] = true;
		MinPick = int.MaxValue;
		DustType = ModContent.DustType<Heatproof_Furniture_Dust>();

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.Width = 5;

		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, };
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new Point16(3, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(14, 14, 14));
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Player player = Main.LocalPlayer;
		if (player.Bottom.ToTileCoordinates().X == i && player.Bottom.ToTileCoordinates().Y == j)
		{
			if (YggdrasilTownFurnaceSystem.SwitchPlayerCooling == 0)
			{
				if(YggdrasilTownFurnaceSystem.CurrentPlayer == null || YggdrasilTownFurnaceSystem.CurrentPlayer != player)
				{
					YggdrasilTownFurnaceSystem.SwitchPlayerCooling = 30;
					YggdrasilTownFurnaceSystem.CurrentPlayer = player;
				}
			}
		}
		base.NearbyEffects(i, j, closer);
	}

	public override bool CanExplode(int i, int j) => false;

	public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

	public void AddScene(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if ((tile.TileFrameX == 18 || tile.TileFrameX == 144) && tile.TileFrameY == 0)
		{
			var scene = new FloorScaleForPlayers_Screen { position = new Vector2(i, j) * 16, Active = true, originTile = new Point(i, j), originType = Type };
			Ins.VFXManager.Add(scene);
		}
	}
}