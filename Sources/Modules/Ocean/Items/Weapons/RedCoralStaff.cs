using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
    public class RedCoralStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("红珊瑚法杖");
			Item.staff[base.Item.type] = true;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "红珊瑚法杖");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 200;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 8;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.useTime = 26;
			base.Item.useAnimation = 20;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = 12000;
			base.Item.rare = 3;
			base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.珊瑚>();
			base.Item.shootSpeed = 1f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(null, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.红珊瑚>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "RedCoral", 4);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
