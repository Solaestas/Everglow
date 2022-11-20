namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    internal class DiamondSlingshot : GemSlingshotProjectile
    {
        public override void SetDef()
        {
            ShootProjType = ModContent.ProjectileType<DiamondBead>();
            TexPath = "MiscItems/Weapons/Slingshots/Projectiles/Diamond";
            base.SetDef();
        }
    }
}
