using Terraria;
namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class GoldLanternLine7 : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("灯笼须7");
	}
	public override void SetDefaults()
	{
		Projectile.width = 1;
		Projectile.height = 1;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.light = 0.1f;
		Projectile.timeLeft = 6799;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = -1;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
	}
	private float Add = 0;
	public override void AI()
	{
		Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
		Vector2 vpos = new Vector2(0, 300).RotatedBy(Add / 100d);
		vpos.Y *= 0.25f;
		Vector2 v = player.Center - Projectile.Center + vpos + new Vector2(0, -400);
		v = v / (v.Length() + 1) / (v.Length() + 1) * 100;
		Vector2 v2 = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
		Add += 1;
		if (Projectile.timeLeft % 200 == 0)
			Projectile.extraUpdates += 1;
		if (Projectile.extraUpdates > 20)
			Projectile.Kill();
		if (Projectile.timeLeft % 10 == 0)//Projectile.InheritSource(Projectile)
			Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, v2, ModContent.ProjectileType<GoldLanternLine6>(), 2, 0, player.whoAmI, 0, 0);
		Projectile.velocity += v;
		if (sca < 1)
			sca += 0.03f;
		else
		{
			sca = 1;
		}
		if (Wid < 12f)
			Wid += 0.2f;
		else
		{
			Wid = 12;
		}
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		Projectile.Kill();
	}
	public override void OnKill(int timeLeft)
	{
		Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
		for (int j = 0; j < 10; j++)
		{
			Vector2 v2 = Projectile.velocity.RotatedBy(j / 5f * Math.PI);
			Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, v2, ModContent.ProjectileType<GoldLanternLine3>(), 0, 0, player.whoAmI, 0, 0);
		}
		Kill(timeLeft);
	}
	private float Wid = 0;
	private float sca = 0;
	int TrueL = 1;
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		int width = 60;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft;
		TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			TrueL++;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 216, 0, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 216, 0, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 216, 0, 0), new Vector3(0, 0.5f, 1));
			Vx.Add(bars[1]);
			Vx.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				Vx.Add(bars[i]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 1]);

				Vx.Add(bars[i + 1]);
				Vx.Add(bars[i + 2]);
				Vx.Add(bars[i + 3]);
			}
		}
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapLanternLine").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
	}
}
