using Everglow.Commons.Weapons;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;
public class CrystalReflect : NoTextureProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 1;
		Projectile.height = 1;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.timeLeft = 60000;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override bool ShouldUpdatePosition()
	{
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		var drawData = new List<CrystalVertex>();
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		spriteBatch.End();
		graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
		spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
		spriteBatch.End();
		List<Player> players = new List<Player>() { Main.LocalPlayer };
		Main.PlayerRenderer.DrawPlayers(Main.Camera, players);


		graphicsDevice.SetRenderTarget(Main.screenTarget);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
		spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
		spriteBatch.End();

		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
			RasterizerState.CullNone);


		const float CrystalWidth = 150f;
		const float CrystalHeight = 125f;
		const float CrystalDepth = -75f;

		var center = new Vector3(Projectile.Center, 0);
		var top = new Vector3(Projectile.Center + new Vector2(0, -CrystalHeight), CrystalDepth);
		var left = new Vector3(Projectile.Center + new Vector2(-CrystalWidth, 0), CrystalDepth);
		var right = new Vector3(Projectile.Center + new Vector2(CrystalWidth, 0), CrystalDepth);
		var bot = new Vector3(Projectile.Center + new Vector2(0, CrystalHeight), CrystalDepth);


		Vector3 NtopRight;
		Vector3 NtopLeft;
		Vector3 NbotLeft;
		Vector3 NbotRight;
		CreateFace(center, right, top, out NtopRight);
		CreateFace(center, top, left, out NtopLeft);
		CreateFace(center, left, bot, out NbotLeft);
		CreateFace(center, bot, right, out NbotRight);

		//分割成四个三角面依次罗列

		drawData.AddRange(CreateFace(center, right, top, out NtopRight));

		drawData.AddRange(CreateFace(center, top, left, out NtopLeft));

		drawData.AddRange(CreateFace(center, left, bot, out NbotLeft));

		drawData.AddRange(CreateFace(center, bot, right, out NbotRight));

		Effect ef = Commons.ModAsset.ScreenReflection_NormalValue.Value;
		var viewport = Main.graphics.GraphicsDevice.Viewport;
		var projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 500);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

		ef.Parameters["uBaseColor"].SetValue(new Vector3(0.02f, 0.02f, 0.02f));
		ef.Parameters["uFresnelF0"].SetValue(new Vector3(0.17f));
		ef.Parameters["uKs"].SetValue(new Vector3(1.0f));
		ef.Parameters["uScreenDistanceMultipler"].SetValue(Vector2.One);
		ef.Parameters["uViewportSize"].SetValue(new Vector2(Main.screenTargetSwap.Width,
			Main.screenTargetSwap.Height));
		ef.Parameters["uModel"].SetValue(model);
		ef.Parameters["uMNormal"].SetValue(Matrix.Identity);
		ef.Parameters["uViewProj"].SetValue(Main.GameViewMatrix.TransformationMatrix * projection);

		ef.CurrentTechnique.Passes["Test"].Apply();
		Main.graphics.GraphicsDevice.Textures[0] = Main.screenTargetSwap;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, drawData.ToArray(), 0, drawData.Count / 3);
	}

	/// <summary>
	/// 创建一个ABC逆时针组成的三角形面，并且生成其法线信息
	/// </summary>
	/// <param name="A"></param>
	/// <param name="B"></param>
	/// <param name="C"></param>
	/// <returns></returns>
	private List<CrystalVertex> CreateFace(Vector3 A, Vector3 B, Vector3 C, out Vector3 N)
	{
		List<CrystalVertex> data = new List<CrystalVertex>();
		Vector3 Normal = Vector3.Normalize(Vector3.Cross(B - A, C - A));
		data.Add(new CrystalVertex(A, -Normal));
		data.Add(new CrystalVertex(B, -Normal));
		data.Add(new CrystalVertex(C, -Normal));
		N = -Normal;
		return data;
	}

	private struct CrystalVertex : IVertexType
	{
		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[2]
		{
				new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
		});

		public Vector3 Position;
		public Vector3 Normal;

		public CrystalVertex(Vector3 position, Vector3 normal)
		{
			Position = position;
			Normal = normal;
		}

		public VertexDeclaration VertexDeclaration
		{
			get
			{
				return _vertexDeclaration;
			}
		}
	}
}
