using Terraria.DataStructures;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class World : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 572;
            Item.DamageType = DamageClass.Melee;
            Item.width = 118;
            Item.height = 146;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = 20000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.World>();
            Item.shootSpeed = 0;
            Item.crit = 8; 
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] == 0)
            {
                Projectile.NewProjectile(source, player.Center, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
