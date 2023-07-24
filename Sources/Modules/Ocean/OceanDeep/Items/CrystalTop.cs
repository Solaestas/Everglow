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
namespace MythMod.Items.UnderSea
{
    public class CrystalTop : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "璀璨琉璃");
        }
        public override void SetDefaults()
        {
            base.item.width = 40;//宽
            base.item.height = 40;//高
            base.item.rare = 8;//品质
            base.item.scale = 1f;//大小
            base.item.createTile = base.mod.TileType("CrystalTop");
            base.item.useStyle = 1;
            base.item.useTurn = true;
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.autoReuse = true;
            base.item.consumable = true;
            base.item.width = 13;
            base.item.height = 10;
            base.item.maxStack = 999;
            base.item.value = 4000;
        }
    }
}
