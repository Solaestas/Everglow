namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class RubySlingshot : GemSlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<RubyBead>();
			TexPath = "MiscItems/Weapons/Slingshots/Projectiles/Ruby";
			base.SetDef();
		}
	}
}

