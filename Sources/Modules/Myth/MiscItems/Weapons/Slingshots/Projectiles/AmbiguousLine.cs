using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	public class AmbiguousLine : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override void AI()
		{
			Projectile.scale = Projectile.timeLeft / 60f;
			if (Projectile.timeLeft <= 58)
			{
				Projectile.friendly = false;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Texture2D TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, null, Color.White, MathF.PI / 2, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
			foreach (var proj in Main.projectile)
			{
				if (proj.ai[0] == Projectile.ai[0])
				{
					DrawShadowLine(proj.Center - Main.screenPosition, Projectile.Center - Main.screenPosition, Projectile.scale * 15);
				}
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public void DrawShadowLine(Vector2 StartPos, Vector2 EndPos, float width)
		{
			Color color = Color.White;
			Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * width * 4;
			List<Vertex2D> vertex2Ds = new List<Vertex2D>();
			vertex2Ds.Add(new Vertex2D(StartPos + Width, color, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width, color, new Vector3(0, 1, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + Width, color, new Vector3(1, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - Width, color, new Vector3(1, 1, 0)));
			Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotTrailBlack");
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);

			color = new Color(255, 255, 255, 0);
			Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * width;
			vertex2Ds = new List<Vertex2D>();
			vertex2Ds.Add(new Vertex2D(StartPos + Width, color, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width, color, new Vector3(0, 1, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + Width, color, new Vector3(1, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - Width, color, new Vector3(1, 1, 0)));

			Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/ShadowTrail");
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);

			color = Color.White;
			Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * width * 4;
			vertex2Ds = new List<Vertex2D>();
			vertex2Ds.Add(new Vertex2D(StartPos + Width, color, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width, color, new Vector3(0, 1, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + Width, color, new Vector3(1, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - Width, color, new Vector3(1, 1, 0)));
			Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/ShadowTrailFlame");
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);
		}
	}
}
