using System.Net;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class LightSeed : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 150;
	}
	Vector2 Point = Vector2.Zero;
	float x;
	float k;
	public override void AI()
	{
		Projectile.scale *= 0.985f;
		if (Projectile.scale > 0.7f)
		{
		   Projectile.ai[0]*=0.985f;
			
		}
		else
		{
           Projectile.scale *= 1.008f;
		}

		x += Projectile.ai[0];
		Projectile.tileCollide = true;
		Projectile.Center = Point + new Vector2(x, MathF.Sin(MathF.Abs(x)/ 30) *1800/(MathF.Abs(x) + 12) + k * x);

	}
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[0] = Projectile.velocity.X;
		k = Projectile.velocity.Y / Projectile.velocity.X;
		Point = Projectile.Center;
		x = 0;
		Projectile.velocity = Vector2.Zero;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Poisoned, 600);
	}
	public override void OnKill(int timeLeft)
	{

	}
}