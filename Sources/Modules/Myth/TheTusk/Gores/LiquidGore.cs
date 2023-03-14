using Everglow.Myth.Common;

namespace Everglow.Myth.TheTusk.Gores
{

	public abstract class LiquidGore : ModGore
	{
		/// <summary>
		/// 溶解动画贴图的路径
		/// </summary>
		public string DissolveAnimationTexture;
		/// <summary>d
		/// 新死状态贴图的路径
		/// </summary>
		public string FreshDeathTexture;
		public override void SetStaticDefaults()
		{
			GoreID.Sets.DisappearSpeed[Type] = 3;
			DissolveAnimationTexture = "Everglow/" + Texture + "G";
			FreshDeathTexture = "Everglow/" + Texture + "S";
			SSD();
		}
		private string CheckHasNameSpace(string path)
		{
			if (!path.Contains("Everglow"))
				return "Everglow/" + path;
			if (path.Contains("Everglow/Everglow/"))
				return path.Replace("Everglow/Everglow/", "Everglow/");
			return path;
		}
		public virtual void SSD()
		{

		}
		public override Color? GetAlpha(Gore gore, Color lightColor)
		{
			return base.GetAlpha(gore, new Color(0, 0, 0, 0));
		}
		public override bool Update(Gore gore)
		{
			return base.Update(gore);
		}
		/// <summary>
		/// 从MythModule算起
		/// </summary>
		/// <returns></returns>
		public virtual string EffectPath()
		{
			return "Effects/BloodDrop";
		}
		public virtual void DrawDissolve(Gore gore)
		{
			DissolveAnimationTexture = CheckHasNameSpace(DissolveAnimationTexture);
			FreshDeathTexture = CheckHasNameSpace(FreshDeathTexture);

			Texture2D tex = ModContent.Request<Texture2D>(FreshDeathTexture).Value;
			Texture2D texG = ModContent.Request<Texture2D>(DissolveAnimationTexture).Value;

			Color cg = Lighting.GetColor((int)(gore.position.X / 16f), (int)(gore.position.Y / 16f));
			Effect ef = MythContent.QuickEffect(EffectPath());
			float alphaValue = (600 - gore.timeLeft) / 600f;
			alphaValue = MathF.Sqrt(alphaValue);
			ef.Parameters["alphaValue"].SetValue(alphaValue);
			ef.Parameters["tex0"].SetValue(texG);
			ef.Parameters["environmentLight"].SetValue(new Vector4(cg.R, cg.G, cg.B, 255 - gore.alpha) / 255f);
			ef.Parameters["rotation"].SetValue(0);
			ef.CurrentTechnique.Passes["Test"].Apply();
			float alp = (255 - gore.alpha) / 255f;
			cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);

			Main.spriteBatch.Draw(tex, gore.position + new Vector2(gore.Width / 2f, gore.Height / 2f) - Main.screenPosition, null, cg, gore.rotation, tex.Size() / 2, gore.scale, SpriteEffects.None, 0);
		}
	}
	public class ShaderLiquidGore
	{
		public static void Load()
		{
			On_Main.DrawGore += DrawShaderLiquid;
		}
		private static void DrawShaderLiquid(On_Main.orig_DrawGore orig, Main self)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			foreach (Gore gore in Main.gore)
			{
				if (gore.ModGore is LiquidGore dGore && gore.active)
					dGore.DrawDissolve(gore);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			orig.Invoke(self);
		}
	}
}
