using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Pickaxes
{
	// Token: 0x020000AF RID: 175
    public class HeatOfTheEarth : ModItem
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0003529C File Offset: 0x0003349C
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("地热");
			base.Tooltip.SetDefault("brush!");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "地热");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "用无穷的炽热熔化冰冻的心");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 240;
			base.item.melee = true;
			base.item.width = 46;
			base.item.height = 46;
			base.item.useTime = 5;
			base.item.useAnimation = 5;
			base.item.useTurn = true;
			base.item.pick = 350;
			base.item.useStyle = 1;
			base.item.knockBack = 9f;
			base.item.value = 100000;
			base.item.UseSound = SoundID.Item1;
			base.item.autoReuse = true;
			base.item.tileBoost += 4;
            base.item.rare = 11;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(3) == 0)
			{
				int num = Main.rand.Next(3);
				if (num == 0)
				{
					num = 35;
				}
				else if (num == 1)
				{
					num = 35;
				}
				else
				{
					num = 35;
				}
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 2f);
			}
		}
    }
}
