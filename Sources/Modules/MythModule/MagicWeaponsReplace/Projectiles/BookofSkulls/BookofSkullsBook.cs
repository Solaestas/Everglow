namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BookofSkulls
{
    internal class BookofSkullsBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<SkullII>();
            DustType = DustID.Bone;
            ItemType = ItemID.BookofSkulls;
            MulStartPosByVelocity = 2f;
            UseGlow = false;
        }
    }
}