namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base
{
    internal abstract class BaseHeldItem : BaseItem
    {

    }
    internal abstract class BaseHeldItem<T> : BaseHeldItem where T : BaseHeldProj
    {
        public T projectile;//只有Owner端有，不建议随意调用
        public override ModItem Clone(Item newEntity)
        {
            var clone = base.Clone(newEntity) as BaseHeldItem<T>;
            clone.projectile = null;
            return clone;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.None;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void HoldItem(Player player)
        {
            if (Main.netMode == player.whoAmI && (projectile == null || !projectile.Projectile.active))
            {
                projectile = (T)Projectile.NewProjectileDirect(
                    player.GetSource_ItemUse(Item),
                    player.position, Vector2.Zero,
                    ModContent.ProjectileType<T>(),
                    Item.damage, Item.knockBack, player.whoAmI)
                    .ModProjectile;
                projectile.SetItem(Item);
            }
        }
    }
}
