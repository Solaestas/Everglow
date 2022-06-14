using Terraria.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base
{
    internal abstract class BaseHeldProj : BaseFSMProj
    {
        public abstract void SetItem(BaseHeldItem item);
    }
    internal abstract class BaseHeldProj<T> : BaseHeldProj where T : BaseHeldItem
    {
        public T item;
        public override void SetItem(BaseHeldItem item) => this.item = (T)item;
        public Player Owner => Main.player[Projectile.owner];
        public override void Initialize()
        {
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            item = (T)Main.player[Projectile.owner].inventory[(int)Projectile.ai[0]].ModItem;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            writer.Write(Array.FindIndex(Owner.inventory, i => i == item.Item));
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            item = (T)Main.player[Projectile.owner].inventory[reader.ReadInt32()].ModItem;
        }
    }
}
