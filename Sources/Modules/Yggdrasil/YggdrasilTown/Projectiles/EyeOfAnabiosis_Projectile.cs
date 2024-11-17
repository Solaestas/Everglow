using Everglow.Yggdrasil.YggdrasilTown.VFXs;
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

	private NPC Target => Main.npc[TargetWhoAmI];

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
		Projectile.hide = true;
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
			if (!Target.active || Target.friendly || !Target.CanBeChasedBy())
			{
				return;
			}

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
			float size = Main.rand.NextFloat(0.3f, 0.96f);
			var acytaeaFlame = new AnabiosisFlameDust
			{
				Velocity = new Vector2(0, Main.rand.NextFloat(3, 4f)).RotatedByRandom(MathHelper.TwoPi),
				Active = true,
				Visible = true,
				Position = Projectile.Center,
				MaxTime = Main.rand.Next(24, 36),
				Scale = 25f * size,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Frame = Main.rand.Next(3),
				ai = new float[] { Projectile.Center.X, Main.rand.NextFloat(-0.8f, 0.8f) },
			};
			Ins.VFXManager.Add(acytaeaFlame);
		}
		SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var frame = texture.Frame(horizontalFrames: Main.projFrames[Type], frameX: Projectile.frame);
		var origin = new Vector2(texture.Width / 8, texture.Height / 2);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, 0, origin, 1, SpriteEffects.None, 0);
		return false;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		overPlayers.Add(index);
		base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
	}
}