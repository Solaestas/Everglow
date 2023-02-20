namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class CurseClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 57;
            Item.value = 5000;
            ProjType = ModContent.ProjectileType<Projectiles.CurseClub>();
        }
    }
}
