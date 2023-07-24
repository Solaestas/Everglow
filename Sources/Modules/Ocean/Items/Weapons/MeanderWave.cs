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


namespace MythMod.Items.Weapons
{
    public class MeanderWave : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault(".");
			base.Tooltip.SetDefault(".");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海波粼粼");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "打出海浪的光波");
		}
		public override void SetDefaults()
		{
			base.item.damage = 180;
			base.item.magic = true;
			base.item.mana = 20;
			base.item.width = 28;
			base.item.height = 30;
			base.item.useTime = 25;
			base.item.useAnimation = 25;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 6f;
			base.item.value = 10000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item14;
			base.item.autoReuse = true;
			base.item.shoot = base.mod.ProjectileType("OceanWave2");
			base.item.shootSpeed = 14f;
        }
	}
}
