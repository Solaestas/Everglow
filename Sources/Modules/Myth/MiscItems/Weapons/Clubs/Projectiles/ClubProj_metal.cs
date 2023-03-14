using Everglow.Myth.Common;

namespace Everglow.Myth.MiscItems.Weapons.Clubs.Projectiles;

public abstract class ClubProj_metal : ClubProj
{
	/// <summary>
	/// 反光强度，默认4
	/// </summary>
	internal float ReflectStrength = 4f;
	/// <summary>
	/// 反光的底色(默认武器本身)
	/// </summary>
	internal string ReflectTexturePath = "";

	public override void SetDef()
	{
		Beta = 0.0024f;
		MaxOmega = 0.27f;
	}

	public override void PostPreDraw()
	{
		List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(trailVecs.ToList());//平滑
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
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w * ReflectStrength)));
		}
		bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
		bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center - trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w * ReflectStrength)));
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/MetalClubTrail");
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
		if (ReflectTexturePath != "")
		{
			try
			{
				MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(ReflectTexturePath).Value);
			}
			catch
			{
				MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
			}
		}
		var lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
		lightColor.W = 0.7f * Omega;
		MeleeTrail.Parameters["Light"].SetValue(lightColor);
		MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}
