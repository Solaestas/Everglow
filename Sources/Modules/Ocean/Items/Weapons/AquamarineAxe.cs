using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
	// Token: 0x020000AF RID: 175
    public class AquamarineAxe : ModItem
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0003529C File Offset: 0x0003349C
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("海蓝宝石斧");
			// base.Tooltip.SetDefault("brush!");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石斧");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000352F4 File Offset: 0x000334F4
		public override void SetDefaults()
		{
			base.Item.damage = 155;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			base.Item.width = 66;
			base.Item.height = 74;
			base.Item.useTime = 18;
			base.Item.useAnimation = 18;
			base.Item.useTurn = true;
			base.Item.axe = 47;	
			base.Item.useStyle = 1;
			base.Item.knockBack = 9f;
			base.Item.value = 80000;
			base.Item.UseSound = SoundID.Item1;
			base.Item.autoReuse = true;
            base.Item.rare = 11;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000353C8 File Offset: 0x000335C8



		// Token: 0x060002D2 RID: 722 RVA: 0x000354AC File Offset: 0x000336AC
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height / 4, 59, 0f, 0f, 0, default(Color), 1f);
			Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height / 2, 59, 0f, 0f, 0, default(Color), 1.8f);
			if (Main.rand.Next(3) == 0)
			{
				int num = Main.rand.Next(3);
				if (num == 0)
				{
					num = 59;
				}
				else if (num == 1)
				{
					num = 59;
				}
				else
				{
					num = 59;
				}
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 1f);
			}
		}
        public override void AddRecipes()
        {
            {//合成表1
                Recipe recipe = CreateRecipe(1);//制作一个材料
                recipe.AddIngredient(null, "Aquamarine", 7); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
                recipe.AddIngredient(null, "RedCoral", 1); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
                recipe.requiredTile[0] = 412;
                recipe.Register();
            }
        }
    }
}
