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

namespace Everglow.Ocean.Items.Drawings
{
    public class ShallowBeach : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("珊瑚浅滩");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "珊瑚浅滩");
		}
		public override void SetDefaults()
		{
			base.Item.width = 80;
			base.Item.height = 80;
			base.Item.maxStack = 99;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
			base.Item.value = 50000;
			base.Item.rare = 1;
			base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.珊瑚浅滩>();
		}
	}
}
