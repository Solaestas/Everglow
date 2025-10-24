using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.YggdrasilTown.Dusts.TwilightForest;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.RoomScenes;

public class StoneCave_Scene : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestSoil>()] = true;
		Main.tileMerge[Type][ModContent.TileType<StoneScaleWood>()] = true;
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
		var scene_Close = new StoneCave_Scene_Close { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		var scene_Background = new StoneCave_Scene_Background { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		var scene_Far = new StoneCave_Scene_Far { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		Ins.VFXManager.Add(scene_Close);
		Ins.VFXManager.Add(scene_Background);
		Ins.VFXManager.Add(scene_Far);
	}
}