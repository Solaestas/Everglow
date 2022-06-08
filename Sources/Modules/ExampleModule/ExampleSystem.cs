using Everglow.Sources.Commons.Core.Profiler.Fody;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Everglow.Sources.Modules.ExampleModule
{
    [ProfilerMeasure]
    internal class ExampleSystem : ModSystem
    {
        private RenderTarget2D[] m_waveDataTarget;
        private int m_renderTargetIndex;
        private Asset<Effect> m_waveEffect = null;
        private Asset<Effect> m_waveDisplay = null;
        private Asset<Effect> m_waveDisortionScreen = null;
        private bool init;

        public override void OnModLoad()
        {
            init = false;
            m_waveDataTarget = new RenderTarget2D[2];
            m_waveEffect = ModContent.Request<Effect>("Everglow/Sources/Modules/ExampleModule/Effects/RippleSpread", AssetRequestMode.ImmediateLoad);
            m_waveDisplay = ModContent.Request<Effect>("Everglow/Sources/Modules/ExampleModule/Effects/RippleDisplay", AssetRequestMode.ImmediateLoad);
            m_waveDisortionScreen = ModContent.Request<Effect>("Everglow/Sources/Modules/ExampleModule/Effects/WaterDisortion", AssetRequestMode.ImmediateLoad);
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            m_renderTargetIndex = 0;

            // On.Terraria.GameContent.Shaders.WaterShaderData.Apply += WaterShaderData_Apply;
            IL.Terraria.GameContent.Shaders.WaterShaderData.Apply += WaterShaderData_Apply1;
            ReplaceEffectPass = m_waveDisortionScreen.Value.CurrentTechnique.Passes[0];

            base.OnModLoad();
        }

        public static EffectPass ReplaceEffectPass = null;



        private void WaterShaderData_Apply1(MonoMod.Cil.ILContext il)
        {
            var c = new ILCursor(il);
            // Try to find where 566 is placed onto the stack
            if (!c.TryGotoNext(i => i.MatchCall(typeof(ScreenShaderData).FullName, "Apply")))
                return; // Patch unable to be applied

            //c.Remove();
            c.Index++;

            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, typeof(ExampleSystem).GetField("ReplaceEffectPass"));
            c.EmitDelegate<Action<WaterShaderData, EffectPass>>((shaderData, effect) =>
            {
                var targetPos = shaderData.Shader.Parameters["uTargetPosition"].GetValueVector2();
                var imageOffset = shaderData.Shader.Parameters["uImageOffset"].GetValueVector2();
                var screenPos = Main.screenPosition - targetPos;
                var a = shaderData.Shader.Parameters["uColor"].GetValueVector3();
                var a1 = shaderData.Shader.Parameters["uOpacity"].GetValueSingle();
                var a2 = shaderData.Shader.Parameters["uSecondaryColor"].GetValueVector3();
                var a3 = shaderData.Shader.Parameters["uTime"].GetValueSingle();
                var a4 = shaderData.Shader.Parameters["uScreenResolution"].GetValueVector2();
                var a5 = shaderData.Shader.Parameters["uScreenPosition"].GetValueVector2();
                var a6 = shaderData.Shader.Parameters["uTargetPosition"].GetValueVector2();
                var a7 = shaderData.Shader.Parameters["uImageOffset"].GetValueVector2();
                var a8 = shaderData.Shader.Parameters["uIntensity"].GetValueSingle();
                var a9 = shaderData.Shader.Parameters["uProgress"].GetValueSingle();
                var a10 = shaderData.Shader.Parameters["uDirection"].GetValueVector2();
                var a11 = shaderData.Shader.Parameters["uZoom"].GetValueVector2();
                var a13 = shaderData.Shader.Parameters["uImageSize1"].GetValueVector2();
                var a14 = shaderData.Shader.Parameters["uImageSize2"].GetValueVector2();
                var a15 = shaderData.Shader.Parameters["uImageSize3"].GetValueVector2();


                var shader = m_waveDisortionScreen.Value;
                shader.Parameters["cb0"].SetValue(new Vector4(1 / a13.X, 1 / a13.Y, 0, 0));
                shader.Parameters["cb1"].SetValue(new Vector4(a9 * 0.1f, 0, 0, 0));
                shader.Parameters["cb2"].SetValue(new Vector4(-a9 * 0.1f, 0, 0, 0));
                shader.Parameters["cb3"].SetValue(new Vector4(a11, 0, 0));
                shader.Parameters["cb4"].SetValue(new Vector4(0.04f, 0.444f, 0, 0));
                shader.Parameters["cb5"].SetValue(new Vector4(1f / a15.X, 1f / a15.Y, 0, 0));
                shader.Parameters["cb6"].SetValue(new Vector4(Main.screenWidth, Main.screenHeight, 0, 0));
                shader.Parameters["cb7"].SetValue(new Vector4(screenPos, 0, 0));
                shader.Parameters["cb8"].SetValue(new Vector4(targetPos, 0, 0));
                shader.Parameters["cb9"].SetValue(new Vector4(a8, 0, 0, 0));
                shader.Parameters["cb10"].SetValue(new Vector4(imageOffset, 0, 0));

                effect.Apply();
            });
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
        }

        public override void PostUpdateEverything()
        {
            //if (Main.time % 600 < 1)
            //{
            //    Everglow.ProfilerManager.PrintSummary();
            //}
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            base.PostDrawInterface(spriteBatch);
        }
    }
}
