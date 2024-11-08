using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Projectile : ModProjectile
{
	private const int SearchDistance = 600;

	private int targetWhoAmI = -1;

	private int TargetWhoAmI
	{
		get => targetWhoAmI;
		set => targetWhoAmI = value;
	}

	public override void SetStaticDefaults()
	{
		Main.projFrames[Type] = 4;
	}

	public override void SetDefaults()
	{
		Projectile.width = 14;
		Projectile.height = 26;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 240;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
	}

	private bool HasTarget => TargetWhoAmI >= 0;

	public override void AI()
	{
		if (Projectile.timeLeft % 8 == 0)
		{
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
		}

		if (HasTarget)
		{
			var targetPos = HasTarget ? Main.npc[targetWhoAmI].Center : Vector2.Zero;
			var targetVel = Vector2.Normalize(targetPos - Projectile.Center) * 10f;
			Projectile.velocity = (targetVel + Projectile.velocity * 10) / 11f;
		}
		else
		{
			Projectile.Minion_FindTargetInRange(SearchDistance, ref targetWhoAmI, false);
		}
	}

	public override void OnKill(int timeLeft)
	{
		for (int i = 0; i < 15; i++)
		{
			Dust.NewDust(Projectile.Center, 1, 1, DustID.Shadowflame, newColor: new Color(81, 235, 202), Scale: Main.rand.NextFloat(1f, 2));
		}
		SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var frame = texture.Frame(horizontalFrames: Main.projFrames[Type], frameX: Projectile.frame);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, 0, texture.Size() / 2, 1, SpriteEffects.None, 0);
		return false;
	}
}