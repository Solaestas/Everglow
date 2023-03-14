namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Melee;

class GhostHit : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 26;
		Projectile.height = 26;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 700;
		//Projectile.extraUpdates = 10;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
	}
	private float K = 10;
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{

	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		int duration = player.itemAnimationMax;
		if (Projectile.timeLeft > duration)
			Projectile.timeLeft = duration;
		Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
		for (int i = 0; i < 12; i++)
		{
			if (Projectile.velocity.Length() > i * 10f)
				Projectile.velocity *= 0.9f;
		}
		Projectile.velocity += new Vector2(Projectile.ai[0], Projectile.ai[1]);
	}
	private Effect ef;
	private Vector2[] vpos = new Vector2[15];
	public override void PostDraw(Color lightColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		var bars = new List<Vertex2D>();
		ef = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
		// 把所有的点都生成出来，按照顺序
		int width = 60;
		if (Projectile.timeLeft < 30)
			width = Projectile.timeLeft * 2;
		Player player = Main.player[Projectile.owner];
		int duration = player.itemAnimationMax;
		/*if (Projectile.timeLeft == duration - 10)
            {
                for (int i = 1; i < Projectile.oldPos.Length; ++i)
                {
                    vpos[i] = Projectile.oldPos[i];
                }
            }*/
		int Imax = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			Imax = i;
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
		}

		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			if (vpos[i] != Vector2.Zero)
			{
				var normalDir = Projectile.velocity;
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
				var factor = i / (float)Projectile.oldPos.Length;
				var color = Color.Lerp(Color.White, Color.Red, factor);
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				Vector2 deltaPos = Projectile.position - vpos[1];
				if (Imax - i < 5)
					width = (int)(width * (Imax - i) / 5f);
				bars.Add(new Vertex2D(vpos[i] + normalDir * width + new Vector2(13) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
				bars.Add(new Vertex2D(vpos[i] + normalDir * -width + new Vector2(13) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
			}
			else
			{
				var normalDir = Projectile.velocity;
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
				var factor = i / (float)Projectile.oldPos.Length;
				var color = Color.Lerp(Color.White, Color.Red, factor);
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				if (Imax - i < 5)
					width = (int)(width * (Imax - i) / 5f);
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(13), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(13), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
			}
		}

		var triangleList = new List<Vertex2D>();

		if (bars.Count > 2)
		{

			// 按照顺序连接三角形
			triangleList.Add(bars[0]);
			Vector2 vo = (bars[0].position - bars[1].position) / (bars[0].position - bars[1].position).Length();
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + vo.RotatedBy(-Math.PI / 2d) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
			// 干掉注释掉就可以只显示三角形栅格
			//RasterizerState rasterizerState = new RasterizerState();
			//rasterizerState.CullMode = CullMode.None;
			//rasterizerState.FillMode = FillMode.WireFrame;
			//Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

			// 把变换和所需信息丢给shader
			ef.Parameters["uTransform"].SetValue(model * projection);
			ef.Parameters["uTime"].SetValue(0);
			Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapGhost").Value;
			Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
			Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine2").Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
			//Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
			//Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
			//Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

			ef.CurrentTechnique.Passes[0].Apply();


			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

			Main.graphics.GraphicsDevice.RasterizerState = originalState;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		for (int y = 0; y < 12; y++)
		{
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 25, 4, 4, 183, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
			Main.dust[num90].noGravity = true;
			Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
		}
		Projectile.Kill();
		return true;
	}
}
