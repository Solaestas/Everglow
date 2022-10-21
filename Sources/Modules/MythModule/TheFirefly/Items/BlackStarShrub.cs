﻿using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
    public class BlackStarShrub : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.width = 32;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = ItemRarityID.White;
        }
    }
}
