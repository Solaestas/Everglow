namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    internal class AmbiguousNight : SlingshotProjectile
    {
        public override void SetDef()
        {
            ShootProjType = ModContent.ProjectileType<NormalAmmo>();
            SlingshotLength = 8;
            SplitBranchDis = 8;
        }
    }
}
