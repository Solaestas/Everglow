using Everglow.Sources.Modules.MythModule.TheFirefly;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class NavyThunderBomb : ModProjectile
    {
        private float r = 0;
        private Vector2 v0;
        private int Fra = 0;
        private int FraX = 0;
        private int FraY = 0;
        private float Stre2 = 1;
        public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/MothBall";
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Navy Thunder Bomb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {

            Projectile.velocity *= 0.95f;
            if (Stre2 > 0)
            {
                Stre2 -= 0.005f;
            }
            if (Projectile.timeLeft > 240)
            {
                r += 1f;
            }
            if (Projectile.timeLeft <= 240 && Projectile.timeLeft >= 60)
            {
                r = 60 + (float)(10 * Math.Sin((Projectile.timeLeft - 60) / 60d * Math.PI));
            }
            if (Projectile.timeLeft < 60 && r > 0.5f)
            {
                r -= 1f;
            }
            Fra = ((600 - Projectile.timeLeft) / 3) % 30;
            FraX = (Fra % 6) * 270;
            FraY = (Fra / 6) * 290;
            if (v0 != Vector2.Zero)
            {
                // Projectile.position = v0 - new Vector2(Dx, Dy) / 2f;
            }
        }
        public override void Kill(int timeLeft)
        {
            /*震屏
            MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Shake = 6;
            float Str = 1;

            mplayer.ShakeStrength = Str;*/
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            for (int h = 0; h < 120; h += 3)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.PureBlue>(), 0, 0, 0, default(Color), 15f * Main.rand.NextFloat(0.7f, 2.9f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3 * 6;
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 4.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 27.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 36; y++)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
          
            base.Kill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            Effect hdrDark = Common.MythContent.QuickEffect("Effects/hdrDark");
            hdrDark.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(FraX, FraY + 10, 270, 270), new Color(1f, 1f, 1f, 1f), Projectile.rotation, new Vector2(135f, 135f), r / 420f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            //Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(FraX, FraY + 10, 270, 270), new Color(1f, 1f, 1f, 0), Projectile.rotation, new Vector2(135f, 135f), r / 420f, SpriteEffects.None, 0f);
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/CorruptLight");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(Stre2, Stre2, Stre2, 0), Projectile.rotation, new Vector2(168f, 168f), Projectile.scale * r / 210f, SpriteEffects.None, 0);
            return false;
        }
    }
}
