namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    public class RubyBead : GemAmmo
    {
        public override void SetDef()
        {
            TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailRed";
            TrailColor = Color.Red;
            TrailColor.A = 0;
            dustType = ModContent.DustType<Dusts.RubyDust>();
        }
    }
}
