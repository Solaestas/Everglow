namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    internal class JungleSlingshot : SlingshotProjectile
    {
        public override void SetDef()
        {
            ShootProjType = ModContent.ProjectileType<GlowSporeBead>();
            SlingshotLength = 10;
            SplitBranchDis = 6;
        }
    }
}
