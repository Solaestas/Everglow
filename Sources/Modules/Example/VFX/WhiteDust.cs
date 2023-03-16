using Terraria.GameContent;

namespace Everglow.Example.VFX;

[Pipeline(typeof(WCSPipeline), typeof(BloomPipeline))]
internal class WhiteDust : Particle
{

	public override void Draw()
	{
		VFXManager.spriteBatch.BindTexture(TextureAssets.MagicPixel.Value).Draw(position, new Rectangle(0, 0, 16, 16), Color.White);
	}
}
