using Terraria;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Projectile : ModProjectile
{
	private const int SearchDistance = 600;

	private int targetWhoAmI = 0;

	private Player Owner => Main.player[Projectile.owner];

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
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 240;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		if (TargetWhoAmI >= 0)
		{
			TargetWhoAmI = (int)Projectile.ai[0];
		}
	}

	private bool HasTarget => TargetWhoAmI >= 0;

	public override void AI()
	{
		if (Main.time % 4 == 0)
		{
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
		}

		//if (!HasTarget)
		//{
		//	Projectile.Minion_FindTargetInRange(SearchDistance, ref targetWhoAmI, false);
		//}

		var targetPos = HasTarget ? Main.npc[targetWhoAmI].Center : Vector2.Zero;
		var targetVel = Vector2.Normalize(targetPos - Projectile.Center) * 10f;
		Projectile.velocity = (targetVel + Projectile.velocity * 10) / 11f;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var frame = texture.Frame(horizontalFrames: Main.projFrames[Type], frameX: Projectile.frame);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, 0, texture.Size() / 2, 1, SpriteEffects.None, 0);
		return false;
	}
}