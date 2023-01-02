namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{

    public class FloatLanternGore6 : ModGore
    {
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
            gore.velocity.Y -= 0.16f;
            return base.Update(gore);
        }
    }
}
