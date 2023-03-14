namespace Everglow.Myth.Bosses.Acytaea.Projectiles;

internal class AcytaeaLight : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 70;
		Projectile.height = 70;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.extraUpdates = 3;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
	}

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
	}

	private float ka = 0;

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Range += 3;
		dx = 200 - Projectile.timeLeft;
		fx = (float)((-(1 / (dx / 4d + 0.25) + Math.Log(dx / 4d + 0.4)) + 3.95) * 40f);
		Projectile.velocity *= 0.99f;
		if (ka == 0)
			ka = Main.rand.NextFloat(Main.rand.NextFloat(0.15f, 1f), 1f);
	}

	private float Range = 30;
	private float dx = 0;
	private float fx = 40;
	private bool[] haihit = new bool[240];

	public override void PostDraw(Color lightColor)
	{
		double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		int width = (int)fx;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		var bars = new List<Vertex2D>();
		Effect ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		for (int i = 0; i < Range; ++i)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 v = new Vector2(Range, 0).RotatedBy((i - Range / 2d) / Range * 2);
			var v2 = new Vector2(v.X, v.Y * Projectile.ai[0]);
			Vector2 v3 = v2.RotatedBy(o1);
			/*for (int j = 0; j < 150; j++)
            {
                if (Collision.SolidCollision(v3 * (1 + width / 200f) / (float)(Math.Sqrt(ka)) + Projectile.Center - Vector2.One * 5f, 10, 10))
                {
                    v3 *= 0.95f;
                    if (!Main.gamePaused)
                    {
                        Projectile.velocity *= 0.999f;
                    }
                }
                else
                {
                    break;
                }
            }*/
			if (!Main.gamePaused)
			{
				if (i % 15 == 0)
				{
					for (int j = 0; j < Main.player.Length; j++)
					{
						if (!Main.player[j].dead)
						{
							if ((Main.player[j].Center - (v3 * (1 + width / 200f) / (float)Math.Sqrt(ka) + Projectile.Center)).Length() < 40)
							{
								haihit[j] = true;
								if (!hi)
								{
									//Projectile.NewProjectile(null, Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Typeless.playerHit>(), Projectile.damage, 0, j, 0, 0);
								}
								hi = true;
							}
						}
					}
				}
			}
			var factor = i / Range;
			var color = Color.Lerp(Color.White, Color.Red, factor);
			var w = MathHelper.Lerp(1f, 0.05f, 0.1f);

			bars.Add(new Vertex2D(Projectile.Center + v3 * (1 - width / 200f) / (float)Math.Sqrt(ka), color, new Vector3((float)factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.Center + v3 * (1 + width / 200f) / (float)Math.Sqrt(ka), color, new Vector3((float)factor, 0, w)));
			if (i % 8 == 0)
			{
				float h0 = 1;
				if (Projectile.timeLeft < 60f)
					h0 = Projectile.timeLeft / 60f;
				float k0 = (255 - Projectile.alpha) * (float)Math.Sin(factor * Math.PI) / 555f * h0;
				Lighting.AddLight(Projectile.Center + v3 * (1 + width / 200f) / (float)Math.Sqrt(ka), k0, 0, 0);
			}
		}

		var triangleList = new List<Vertex2D>();

		if (bars.Count > 2)
		{
			triangleList.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Projectile.velocity, Color.White, new Vector3(0, 0.5f, 1));
			triangleList.Add(bars[1]);
			triangleList.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				triangleList.Add(bars[i]);
				triangleList.Add(bars[i + 2]);
				triangleList.Add(bars[i + 1]);

				triangleList.Add(bars[i + 1]);
				triangleList.Add(bars[i + 2]);
				triangleList.Add(bars[i + 3]);
			}
			RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
			ef.Parameters["uTransform"].SetValue(model * projection);
			ef.Parameters["uTime"].SetValue(0);
			Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapGhost").Value;
			Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/MoonLight2").Value;
			Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Grey").Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
			ef.CurrentTechnique.Passes[0].Apply();
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
			Main.graphics.GraphicsDevice.RasterizerState = originalState;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}

	private bool hi = false;
}