namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.TownNPCAttack;

public class Schorl_Mark : ModProjectile
{
	public int Timer;

	public NPC Target = null;

	public override void SetDefaults()
	{
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 60;
		Projectile.ArmorPenetration = 0;
		Projectile.friendly = true;
		Projectile.timeLeft = 140;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
	}

	public override void AI()
	{
		if (Target == null)
		{
			if (Projectile.ai[0] is >= 0 and < 200)
			{
				Target = Main.npc[(int)Projectile.ai[0]];
			}
		}
		Projectile.velocity *= 0;
		if (Target != null && Target.active)
		{
			Projectile.Center = Target.Center;
		}
		else
		{
			Projectile.active = false;
			return;
		}
	}

	public override bool ShouldUpdatePosition() => false;

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return base.Colliding(projHitbox, targetHitbox);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if (Target == null)
		{
			return false;
		}
		Texture2D tex = ModAsset.Schorl_Mark.Value;
		Main.EntitySpriteDraw(tex, Target.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), Target.rotation, tex.Size() * 0.5f, Target.scale, SpriteEffects.None, 0);
		return false;
	}
}