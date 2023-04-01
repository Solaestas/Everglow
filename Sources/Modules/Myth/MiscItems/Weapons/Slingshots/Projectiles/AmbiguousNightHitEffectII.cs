using Everglow.Myth.MiscItems.Weapons.Slingshots.Buffs;

namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

public class AmbiguousNightHitEffectII : ModProjectile
{
	public override string Texture => "Everglow/Myth/MiscItems/Weapons/Slingshots/AmbiguousNight";
	public override void SetDefaults()
	{
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.friendly = false;
		Projectile.hostile = false;
	}
	public override void AI()
	{
		Projectile.velocity = Vector2.Zero;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();
		for (int h = 0; h < radious / 2; h++)
		{
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
		if (circle.Count > 0)
		{
			Main.graphics.GraphicsDevice.Textures[0] = tex;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<ShadowSupervisor>(), (int)(600 * Projectile.ai[0]) + 120);
	}
}
