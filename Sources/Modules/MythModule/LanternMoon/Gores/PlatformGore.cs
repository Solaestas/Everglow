namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{
    public class PlatformGore : ModGore
    {
        public static Vector2[] PlatFrame = new Vector2[1000];
        public override void SetStaticDefaults()
        {
            //GoreID.Sets.DisappearSpeed[ModContent.GoreType<Gores.FloatLanternGore1>()] = 6;
        }
        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            return base.GetAlpha(gore, new Color(0, 0, 0, 0));
        }
        public override bool Update(Gore gore)
        {
            return base.Update(gore);
        }
    }
}
