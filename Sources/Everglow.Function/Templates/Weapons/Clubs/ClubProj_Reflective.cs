using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Templates.Weapons.Clubs;

public abstract class ClubProj_Reflective : ClubProj
{
	/// <summary>
	/// Reflection Strength. Default to <c>4f</c>.
	/// </summary>
	public float ReflectionStrength { get; protected set; } = 4f;

	/// <summary>
	/// Reflection texture.
	/// </summary>
	public string ReflectionTexture { get; protected set; } = string.Empty;

	public override void SetCustomDefaults()
	{
		Beta = 0.0024f;
		MaxOmega = 0.27f;
	}

	protected override float SpecialTrailAlpha(Vector2 trailVector, float factor) => base.SpecialTrailAlpha(trailVector, factor) * ReflectionStrength;

	public override void PostPreDraw()
	{
		var bars = CreateTrailVertices(useSpecialAplha: true);
		if (bars == null)
		{
			return;
		}

		var lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
		lightColor.W = 0.7f * Omega;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		Texture2D projTexture = ReflectionTexture != string.Empty
			? ModContent.Request<Texture2D>(ReflectionTexture).Value
			: (Texture2D)ModContent.Request<Texture2D>(Texture);

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);

		Effect clubTrailEffect = ModAsset.MetalClubTrail.Value;
		clubTrailEffect.Parameters["uTransform"].SetValue(model * projection);
		clubTrailEffect.Parameters["tex1"].SetValue(projTexture);
		clubTrailEffect.Parameters["Light"].SetValue(lightColor);
		clubTrailEffect.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}