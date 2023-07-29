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

namespace Everglow.Ocean.Items
{
	public class DarkSeaHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("深渊的炽岩，具有灼热的破坏力");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "暗影重锤");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊的炽岩，具有灼热的破坏力");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 92;
			Item.crit = 4;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 60;
			Item.height = 64;
			Item.useTime = 25;
			Item.useAnimation = 25;
            Item.hammer = 120;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 45000;
			Item.rare = 10;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
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
