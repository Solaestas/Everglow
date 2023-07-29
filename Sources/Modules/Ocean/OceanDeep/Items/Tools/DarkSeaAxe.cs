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
using Terraria.WorldBuilding;

namespace Everglow.Ocean.Items
{
    public class DarkSeaAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("炽海巨斧");
			// base.Tooltip.SetDefault("crash!");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "炽海巨斧");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊的炽岩，具有灼热的破坏力");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.damage = 129;
			base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			base.Item.crit = 5;
			base.Item.width = 64;
			base.Item.height = 64;
			base.Item.useTime = 25;
			base.Item.useAnimation = 25;
			base.Item.useTurn = true;
			base.Item.axe = 36;	
			base.Item.useStyle = 1;
			base.Item.knockBack = 9f;
			base.Item.value = 45000;
			base.Item.UseSound = SoundID.Item1;
			base.Item.autoReuse = true;
            base.Item.rare = 10;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
			modRecipe.AddIngredient(null, "DarkSeaBar", 12);
            modRecipe.requiredTile[0] = 412;
			modRecipe.Register();
		}
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
