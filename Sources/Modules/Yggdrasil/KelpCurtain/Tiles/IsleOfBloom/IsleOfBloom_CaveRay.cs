using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.KelpCurtain.Background;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.KelpCurtain.VFXs;
using MonoMod.Core.Platforms;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.IsleOfBloom;

public class IsleOfBloom_CaveRay : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileWaterDeath[Type] = false;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
		TileObjectData.newTile.AnchorWall = true;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(36, 56, 23));
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		BackgroundSystem bgSystem = ModContent.GetInstance<BackgroundSystem>();

		if (bgSystem.HasBgSlide("Everglow.Yggdrasil.KelpCurtain.Background.KelpCurtainSky")/* && !bgSystem.HasBgSlide("Everglow.Yggdrasil.KelpCurtain.Background.IsleOfBloom_Underground_close")*/)
		{
			List<Vector2> polygon = new List<Vector2>();
			Vector2 centerPosWorld = new Point(i, j).ToWorldCoordinates() + new Vector2(0, 720);
			polygon.Add(centerPosWorld + new Vector2(0, -12) * 16);
			polygon.Add(centerPosWorld + new Vector2(130, 0) * 16);
			polygon.Add(centerPosWorld + new Vector2(0, 12) * 16);
			polygon.Add(centerPosWorld + new Vector2(-130, 0) * 16);
			List<Point> bgArea = TileUtils.GetPolygonAreaOfTilePos(polygon);

			IsleOfBloom_Underground_close iob_bg_close = new IsleOfBloom_Underground_close();
			iob_bg_close.WorldAnchor = centerPosWorld;
			iob_bg_close.BgTiles = bgArea;
			bgSystem.AddBackgroundSlide(iob_bg_close);

			IsleOfBloom_Underground_middle iob_bg_middle = new IsleOfBloom_Underground_middle();
			iob_bg_middle.WorldAnchor = centerPosWorld;
			iob_bg_middle.BgTiles = bgArea;
			bgSystem.AddBackgroundSlide(iob_bg_middle);

			IsleOfBloom_Underground_far iob_bg_far = new IsleOfBloom_Underground_far();
			iob_bg_far.WorldAnchor = centerPosWorld;
			iob_bg_far.BgTiles = bgArea;
			bgSystem.AddBackgroundSlide(iob_bg_far);

			IsleOfBloom_Underground_sky iob_bg_sky = new IsleOfBloom_Underground_sky();
			iob_bg_sky.WorldAnchor = centerPosWorld;
			bgSystem.AddBackgroundSlide(iob_bg_sky);
		}
		base.NearbyEffects(i, j, closer);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return false;
	}

	public void AddScene(int i, int j)
	{
		IsleOfBloom_CaveRay_VFX ray = new IsleOfBloom_CaveRay_VFX();
		ray.Active = true;
		ray.Visible = true;
		ray.OriginTilePos = new Point(i, j);
		ray.Position = new Point(i, j).ToWorldCoordinates();
		ray.OriginTileType = Type;
		Ins.VFXManager.Add(ray);
	}
}