using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class BrittleRockSlingshotStone : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.scale = 0.75f;
		Projectile.aiStyle = 0;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.penetrate = 2;
		Projectile.timeLeft = 600;
	}

	public override void AI()
	{
		if (Projectile.velocity.Y <= 12)
		{
			Projectile.velocity.Y += 0.2f;
		}
		if (Projectile.timeLeft == 597)
		{
			Projectile.tileCollide = true;
		}
		Projectile.rotation += Projectile.ai[0];
		if (Projectile.timeLeft > 540)
		{
			float value = (Projectile.timeLeft - 540) / 30f;
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<RockElemental_Energy_normal>());
			d.velocity = Projectile.velocity * 0.5f;
			d.scale = Main.rand.NextFloat(0.75f, 1f) * Math.Min(value, 1);
			d.noGravity = true;
		}
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.scale = Main.rand.NextFloat(0.75f, 1f);
		Projectile.rotation = Main.rand.NextFloat(6.283f);
		Projectile.ai[0] = Main.rand.NextFloat(-0.15f, 0.15f);
	}
	public override void OnKill(int timeLeft)
	{
		for (int x = 0; x < 3; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<RockElemental_fragments>(), 0f, 0f, 0, default, 0.7f);
			d.velocity = new Vector2(0, Main.rand.NextFloat(0.1f, 1.6f)).RotatedByRandom(6.283);
		}
		for (int x = 0; x < 5; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default, Main.rand.NextFloat(0.4f, 0.6f));
			d.velocity = new Vector2(0, Main.rand.NextFloat(0.7f, 4.1f)).RotatedByRandom(6.283);
		}
		GenerateSmog(3);
		SoundEngine.PlaySound(SoundID.NPCHit2.WithVolume(0.3f), Projectile.Center);
		if (Projectile.ai[1] != 1)
		{
			int n = Main.rand.Next(3, 4);
			for (int i = 0; i < n; i++)
			{
				Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathF.PI * 0.25f), ModContent.ProjectileType<BrittleRockSlingshotStone>(), Projectile.damage / n, Projectile.knockBack / n, Projectile.owner);
				p.scale = Projectile.scale / MathF.Sqrt(n);
				p.penetrate = 1;
				p.ai[1] = 1;
			}
		}
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(10f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.RockElemental_Stonefragment.Value;
		Vector2 drawCenter = Projectile.Center - Main.screenPosition;
		lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		if (Projectile.timeLeft > 540)
		{
			float value = (Projectile.timeLeft - 540) / 60f;
			for (int i = 0; i < 7; i++)
			{
				Main.EntitySpriteDraw(texture, drawCenter, null, new Color(0.7f, 0.4f, 1f, 0f) * value * 0.4f, Projectile.rotation + i, texture.Size() * 0.5f, Projectile.scale * (1.25f + value), SpriteEffects.None, 0);
			}
		}
		Main.EntitySpriteDraw(texture, drawCenter, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}