using Everglow.Myth.Bosses.Acytaea;
using ReLogic.Content;

namespace Everglow.Myth.Bosses.Acytaea.Dusts;

[Pipeline(typeof(NPPipeline), typeof(AcytaeaPipeline))]
public class CosmicFlame2 : Particle
{
	public override void Update()
	{
		scale *= 0.99f;
		velocity *= 1.05f;
		if (scale <= 0.1f)
			Active = false;
	}

	public override void Draw()
	{
		var texture = ModAsset.CosmicFlame2;
		Ins.Batch.BindTexture(texture.Value).Draw(position, null, Color.White, 0, texture.Value.Size() / 2, scale, SpriteEffects.None);
	}
}