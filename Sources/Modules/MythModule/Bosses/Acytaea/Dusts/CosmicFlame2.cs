using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Dusts
{
    public class CosmicFlame2 : ModDust
    {
        public override void SetStaticDefaults()
        {
            Everglow.HookSystem.AddMethod(DrawAll, Commons.Core.CallOpportunity.PostDrawDusts);
        }
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale *= 0.9f;
            if (dust.scale <= 0.005f)
            {
                dust.active = false;
            }

            return false;
        }
        public void DrawAll()
        {
            var sb = Main.spriteBatch;
            var tex = ModContent.Request<Texture2D>(Texture).Value;
            var type = Type;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone,
                null, Matrix.CreateTranslation(Main.screenPosition.X, Main.screenPosition.Y, 0));
            for (int i = 0; i < Main.dust.Length; i++)
            {
                Dust d = Main.dust[i];
                if (d.type == type && d.active)
                {
                    sb.Draw(tex, d.position, null, Color.White, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0);
                }
            }
            sb.End();
        }
    }
}
