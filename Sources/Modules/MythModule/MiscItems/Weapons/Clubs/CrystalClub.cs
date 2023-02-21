namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class CrystalClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 51;
            Item.value = 5000;
            ProjType = ModContent.ProjectileType<Projectiles.CurseClub>();
        }
    }
}
