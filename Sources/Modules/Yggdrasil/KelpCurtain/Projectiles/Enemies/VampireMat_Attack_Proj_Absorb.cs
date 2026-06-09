using Everglow.Yggdrasil.KelpCurtain.NPCs.VampireMat;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Enemies;

[Pipeline(typeof(WCSPipeline))]
public class VampireMat_Attack_Proj_Absorb : VisualProjectile
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicProjectiles;

	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 1200;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.friendly = false;
		Projectile.hostile = true;
		ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1024;
		base.SetDefaults();
	}

	public override void OnSpawn(IEntitySource source)
	{
		base.OnSpawn(source);
	}

	public override void AI()
	{
		Projectile.velocity *= 0;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return false;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		VampireMat.VampireMatHitCommonEffect(target);
		base.OnHitPlayer(target, info);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void Draw()
	{
		List<Vertex2D> bars = [];
		SpriteBatchUtils.AddVerticesForCircleRing(bars, Projectile.Center, 500, 50, new Color(1f, 0, 0, 0), 0, 5);
		Ins.Batch.Draw(Commons.ModAsset.Trail_10.Value, bars, PrimitiveType.TriangleStrip);
	}
}