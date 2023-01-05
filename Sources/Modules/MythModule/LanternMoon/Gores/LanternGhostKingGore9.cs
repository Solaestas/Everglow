namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{
    public class LanternGhostKingGore9 : ModGore
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
            //gore.velocity.Y -= 0.06f;
            return base.Update(gore);
        }
    }
}
