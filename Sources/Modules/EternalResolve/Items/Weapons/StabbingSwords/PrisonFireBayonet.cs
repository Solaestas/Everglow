using MythMod.EternalResolveMod.Common;
using MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj;
using Terraria.DataStructures;
namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Contents
{
    public class PrisonFireBayonet : ERItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(Chinese, "狱火刺剑");
            DisplayName.AddTranslation(English, "Hellfire Rapier");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
            base.SetStaticDefaults();
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            ToStabbing(3);
            Item.damage = 12;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 75);
            Item.shoot = ModContent.ProjectileType<PrisonFireBayonet_Pro>();
            base.SetDefaults();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellstoneBar, 48).
                AddTile(TileID.Anvils).
                Register();
            base.AddRecipes();
        }
        public override bool AltFunctionUse(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StabPower>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<StabPower>(), damage, knockback, player.whoAmI, 0f, 0f);
                return false;
            }
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StabPower>()] < 1;
        }
    }
}