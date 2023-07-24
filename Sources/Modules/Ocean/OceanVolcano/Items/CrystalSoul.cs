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
namespace MythMod.Items.Volcano
{
    public class CrystalSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "结晶原萃");
            Tooltip.AddTranslation(GameCulture.Chinese, "把天地间所有含灵力之物高强度压缩淬炼得到的结晶,内部磅礴的能量让人望而生畏");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.AnimatesAsSoul[item.type] = false;
            ItemID.Sets.ItemIconPulse[item.type] = false;
        }
        public override void SetDefaults()
        {
            base.item.width = 32;
            base.item.height = 52;
            base.item.rare = 10;
            base.item.scale = 1f;
            base.item.maxStack = 999;
            base.item.value = 100000;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "Crystal", 50);
            modRecipe.AddIngredient(null, "Gypsum", 50);
            modRecipe.AddIngredient(109, 50);
            modRecipe.AddIngredient(502, 100);
            modRecipe.AddIngredient(1508, 50);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        private int F = 0;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            F = (int)(Main.time / 8d);
            Vector2 origin = new Vector2(16f, 26f);
            spriteBatch.Draw(base.mod.GetTexture("Items/Volcano/CrystalSoulGlow"), base.item.Center - Main.screenPosition, new Rectangle(0, 52 * (F % 4),32,52),new Color(1f,1f,1f,0), rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
