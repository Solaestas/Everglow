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


namespace MythMod.Items.Weapons
{
    public class LightOfAbyss : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault(".");
			base.Tooltip.SetDefault(".");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "幽渊之光");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊之下,点点磷光");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 260;
			base.item.magic = true;
			base.item.mana = 11;
			base.item.width = 28;
			base.item.height = 30;
			base.item.useTime = 24;
			base.item.useAnimation = 25;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 6f;
			base.item.value = 60000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item8;
			base.item.autoReuse = true;
			base.item.shoot = base.mod.ProjectileType("Phosphorescence");
			base.item.shootSpeed = 8f;
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
