using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using ReLogic.Content;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Dusts;

[Pipeline(typeof(WCSPipeline))]
public class CosmicCrack : Particle
{
    public static Asset<Texture2D> texture;
    public float rotation;

    public override void Load()
    {
        base.Load();
        texture = ModContent.Request<Texture2D>((GetType().Namespace + "." + Name).Replace('.', '/'));
    }

    public override void OnSpawn()
    {
        scale = 1;
        rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
    }

    public override void Update()
    {
        scale -= 0.03f;
        velocity *= 0.99f;
        if (scale <= 0)
        {
            Active = false;
        }
    }

    public override void Draw()
    {
        VFXManager.spriteBatch.BindTexture(texture.Value).Draw(position, null, Color.White, rotation, texture.Value.Size() / 2, scale, SpriteEffects.None);
    }
}