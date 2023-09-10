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
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class CrystalSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("");
            // // DisplayName.AddTranslation(GameCulture.Chinese, "结晶原萃");
            // Tooltip.AddTranslation(GameCulture.Chinese, "把天地间所有含灵力之物高强度压缩淬炼得到的结晶,内部磅礴的能量让人望而生畏");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = false;
            ItemID.Sets.ItemIconPulse[Item.type] = false;
        }
        public override void SetDefaults()
        {
            base.Item.width = 32;
            base.Item.height = 52;
            base.Item.rare = 10;
            base.Item.scale = 1f;
            base.Item.maxStack = 999;
            base.Item.value = 100000;
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "Crystal", 50);
            modRecipe.AddIngredient(null, "Gypsum", 50);
            modRecipe.AddIngredient(109, 50);
            modRecipe.AddIngredient(502, 100);
            modRecipe.AddIngredient(1508, 50);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        private int F = 0;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            F = (int)(Main.time / 8d);
            Vector2 origin = new Vector2(16f, 26f);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Items/Volcano/CrystalSoulGlow"), base.Item.Center - Main.screenPosition, new Rectangle(0, 52 * (F % 4),32,52),new Color(1f,1f,1f,0), rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
