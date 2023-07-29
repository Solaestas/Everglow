using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items
{
	public class BackWaveHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "回流锤");
        }
		public override void SetDefaults()
		{
			Item.damage = 84;
			Item.crit = 4;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 36;
			Item.height = 34;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.hammer = 110;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 40000;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
        public override void AddRecipes()
        {
            Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.Register();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(3) == 0)
			{
				int num = Main.rand.Next(3);
				if (num == 0)
				{
					num = 33;
				}
				else if (num == 1)
				{
					num = 56;
				}
				else
				{
					num = 277;
				}
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 1f);
			}
		}
	}
}
