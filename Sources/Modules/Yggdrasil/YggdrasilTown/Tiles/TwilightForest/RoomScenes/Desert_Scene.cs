using System.Security.Cryptography;
using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts.TwilightForest;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

public class Desert_Scene : ModTile, ISceneTile
{
	public override string Texture => ModAsset.GreenRelicBrick_Mod;

	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestSoil>()] = true;
		Main.tileMerge[Type][ModContent.TileType<StoneScaleWood>()] = true;

		Main.tileMerge[Type][ModContent.TileType<GreenRelicBrick>()] = true;
		Main.tileMerge[ModContent.TileType<GreenRelicBrick>()][Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<GreenRelicBrick_dust>();
		HitSound = SoundID.Dig;
		MinPick = int.MaxValue;
		AddMapEntry(new Color(35, 58, 58));
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public void AddScene(int i, int j)
	{
		var scene_Close = new TwilightCastle_RoomScene_OverTiles { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		scene_Close.CustomDraw += DrawOverTile;

		var scene_Background = new TwilightCastle_RoomScene_Background { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		scene_Background.CustomDraw += DrawBackground;

		Ins.VFXManager.Add(scene_Close);
		Ins.VFXManager.Add(scene_Background);
	}

	public void DrawOverTile(TwilightCastle_RoomScene_OverTiles otD)
	{
		Texture2D tex0 = ModAsset.Desert_Scene_Close.Value;
		bool flipH = otD.FlipHorizontally(otD.originTile.X, otD.originTile.Y);
		int direction = 1;
		if (flipH)
		{
			direction = -1;
		}

		Tile tile_lantern = YggdrasilWorldGeneration.SafeGetTile(otD.originTile + new Point(7 * direction, 5));
		if (tile_lantern.TileType == ModContent.TileType<SandgoldLantern>() && tile_lantern.TileFrameX == 0)
		{
			tex0 = ModAsset.Desert_Scene_Close_lightUp.Value;
		}

		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(otD.originTile.X, otD.originTile.Y, tex0, bars, flipH);
		Ins.Batch.Draw(tex0, bars, PrimitiveType.TriangleList);
	}

	public void DrawBackground(TwilightCastle_RoomScene_Background bg)
	{
		Texture2D tex0 = ModAsset.Desert_Scene_Background.Value;
		Texture2D tex1 = ModAsset.Desert_Scene_Far.Value;
		Texture2D tex2 = ModAsset.Desert_Scene_WallGemsReflection.Value;

		bool flipH = bg.FlipHorizontally(bg.originTile.X, bg.originTile.Y);
		int direction = 1;
		if (flipH)
		{
			direction = -1;
		}
		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(bg.originTile.X, bg.originTile.Y, tex0, bars, flipH);
		Ins.Batch.Draw(tex0, bars, PrimitiveType.TriangleList);

		bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(bg.originTile.X, bg.originTile.Y, tex1, bars, flipH);
		Ins.Batch.Draw(tex1, bars, PrimitiveType.TriangleList);

		bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(bg.originTile.X + 34 * direction, bg.originTile.Y + 11, tex2, bars, flipH, 2.6f);
		Ins.Batch.Draw(tex2, bars, PrimitiveType.TriangleList);
	}
}