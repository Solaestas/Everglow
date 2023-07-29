using Everglow.Ocean.NPCs;
using Terraria.GameContent.Generation;
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
using Terraria.WorldBuilding;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
	public class AquamarineHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.damage = 85;
			Item.crit = 4;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 68;
			Item.height = 68;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.hammer = 120;		//How much hammer power the weapon has
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 45000;
			Item.rare = 7;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);//制作一个材料
            recipe.AddIngredient(null, "Aquamarine", 7); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
            recipe.AddIngredient(null, "RedCoral", 1); //要用一个泥土制作，1是数量，ItemID.DirtBlock是泥土
            recipe.requiredTile[0] = 412;
            recipe.Register();
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
