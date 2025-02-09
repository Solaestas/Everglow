namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class IstafelsSunfireGrasp_SkillVFX : ModProjectile
{
	public const int TimeLeftMax = 30;

	public override string Texture => Commons.ModAsset.Trail_1_Mod;

	public int TargetWhoAmI => (int)Projectile.ai[0];

	public float TargetSize => (int)Projectile.ai[1];

	public override void SetDefaults()
	{
		Projectile.width = 50;
		Projectile.height = 50;

		Projectile.timeLeft = TimeLeftMax;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
	}

	public override void AI()
	{
		if (TargetWhoAmI >= 0 && Main.npc[TargetWhoAmI].active)
		{
			Projectile.Center = Main.npc[TargetWhoAmI].Center;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var drawCenter = Projectile.Center - Main.screenPosition;
		var drawColor = new Color(1f, 0.3f, 0f, 0);
		var progress = MathF.Pow(Projectile.timeLeft / (float)TimeLeftMax, 2);

		var scale = progress * MathF.Max(0.5f, TargetSize / 100f);
		var vertices = new List<Vertex2D>();
		for (int i = 0; i <= 30; i++)
		{
			var direction = new Vector2(1, 0).RotatedBy(MathHelper.TwoPi * i / 30f);
			vertices.Add(drawCenter + direction * 120 * scale, drawColor, new(i, 0, 0));
			vertices.Add(drawCenter + direction * 200 * scale, drawColor, new(i, 1, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		return false;
	}
}