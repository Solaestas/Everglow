using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base
{
    internal abstract class BaseProjectile : ModProjectile
    {
        public Asset<Texture2D> Asset => ModContent.Request<Texture2D>(Texture);
        protected override bool CloneNewInstances => true;
        public override bool IsCloneable => true;
        public void FaceTo(Vector2 dir)
        {
            Projectile projectile = Projectile;
            projectile.rotation = dir.ToRotation();
            projectile.direction = projectile.spriteDirection = dir.X > 0 ? 1 : -1;
        }
    }
}
