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
namespace Everglow.Myth.TheMarbleRemains.Items
{
    public class GiantMarbalClock : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // DisplayName.SetDefault("遗迹大理石巨钟");
        }
        public override void SetDefaults()
        {
            base.Item.width = 40;
            base.Item.height = 40;
            base.Item.rare = 8;
            base.Item.scale = 1f;
            base.Item.createTile = ModContent.TileType<Tiles.GiantMarbalClock>();
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            base.Item.width = 13;
            base.Item.height = 10;
            base.Item.maxStack = 99;
            base.Item.value = 4000;
        }
    }
}
