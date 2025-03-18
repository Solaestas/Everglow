using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;

public class BloodySwamp_area : ModProjectile
{
	public override string Texture => Commons.ModAsset.Empty_Mod;

	public override void SetDefaults()
	{
		Projectile.timeLeft = 1200;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.penetrate = -1;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override void OnSpawn(IEntitySource source) => base.OnSpawn(source);

	public override void AI()
	{
		Projectile.velocity *= 0;
		Color colorLight = new Color(0.9f, 0.0f, Main.rand.NextFloat(0.12f), 1);
		float scaleMul = Main.rand.NextFloat(75, 115);
		var dustVFX = new Heart_VFX_spin
		{
			omega = 0.01f + scaleMul * 0.0002f,
			rotatedCenter = Projectile.Center,
			radius = scaleMul,
			rotPos = Main.rand.NextFloat(MathHelper.TwoPi),
			Active = true,
			Visible = true,
			position = Projectile.Center,
			maxTime = Main.rand.Next(70, 120),
			maxScale = scaleMul / 6f + Main.rand.NextFloat(-5, 5),
			scale = Main.rand.Next(8, 10),
			color = colorLight,
			ai = new float[] { Main.rand.NextFloat(1f, 8f) },
		};
		Ins.VFXManager.Add(dustVFX);
		if (Projectile.timeLeft % 60 == 0)
		{
			foreach (Player player in Main.player)
			{
				if ((player.Center - Projectile.Center).Length() < 110)
				{
					player.Heal(5);
				}
			}
		}
	}

	public override void OnKill(int timeLeft)
	{
		base.OnKill(timeLeft);
	}

	public override bool PreDraw(ref Color lightColor)
	{

		return false;
	}
}