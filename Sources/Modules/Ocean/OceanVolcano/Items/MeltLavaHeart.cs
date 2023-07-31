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
    public class MeltLavaHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("熔火心晶");
            // base.Tooltip.SetDefault("让火焰的能量融入血液,就像在火山一样");
		}
		public override void SetDefaults()
		{
			base.Item.width = 76;
			base.Item.height = 68;
            base.Item.rare = 11;
			base.Item.useAnimation = 20;
			base.Item.useTime = 20;
			base.Item.useStyle = 2;
			base.Item.UseSound = SoundID.Item8;
			base.Item.consumable = true;
            base.Item.maxStack = 200;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 origin = new Vector2(38f, 34f);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Items/Volcano/熔火心晶Glow"), base.Item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            if (!mplayer.FinalLava)
            {
                mplayer.FinalLava = true;
                return true;
            }     
            return false;
        }
    }
}
