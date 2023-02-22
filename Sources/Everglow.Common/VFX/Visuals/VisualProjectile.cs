using Everglow.Common.Enums;
using Everglow.Common.Interfaces;
using Terraria.ID;

namespace Everglow.Common.VFX.Visuals;

internal abstract class VisualProjectile : ModProjectile, IVisual
{
	public bool Active => Projectile.active && Main.projectile[Projectile.whoAmI] == Projectile;

	public virtual CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public bool Visible => !Projectile.hide && VFXManager.InScreen(Projectile.position, ProjectileID.Sets.DrawScreenCheckFluff[Type]);

	public virtual void Draw()
	{
	}

	public override void SetDefaults()
	{
		if (!Main.gameMenu)
		{
			Ins.VFXManager.Add(this);
		}
	}

	public void Kill()
	{
		Projectile.Kill();
	}

	public void Update()
	{
	}
}