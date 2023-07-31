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
namespace Everglow.Ocean.Items
{
    public class LavaStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.AddTranslation(GameCulture.Chinese, "熔岩石");
            // Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.MeltingLava>();
			base.Item.useStyle = 1;
			base.Item.useTurn = true;
            base.Item.useAnimation = 15;
			base.Item.useTime = 10;
            base.Item.autoReuse = true;
			base.Item.consumable = true;
            Item.width = 26;
            Item.height = 24;
            Item.rare = 2;
            Item.scale = 1f;
            Item.value = 500;
            Item.maxStack = 999;
            Item.useTime = 14;
        }
    }
}
