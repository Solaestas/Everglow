using Terraria.Graphics.Effects;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.LanternMoon.Skies;
namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles
{
    class RainbowWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }
        private Effect Rainbow;
        int AimProj = -1;
        public override ModProjectile Clone(Projectile projectile)
        {
            var clone = base.Clone(projectile) as RainbowWave;
            //AimProj = -1;
            return clone;
        }
        public override void AI()
        {
            Projectile.velocity *= 0;
            if (AimProj == -1)
            {
                for (int f = 0; f < Main.projectile.Length; f++)
                {
                    if (Main.projectile[f].active && Main.projectile[f].type == ModContent.ProjectileType<LMeteor>())
                    {
                        AimProj = f;
                        break;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Rainbow = MythContent.QuickEffect("Effects/RainbowVague");
            Texture2D RainbowTex = MythContent.QuickTexture("UIimages/VisualTextures/Rainbow");
            if (Projectile.timeLeft > 6)
            {
                LanternSky lanternSky = ModContent.GetInstance<LanternSky>();
                if (!Filters.Scene["RainbowVague"].IsActive())
                {
                    Filters.Scene.Activate("RainbowVague");
                }
                if(!SkyManager.Instance["LanternSky"].IsActive())
                {
                    SkyManager.Instance.Activate("LanternSky");
                }
                Vector2 ScreenPosTOShader = Projectile.Center - Main.screenPosition;
                ScreenPosTOShader.X /= (float)(Main.screenWidth);
                ScreenPosTOShader.Y /= (float)(Main.screenHeight);
                Vector2 ScreenCenTOShader = (AimProj == -1 ? Projectile.Center : Main.projectile[AimProj].Center) - Main.screenPosition;
                ScreenCenTOShader.X /= (float)(Main.screenWidth);
                ScreenCenTOShader.Y /= (float)(Main.screenHeight);
                float WaveSize = (180 - Projectile.timeLeft) / 200f;
                float WaveWidth = (Projectile.timeLeft - 6) / 360f;
                float Darkness = (Projectile.timeLeft - 6) / 220f;//ProjCen//AimProj

                Rainbow.Parameters["ProjPos"].SetValue(ScreenPosTOShader);
                Rainbow.Parameters["waveSize"].SetValue(WaveSize);
                Rainbow.Parameters["waveWidth"].SetValue(WaveWidth);
                Rainbow.Parameters["uImage2"].SetValue(RainbowTex);
                Rainbow.Parameters["darkness"].SetValue(Darkness);
                Rainbow.Parameters["ProjCen"].SetValue(ScreenCenTOShader);
            }
            else
            {
                if (Filters.Scene["RainbowVague"].IsActive())
                {
                    Filters.Scene.Deactivate("RainbowVague");
                }
            }
        }
    }
}
