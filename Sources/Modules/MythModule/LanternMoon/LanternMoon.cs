using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.LanternMoon.NPCs.LanternGhostKing;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon
{
    public class LanternMoon : ModSystem//灯笼月
    {
        public static Effect LanternGore, LanternGoreFlame, OcenaUBG, TrailRainbow, XiaoDash;
        public Effect VagueBoss = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/VagueBoss").Value;
        public override void Load()
        {
            Gores.ShaderLanternGore.Load();
            DrawBossUI.Load();
			base.Load();
        }
        public override void Unload()
        {
            Gores.ShaderLanternGore.UnLoad();
            DrawBossUI.Unload();
            base.Unload();
        }
        public override void SetStaticDefaults()
        {
            var VagueBoss = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/VagueBoss").Value;
			Filters.Scene["VagueBoss"] = new Filter(new ScreenShaderData(new Ref<Effect>(VagueBoss), "Test"), EffectPriority.Medium);
			Filters.Scene["VagueBoss"].Load();
			base.SetStaticDefaults();
        }
    }
    /*public class DrawLanternMoon
    {
        public static float OffsetUIY = 300;
        public static float Col = 1f;
        private static void Main_DrawUI(On.Terraria.Main.orig_DrawInterface_33_MouseText orig, Terraria.Main self)
        {
            orig.Invoke(self);
			
		}
        public static void Load()
        {
            On.Terraria.Main.DrawInterface_33_MouseText += Main_DrawUI;
        }
        public static void Unload()
        {
            //On.Terraria.Main.DrawInterface_33_MouseText -= Main_DrawUI;
        }
    }*/
}
