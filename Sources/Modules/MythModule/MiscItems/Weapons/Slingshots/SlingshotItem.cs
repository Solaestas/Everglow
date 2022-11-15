using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class SlingshotItem : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.crit = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 36;

           
            Item.UseSound = SoundID.Item5;
            Item.noUseGraphic = true;
            SetDef();
        }
        public virtual void SetDef()
        {

        }
        internal int ProjType;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ProjType] < 1)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ProjType, damage, knockback, player.whoAmI, Item.shootSpeed, Item.useAnimation);
            }
            return false;
        }
    }
}
