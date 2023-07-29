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

namespace Everglow.Ocean.Items.Shore
{
    public class ShoreMud : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // DisplayName.SetDefault("滩涂泥");
        }
        public override void SetDefaults()
        {
            base.Item.width = 40;
            base.Item.height = 40;
            base.Item.rare = 0;
            base.Item.scale = 1f;
            base.Item.createTile = base.Mod.Find<ModTile>("ShoreMud").Type;
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            base.Item.width = 13;
            base.Item.height = 10;
            base.Item.maxStack = 999;
            base.Item.value = 4000;
        }
    }
}
