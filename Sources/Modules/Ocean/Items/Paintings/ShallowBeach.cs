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

namespace MythMod.Items.Drawings
{
    public class ShallowBeach : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("珊瑚浅滩");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "珊瑚浅滩");
		}
		public override void SetDefaults()
		{
			base.item.width = 80;
			base.item.height = 80;
			base.item.maxStack = 99;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.consumable = true;
			base.item.value = 50000;
			base.item.rare = 1;
			base.item.createTile = base.mod.TileType("珊瑚浅滩");
		}
	}
}
