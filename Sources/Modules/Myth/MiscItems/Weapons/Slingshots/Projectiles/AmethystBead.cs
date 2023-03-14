
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	public class AmethystBead : GemAmmo
	{
		public override void SetDef()
		{
			TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailPurple";
			TrailColor = Color.Purple;
			TrailColor.A = 0;
			dustType = ModContent.DustType<Dusts.AmethystDust>();
		}
	}
}