using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using ReLogic.Content;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Dusts;

[Pipeline(typeof(NPPipeline), typeof(AcytaeaPipeline))]
public class CosmicFlame : Particle
{
    public static Asset<Texture2D> texture;

    public override void Load()
    {
        base.Load();
        texture = ModContent.Request<Texture2D>((GetType().Namespace + "." + Name).Replace('.', '/'));
    }

    public override void Update()
    {
        scale *= 0.99f;
        velocity *= 1.05f;
        if (scale <= 0.1f)
        {
            Active = false;
        }
    }

    public override void Draw()
    {
        VFXManager.spriteBatch.BindTexture(texture.Value).Draw(position, null, Color.White, 0, texture.Value.Size() / 2, scale, SpriteEffects.None);
    }
}