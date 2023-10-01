using Everglow.Myth.Common;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.CursedFlames;

public class CursedFlameHit : ModProjectile, IWarpProjectile
{
	public override bool CloneNewInstances => false;
	public override bool IsCloneable => false;

	public override void SetDefaults()
	{
		Projectile.width = 120;
		Projectile.height = 120;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override void AI()
	{
		Projectile.velocity *= 0.95f;

		if (Projectile.timeLeft <= 198)
			Projectile.friendly = false;


		int MaxC = (int)(Projectile.ai[0] / 6 + 5);
		MaxC = Math.Min(26, MaxC);
		Projectile.velocity *= 0;
	}

	public override void PostDraw(Color lightColor)
	{
		Texture2D Shadow = ModAsset.CursedHitLight.Value;
		float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
		Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(55, 255, 0, 0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * Dark, SpriteEffects.None, 0);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D Shadow = ModAsset.CursedHit.Value;
		float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
		Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, Color.White * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f, SpriteEffects.None, 0);
		Texture2D light = ModAsset.CursedHitStar.Value;
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(55, 255, 0, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 40f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(55, 255, 0, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] / 40f, SpriteEffects.None, 0);
		float size = Math.Clamp(Projectile.timeLeft / 8f - 10, 0f, 20f);
		return false;
	}
	private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();

		Color c0 = color;
		c0.R = 0;
		for (int h = 0; h < radious / 2; h += 1)
		{

			c0.R = (byte)(h / radious * 2 * 255);
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), c0, new Vector3(h * 2 / radious, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), c0, new Vector3(h * 2 / radious, 0, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), c0, new Vector3(1, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), c0, new Vector3(1, 0, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), c0, new Vector3(0, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), c0, new Vector3(0, 0, 0)));
		if (circle.Count > 2)
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float value = (200 - Projectile.timeLeft) / 200f;
		float colorV = 0.9f * (1 - value);
		if (Projectile.ai[0] >= 10)
			colorV *= Projectile.ai[0] / 10f;
		Texture2D t = Commons.ModAsset.Trail.Value;
		float width = 60;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft;

		DrawTexCircle_VFXBatch(spriteBatch, value * 27 * Projectile.ai[0], width * 2, new Color(colorV, colorV * 0.6f, colorV, 0f), Projectile.Center - Main.screenPosition, t, Math.PI * 0.5);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.CursedInferno, 300);
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		target.AddBuff(BuffID.CursedInferno, 300);
	}
}