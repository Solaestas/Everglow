using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Interfaces;

namespace Everglow.Sources.Commons.Core.VFX.Base
{
    internal class VisualProjectile : ModProjectile, IVisual
    {
        public bool Active => Projectile.active && Main.projectile[Projectile.whoAmI] == Projectile;

        public virtual CallOpportunity DrawLayer => CallOpportunity.PostDrawProjectiles;

        public bool Visible => !Projectile.hide && VFXManager.InScreen(Projectile.position, ProjectileID.Sets.DrawScreenCheckFluff[Type]);

        public virtual void Draw()
        {
            
        }
        public override void SetDefaults()
        {
            if(!Main.gameMenu)
            {
                VFXManager.Instance.Add(this);
            }
        }
        public void Kill()
        {
            Projectile.Kill();
        }

        public void Update()
        {
            
        }
    }
}
