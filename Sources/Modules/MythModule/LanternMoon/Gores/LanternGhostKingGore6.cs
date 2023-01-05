namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{
    public class LanternGhostKingGore6 : ModGore
    {
        public override void SetStaticDefaults()
        {
        }
        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            return base.GetAlpha(gore, new Color(0, 0, 0, 0));
        }
        public override bool Update(Gore gore)
        {
            gore.velocity.Y -= 0.1f;
            return base.Update(gore);
        }
    }
}
