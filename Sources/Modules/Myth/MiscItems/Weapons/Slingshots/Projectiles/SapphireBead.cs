namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	public class SapphireBead : GemAmmo
	{
		public override void SetDef()
		{
			TrailTexPath = "MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailBlue";
			TrailColor = Color.Blue;
			TrailColor.A = 0;
			dustType = ModContent.DustType<Dusts.SapphireDust>();
		}
	}
}
