using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class LiftLamp : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18
		};
		TileObjectData.newTile.CoordinateWidth = 32;
		TileObjectData.addTile(Type);
		DustType = DustID.Iron;
		ModTranslation modTranslation = base.CreateMapEntryName(null);
		AddMapEntry(new Color(191, 142, 111), modTranslation);
		modTranslation.SetDefault("Lift Lamp");
		modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "电梯指示灯");
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		int FrameX = 0;
		foreach (var Dtile in TileSystem.GetTiles<YggdrasilElevator>())
		{
			Vector2 Dc = Dtile.Center;
			if (Math.Abs(Dc.Y / 16f - j) < 4 && Main.tile[i, j].TileFrameY == 0 && Math.Abs(Dc.X / 16f - i) < Dtile.size.X / 32f + 5)
				FrameX = 32;
		}
		if (FrameX == 32 && Main.tile[i, j].TileFrameY == 0)
		{
			for (int y = 0; y < 3; y++)
			{
				if (Main.tile[i, j + y].TileType == ModContent.TileType<LiftLamp>())
				{
					if (Main.tile[i, j + y].TileFrameX == 0 && Main.tile[i, j + y].TileFrameY == Main.tile[i, j].TileFrameY + y * 18)
						Main.tile[i, j + y].TileFrameX = 32;
				}
			}
		}
		if (FrameX == 0 && Main.tile[i, j].TileFrameY == 0)
		{
			for (int y = 0; y < 3; y++)
			{
				if (Main.tile[i, j + y].TileType == ModContent.TileType<LiftLamp>())
				{
					if (Main.tile[i, j + y].TileFrameX == 32 && Main.tile[i, j + y].TileFrameY == Main.tile[i, j].TileFrameY + y * 18)
						Main.tile[i, j + y].TileFrameX = 0;
				}
			}
		}
		if (Main.tile[i, j].TileFrameX == 32)
			Lighting.AddLight(new Vector2(i * 16, j * 16), new Vector3(1f, 0.8f, 0.3f));
	}
	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.LiftLamp>());
	}
}

