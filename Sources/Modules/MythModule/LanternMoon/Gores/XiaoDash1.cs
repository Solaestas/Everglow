namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{
    public class XiaoDash1 : ModGore
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
            gore.velocity.Y -= 0.15f;
            gore.velocity *= 0.95f;
            gore.velocity *= 0.95f;
            gore.scale *= 0.95f;
            gore.timeLeft -= 3;
            if (gore.scale < 0.05f)
            {
                gore.active = false;
            }
            return base.Update(gore);
        }
    }
}
