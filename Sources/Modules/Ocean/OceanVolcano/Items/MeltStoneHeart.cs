using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MythMod.Items.Volcano
{
    public class MeltStoneHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔岩心石");
            base.Tooltip.SetDefault("在火山区域内生命上限永久提升5点");
		}
		public override void SetDefaults()
		{
			base.item.width = 30;
			base.item.height = 26;
            base.item.rare = 2;
			base.item.useAnimation = 20;
			base.item.useTime = 20;
			base.item.useStyle = 2;
			base.item.UseSound = SoundID.Item8;
			base.item.consumable = true;
            base.item.maxStack = 200;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 origin = new Vector2(15f, 13f);
            spriteBatch.Draw(base.mod.GetTexture("Items/Volcano/熔岩心石Glow"), base.item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public override bool UseItem(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (mplayer.LavaCryst < 60)
            {
                mplayer.LavaCryst += 1;
                return true;
            }
            return false;
        }
    }
}
