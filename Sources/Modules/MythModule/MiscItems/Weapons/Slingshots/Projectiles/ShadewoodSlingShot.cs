namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    internal class ShadewoodSlingShot : SlingshotProjectile
    {
        public override void SetDef()
        {
            ShootProjType = ModContent.ProjectileType<NormalAmmo>();
        }
    }
}
