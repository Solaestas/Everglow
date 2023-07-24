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

namespace MythMod.Items
{
    public class DarkSeaAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("炽海巨斧");
			base.Tooltip.SetDefault("crash!");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "炽海巨斧");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "深渊的炽岩，具有灼热的破坏力");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 129;
			base.item.melee = true;
			base.item.crit = 5;
			base.item.width = 64;
			base.item.height = 64;
			base.item.useTime = 25;
			base.item.useAnimation = 25;
			base.item.useTurn = true;
			base.item.axe = 36;	
			base.item.useStyle = 1;
			base.item.knockBack = 9f;
			base.item.value = 45000;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
            base.item.rare = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(null, "DarkSeaBar", 12);
            modRecipe.requiredTile[0] = 412;
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
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
