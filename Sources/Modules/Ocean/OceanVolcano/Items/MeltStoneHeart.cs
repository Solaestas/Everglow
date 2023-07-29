using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Ocean.OceanVolcano.Items
{
    public class MeltStoneHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("熔岩心石");
            // base.Tooltip.SetDefault("在火山区域内生命上限永久提升5点");
		}
		public override void SetDefaults()
		{
			base.Item.width = 30;
			base.Item.height = 26;
            base.Item.rare = 2;
			base.Item.useAnimation = 20;
			base.Item.useTime = 20;
			base.Item.useStyle = 2;
			base.Item.UseSound = SoundID.Item8;
			base.Item.consumable = true;
            base.Item.maxStack = 200;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 origin = new Vector2(15f, 13f);
            spriteBatch.Draw(base.Mod.GetTexture("Items/Volcano/熔岩心石Glow"), base.Item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            if (mplayer.LavaCryst < 60)
            {
                mplayer.LavaCryst += 1;
                return true;
            }
            return false;
        }
    }
}
