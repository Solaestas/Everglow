using Everglow.Myth.Common.FogEffect.Sky;
using Everglow.Myth.TheFirefly.Backgrounds;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;

namespace Everglow.Myth
{
	public class MythModule : IModule
	{
		public string Name { get; } = "神话";

		private Asset<Effect> m_waveDisortionScreen = null;

		private FogPass m_fogPass = null;

		public void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				// 水波扰动Shader
				m_waveDisortionScreen = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/WaterDisortion", AssetRequestMode.ImmediateLoad);
				ReplaceEffectPass = m_waveDisortionScreen.Value.CurrentTechnique.Passes[0];

				On_WaterShaderData.Update += WaterShaderData_Update;
				IL_WaterShaderData.Apply += WaterShaderData_Apply;
				On_WaterShaderData.StepLiquids += WaterShaderData_StepLiquids;

				Terraria.Graphics.Effects.On_FilterManager.EndCapture += FilterManager_EndCapture;


				m_fogPass = new FogPass();
			}
		}

		private void FilterManager_EndCapture(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
		{
			m_fogPass.Update();
			m_fogPass.Apply(screenTarget1, screenTarget2);
			orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
		}

		private void WaterShaderData_StepLiquids(On_WaterShaderData.orig_StepLiquids orig, WaterShaderData self)
		{
			orig(self);
		}

		private void WaterShaderData_Update(On_WaterShaderData.orig_Update orig, WaterShaderData self, GameTime gameTime)
		{
			// 关掉_useViscosityFilter来防止出现明显视觉bug
			orig(self, gameTime);
			if (MothBackground.BiomeActive())
				self._useViscosityFilter = false;
		}

		public static EffectPass ReplaceEffectPass = null;

		private void WaterShaderData_Apply(ILContext il)
		{
			var c = new ILCursor(il);
			// Try to find where 566 is placed onto the stack
			if (!c.TryGotoNext(i => i.MatchCall(typeof(ScreenShaderData).FullName, "Apply")))
				return; // Patch unable to be applied

			//c.Remove();
			c.Index++;

			c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
			c.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, typeof(MythModule).GetField("ReplaceEffectPass"));
			c.EmitDelegate<Action<WaterShaderData, EffectPass>>((shaderData, effect) =>
			{
				if (!MothBackground.BiomeActive())
					return;
				var targetPos = shaderData.Shader.Parameters["uTargetPosition"].GetValueVector2();
				var imageOffset = shaderData.Shader.Parameters["uImageOffset"].GetValueVector2();
				var screenPos = Main.screenPosition - targetPos;
				var screenResolution = shaderData.Shader.Parameters["uScreenResolution"].GetValueVector2();
				var intensity = shaderData.Shader.Parameters["uIntensity"].GetValueSingle();
				var progress = shaderData.Shader.Parameters["uProgress"].GetValueSingle();
				var zoom = shaderData.Shader.Parameters["uZoom"].GetValueVector2();
				var noiseSize = shaderData.Shader.Parameters["uImageSize1"].GetValueVector2();
				var waterTargetSize = shaderData.Shader.Parameters["uImageSize3"].GetValueVector2();

				var shader = m_waveDisortionScreen.Value;
				shader.Parameters["cb0"].SetValue(new Vector4(1 / noiseSize.X, 1 / noiseSize.Y, 0, 0));
				shader.Parameters["cb1"].SetValue(new Vector4(progress * 0.05f, 0, 0, 0));
				shader.Parameters["cb2"].SetValue(new Vector4(-progress * 0.05f, 0, 0, 0));
				shader.Parameters["cb3"].SetValue(new Vector4(1f / zoom.X, 1f / zoom.Y, 0, 0));
				shader.Parameters["cb4"].SetValue(new Vector4(32 / screenResolution.X, 320 / screenResolution.Y, 0, 0));
				shader.Parameters["cb5"].SetValue(new Vector4(1f / waterTargetSize.X, 1f / waterTargetSize.Y, 0, 0));
				shader.Parameters["cb6"].SetValue(new Vector4(screenResolution, 0, 0));
				shader.Parameters["cb7"].SetValue(new Vector4(screenPos, 0, 0));
				shader.Parameters["cb8"].SetValue(new Vector4(targetPos, 0, 0));
				shader.Parameters["cb9"].SetValue(new Vector4(intensity, 0, 0, 0));
				shader.Parameters["cb10"].SetValue(new Vector4(imageOffset, 0, 0));
				shader.Parameters["uThreashhold"].SetValue(0.02f);
				shader.Parameters["uPower"].SetValue(1.5f);
				shader.Parameters["uColor"].SetValue(new Vector3(0, 0.5f, 1.0f));

				effect.Apply();
			});
		}
		public void Unload()
		{
			ReplaceEffectPass = null;
		}
	}
}