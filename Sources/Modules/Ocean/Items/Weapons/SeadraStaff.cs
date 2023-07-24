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
    public class SeadraStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海龙风暴");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海龙风暴");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "扶摇直上九万里\n神话");
        }
        public override void SetDefaults()
		{
			base.item.damage = 85;
			base.item.magic = true;
			base.item.mana = 150;
			base.item.width = 54;
			base.item.height = 54;
			base.item.useTime = 90;
			base.item.useAnimation = 90;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 0.5f;
			base.item.value = 50000;
			base.item.rare = 6;
			base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("SeadraStaff");
			base.item.shootSpeed = 18f;
		}
	}
}
