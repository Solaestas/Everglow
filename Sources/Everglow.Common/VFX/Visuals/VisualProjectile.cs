using Everglow.Common.Enums;

namespace Everglow.Common.VFX.Visuals;

internal abstract class VisualProjectile : ModProjectile, IVisual
{
	public bool Active => Projectile.active && Main.projectile[Projectile.whoAmI] == Projectile;

	public virtual CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public bool Visible => !Projectile.hide && VFXManager.InScreen(Projectile.position, ProjectileID.Sets.DrawScreenCheckFluff[Type]);

	public new int Type => throw new NotImplementedException();

	public string Name => throw new NotImplementedException();

	public virtual void Draw()
	{
	}

	public override void SetDefaults()
	{
		if (!Main.gameMenu)
		{
			VFXManager.Add(this);
		}
	}

	public void Kill()
	{
		Projectile.Kill();
	}

	public void Update()
	{
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Unload()
	{
		throw new NotImplementedException();
	}
}