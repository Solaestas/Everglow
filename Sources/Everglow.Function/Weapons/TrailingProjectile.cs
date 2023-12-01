using System.Linq;
using Everglow.Commons;
using Everglow.Commons.MEAC;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Terraria.DataStructures;

namespace Everglow.Commons.Weapons;

public abstract class TrailingProjectile : ModProjectile, IWarpProjectile
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
		SetDef();
	}
	public virtual void SetDef()
	{

	}
	public int TimeTokill = -1;
	public Color TrailColor;
	public bool SelfLuminous;
	public float TrailWidth;
	public Texture2D TrailTexture;
	public override void OnSpawn(IEntitySource source)
	{
	}
	public override void AI()
	{
		Vector2 v1 = Projectile.velocity;
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
		Projectile.velocity += Vector2.Normalize(Main.MouseWorld - Projectile.Center - Projectile.velocity).RotatedBy(0) * 12.5f;
		Projectile.velocity *= 0.95f;
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
		Projectile.damage = 0;
		if (TimeTokill < 0)
		{
			Explosion();
		}
		TimeTokill = ProjectileID.Sets.TrailCacheLength[Projectile.type];
	}
	public virtual void Explosion()
	{

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
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			var normalDir = SmoothTrail[i - 1] - SmoothTrail[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			float factor = i / (float)SmoothTrail.Count;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.05f;
			factor += timeValue;
			float widthMax = TrailWidth;
			float widthMax2 = TrailWidth;
			Color drawC = TrailColor;
			//Only correct when the track swril conterclockwise. 
			if (i < SmoothTrail.Count - 1)
			{
				float cR = GetCurvatureRadius(SmoothTrail[i - 1], SmoothTrail[i], SmoothTrail[i + 1]);
				if (i < SmoothTrail.Count - 2 && i > 2)
				{
					float cRUp = GetCurvatureRadius(SmoothTrail[i - 1], SmoothTrail[i], SmoothTrail[i + 2]);
					float cRDown = GetCurvatureRadius(SmoothTrail[i - 2], SmoothTrail[i], SmoothTrail[i + 1]);
					if (cR < 0)
					{
						cR = MathF.Max(MathF.Max(cRUp, cRDown), cR);
					}
				}
				if (cR > -widthMax && cR < 0)
				{
					widthMax = -cR;
				}
				if (cR < widthMax2 && cR > 0)
				{
					widthMax2 = cR;
				}
			}
			bars.Add(new Vertex2D(SmoothTrail[i] + normalDir * widthMax + halfSize, drawC, new Vector3(factor * 7 + timeValue, 1, width)));
			bars.Add(new Vertex2D(SmoothTrail[i] + halfSize, drawC, new Vector3(factor * 7 + timeValue, 0.5f, width)));


			bars2.Add(new Vertex2D(SmoothTrail[i] + halfSize, drawC, new Vector3(factor * 7 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(SmoothTrail[i] - normalDir * widthMax2 + halfSize, drawC, new Vector3(factor * 7 + timeValue, 0, width)));
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = ModAsset.Trailing.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes["Test"].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);

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
		//Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		//float velocityValue = Projectile.velocity.Length() / 30f;
		//float colorValueG = velocityValue;
		//int trueLength = 0;
		//for (int i = 1; i < Projectile.oldPos.Length; ++i)
		//{
		//	if (Projectile.oldPos[i] == Vector2.Zero)
		//		break;
		//	trueLength++;
		//}
		//var bars = new List<Vertex2D>();
		//for (int i = 1; i < Projectile.oldPos.Length; ++i)
		//{
		//	if (Projectile.oldPos[i] == Vector2.Zero)
		//		break;
		//	float MulColor = 1f;
		//	var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
		//	normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
		//	if (i == 1)
		//		MulColor = 0f;
		//	if (i >= 2)
		//	{
		//		var normalDirII = Projectile.oldPos[i - 2] - Projectile.oldPos[i - 1];
		//		normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
		//		if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
		//			MulColor = 0f;
		//	}
		//	if (i < Projectile.oldPos.Length - 1)
		//	{
		//		var normalDirII = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
		//		normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
		//		if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
		//			MulColor = 0f;
		//	}

		//	float colorValue0 = (float)Math.Atan2(normalDir.Y, normalDir.X);
		//	colorValue0 += 3.14f + 1.57f;
		//	if (colorValue0 > 6.28f)
		//		colorValue0 -= 6.28f;
		//	var c0 = new Color(colorValue0, 0.4f * colorValueG * MulColor, 0, 0);

		//	var factor = i / (float)trueLength;
		//	float x0 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
		//	bars.Add(new Vertex2D(Projectile.oldPos[i] - normalDir * TrailWidth * (1 - factor) + halfSize - Main.screenPosition, c0, new Vector3(x0, 1, 0)));
		//	bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * TrailWidth * (1 - factor) + halfSize - Main.screenPosition, c0, new Vector3(x0, 0, 0)));
		//}
		//Texture2D t = Commons.ModAsset.Trail_4.Value;
		//spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		//if (bars.Count > 3)
		//	spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
	}
}