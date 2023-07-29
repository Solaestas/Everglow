using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Everglow.Ocean.Items.Shells
{
	// Token: 0x0200015B RID: 347
    public class Pearl : ModItem
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00041728 File Offset: 0x0003F928
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("珍珠");
            // base.Tooltip.SetDefault("");
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00041780 File Offset: 0x0003F980
		public override void SetDefaults()
		{
			base.Item.width = 16;
			base.Item.height = 16;
			base.Item.useAnimation = 20;
			base.Item.useTime = 20;
            base.Item.maxStack = 999;
            base.Item.rare = 8;
            base.Item.value = Item.sellPrice(0, 5, 0, 0);
		}
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Mod.Find<ModItem>("ShinyPearl").Type, 1);//制作一个武器
            recipe.AddIngredient(null, "Pearl", 1); //需要一个材料
            recipe.AddIngredient(513, 5); //需要一个材料          
            recipe.AddIngredient(75, 3); //需要一个材料    
            recipe.requiredTile[0] = 16;
            recipe.Register();
        }
    }
}
