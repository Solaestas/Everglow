using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Myth.Bosses.Acytaea.Projectiles
{
	public class BrokenAcytaea : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Broken Acytaea");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "碎雅斯塔亚");
		}

		private float RamdomC = -1;
		private float RamdomC2 = -1;

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 400000;
			//Projectile.extraUpdates = 10;
			Projectile.tileCollide = false;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
		}

		private float Ome = 0;
		private float Ros = 0;
		private float Theta = 0;
		private bool Ang = false;
		private Vector2 p1;
		private Vector2 p2;
		private Vector2 p3;
		private Vector2 po1;
		private Vector2 po2;
		private Vector2 po3;

		public override void AI()
		{
			if (Projectile.timeLeft == 399999)
			{
				Projectile.timeLeft = Main.rand.Next(3500, 4600);
				if (RamdomC == -1)
				{
					RamdomC = Main.rand.NextFloat(0f, 1500f);
					RamdomC2 = Main.rand.NextFloat(0f, 0.25f);
				}
			}
			RamdomC += RamdomC2;
			if (!Ang)
			{
				Ang = true;
				Ome = Main.rand.NextFloat(-0.15f, 0.15f);
				Ros = Main.rand.NextFloat(-0.15f, 0.15f);
				Theta += Main.rand.NextFloat(-3.14f, 3.14f);
				p1 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
				p2 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
				p3 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
			}
			Theta += Ros;
			po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
			po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
			po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
			Projectile.rotation -= Ome * 0.66f;
			Projectile.velocity *= 0.99f;
			Projectile.scale *= 0.99f;
			if (Projectile.scale < 0.05f)
				Projectile.Kill();
		}

		public override void PostDraw(Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			var Vx = new List<VertexInfo2>
			{
				new VertexInfo2(po1 + Projectile.Center - Main.screenPosition, new Color(255, (int)(122.5 + Math.Sin(RamdomC) * 122.5), (int)(122.5 + Math.Sin(RamdomC) * 122.5), 0), new Vector3(0, 0, 0)),
				new VertexInfo2(po2 + Projectile.Center - Main.screenPosition, new Color(255, (int)(122.5 + Math.Sin(RamdomC) * 122.5), (int)(122.5 + Math.Sin(RamdomC) * 122.5), 0), new Vector3(0, 0, 0)),
				new VertexInfo2(po3 + Projectile.Center - Main.screenPosition, new Color(255, (int)(122.5 + Math.Sin(RamdomC) * 122.5), (int)(122.5 + Math.Sin(RamdomC) * 122.5), 0), new Vector3(0, 0, 0))
			};
			Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
		}

		private struct VertexInfo2 : IVertexType
		{
			private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
			{
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
				new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
			});

			public Vector2 Position;
			public Color Color;
			public Vector3 TexCoord;

			public VertexInfo2(Vector2 position, Color color, Vector3 texCoord)
			{
				Position = position;
				Color = color;
				TexCoord = texCoord;
			}

			public VertexDeclaration VertexDeclaration
			{
				get
				{
					return _vertexDeclaration;
				}
			}

			public static CullMode Cull => CullMode.None;
		}
	}
}