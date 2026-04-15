using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.KelpCurtain.VFXs;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class WaterDeliveryHole : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Width = 5;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			18,
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.addTile(Type);
		DustType = ModContent.DustType<WaterDeliveryHoleDust>();
		AddMapEntry(new Color(78, 162, 255));
	}

	public void AddScene(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX == 36 & tile.TileFrameY == 0)
		{
			var vfx = new WaterDeliveryHole_VFX
			{
				Active = true,
				Visible = true,
				Position = new Vector2(i, j).ToWorldCoordinates(8, 8),
				OriginTilePos = new Point(i, j),
				OriginTileType = Type,
				Direction = -1,
			};
			Ins.VFXManager.Add(vfx);
		}
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch) => base.PostDraw(i, j, spriteBatch);
}