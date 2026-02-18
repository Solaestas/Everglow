using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Commons.MEAC;

/// <summary>
/// Postures, include player posture and weapon motivation.
/// </summary>
public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	public Player Owner => Main.player[Projectile.owner];

	public void HoldWeapon()
	{
		Vector3 currentPos3D = WeaponAxis + new Vector3(0, 0, CenterZ);
		Vector2 currentPos = Project(currentPos3D, ProjectionMatrix);
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentPos.ToRotation() - MathHelper.PiOver2);
	}
}