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
    public class MeltLavaHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔火心晶");
            base.Tooltip.SetDefault("让火焰的能量融入血液,就像在火山一样");
		}
		public override void SetDefaults()
		{
			base.item.width = 76;
			base.item.height = 68;
            base.item.rare = 11;
			base.item.useAnimation = 20;
			base.item.useTime = 20;
			base.item.useStyle = 2;
			base.item.UseSound = SoundID.Item8;
			base.item.consumable = true;
            base.item.maxStack = 200;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 origin = new Vector2(38f, 34f);
            spriteBatch.Draw(base.mod.GetTexture("Items/Volcano/熔火心晶Glow"), base.item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public override bool UseItem(Player player)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (!mplayer.FinalLava)
            {
                mplayer.FinalLava = true;
                return true;
            }     
            return false;
        }
    }
}
