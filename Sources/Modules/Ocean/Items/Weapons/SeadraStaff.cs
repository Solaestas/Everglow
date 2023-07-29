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
    public class SeadraStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("海龙风暴");
			Item.staff[base.Item.type] = true;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海龙风暴");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "扶摇直上九万里\n神话");
        }
        public override void SetDefaults()
		{
			base.Item.damage = 85;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 150;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.useTime = 90;
			base.Item.useAnimation = 90;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = 50000;
			base.Item.rare = 6;
			base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.SeadraStaff>();
			base.Item.shootSpeed = 18f;
		}
	}
}
