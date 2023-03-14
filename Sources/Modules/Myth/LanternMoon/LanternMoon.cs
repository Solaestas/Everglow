namespace Everglow.Sources.Modules.MythModule.LanternMoon
{
	public class LanternMoon : ModSystem//灯笼月
	{
		public static Effect LanternGore, LanternGoreFlame, OcenaUBG, TrailRainbow, XiaoDash;
		public override void Load()
		{
			Gores.ShaderLanternGore.Load();
			base.Load();
		}
		public override void Unload()
		{
			Gores.ShaderLanternGore.UnLoad();
			base.Unload();
		}
		public override void SetStaticDefaults()
		{
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
