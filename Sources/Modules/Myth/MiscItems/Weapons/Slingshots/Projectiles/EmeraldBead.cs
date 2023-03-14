namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	public class EmeraldBead : GemAmmo
	{
		public override void SetDef()
		{
			TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailGreen";
			TrailColor = Color.Green;
			TrailColor.A = 0;
			dustType = ModContent.DustType<Dusts.EmeraldDust>();
		}
	}
}
