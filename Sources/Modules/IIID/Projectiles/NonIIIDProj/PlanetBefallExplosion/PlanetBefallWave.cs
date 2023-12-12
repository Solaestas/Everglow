using Everglow.Commons.VFX;
using Everglow.Commons.Vertex;
using Everglow.Commons.MEAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallExplosion
{
    public class PlanetBefallWave : ModProjectile//, IWarpProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.aiStyle = -1;
        }     
    }

}
