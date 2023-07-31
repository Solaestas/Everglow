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
    public class AbyssOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("深渊之下");
            // DisplayName.SetDefault("渊海矿");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.width = 40;
            base.Item.height = 40;
            base.Item.rare = 8;
            base.Item.scale = 1f;
            base.Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.AbyssOre>();
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
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Everglow.Ocean.Items.DarkSeaBar>(), 1);//制作一个武器
            recipe.AddIngredient(null, "AbyssOre", 4); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
    }
}
