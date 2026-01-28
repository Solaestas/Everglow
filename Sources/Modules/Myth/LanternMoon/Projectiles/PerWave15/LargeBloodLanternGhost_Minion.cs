using Everglow.Commons.DataStructures;
using Everglow.Myth.LanternMoon.NPCs;

namespace Everglow.Myth.LanternMoon.Projectiles.PerWave15;

public class LargeBloodLanternGhost_Minion : ModProjectile
{
	public float Timer = 0;

	public NPC OwnerNPC;

	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 500;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;
	}

	public override void AI()
	{
		Timer++;
		if(OwnerNPC != null && OwnerNPC.active && OwnerNPC.type == ModContent.NPCType<LargeBloodLanternGhost>())
		{
			if(Timer < 120)
			{
				var toTarget = OwnerNPC.Center - Projectile.Center - Projectile.velocity + new Vector2(MathF.Sin(Timer * 0.03f + Projectile.whoAmI) * 120f, MathF.Sin(Timer * 0.015f + Projectile.whoAmI) * 50f);
				Projectile.velocity = toTarget.SafeNormalize(Vector2.Zero) * 6 * 0.1f + Projectile.velocity * 0.9f;
			}
			else if (Timer < 240)
			{
				Vector2 targetPos = Vector2.zeroVector;
				int targetIndex = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
				Player player = null;
				if (targetIndex != -1)
				{
					player = Main.player[targetIndex];
				}
				if (player != null)
				{
					targetPos = player.Center;
				}
				if(targetPos != Vector2.zeroVector)
				{
					var toTarget = targetPos - Projectile.Center - Projectile.velocity;
					Projectile.velocity = toTarget.SafeNormalize(Vector2.Zero) * 4 * 0.1f + Projectile.velocity * 0.9f;
				}
			}
		}
		float fade = 1f;
		if (Projectile.timeLeft < 60f)
		{
			fade = Projectile.timeLeft / 60f;
			Projectile.hostile = false;
		}
		Projectile.rotation = MathF.Sin(Timer * 0.07f + Projectile.whoAmI) * 0.4f;
		Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.3f * MathF.Sin(Timer * 0.03f + Projectile.whoAmI) + 0.3f, 0.3f * MathF.Cos(Timer * 0.03f + Projectile.whoAmI) + 0.3f) * 0.6f * fade);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		float fade = 1f;
		if (Projectile.timeLeft < 60f)
		{
			fade = Projectile.timeLeft / 60f;
		}
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Lighting.GetColor(Projectile.Center.ToTileCoordinates()) *fade, Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}