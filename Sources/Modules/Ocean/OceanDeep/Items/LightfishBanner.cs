using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.OceanDeep.Items
{
	// Token: 0x0200038F RID: 911
	public class LightfishBanner : ModItem
	{
				// Token: 0x06001475 RID: 5237 RVA: 0x000082F6 File Offset: 0x000064F6
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("灯笼鱼Banner");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "灯笼鱼旗");
		}
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00088C14 File Offset: 0x00086E14
		public override void SetDefaults()
		{
			base.Item.width = 12;
			base.Item.height = 28;
			base.Item.maxStack = 99;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
			base.Item.rare = 1;
			base.Item.value = Item.buyPrice(0, 0, 10, 0);
			base.Item.createTile = base.Mod.Find<ModTile>("MonsterBanner").Type;
			base.Item.placeStyle = 5;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Vector2 origin = new Vector2(6f, 14f);
            spriteBatch.Draw(ModAsset.灯笼鱼BannerGLOW.Value, base.Item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
