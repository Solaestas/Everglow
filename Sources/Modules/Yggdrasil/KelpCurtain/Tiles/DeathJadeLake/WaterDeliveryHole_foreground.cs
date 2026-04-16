using Everglow.Commons.VFX.Scene;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

[Pipeline(typeof(WCSPipeline))]
public class WaterDeliveryHole_foreground : TileVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawPlayers;

	public override void Update()
	{
		base.Update();
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.WaterDeliveryHole_foreground.Value;
		Ins.Batch.Draw(tex, Position, null, Lighting.GetColor(Position.ToTileCoordinates()), 0, tex.Size() * 0.5f, 1f, SpriteEffects.None);
	}
}