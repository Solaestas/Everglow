using MythMod.NPCs;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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

namespace MythMod.Items.Weapons.OceanWeapons
{
	public class OlivineHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石锤");
        }

		public override void SetDefaults()
		{
			item.damage = 85;
			item.crit = 4;
			item.melee = true;
			item.width = 68;
			item.height = 68;
			item.useTime = 25;
			item.useAnimation = 25;
			item.hammer = 140;		//How much hammer power the weapon has
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 50000;
			item.rare = 11;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 7); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
            recipe.AddIngredient(null, "GoldGorgonianBranch", 10); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);//制作一个材料
            recipe.AddRecipe();
        }

        // Token: 0x060002D2 RID: 722 RVA: 0x000354AC File Offset: 0x000336AC
        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
		    int num = mod.DustType("Olivine");
			Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 1f);
		}
	}
}
