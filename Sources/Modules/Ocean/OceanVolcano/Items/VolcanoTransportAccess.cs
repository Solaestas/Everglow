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
    public class VolcanoTransportAccess : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("火山传送门");
            // base.Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.width = 76;
			base.Item.height = 68;
            base.Item.rare = 0;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.useStyle = 1;
            base.Item.consumable = true;
            base.Item.useTurn = true;
            base.Item.autoReuse = true;
            base.Item.createTile = base.Mod.Find<ModTile>("火山传送门").Type;
            base.Item.UseSound = SoundID.Item8;
			base.Item.consumable = true;
            base.Item.maxStack = 200;
		}
    }
}
