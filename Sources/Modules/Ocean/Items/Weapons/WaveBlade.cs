using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
namespace Everglow.Ocean.Items.Weapons
{
    public class WaveBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "浪花之刃");
            // base.Tooltip.AddTranslation(GameCulture.Chinese, "浪花之刃");
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void SetDefaults()
        {
            Item.damage = 237;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 60;
            Item.height = 80;
            Item.useTime = 36;
            Item.rare = 11;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.knockBack = 1.4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit =8;
            Item.value = 40000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.WaveBall>();
            Item.shootSpeed = 12;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float shootSpeed = base.Item.shootSpeed;
			Projectile.NewProjectile((float)position.X, (float)position.Y, (float)speedX, (float)speedY, (int)type, (int)damage * 7, (float)knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height / 3, 33, 0f, 0f, 0, default(Color), 1f);
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, (int)(hitbox.Height / 1.5f), 33, 0f, 0f, 0, default(Color), 1.5f);
            int num = Main.rand.Next(3);
            if (num == 0)
            {
                num = 56;
            }
            else if (num == 1)
            {
                num = 56;
            }
            else
            {
                num = 277;
            }
            if (Main.rand.Next(3) == 0)
			{
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 0.7f);
            }
        }
    }
}
