using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
using Terraria.Graphics.Shaders;


namespace Everglow.Ocean.Items.Weapons
{
    public class LightOfAbyss : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault(".");
			// base.Tooltip.SetDefault(".");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "幽渊之光");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊之下,点点磷光");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.damage = 260;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 11;
			base.Item.width = 28;
			base.Item.height = 30;
			base.Item.useTime = 24;
			base.Item.useAnimation = 25;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 6f;
			base.Item.value = 60000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item8;
			base.Item.autoReuse = true;
			base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.Phosphorescence>();
			base.Item.shootSpeed = 8f;
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float num = 0.7854f;
			double num2 = Math.Atan2((double)speedX, (double)speedY) - (double)(num / 2f);
			double num3 = (double)(num / 8f);
            for (int i = 0; i < 4; i++)
            {
                double num4 = num2 + num3 * (double)(i + i * i) / 2.0 + (double)(32f * (float)i);
            }
            for (int k = 0; k < 10; k++)
            {
                int num5 = Main.rand.Next(-2, 2);
                Projectile.NewProjectile(position.X, position.Y, speedX * 1.3f + num5, speedY * 1.3f + num5, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            }
			return false;
        }
	}
}
