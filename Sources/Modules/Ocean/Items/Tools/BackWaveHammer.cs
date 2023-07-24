using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythMod.Items
{
	public class BackWaveHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "回流锤");
        }
		public override void SetDefaults()
		{
			item.damage = 84;
			item.crit = 4;
			item.melee = true;
			item.width = 36;
			item.height = 34;
			item.useTime = 20;
			item.useAnimation = 20;
			item.hammer = 110;
			item.useStyle = 1;
			item.knockBack = 4;
			item.value = 40000;
			item.rare = 8;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
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
