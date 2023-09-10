using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
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

namespace Everglow.Ocean.Items
{
	// Token: 0x0200038F RID: 911
	public class AlarmJellyfishBanner : ModItem
	{
				// Token: 0x06001475 RID: 5237 RVA: 0x000082F6 File Offset: 0x000064F6
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("警报水母Banner");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "警报水母旗");
        }
        public override void SetDefaults()
        {
            base.Item.width = 10;
			base.Item.height = 24;
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
			base.Item.placeStyle = 1;
		}
	}
}
