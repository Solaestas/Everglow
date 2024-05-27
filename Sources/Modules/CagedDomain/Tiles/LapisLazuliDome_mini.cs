using Everglow.CagedDomain.Dusts;
using Terraria.ObjectData;

namespace Everglow.CagedDomain.Tiles
{
	public class LapisLazuliDome_mini : ModTile
	{
		public override void SetStaticDefaults()
		{
			DustType = ModContent.DustType<LapisLazuliDome_dust>();
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;

			TileObjectData.newTile.Width = 8;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinateHeights = new int[7];
			Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
			TileObjectData.newTile.CoordinateHeights[^1] = 18;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Origin = new(4, 2);
			TileObjectData.newTile.AnchorBottom = new(Terraria.Enums.AnchorType.SolidTile, 3, 2);
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(15, 80, 137));
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
	}
}