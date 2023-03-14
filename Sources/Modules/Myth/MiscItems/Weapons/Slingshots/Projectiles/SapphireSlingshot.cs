namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class SapphireSlingshot : GemSlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<SapphireBead>();
			TexPath = "MiscItems/Weapons/Slingshots/Projectiles/Sapphire";
			base.SetDef();
		}
	}
}

