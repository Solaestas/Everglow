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
namespace Everglow.Ocean.Items.Pickaxes
{
    public class AbyssPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("渊影镐");
			// base.Tooltip.SetDefault("crash!");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "渊影镐");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊的炽岩，具有灼热的破坏力");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.damage = 100;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			base.Item.width = 46;
			base.Item.height = 46;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.useTurn = true;
			base.Item.pick = 265;
			base.Item.useStyle = 1;
			base.Item.knockBack = 1f;
			base.Item.value = 45000;
			base.Item.UseSound = SoundID.Item1;
			base.Item.autoReuse = true;
			base.Item.tileBoost += 4;
			base.Item.crit = 5;
            base.Item.rare = 10;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000353C8 File Offset: 0x000335C8

		// Token: 0x060002D1 RID: 721 RVA: 0x00035450 File Offset: 0x00033650
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
			modRecipe.AddIngredient(null, "DarkSeaBar", 15);
            modRecipe.requiredTile[0] = 412;
			modRecipe.Register();
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
