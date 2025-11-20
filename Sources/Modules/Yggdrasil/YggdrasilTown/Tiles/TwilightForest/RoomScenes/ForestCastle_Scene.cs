using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.YggdrasilTown.Dusts.TwilightForest;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

public class ForestCastle_Scene : ModTile, ISceneTile
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
		var scene_Close = new TwilightCastle_RoomScene_OverTiles { Position = new Vector2(i, j) * 16, Active = true, Visible = true, OriginTilePos = new Point(i, j), OriginTileType = Type, Texture = ModAsset.ForestCastle_Scene_Close.Value };
		scene_Close.CustomDraw += DrawForestCastleOverTile;
		var scene_Background = new TwilightCastle_RoomScene_Background { Position = new Vector2(i, j) * 16, Active = true, Visible = true, OriginTilePos = new Point(i, j), OriginTileType = Type, Texture = ModAsset.ForestCastle_Scene_Background.Value };
		scene_Background.CustomDraw += DrawForestCastleBackground;
		var flag_Scene = new FlagLikeVFX { Position = new Vector2(i, j) * 16, Active = true, Visible = true, OriginTilePos = new Point(i, j), OriginTileType = Type, Texture = Commons.ModAsset.TileBlock3x3.Value, AnchorOffset = new Vector2(600, 0) };
		Ins.VFXManager.Add(flag_Scene);
		var flag_Scene2 = new FlagLikeVFX { Position = new Vector2(i, j) * 16, Active = true, Visible = true, OriginTilePos = new Point(i, j), OriginTileType = Type, Texture = Commons.ModAsset.TileBlock4x4.Value, AnchorOffset = new Vector2(690, 0) };
		Ins.VFXManager.Add(flag_Scene2);


		Ins.VFXManager.Add(scene_Close);
		Ins.VFXManager.Add(scene_Background);
	}

	public void DrawForestCastleOverTile(TwilightCastle_RoomScene_OverTiles otD)
	{
		Texture2D tex0 = ModAsset.ForestCastle_Scene_Close.Value;

		bool flipH = otD.FlipHorizontally(otD.OriginTilePos.X, otD.OriginTilePos.Y);
		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(otD.OriginTilePos.X, otD.OriginTilePos.Y, tex0, bars, flipH);
		Ins.Batch.Draw(tex0, bars, PrimitiveType.TriangleList);
	}

	public void DrawForestCastleBackground(TwilightCastle_RoomScene_Background bg)
	{
		Texture2D tex0 = ModAsset.ForestCastle_Scene_Background.Value;
		Texture2D tex1 = ModAsset.ForestCastle_Scene_Middle.Value;

		bool flipH = bg.FlipHorizontally(bg.OriginTilePos.X, bg.OriginTilePos.Y);
		List<Vertex2D> bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(bg.OriginTilePos.X, bg.OriginTilePos.Y, tex0, bars, flipH);
		Ins.Batch.Draw(tex0, bars, PrimitiveType.TriangleList);

		bars = new List<Vertex2D>();
		SceneUtils.DrawMultiSceneTowardBottom(bg.OriginTilePos.X, bg.OriginTilePos.Y, tex1, bars, flipH);
		Ins.Batch.Draw(tex1, bars, PrimitiveType.TriangleList);

		Texture2D texFern0 = Commons.ModAsset.TileBlock3x3.Value; // ;ModAsset.ForestCastle_Scene_Fern0.Value;
		Texture2D texFern1 = ModAsset.ForestCastle_Scene_Fern1.Value;
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		base.NearbyEffects(i, j, closer);
	}
}