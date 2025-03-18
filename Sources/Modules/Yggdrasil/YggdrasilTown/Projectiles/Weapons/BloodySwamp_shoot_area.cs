using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Mono.Cecil;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;

public class BloodySwamp_shoot_area : ModProjectile
{
	public override string Texture => Commons.ModAsset.Empty_Mod;

	public override void SetDefaults()
	{
		Projectile.timeLeft = 12000;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 6;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.DamageType = DamageClass.Magic;
	}

	public Vector2 EndPos = default;

	public override void OnSpawn(IEntitySource source)
	{
		EndPos = Main.MouseWorld;
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];

		Vector3 colorLight = new Vector3(0.9f, 0.0f, 0.12f);
		Vector2 velocity = new Vector2(0, 2).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) + Projectile.velocity * 0.25f;
		var somg = new BloodSwampDust
		{
			velocity = velocity,
			Active = true,
			Visible = true,
			position = Projectile.Center,
			maxTime = 90,
			scale = 25,
			rotation = Main.rand.NextFloat(6.283f),
			MaxScale = Main.rand.NextFloat(12.0f, 28.0f),
			ChasedProjectile = Projectile,
			ai = new float[] { Main.rand.NextFloat(-0.12f, 0.12f), Main.rand.NextFloat(MathHelper.TwoPi), 0 },
		};
		Ins.VFXManager.Add(somg);
		Lighting.AddLight(Projectile.Center, colorLight);
		if((Projectile.Center - EndPos).Length() < 20)
		{
			Projectile.Center = EndPos;
			Projectile.Kill();
		}
	}

	public override void OnKill(int timeLeft)
	{
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<BloodySwamp_area>(), Projectile.damage, 0, Projectile.owner);
		base.OnKill(timeLeft);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
}