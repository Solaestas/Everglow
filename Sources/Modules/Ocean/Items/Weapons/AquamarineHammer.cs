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
using Terraria.Graphics.Shaders;

namespace MythMod.Items.Weapons.OceanWeapons
{
	public class AquamarineHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("");
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
			item.hammer = 120;		//How much hammer power the weapon has
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 45000;
			item.rare = 7;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Aquamarine", 7); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
            recipe.AddIngredient(null, "RedCoral", 1); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);//制作一个材料
            recipe.AddRecipe();
        }

        // Token: 0x060002D2 RID: 722 RVA: 0x000354AC File Offset: 0x000336AC
        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(6) == 0)
			{
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 127, 0f, 0f, 0, default(Color), 0.8f);
			}
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
}
