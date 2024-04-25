using Everglow.Commons.Weapons;
using Terraria;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class LampFruitCurrent : TrailingProjectile
{
	public override void SetDef()
	{	
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.hostile = true;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 6;
		Projectile.timeLeft = 3600;
		TrailTexture = Commons.ModAsset.Trail_2.Value;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
		TrailColor = new Color(1, 0.7f, 0.1f, 0f);
		TrailWidth = 15f;
	}
	public override void AI()
	{
		if(TimeTokill < 0)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1, 0.7f, 0.1f));
			if(Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4),0,0,ModContent.DustType<Dusts.LampWood_Dust_fluorescent_appear>());
				dust.alpha = 0;
				dust.rotation = Main.rand.NextFloat(0.3f, 0.7f);
				dust.velocity = Projectile.velocity * 0.1f;
			}
		}
		else
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1, 0.7f, 0.1f) * TimeTokill / 10f);
		}
		base.AI();
	}
	public override void DrawSelf()
	{
		if(Projectile.timeLeft > 3597)
		{
			return;
		}
		base.DrawSelf();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if(Main.rand.NextBool(2))
		{
			target.AddBuff(ModContent.BuffType<Buffs.Photolysis>(), 240);
		}
		base.OnHitNPC(target, hit, damageDone);
	}
	public override void KillMainStructure()
	{
		for(int i = 0;i < 15;i++)
		{
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<Dusts.LampWood_Dust_fluorescent_appear>());
			dust.alpha = 0;
			dust.rotation = Main.rand.NextFloat(0.3f, 0.9f);
			dust.velocity = Vector2.One.RotateRandom(6.283) * 3.4f;
		}
		base.KillMainStructure();
	}
}