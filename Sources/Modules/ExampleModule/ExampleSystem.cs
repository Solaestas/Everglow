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

            Main.OnRenderTargetsInitialized += Main_OnRenderTargetsInitialized;
            Main.OnPreDraw += Main_OnPreDraw;
            // On.Terraria.GameContent.Shaders.WaterShaderData.Apply += WaterShaderData_Apply;
            IL.Terraria.GameContent.Shaders.WaterShaderData.Apply += WaterShaderData_Apply1;
            ReplaceEffectPass = m_waveDisortionScreen.Value.CurrentTechnique.Passes[0];

            base.OnModLoad();
        }

        public static EffectPass ReplaceEffectPass = null;
        //private void WaterShaderData_Apply(On.Terraria.GameContent.Shaders.WaterShaderData.orig_Apply orig, Terraria.GameContent.Shaders.WaterShaderData self)
        //{
        //    if (m_prevEffect == null)
        //    {
        //        var effectPass = typeof(WaterShaderData)
        //            .GetField("_effectPass", BindingFlags.NonPublic | BindingFlags.Instance);
        //        m_prevEffect = (EffectPass)effectPass.GetValue(self);
        //        effectPass.SetValue(self, m_waveDisortionScreen.Value);
        //    }
        //    orig(self);
        //    IL.Terraria.GameContent.Shaders.WaterShaderData.Apply += WaterShaderData_Apply1;
        //}



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
                effect.Apply();
            });
        }

        public override void PostSetupContent()
        {

        }


        private void Main_OnRenderTargetsInitialized(int width, int height)
        {

        }

        private void Main_OnPreDraw(GameTime gameTime)
        {
            var device = Main.graphics.GraphicsDevice;
            if (!init)
            {
                for (int i = 0; i < 2; i++)
                {
                    m_waveDataTarget[i] = new RenderTarget2D(device, 512, 512, false, SurfaceFormat.Vector4, DepthFormat.None);
                    device.SetRenderTarget(m_waveDataTarget[i]);
                    device.Clear(new Color(0.5f, 0.5f, 0.5f, 1.0f));
                }
                init = true;
                device.SetRenderTarget(null);
                return;
            }
            int nextIndex = (m_renderTargetIndex + 1) % 2;
            var spriteBatch = Main.spriteBatch;
            var effect = m_waveEffect.Value;
            device.SetRenderTarget(m_waveDataTarget[m_renderTargetIndex]);
            device.Clear(new Color(0.5f, 0.5f, 0.5f, 1.0f));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, 
                SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            effect.Parameters["uDeltaXY"].SetValue(new Vector2(1.0f / 512f, 1.0f / 512f));
            effect.Parameters["uDamping"].SetValue(0.99f);
            effect.CurrentTechnique.Passes[0].Apply();
            device.Textures[0] = m_waveDataTarget[nextIndex];
            spriteBatch.Draw(m_waveDataTarget[nextIndex],
                Vector2.Zero, Color.White);
            spriteBatch.End();

            if (Main.time % 300 < 1)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                    new Rectangle(100, 100, 10, 10), new Color(0.1f, 0f, 0f));
                spriteBatch.End();
            }


            device.SetRenderTarget(null);

            //var shader = Filters.Scene["HeatDistortion"];
            //if (true)
            //{

            //}

            var shader = m_waveDisortionScreen.Value;
            Vector2 value = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
            shader.Parameters["cb0"].SetValue(new Vector4(1 / 256f, 1 / 256f, 0, 0));
            shader.Parameters["cb1"].SetValue(new Vector4(Main.GlobalTimeWrappedHourly * 0.01f, 0, 0, 0));
            shader.Parameters["cb2"].SetValue(new Vector4(-Main.GlobalTimeWrappedHourly * 0.01f, 0, 0, 0));
            shader.Parameters["cb3"].SetValue(new Vector4(1.0f, 1.0f, 0, 0));
            shader.Parameters["cb4"].SetValue(new Vector4(0.04f, 0.04444f, 0, 0));
            shader.Parameters["cb5"].SetValue(new Vector4(0.0008f, 0.0009f, 0, 0));
            shader.Parameters["cb6"].SetValue(new Vector4(Main.screenWidth, Main.screenHeight, 0, 0));
            shader.Parameters["cb7"].SetValue(new Vector4(Main.screenPosition, 0, 0));
            shader.Parameters["cb8"].SetValue(new Vector4(192, 192, 0, 0));
            shader.Parameters["cb9"].SetValue(new Vector4(1.0f, 0, 0, 0));
            shader.Parameters["cb10"].SetValue(new Vector4(0, -0.0014f, 0, 0));

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
            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            ////m_waveDisplay.Value.CurrentTechnique.Passes[0].Apply();
            //spriteBatch.Draw(m_waveDataTarget[m_renderTargetIndex],
            //    new Vector2(Main.ScreenSize.X, Main.ScreenSize.Y) * 0.5f - new Vector2(256, 256), Color.White);
            //m_renderTargetIndex = (m_renderTargetIndex + 1) % 2;
            //spriteBatch.End();
            //spriteBatch.Begin();
            base.PostDrawInterface(spriteBatch);
        }
    }
}
