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
namespace MythMod.Items
{
    public class AbyssOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("深渊之下");
            DisplayName.SetDefault("渊海矿");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.width = 40;
            base.item.height = 40;
            base.item.rare = 8;
            base.item.scale = 1f;
            base.item.createTile = base.mod.TileType("AbyssOre");
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
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AbyssOre", 4); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.SetResult(mod.ItemType("DarkSeaBar"), 1); //制作一个武器
            recipe.AddRecipe();
        }
    }
}
