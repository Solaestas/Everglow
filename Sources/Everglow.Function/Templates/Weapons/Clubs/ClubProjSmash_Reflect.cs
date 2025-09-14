using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.Templates.Weapons.Clubs;

public abstract class ClubProjSmash_Reflect : ClubProjSmash
{
	public float ReflectStrength { get; protected set; } = 4f;

	public override void DrawSmashTrail(Color color)
	{
		if (SmashTrailVecs.Smooth(out var trail))
		{
			return;
		}

		var length = trail.Count;
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			Vector2 trailSelfPos = trail[i] - Projectile.Center;
			float w = 1 - Math.Abs((trailSelfPos.X * 0.5f + trailSelfPos.Y * 0.5f) / trailSelfPos.Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			Color c0 = i == 0 ? Color.Transparent : Color.White;
			bars.Add(new Vertex2D(Projectile.Center, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(trail[i], c0, new Vector3(factor, 0, w * ReflectStrength)));
		}

		var vColor = color.ToVector4();
		vColor.W *= 0.15f;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);

		Effect MeleeTrail = ModAsset.ClubTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		MeleeTrail.Parameters["tex0"].SetValue(ModAsset.Noise_flame_0.Value);
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.Parameters["Light"].SetValue(vColor);
		MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}