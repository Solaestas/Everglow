using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Buffs;
using Terraria;

namespace Everglow.Myth.TheFirefly.Projectiles;

internal class DarkFanFly : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 450;
		Projectile.extraUpdates = 1;
		Projectile.tileCollide = true;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.scale = 1.5f;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Player player = Main.player[Projectile.owner];
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<FanHit>(), 0, 0, player.whoAmI, 15, Main.rand.NextFloat(6.283f));
		int[] array = Projectile.localNPCImmunity;
		bool flag = !Projectile.usesLocalNPCImmunity && !Projectile.usesIDStaticNPCImmunity || Projectile.usesLocalNPCImmunity && array[target.whoAmI] == 0 || Projectile.usesIDStaticNPCImmunity && Projectile.IsNPCIndexImmuneToProjectileType(Projectile.type, target.whoAmI);
		if (target.active && !target.dontTakeDamage && flag && (target.aiStyle != 112 || target.ai[2] <= 1f))
		{
			if (target.active)
				Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}
		target.AddBuff(ModContent.BuffType<OnMoth>(), 300);

		if (MothBuffTarget.mothStack[target.whoAmI] < 5)
			MothBuffTarget.mothStack[target.whoAmI] += 1;
		else
		{
			MothBuffTarget.mothStack[target.whoAmI] = 5;
		}
		target.AddBuff(ModContent.BuffType<FireflyInferno>(), 120);
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Vector2 v0 = player.Center - Projectile.Center;
		Vector2 v1 = Vector2.Normalize(v0) * 120f;
		Vector2 v2 = player.Center + v1 - Projectile.Center;
		Vector2 v3 = Vector2.Normalize(v2) * 0.48f;
		Projectile.velocity += v3;
		if (Projectile.timeLeft is < 380 and > 60)
		{
			if (v0.Length() < 48)
				Projectile.timeLeft = 20;
		}
		Projectile.velocity *= 0.99f;
		for (int x = 58; x >= 0; x--)
		{
			OldVelocity[x + 1] = OldVelocity[x];
		}
		OldVelocity[0] = Projectile.velocity;

		for (int x = 58; x >= 0; x--)
		{
			OldScale[x + 1] = OldScale[x];
		}
		OldScale[0] = Projectile.scale;
		if (Projectile.timeLeft < 300)
			Projectile.tileCollide = false;
		if (Projectile.timeLeft < 60)
			Projectile.timeLeft -= 4;
		int frequency = 70 / (2 + player.maxMinions);
		if (Projectile.timeLeft % frequency == 0)
			Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Projectile.velocity * 0.3f, ModContent.ProjectileType<GlowingButterfly>(), Projectile.damage / 3, Projectile.knockBack, player.whoAmI, Main.rand.Next(2), 0f);
	}

	public override void Kill(int timeLeft)
	{
	}

	private Vector2 PosRot0 = new Vector2(-16, -100);
	private Vector2 PosRot1 = new Vector2(100, 0);
	private Vector2 PosRot2 = new Vector2(-100, 100);

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	private Vector2[] OldVelocity = new Vector2[60];
	private float[] OldScale = new float[60];

	public override void PostDraw(Color lightColor)
	{
		Texture2D tex = ModAsset.DarkFanFly.Value;
		Texture2D texG = ModAsset.DarkFanFlyGlow.Value;
		Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		var vertex = new List<Vertex2D>();
		Vector2 v0 = PosRot0.RotatedBy(Projectile.timeLeft / 10f);
		v0 = new Vector2(v0.X, v0.Y / 2f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
		Vector2 v1 = PosRot1.RotatedBy(Projectile.timeLeft / 10f);
		v1 = new Vector2(v1.X, v1.Y / 2f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
		Vector2 v2 = PosRot2.RotatedBy(Projectile.timeLeft / 10f);
		v2 = new Vector2(v2.X, v2.Y / 2f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
		vertex.Add(new Vertex2D(Projectile.Center + v0 * Projectile.scale - Main.screenPosition, color, new Vector3(84f / 200f, 0f / 200f, 0)));
		vertex.Add(new Vertex2D(Projectile.Center + v1 * Projectile.scale - Main.screenPosition, color, new Vector3(200f / 200f, 100f / 200f, 0)));
		vertex.Add(new Vertex2D(Projectile.Center + v2 * Projectile.scale - Main.screenPosition, color, new Vector3(0f / 200f, 200f / 200f, 0)));
		Main.graphics.GraphicsDevice.Textures[0] = tex;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex.ToArray(), 0, vertex.Count / 3);

		vertex.Add(new Vertex2D(Projectile.Center + v0 * Projectile.scale - Main.screenPosition, new Color(0, 100, 255, 0), new Vector3(84f / 200f, 0f / 200f, 0)));
		vertex.Add(new Vertex2D(Projectile.Center + v1 * Projectile.scale - Main.screenPosition, new Color(0, 100, 255, 0), new Vector3(200f / 200f, 100f / 200f, 0)));
		vertex.Add(new Vertex2D(Projectile.Center + v2 * Projectile.scale - Main.screenPosition, new Color(0, 100, 255, 0), new Vector3(0f / 200f, 200f / 200f, 0)));
		Main.graphics.GraphicsDevice.Textures[0] = texG;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex.ToArray(), 0, vertex.Count / 3);
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (Projectile.velocity.X != oldVelocity.X)
			Projectile.velocity.X = -oldVelocity.X;
		if (Projectile.velocity.Y != oldVelocity.Y)
			Projectile.velocity.Y = -oldVelocity.Y;
		Projectile.velocity *= 0.98f;
		return false;
	}
}