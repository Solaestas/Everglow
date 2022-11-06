namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.RazorbladeTyphoon
{
    internal class RazorbladeTyphoonBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            UseGlow = false;
            ItemType = ItemID.RazorbladeTyphoon;
            DustType = DustID.RazorbladeTyphoon;
            ProjType = ProjectileID.Typhoon;
        }
    }
}