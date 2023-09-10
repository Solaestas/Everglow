using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using System;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class OlivineSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // // DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石剑");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {
            Item.damage = 146;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 68;
            Item.height = 68;
            Item.useTime = 6;
            Item.rare = 11;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 7;
            Item.value = 14000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.Olivine>();
            Item.shootSpeed = 15f;

        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0f, 0f, 0,  default(Color), 0.6f);
            //string key = hitbox.Width.ToString();
            //Color messageColor = Color.Purple;
            //Main.NewText(Language.GetTextValue(key), messageColor);
            if (hitbox.Width == 104 && k)
            {
                num = 0;
                k = false;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = new Vector2(velocity.X, velocity.Y);
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            Projectile.NewProjectile(null, pc.X, pc.Y, v.X, v.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Olivine>(), damage / 2, knockback, player.whoAmI);
            if(num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Olivine", 7);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 10);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
