using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using System;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class OlivineSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石剑");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {
            item.damage = 146;
            item.melee = true;
            item.width = 68;
            item.height = 68;
            item.useTime = 6;
            item.rare = 11;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 14000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("Olivine");
            item.shootSpeed = 15f;

        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("Olivine"), 0f, 0f, 0,  default(Color), 0.6f);
            //string key = hitbox.Width.ToString();
            //Color messageColor = Color.Purple;
            //Main.NewText(Language.GetTextValue(key), messageColor);
            if (hitbox.Width == 104 && k)
            {
                num = 0;
                k = false;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            num += 1;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Vector2 v = new Vector2(speedX, speedY);
            v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
            Projectile.NewProjectile(pc.X, pc.Y, v.X, v.Y, mod.ProjectileType("Olivine"), damage / 2, knockBack, player.whoAmI);
            if(num >= 4)
            {
                k = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 7);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 10);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
