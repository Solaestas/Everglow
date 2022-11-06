namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BookofSkulls
{
    internal class BookofSkullsBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            ProjType = ProjectileID.BookOfSkullsSkull;
            DustType = DustID.Bone;
            ItemType = ItemID.BookofSkulls;
            UseGlow = false;
        }
    }
}