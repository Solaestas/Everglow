namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class TopazSlingshot : GemSlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<TopazBead>();
			TexPath = "MiscItems/Weapons/Slingshots/Projectiles/Topaz";
			base.SetDef();
		}
	}
}

