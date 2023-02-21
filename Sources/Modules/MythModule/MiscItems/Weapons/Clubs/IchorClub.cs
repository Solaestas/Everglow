namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class IchorClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 47;
            Item.value = 5000;
            ProjType = ModContent.ProjectileType<Projectiles.IchorClub>();
        }
    }
}