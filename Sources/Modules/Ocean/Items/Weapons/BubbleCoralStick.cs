using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class BubbleCoralStick : ModItem
    {
        private int num = 0;
        private bool k = true;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 220;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 52;
            Item.height = 56;
            Item.useTime = 5;
            Item.rare = 11;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 7;
            Item.value = 14000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.BubbleCoral>();
            Item.shootSpeed = 1.5f;

        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            num += 1;
            if(Main.rand.Next(3) == 1)
            {
                Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
                Vector2 v = new Vector2(velocity.X, velocity.Y);
                v = v.RotatedBy(Math.PI * (2 - (num % 5)) / 5f * -player.direction);
                Projectile.NewProjectile(null, pc.X, pc.Y, v.X, v.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.BubbleCoral>(), damage, knockback, player.whoAmI);
                if (num >= 4)
                {
                    k = true;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "BubbleCoral", 7);
            recipe.AddIngredient(null, "OceanDustCore", 10);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
