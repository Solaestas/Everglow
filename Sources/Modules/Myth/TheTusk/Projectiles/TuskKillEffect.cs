namespace Everglow.Myth.TheTusk.Projectiles;

public class TuskKillEffect : ModNPC
{
	public override void SetDefaults()
	{
		NPC.width = 10;
		NPC.height = 10;
		NPC.aiStyle = -1;
		NPC.friendly = false;
		NPC.damage = 0;
		NPC.behindTiles = true;
		NPC.aiStyle = -1;
		NPC.alpha = 0;
		NPC.lifeMax = 1;
		NPC.dontTakeDamage = true;
		NPC.dontCountMe = true;
		NPC.scale = 1f;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPCID.Sets.TrailingMode[NPC.type] = 0;
		NPCID.Sets.TrailCacheLength[NPC.type] = 40;
	}

	private bool Start = false;
	private Vector2 Cent;
	private Vector2 Acc;
	private float Ome = 0;
	private float kx = 1;

	public override void AI()
	{
		if (!Start)
		{
			NPC.velocity = new Vector2(Main.rand.NextFloat(0, 10f), 0).RotatedByRandom(6.28);
			Acc = new Vector2(Main.rand.NextFloat(0, 0.35f), 0).RotatedByRandom(6.28);
			Cent = NPC.Center;
			NPC.position += new Vector2(0, -Main.rand.NextFloat(350f, 500f)).RotatedBy(Main.rand.NextFloat(-1.6f, 1.6f));
			Ome = Main.rand.NextFloat(-0.16f, 0.16f);
			Start = true;
		}
		Vector2 v0 = Cent - NPC.Center;
		if (v0.Length() >= 12)
		{
			Vector2 v = Cent - (NPC.Center + NPC.velocity * 30);
			Vector2 v2 = v / v.Length() * 0.05f * (float)(1 + Math.Log(v.Length() + 1));

			Acc *= 0.95f;
			NPC.velocity += Acc + v2;
			NPC.velocity = NPC.velocity.RotatedBy(Ome);
			Ome *= 0.96f;
			kx = 20 - v0.Length() / 12f;
			if (kx < 1)
				kx = 1;
		}
		else
		{
			NPC.velocity *= 0.8f;
			kx--;
			if (kx <= 1)
			{
				NPC.active = false;
				;
			}
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}

	private Effect ef;

	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		var bars = new List<VertexBase.CustomVertexInfo>();
		ef = ModAsset.Trail.Value;
		Vector2 v = Cent - NPC.Center;
		int width = (int)(4 * kx);
		for (int i = 1; i < NPC.oldPos.Length - 1; ++i)
		{
			if (NPC.oldPos[i] == Vector2.Zero)
				break;
			var normalDir = NPC.oldPos[i - 1] - NPC.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)NPC.oldPos.Length;
			var color = Color.Lerp(Color.White, Color.Red, factor);

			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new VertexBase.CustomVertexInfo(NPC.oldPos[i] + normalDir * width + new Vector2(4, 35), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
			bars.Add(new VertexBase.CustomVertexInfo(NPC.oldPos[i] + normalDir * -width + new Vector2(4, 35), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
		}

		var triangleList = new List<VertexBase.CustomVertexInfo>();

		if (bars.Count > 2)
		{
			triangleList.Add(bars[0]);
			var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(NPC.velocity) * 5, Color.White, new Vector3(0, 0.5f, 1));
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
			Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapBloodTusk").Value;
			Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/Projectiles/Metero").Value;
			Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Myth/Bosses/Acytaea/Projectiles/Metero").Value;
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
}