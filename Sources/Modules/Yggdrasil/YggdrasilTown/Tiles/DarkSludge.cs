
using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.Common;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class DarkSludge : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<StoneScaleWood>()] = true;
		Main.tileMerge[(ushort)ModContent.TileType<StoneScaleWood>()][Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileNoSunLight[Type] = true;
		DustType = DustID.BorealWood;
		MinPick = 250;
		HitSound = SoundID.Dig;
		AddMapEntry(new Color(31, 26, 45));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override void FloorVisuals(Player player)
	{
		player.velocity *= 0.1f;

		// player.position += new Vector2(0, player.gravDir * 15f);
		player.AddBuff(BuffID.Shimmer, 60);
	}

	public override void RandomUpdate(int i, int j)
	{
		Tile thisTile = Main.tile[i, j];
		UpdateTileFrameX(thisTile);
		if (i >= 24 && i <= Main.maxTilesX - 24)
		{
			for (int x = -5; x <= 5; x += 2)
			{
				Tile otherTile = Main.tile[i + x, j];
				UpdateTileFrameX(otherTile);
			}
		}
		void UpdateTileFrameX(Tile tile)
		{
			if (tile.TileFrameY == 0)
			{
				if (tile.TileFrameX >= 18 && tile.TileFrameX <= 54)
				{
					tile.TileFrameX = (short)(Main.rand.Next(1, 4) * 18);
				}
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		if(!Main.tile[i, j - 1].HasTile)
		{
			return false;
		}
		return base.PreDraw(i, j, spriteBatch);
	}

	public void AddScene(int i, int j)
	{
		if (i == 1330 && j == Main.maxTilesY - 329)
		{
			DarkSludge_Scene scene = new DarkSludge_Scene { position = new Vector2(i, j), Active = true, originTile = new Point(i, j), originType = ModContent.TileType<BoneAndPlatform_tile>() };
			Ins.VFXManager.Add(scene);
		}
	}
}