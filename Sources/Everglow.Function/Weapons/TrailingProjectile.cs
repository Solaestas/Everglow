using System.Linq;
using Everglow.Commons;
using Everglow.Commons.MEAC;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Terraria.DataStructures;

namespace Everglow.Commons.Weapons;

public abstract class TrailingProjectile : ModProjectile, IWarpProjectile_warpStyle2
{
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 360000;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 90;
		TrailColor = new Color(1, 1, 1, 0f);
		TrailWidth = 30f;
		SelfLuminous = false;
		TrailTexture = ModAsset.Trail.Value;
		TrailTextureBlack = ModAsset.Trail_black.Value;
		TrailShader = ModAsset.Trailing.Value;
		SetDef();
	}
	public virtual void SetDef()
	{

	}
	/// <summary>
	/// Negative before projectile nominal killed(when projectile hit tile, hit enemies...), positive after.Substrate 1 by every frame. 
	/// </summary>
	public int TimeTokill = -1;
	/// <summary>
	/// Update every frame(1/60s), add by 1.
	/// </summary>
	public int Timer = 0;
	/// <summary>
	/// The color of trailing track.
	/// </summary>
	public Color TrailColor;
	/// <summary>
	/// If true, the trailing track will not influenced by environment light.
	/// </summary>
	public bool SelfLuminous;
	/// <summary>
	/// The width of trailing track.
	/// </summary>
	public float TrailWidth;
	/// <summary>
	/// The value of warp strength.
	/// </summary>
	public float WarpStrength = 1f;
	/// <summary>
	/// The first texture of a trailing track.
	/// </summary>
	public Texture2D TrailTexture;
	/// <summary>
	/// The second texture of a trailing track. Reserved for custom trail.
	/// </summary>
	public Texture2D TrailTextureBlack;
	/// <summary>
	/// The shader use to trail, parameter "uTransform" should be contained.
	/// </summary>
	public Effect TrailShader;
	public override void OnSpawn(IEntitySource source)
	{
	}
	public override void AI()
	{
		Timer++;
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		TimeTokill--;
		if (TimeTokill < 0)
		{

		}
		else
		{
			Projectile.velocity *= 0f;
			return;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		KillMainStructure();
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		KillMainStructure();
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		KillMainStructure();
		Projectile.tileCollide = false;
		return false;
	}
	public virtual void KillMainStructure()
	{
		Projectile.velocity = Projectile.oldVelocity;
		Projectile.friendly = false;
		if (TimeTokill < 0)
		{
			Explosion();
		}
		TimeTokill = ProjectileID.Sets.TrailCacheLength[Projectile.type];
	}
	public virtual void Explosion()
	{

	}
	/// <summary>
	/// Reserved for possible use.
	/// </summary>
	public virtual void DrawTrailDark()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = SmoothTrail.Count - 1; i > 0; --i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.06f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = Color.White;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTextureBlack;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
	public virtual void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.0005f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor((drawPos / 16f).ToPoint());
				drawC.R = (byte)(lightC.R * drawC.R / 255f);
				drawC.G = (byte)(lightC.G * drawC.G / 255f);
				drawC.B = (byte)(lightC.B * drawC.B / 255f);
			}
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


	}
	public float GetCurvatureRadius(Vector2 p0, Vector2 p1, Vector2 p2)
	{
		Vector2 v1 = p1 - p0;
		Vector2 v2 = p2 - p1;
		float cross = v1.X * v2.Y - v1.Y * v2.X;
		float length1 = v1.Length();
		float length2 = v2.Length();
		float radius = length1 * length1 * length2 / (2 * cross + 0.0001f);
		return radius;
	}
	public float GetCurvatureRadius(Vector2 v1, Vector2 v2)
	{
		float cross = v1.X * v2.Y - v1.Y * v2.X;
		float length1 = v1.Length();
		float length2 = v2.Length();
		float radius = length1 * length1 * length2 / (2 * cross + 0.0001f);
		return radius;
	}
	/// <summary>
	/// To make the trail more smooth, we convey this 0~1 parameter to shader. 
	/// </summary>
	/// <returns></returns>
	public virtual float TrailWidthFunction(float factor)
	{
		factor *= 6;
		factor -= 1;
		if (factor < 0)
		{
			return MathF.Pow(MathF.Cos(factor * MathHelper.PiOver2), 0.5f);
		}
		return MathF.Pow(MathF.Cos(factor / 5f * MathHelper.PiOver2), 3);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		if (TimeTokill <= 0)
		{
			DrawSelf();
		}
		return false;
	}
	public virtual void DrawSelf()
	{
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, TrailColor, Projectile.rotation, texMain.Size() / 2f, 1f, SpriteEffects.None, 0);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		float width = TrailWidth;
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			if (SmoothTrail[i] == Vector2.Zero)
				break;
			var normalDir = SmoothTrail[i - 1] - SmoothTrail[i];
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float widthZ = TrailWidthFunction(factor);
			var c0 = new Color(1 - (normalDir.X + 25f) / 50f, 1 - (normalDir.Y + 25f) / 50f, 0.1f * WarpStrength, 1);


			float x0 = factor * 1.3f + (float)(Main.time * 0.03f);
			Vector2 drawPos = SmoothTrail[i] - Main.screenPosition + halfSize;

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * width, c0, new Vector3(x0, 1, widthZ)));
			bars.Add(new Vertex2D(drawPos, c0, new Vector3(x0, 0.5f, widthZ)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * width, c0, new Vector3(x0, 1, widthZ)));
			bars2.Add(new Vertex2D(drawPos, c0, new Vector3(x0, 0.5f, widthZ)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * width, c0, new Vector3(x0, 1, widthZ)));
			bars3.Add(new Vertex2D(drawPos, c0, new Vector3(x0, 0.5f, widthZ)));
		}
		if (bars.Count > 3)
			spriteBatch.Draw(TrailTexture, bars, PrimitiveType.TriangleStrip);
		if (bars2.Count > 3)
			spriteBatch.Draw(TrailTexture, bars2, PrimitiveType.TriangleStrip);
		if (bars3.Count > 3)
			spriteBatch.Draw(TrailTexture, bars3, PrimitiveType.TriangleStrip);
	}
}