using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.Commons.MEAC;

/// <summary>
/// Postures, include player posture and weapon motivation.
/// </summary>
public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	public Player Owner => Main.player[Projectile.owner];
}