using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class StrikePlayer : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 3;
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
        }
    }
}
