using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class SquamousRollingStone : ModProjectile
{
	public int StartDirection = 0;
	public override void OnSpawn(IEntitySource source)
	{
		StartDirection = Math.Sign(Projectile.ai[0]);
	}
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.width = 100;
		Projectile.height = 100;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1500;
		Projectile.aiStyle = -1;
	}
	public override void AI()
	{
		Projectile.velocity.X += StartDirection * 0.04f;
		Projectile.velocity.Y += 0.2f;
		Projectile.rotation += Projectile.velocity.X * 0.01f;
		if(Projectile.velocity.Length() > 4)
		{
			Dust dust = Dust.NewDustDirect(Projectile.Bottom, 0, 0, ModContent.DustType<SquamousShellStone>(), Projectile.velocity.X * Main.rand.NextFloat(0.3f, 0.6f), Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 0.6f), 0, default, Main.rand.NextFloat(0.9f, 1.6f));
			dust.noGravity = false;
			GenerateSmogAtBottom(1);
		}
	}
	public void GenerateSmogAtBottom(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(0f, 1f))).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Bottom + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(17, 45),
				scale = Main.rand.NextFloat(10f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (MathF.Abs(Projectile.velocity.Y) > 2)
		{
			for(int x = 0;x < Projectile.velocity.Length() * 5;x++)
			{
				Vector2 newVel = Projectile.velocity.RotateRandom(6.283);
				Dust dust = Dust.NewDustDirect(Projectile.Bottom, 0, 0, ModContent.DustType<SquamousShellStone>(), newVel.X * Main.rand.NextFloat(0.3f, 0.6f), newVel.Y * Main.rand.NextFloat(0.3f, 0.6f), 0, default, Main.rand.NextFloat(0.9f, 1.6f));
				dust.noGravity = false;
			}
		}
		if(Projectile.velocity.X * Projectile.oldVelocity.X < 0)
		{
			Projectile.velocity.X = 0;
		}
		return false;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D drawTex = ModAsset.SquamousRollingStone.Value;
		Main.spriteBatch.Draw(drawTex,Projectile.Center - Main.screenPosition,null,lightColor,Projectile.rotation,drawTex.Size() * 0.5f,Projectile.scale,SpriteEffects.None,0);
		return false;
	}
	public override void Kill(int timeLeft)
	{

	}
}

