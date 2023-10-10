using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class LampWood_Bone_8x5 : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileLavaDeath[Type] = false;

		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.Width = 8;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(64, 64, 75));
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		return base.PreDraw(i, j, spriteBatch);
	}
}