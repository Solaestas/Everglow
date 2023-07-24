using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class BubbleCoralStick : ModItem
    {
        private int num = 0;
        private bool k = true;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            item.damage = 220;
            item.melee = true;
            item.width = 52;
            item.height = 56;
            item.useTime = 5;
            item.rare = 11;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 14000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("BubbleCoral");
            item.shootSpeed = 1.5f;

        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int i = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 88, 0f, 0f, 0,  default(Color), 1.2f);
            Main.dust[i].noGravity = true;
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
            if(Main.rand.Next(3) == 1)
            {
                Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
                Vector2 v = new Vector2(speedX, speedY);
                v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
                Projectile.NewProjectile(pc.X, pc.Y, v.X, v.Y, mod.ProjectileType("BubbleCoral"), damage, knockBack, player.whoAmI);
                if (num >= 4)
                {
                    k = true;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BubbleCoral", 7);
            recipe.AddIngredient(null, "OceanDustCore", 10);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
