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


namespace Everglow.Ocean.Items.Weapons
{
    public class MeanderWave : ModItem
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault(".");
			// base.Tooltip.SetDefault(".");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海波粼粼");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "打出海浪的光波");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 180;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 20;
			base.Item.width = 28;
			base.Item.height = 30;
			base.Item.useTime = 25;
			base.Item.useAnimation = 25;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 6f;
			base.Item.value = 10000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item14;
			base.Item.autoReuse = true;
			base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanWave2>();
			base.Item.shootSpeed = 14f;
        }
	}
}
