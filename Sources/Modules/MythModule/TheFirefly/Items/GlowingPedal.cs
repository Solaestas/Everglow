using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
    public class GlowingPedal : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }

        public static short GetGlowMask = 0;

        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.width = 42;
            Item.height = 26;
            Item.maxStack = 999;
        }
    }
}