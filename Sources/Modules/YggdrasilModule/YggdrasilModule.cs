using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.YggdrasilModule
{
    internal class YggdrasilModule : IModule
    {
        string IModule.Name => "Yggdrasil";
        private RenderTarget2D screen = null, OcclusionRender = null, EffectTarget = null, TotalEffeftsRender = null;
        private Effect ScreenOcclusion;
        void IModule.Load()
        {
            if (!Main.dedServ)
            {
                On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
                ScreenOcclusion = ModContent.Request<Effect>("Everglow/Sources/Modules/YggdrasilModule/Effects/Occlusion", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
        }

        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            // 直接从RT池子里取
            var renderTargets = Everglow.RenderTargetPool.GetRenderTarget2DArray(4);
            screen = renderTargets.Resource[0];
            OcclusionRender = renderTargets.Resource[1];
            EffectTarget = renderTargets.Resource[2];
            TotalEffeftsRender = renderTargets.Resource[3];

            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
            GetOrig(graphicsDevice);

            graphicsDevice.SetRenderTarget(OcclusionRender);
            graphicsDevice.Clear(Color.Transparent);
            bool flag = DrawOcclusion(VFXManager.spriteBatch);

            graphicsDevice.SetRenderTarget(EffectTarget);
            graphicsDevice.Clear(Color.Transparent);
            DrawEffect(VFXManager.spriteBatch);

            if (flag)
            {
                //保存原图
                graphicsDevice.SetRenderTarget(screen);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();

                graphicsDevice.SetRenderTarget(TotalEffeftsRender);
                graphicsDevice.Clear(Color.Transparent);
                graphicsDevice.Textures[1] = OcclusionRender;
                graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                ScreenOcclusion.CurrentTechnique.Passes[0].Apply();

                Main.spriteBatch.Draw(EffectTarget, Vector2.Zero, Color.White);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,SamplerState.AnisotropicWrap,DepthStencilState.None,RasterizerState.CullNone,null,Main.GameViewMatrix.TransformationMatrix);
                Main.spriteBatch.Draw(TotalEffeftsRender, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }
            screen = null;
            renderTargets.Release();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private bool DrawOcclusion(VFXBatch spriteBatch)//遮盖层
        {
            spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
            Effect effect = YggdrasilContent.QuickEffect("Effects/Null");
            effect.CurrentTechnique.Passes[0].Apply();
            bool flag = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.ModProjectile is IOcclusionProjectile ModProj)
                    {
                        flag = true;
                        ModProj.DrawOcclusion(spriteBatch);
                    }
                }
            }
            spriteBatch.End();
            return flag;
        }
        private bool DrawEffect(VFXBatch spriteBatch)//特效层
        {
            spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
            Effect MeleeTrail = YggdrasilContent.QuickEffect("Effects/FlameTrail");
            MeleeTrail.Parameters["uTime"].SetValue((float)(Main.timeForVisualEffects) * 0.007f);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/Projectiles/FlameLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/Projectiles/DeathSickle_Color", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MeleeTrail.CurrentTechnique.Passes["Trail"].Apply();
            bool flag = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.ModProjectile is IOcclusionProjectile ModProj)
                    {
                        flag = true;
                        ModProj.DrawEffect(spriteBatch);
                    }
                }
            }
            spriteBatch.End();
            return flag;
        }
        private void GetOrig(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(screen);
            graphicsDevice.Clear(Color.Transparent);
            VFXManager.spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
            VFXManager.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            VFXManager.spriteBatch.End();
        }

        void IModule.Unload()
        {
        }
    }
}
