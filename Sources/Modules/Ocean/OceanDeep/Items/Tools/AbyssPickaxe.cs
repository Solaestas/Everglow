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
namespace MythMod.Items.Pickaxes
{
    public class AbyssPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("渊影镐");
			base.Tooltip.SetDefault("crash!");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "渊影镐");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊的炽岩，具有灼热的破坏力");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 100;
			base.item.melee = true;
			base.item.width = 46;
			base.item.height = 46;
			base.item.useTime = 12;
			base.item.useAnimation = 12;
			base.item.useTurn = true;
			base.item.pick = 265;
			base.item.useStyle = 1;
			base.item.knockBack = 1f;
			base.item.value = 45000;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
			base.item.tileBoost += 4;
			base.item.crit = 5;
            base.item.rare = 10;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000353C8 File Offset: 0x000335C8

		// Token: 0x060002D1 RID: 721 RVA: 0x00035450 File Offset: 0x00033650
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(null, "DarkSeaBar", 15);
            modRecipe.requiredTile[0] = 412;
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
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
				num = 96;
			}
			else if (num == 1)
			{
				num = 96;
			}
			else
			{
				num = 96;
			}
			Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 1f);
		}
	}
}
