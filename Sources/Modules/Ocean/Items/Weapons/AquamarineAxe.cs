using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
	// Token: 0x020000AF RID: 175
    public class AquamarineAxe : ModItem
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0003529C File Offset: 0x0003349C
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蓝宝石斧");
			base.Tooltip.SetDefault("brush!");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝宝石斧");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000352F4 File Offset: 0x000334F4
		public override void SetDefaults()
		{
			base.item.damage = 155;
			base.item.melee = true;
			base.item.width = 66;
			base.item.height = 74;
			base.item.useTime = 18;
			base.item.useAnimation = 18;
			base.item.useTurn = true;
			base.item.axe = 47;	
			base.item.useStyle = 1;
			base.item.knockBack = 9f;
			base.item.value = 80000;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
            base.item.rare = 11;
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
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "Aquamarine", 7); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
                recipe.AddIngredient(null, "RedCoral", 1); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
                recipe.requiredTile[0] = 412;
                recipe.SetResult(this, 1);//制作一个材料
                recipe.AddRecipe();
            }
        }
    }
}
