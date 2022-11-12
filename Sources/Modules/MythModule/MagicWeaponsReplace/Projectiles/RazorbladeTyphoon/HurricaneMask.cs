using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.RazorbladeTyphoon
{
    public class HurricaneMask : ModProjectile, IWarpProjectile
    {
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20000;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
        }

        public override void AI()
        {
            if(Projectile.timeLeft == 19980)
            {
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BlackHole.BlackHole>(), Projectile.damage * 12, 0, Projectile.owner, Projectile.ai[0]);
                p.CritChance = (int)Main.player[Projectile.owner].GetCritChance(DamageClass.Generic);
                p.timeLeft = 100 + (int)(Projectile.ai[0] * 240);
                Projectile.Kill();
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitLight");
            float Dark = Math.Clamp(((Projectile.timeLeft - 150) / 50f), 0, 20f);
            Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(0, 225, 255,0) * Dark, 0, Shadow.Size() / 2f, Projectile.ai[0] / 300f * Dark, SpriteEffects.None, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public void DrawWarp()
        {
        }
    }
}