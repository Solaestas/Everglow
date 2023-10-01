using Terraria;
namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Hepuyuan;

public class HepuyuanSpice : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 100;
		Projectile.height = 180;
		Projectile.friendly = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 80;
		Projectile.penetrate = -1;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		var Vx = new List<Vertex2D>();
		Vector2 Vbase = Projectile.Center - Main.screenPosition + new Vector2(0, 24 * player.gravDir);
		var v0 = new Vector2(0, -1);
		var v0T = new Vector2(1, 0);
		float length = Projectile.ai[0];
		v0 = v0 * length * Math.Clamp((80 - Projectile.timeLeft) / 12f, 0, 1f);
		v0T = v0T * 77.77f;
		v0 = v0.RotatedBy(Projectile.rotation);
		v0T = v0T.RotatedBy(Projectile.rotation);

		Color ct = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);
		ct.A = 180;
		var cp = new Color(200, 200, 200, 0);
		float fadeK = Math.Clamp((Projectile.timeLeft - 10) / 24f, 0, 1f);
		float fadeG = Math.Clamp((Projectile.timeLeft - 10) / 24f + 0.12f, 0, 1f);

		Vx.Add(new Vertex2D(Vbase + v0 * 2, cp, new Vector3(1, 0, 0)));
		Vx.Add(new Vertex2D(Vbase + (v0 + v0T) * fadeG + v0 * 2 * (1 - fadeG), cp, new Vector3(1, fadeG, 0)));
		Vx.Add(new Vertex2D(Vbase + v0 * 2 * (1 - fadeG), cp, new Vector3(1 - fadeG, fadeG, 0)));

		Vx.Add(new Vertex2D(Vbase + v0 * 2, cp, new Vector3(1, 0, 0)));
		Vx.Add(new Vertex2D(Vbase + v0 * 2 * (1 - fadeG), cp, new Vector3(1 - fadeG, fadeG, 0)));
		Vx.Add(new Vertex2D(Vbase + (v0 - v0T) * fadeG + v0 * 2 * (1 - fadeG), cp, new Vector3(1 - fadeG, 0, 0)));

		if (Projectile.Center.X > player.Center.X)
		{
			Vx.Add(new Vertex2D(Vbase + v0 * 2, ct, new Vector3(1, 0, 0)));
			Vx.Add(new Vertex2D(Vbase + v0 * 2 * (1 - fadeK), ct, new Vector3(1 - fadeK, fadeK, 0)));
			Vx.Add(new Vertex2D(Vbase + (v0 - v0T) * fadeK + v0 * 2 * (1 - fadeK), ct, new Vector3(1 - fadeK, 0, 0)));
		}
		else
		{
			Vx.Add(new Vertex2D(Vbase + v0 * 2, ct, new Vector3(1, 0, 0)));
			Vx.Add(new Vertex2D(Vbase + (v0 + v0T) * fadeK + v0 * 2 * (1 - fadeK), ct, new Vector3(1, fadeK, 0)));
			Vx.Add(new Vertex2D(Vbase + v0 * 2 * (1 - fadeK), ct, new Vector3(1 - fadeK, fadeK, 0)));
		}

		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/Misc/Projectiles/Weapon/Melee/Hepuyuan/HepuyuanSpice").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		return false;
	}
	public override void AI()
	{
		if (Projectile.timeLeft <= 78)
			Projectile.friendly = false;
		Projectile.hide = true;
	}
	public static int CyanStrike = 0;
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		CyanStrike = 1;
		Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
	}
	public override void Load()
	{
		On_CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
	}
	private int CombatText_NewText_Rectangle_Color_string_bool_bool(On_CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
	{
		if (CyanStrike > 0)
		{
			color = new Color(0, 255, 174);
			CyanStrike--;
		}
		return orig(location, color, text, dramatic, dot);
	}
}