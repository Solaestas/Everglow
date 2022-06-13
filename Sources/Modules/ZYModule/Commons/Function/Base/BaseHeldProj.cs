using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base
{
    internal abstract class BaseHeldProj : BaseFSMProj
    {
        public abstract void SetItem(Item item);
    }
    internal abstract class BaseHeldProj<T> : BaseHeldProj where T : BaseHeldItem
    {
        public T item;
        public override void SetItem(Item item) => this.item = item.ModItem as T;
        public Player Owner => Main.player[Projectile.owner];
        public override void Initialize()
        {
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
    }
}
