namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	public class TopazBead : GemAmmo
	{
		public override void SetDef()
		{
			TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailYellow";
			TrailColor = Color.Yellow;
			TrailColor.A = 0;
			dustType = ModContent.DustType<Dusts.TopazDust>();
		}
	}
}
