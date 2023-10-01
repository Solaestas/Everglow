using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Shaders;

namespace Everglow.Commons.MEAC;

public abstract class MeleeProj : ModProjectile, IWarpProjectile, IBloomProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 15;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 30;
		Projectile.extraUpdates = 1;
		Projectile.scale = 1f;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
		Projectile.DamageType = DamageClass.Melee;
		SetDef();
		trailVecs = new Queue<Vector2>(trailLength + 1);
	}
	public virtual void SetDef()
	{

	}
	public int attackType = 0;
	public int maxAttackType = 3;
	public Vector2 mainVec;
	public Queue<Vector2> trailVecs;
	public int trailLength = 40;
	public int timer = 0;

	/// <summary>
	/// 是否采用自己的DrawWarp
	/// </summary>
	public bool selfWarp = false;
	public bool isAttacking = false;
	public bool useTrail = true;
	/// <summary>
	/// 断开左键是否自动结束攻击
	/// </summary>
	public bool AutoEnd = true;
	/// <summary>
	/// 绑定AutoEnd参数,用于判定上次攻击结束前是否按下鼠标左键从而实现连击
	/// </summary>
	public bool HasContinueLeftClick = false;
	/// <summary>
	/// 允许长按左键?(一般情况用来做重击)
	/// </summary>
	public bool CanLongLeftClick = false;
	/// <summary>
	/// 绑定CanLongLeftClick,用于判定重击
	/// </summary>
	public int Clicktimer = 0;
	/// <summary>
	/// 绑定CanLongLeftClick,用于判定重击所需要的蓄力时长
	/// </summary>
	public int ClickMaxtimer = 90;
	/// <summary>
	/// 能否穿墙攻击敌人
	/// </summary>
	public bool CanIgnoreTile = false;
	/// <summary>
	/// 是否为长柄武器
	/// </summary>
	public bool longHandle = false;

	public float drawScaleFactor = 1f;

	public float disFromPlayer = 6;
	public string shadertype = "Trail0";
	public bool isRightClick = false;
	public Player Player => Main.player[Projectile.owner];
	public Vector2 MainVec_WithoutGravDir
	{
		get
		{
			Vector2 vec = mainVec;
			if (Player.gravDir == -1)
				vec.Y *= -1;
			return vec;
		}
	}
	public Vector2 MouseWorld_WithoutGravDir
	{
		get
		{
			Vector2 vec = Main.MouseWorld;
			if (Player.gravDir == -1)
				vec = WrapY(vec);
			return vec;
		}
	}
	public Vector2 ProjCenter_WithoutGravDir
	{
		get
		{
			Vector2 vec = Projectile.Center;
			if (Player.gravDir == -1)
				vec = WrapY(vec);
			return vec;
		}
	}
	private Vector2 WrapY(Vector2 vec)
	{
		vec.Y -= Main.screenPosition.Y;
		float d = vec.Y - Main.screenHeight / 2;
		vec.Y -= 2 * d;
		vec.Y += Main.screenPosition.Y;
		return vec;
	}
	public override void SendExtraAI(BinaryWriter writer)
	{
		writer.WriteVector2(mainVec);
		writer.Write(disFromPlayer);
		//writer.Write(Projectile.spriteDirection);
	}
	public override void ReceiveExtraAI(BinaryReader reader)
	{
		mainVec = reader.ReadVector2();
		disFromPlayer = reader.ReadSingle();
		//Projectile.spriteDirection = reader.ReadInt32();
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		modifiers.HitDirectionOverride = target.Center.X > Main.player[Projectile.owner].Center.X ? 1 : -1;
	}
	public float GetMeleeSpeed(Player player, float max = 100)
	{
		return Math.Min((player.GetAttackSpeed(DamageClass.Melee) - 1) * 100, max);
	}
	public override void AI()
	{
		Player.heldProj = Projectile.whoAmI;
		Player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = true;
		Projectile.Center = Player.Center + Utils.SafeNormalize(mainVec, Vector2.One) * disFromPlayer;
		isAttacking = false;

		Projectile.timeLeft++;
		Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, mainVec.ToRotation() - 1.57f);
		Attack();
		timer++;
		if (!isAttacking)
		{
			if (!isRightClick)
			{
				bool IsEnd = AutoEnd ? !Player.controlUseItem || Player.dead : Player.dead;
				if (IsEnd)
					End();
			}
			else
			{
				bool IsEnd = AutoEnd ? !Player.controlUseTile || Player.dead : Player.dead;
				if (IsEnd)
					End();
			}
		}
		if (!HasContinueLeftClick && timer > 15)//大于1/60s即判定为下一击继续
		{
			if (Main.mouseLeft)
				HasContinueLeftClick = true;
		}
		if (isAttacking)
			Player.direction = Projectile.spriteDirection;
		if (useTrail)
		{
			trailVecs.Enqueue(mainVec);
			if (trailVecs.Count > trailLength)
				trailVecs.Dequeue();
		}
		else//清空！
		{
			trailVecs.Clear();
		}
		if (CanLongLeftClick)
		{
			if (Main.mouseLeft)
			{
				Clicktimer++;
			}
			else
			{
				Clicktimer = 0;
			}
		}
		ProduceWaterRipples(new Vector2(mainVec.Length(), 30));
	}
	public virtual void Attack()
	{

	}
	public override bool ShouldUpdatePosition()
	{
		return false;
	}

	private void ProduceWaterRipples(Vector2 beamDims)
	{
		var shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
		float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
		Vector2 ripplePos = Projectile.Center + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(mainVec.ToRotation());
		Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
		shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, mainVec.ToRotation());
	}
	public override void CutTiles()
	{
		DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
		var cut = new Terraria.Utils.TileActionAttempt(DelegateMethods.CutTiles);
		Vector2 beamStartPos = Projectile.Center;
		Vector2 beamEndPos = beamStartPos + mainVec;
		Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
	}
	public void ScreenShake(int time)
	{
		//震屏没写
		//Main.player[projectile.owner].GetModPlayer<EffectPlayer>().screenShake = time;
	}
	public void NextAttackType()
	{
		if (!isAttacking && !AutoEnd)
		{
			if (Clicktimer >= ClickMaxtimer)
			{
				LeftLongThump();
				End();
			}
			if (!isRightClick)
			{
				if (!HasContinueLeftClick || Player.dead)
				{
					Player player = Main.player[Projectile.owner];
					End();
					player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
				}
			}
			else
			{
				if (!Player.controlUseTile || Player.dead)
				{
					Player player = Main.player[Projectile.owner];
					End();
					player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
				}
			}
		}
		HasContinueLeftClick = false;
		timer = 0;
		attackType++;
		if (attackType > maxAttackType)
			attackType = 0;

	}
	public virtual void LeftLongThump()
	{

	}
	public virtual void End()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
		player.fullRotation = 0;
		player.legRotation = 0;
		Tplayer.HeadRotation = 0;
		Tplayer.HideLeg = false;
		player.legPosition = Vector2.Zero;
		Projectile.Kill();
		player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float point = 0;
		if (isAttacking && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ProjCenter_WithoutGravDir + MainVec_WithoutGravDir * Projectile.scale * (longHandle ? 0.2f : 0.1f), ProjCenter_WithoutGravDir + MainVec_WithoutGravDir * Projectile.scale, Projectile.height, ref point))
		{
			if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, targetHitbox.TopLeft(), targetHitbox.Width, targetHitbox.Height) || CanIgnoreTile)
				return true;
		}

		return false;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail(lightColor);
		DrawSelf(Main.spriteBatch, lightColor);
		return false;
	}

	public virtual void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = new Vector4(), Vector2 coord0 = new Vector2(), float DrawScale = 1, Texture2D glowTexture = null)
	{
		Player player = Main.player[Projectile.owner];
		if(diagonal == new Vector4())
		{
			diagonal = new Vector4(0, 1, 1, 0);
		}
		Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		DrawVertexByTwoLine(tex, lightColor, diagonal.XY(), diagonal.ZW(), ProjCenter_WithoutGravDir, ProjCenter_WithoutGravDir + mainVec);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public void DrawVertexByTwoLine(Texture2D texture, Color drawColor, Vector2 textureCoordStart, Vector2 textureCoordEnd, Vector2 positionStart, Vector2 positionEnd)
	{
		Vector2 coordVector = textureCoordEnd - textureCoordStart;
		coordVector.X *= texture.Width;
		coordVector.Y *= texture.Height;
		float theta = MathF.Atan2(coordVector.Y, coordVector.X);
		Vector2 mainVectorI = mainVec.RotatedBy(theta) * MathF.Cos(theta);
		Vector2 mainVectorJ = mainVec.RotatedBy(theta - MathHelper.PiOver2) * MathF.Sin(theta);
		List<Vertex2D> vertex2Ds = new List<Vertex2D>
		{
			new Vertex2D(positionStart, drawColor, new Vector3(textureCoordStart, 0)),
			new Vertex2D(positionStart + mainVectorI, drawColor, new Vector3(textureCoordEnd.X, positionStart.Y, 0)),

			new Vertex2D(positionEnd + mainVectorJ, drawColor, new Vector3(positionStart.X, textureCoordEnd.Y, 0)),
			new Vertex2D(positionEnd, drawColor, new Vector3(textureCoordEnd, 0))
		};
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);
	}
	public virtual float TrailAlpha(float factor)
	{
		float w;
		w = MathHelper.Lerp(0f, 1, factor);
		return w;
	}
	public virtual string TrailShapeTex()
	{
		return "Everglow/MEAC/Images/Melee";
	}
	public virtual string TrailColorTex()
	{
		return "Everglow/MEAC/Images/TestColor";
	}
	public virtual BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}
	public virtual void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = TrailAlpha(factor);
			if (!longHandle)
			{
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.15f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
			}
			else
			{
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.3f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
			}
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

		Effect MeleeTrail = ModContent.Request<Effect>("Everglow/MEAC/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		//Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}

	private Vector2 r = Vector2.One;
	public void DrawWarp(VFXBatch spriteBatch)
	{
		if (selfWarp)
			return;
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1f;
			float d = trail[i].ToRotation() + 3.14f + 1.57f;
			if (d > 6.28f)
				d -= 6.28f;
			float dir = d / MathHelper.TwoPi;

			float dir1 = dir;
			if (i > 0)
			{
				float d1 = trail[i - 1].ToRotation() + 3.14f + 1.57f;
				if (d1 > 6.28f)
					d1 -= 6.28f;
				dir1 = d1 / MathHelper.TwoPi;
			}

			if (dir - dir1 > 0.5)
			{
				var midValue = (1 - dir) / (1 - dir + dir1);
				var midPoint = midValue * trail[i] + (1 - midValue) * trail[i - 1];
				var oldFactor = (i - 1) / (length - 1f);
				var midFactor = midValue * factor + (1 - midValue) * oldFactor;
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
			}
			if (dir1 - dir > 0.5)
			{
				var midValue = (1 - dir1) / (1 - dir1 + dir);
				var midPoint = midValue * trail[i - 1] + (1 - midValue) * trail[i];
				var oldFactor = (i - 1) / (length - 1f);
				var midFactor = midValue * oldFactor + (1 - midValue) * factor;
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
			}
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, w, 0, 1), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, w, 0, 1), new Vector3(factor, 0, w)));
		}

		spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/MEAC/Images/Warp").Value, bars, PrimitiveType.TriangleStrip);
	}
	/// <summary>
	/// 根据鼠标位置锁定玩家方向
	/// </summary>
	/// <param name="player"></param>
	public void LockPlayerDir(Player player)
	{
		Projectile.spriteDirection = Main.MouseWorld.X > player.Center.X ? 1 : -1;
		player.direction = Projectile.spriteDirection;
	}
	/// <summary>
	/// 圆的透视投影
	/// </summary>
	/// <param name="radius"></param>
	/// <param name="rot0"></param>
	/// <param name="rot1"></param>
	/// <param name="rot2"></param>
	/// <param name="viewZ"></param>
	/// <returns></returns>
	public static Vector2 Vector2Elipse(float radius, float rot0, float rot1, float rot2 = 0, float viewZ = 1000)
	{
		Vector3 v = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationZ(rot0)) * radius;
		v = Vector3.Transform(v, Matrix.CreateRotationX(-rot1));
		if (rot2 != 0)
			v = Vector3.Transform(v, Matrix.CreateRotationZ(-rot2));
		float k = -viewZ / (v.Z - viewZ);
		return k * new Vector2(v.X, v.Y);
	}
	public float GetAngToMouse()
	{
		Vector2 vec = MouseWorld_WithoutGravDir - Main.player[Projectile.owner].Center;
		if (vec.X < 0)
			vec = -vec;
		return -vec.ToRotation();
	}
	public void AttSound(SoundStyle sound)
	{
		SoundEngine.PlaySound(sound, Projectile.Center);
	}
	public void DrawBloom()
	{
		DrawTrail(Color.White);
	}
}