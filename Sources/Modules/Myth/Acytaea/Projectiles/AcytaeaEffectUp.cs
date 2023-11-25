using Terraria;

namespace Everglow.Myth.Acytaea.Projectiles;

internal class AcytaeaEffectUp : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.extraUpdates = 6;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}

	//bool shake;
	public override void AI()
	{
		//TODO Shake verify and adjust if needed
		//ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
		//mplayer.FlyCamPosition = new Vector2(0, 28).RotatedByRandom(6.283);
		ShakerManager.AddShaker(UndirectedShakerInfo.Create(Main.LocalPlayer.Center, 28));
		//if (!shake) // KEEP FOR REFERENCE
		//{
		//    MythContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythContentPlayer>();
		//    mplayer.ShakeStrength = 7;
		//    mplayer.Shake = 15;
		//    shake = true;
		//}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public static float timer = 0;
	public static int WHOAMI = -1;
	public static int Typ = -1;
	private float yd = 1;
	private Vector2[,] Vlaser = new Vector2[30, 200];

	public override void PostDraw(Color lightColor)
	{
		for (int k = 0; k < 15; ++k)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			var bars = new List<Vertex2D>();

			float step = 8;
			float fx = (200 - Projectile.timeLeft) / 200f;
			int Count = (int)(fx * fx * 200);
			for (int m = 0; m < Count; ++m)
			{
				Vlaser[k, m] = Projectile.Center + new Vector2(0, step).RotatedBy(k / 7.5d * Math.PI) * m;
			}
			for (int i = 1; i < Count; ++i)
			{
				if (Vlaser[k, i] == Vector2.Zero)
					break;

				var normalDir = Vlaser[k, i - 1] - Vlaser[k, i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

				var factor = (float)Math.Pow(Count - i, 0.2) / 1f + Projectile.timeLeft / 6f;
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				float width = (float)(2 * 8 * Math.PI / 30d * i + 7);

				width *= yd;
				Lighting.AddLight(Vlaser[k, i], (255 - Projectile.alpha) * 1.2f / 50f * yd, 0, 0);
				if (Count - i < 5)
				{
					bars.Add(new Vertex2D(Vlaser[k, i] + normalDir * width - Main.screenPosition, new Color(Math.Clamp((int)(155 * (Count - i - 1) / 5f), 0, 155), 0, 0, 0), new Vector3(factor % 1f, 1, w)));
					bars.Add(new Vertex2D(Vlaser[k, i] + normalDir * -width - Main.screenPosition, new Color(Math.Clamp((int)(155 * (Count - i - 1) / 5f), 0, 155), 0, 0, 0), new Vector3(factor % 1f, 0, w)));
				}
				else
				{
					bars.Add(new Vertex2D(Vlaser[k, i] + normalDir * width - Main.screenPosition, new Color(155, 0, 0, 0), new Vector3(factor % 1f, 1, w)));
					bars.Add(new Vertex2D(Vlaser[k, i] + normalDir * -width - Main.screenPosition, new Color(155, 0, 0, 0), new Vector3(factor % 1f, 0, w)));
				}
			}
			var Vx = new List<Vertex2D>();
			if (bars.Count > 2)
			{
				Vx.Add(bars[0]);
				var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + new Vector2(-5, 0).RotatedBy(Projectile.rotation), new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
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
			Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/ForgeWave2").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
	}
}