using MythMod.EternalResolveMod.Common;
using MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj;
using Terraria.DataStructures;

namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Contents
{
    public class GoldenStabbingSword : ERItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(Chinese, "金刺剑");
            DisplayName.AddTranslation(English, "Gold Rapier");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            ToStabbing(2);
            Item.damage = 6;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 75);
            Item.shoot = ModContent.ProjectileType<GoldenStabbingSword_Pro>();
            base.SetDefaults();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GoldBar, 24).
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