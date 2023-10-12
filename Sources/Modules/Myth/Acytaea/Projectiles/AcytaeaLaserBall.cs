using Everglow.Myth.Common;
using Terraria.Localization;

namespace Everglow.Myth.Acytaea.Projectiles;

public class AcytaeaLaserBall : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Red Laser");
			}

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.friendly = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 800;
		Projectile.hostile = true;
		Projectile.tileCollide = false;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
	}

	private int ControlP = -1;
	private float St = 0;
	private int AIMNpc = -1;
	private float Radius = 0;

	public override void AI()
	{
		if (Projectile.timeLeft < 60)
			St *= 0.96f;
		else
		{
			St += 40f;
		}
		if (AIMNpc < 0)
		{
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].type == ModContent.NPCType<NPCs.Acytaea>())
				{
					AIMNpc = i;
					break;
				}
			}
			if (AIMNpc != -1)
			{
				Vector2 Vv = Vector2.Normalize(Main.npc[AIMNpc].Center - Projectile.Center).RotatedBy(Math.PI / 2d) * 1;
				Projectile.velocity = Vv;
				Radius = (Main.npc[AIMNpc].Center - Projectile.Center).Length();
			}
		}
		if (AIMNpc != -1)
		{
			if (Main.npc[AIMNpc].type == ModContent.NPCType<NPCs.Acytaea>() && Main.npc[AIMNpc].active)
				Projectile.velocity += Vector2.Normalize(Main.npc[AIMNpc].Center - Projectile.Center) * 1 / Radius;
		}
		if (Projectile.timeLeft == 750)
		{
			Vector2 vp = Vector2.One;
			for (int f = 0; f < 200; f++)
			{
				if (Main.npc[f].type == ModContent.NPCType<NPCs.Acytaea>())
				{
					vp = Main.npc[f].Center - Projectile.Center;
					break;
				}
			}

			float Rot = (float)Math.Atan2(vp.Y, vp.X) + (float)(Math.PI / 2d);
			ControlP = Projectile.NewProjectile(null, Projectile.Center, new Vector2(34, 0).RotatedBy(Projectile.ai[0]), ModContent.ProjectileType<AcytaeaLaser>(), 100, 3, Main.LocalPlayer.whoAmI, Rot);
		}
		if (ControlP != -1)
		{
			if (Main.projectile[ControlP].type == ModContent.ProjectileType<AcytaeaLaser>() && Main.projectile[ControlP].active)
				Main.projectile[ControlP].Center = Projectile.Center;
		}
		St *= 0.99f;
	}

	public override void OnKill(int timeLeft)
	{
	}

	public override void PostDraw(Color lightColor)
	{

		Effect ef2 = MythContent.QuickEffect("Bosses/Acytaea/SpherePerspective3");
		var triangleList2 = new List<Vertex2D>();
		int radius = (int)(St / 80f);//sss
		triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(radius, radius), Color.White, new Vector3(-1, 1, 0)));
		triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(radius, -radius), Color.White, new Vector3(-1, -1, 0)));
		triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(-radius, -radius), Color.White, new Vector3(1, -1, 0)));

		triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(radius, radius), Color.White, new Vector3(-1, 1, 0)));
		triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(-radius, -radius), Color.White, new Vector3(1, -1, 0)));
		triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(-radius, radius), Color.White, new Vector3(1, 1, 0)));

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		RasterizerState originalState2 = Main.graphics.GraphicsDevice.RasterizerState;


		var projection2 = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model2 = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

		ef2.Parameters["uTransform"].SetValue(model2 * projection2);
		ef2.Parameters["circleCenter"].SetValue(new Vector3(0, 0, -2));
		ef2.Parameters["radiusOfCircle"].SetValue(1f);
		ef2.Parameters["uTime"].SetValue((float)Main.time * 0.02f);
		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/RedBall2").Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		ef2.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);
		Main.graphics.GraphicsDevice.RasterizerState = originalState2;
		Main.spriteBatch.End();
		//TODO 万象你到底有多少Immediate啊！
		//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}