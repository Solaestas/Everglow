using Terraria.GameContent;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Gores
{

    public abstract class DissolveGore : ModGore
    {
        /// <summary>
        /// 轻度值,下降速度的减缓程度,0~0.1为佳
        /// </summary>
        public float LightValue = 0;
        /// <summary>
        /// 溶解动画贴图的路径
        /// </summary>
        public string DissolveAnimationTexture;
        /// <summary>d
        /// 新死状态贴图的路径
        /// </summary>
        public string FreshDeathTexture;
        /// <summary>
        /// 烧剩后的骨架的路径
        /// </summary>
        public string BurnedTexture;
        public override void SetStaticDefaults()
        {
            GoreID.Sets.DisappearSpeed[Type] = 3;
            LightValue = 0.06f;
            DissolveAnimationTexture = "Everglow/" + Texture + "G";
            FreshDeathTexture = "Everglow/" + Texture + "S";
            BurnedTexture = "Everglow/" + Texture + "B";
            SSD();
        }
        private string CheckHasNameSpace(string path)
        {
            if (!path.Contains("Everglow"))
            {
                return "Everglow/" + path;
            }
            if(path.Contains("Everglow/Everglow/"))
            {
                return path.Replace("Everglow/Everglow/", "Everglow/");
            }
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
            gore.velocity.Y -= LightValue;
            return base.Update(gore);
        }
        public virtual void DrawDissolve(Gore gore)
        {
            //TODO:I cant understand! WHY!!!
            DissolveAnimationTexture = CheckHasNameSpace(DissolveAnimationTexture);
            FreshDeathTexture = CheckHasNameSpace(FreshDeathTexture);
            BurnedTexture = CheckHasNameSpace(BurnedTexture);
            Main.NewText(BurnedTexture);
            Texture2D tex = ModContent.Request<Texture2D>(FreshDeathTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D texG = ModContent.Request<Texture2D>(DissolveAnimationTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D texB = ModContent.Request<Texture2D>(BurnedTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Color cg = Lighting.GetColor((int)(gore.position.X / 16f), (int)(gore.position.Y / 16f));
            Effect ef = MythContent.QuickEffect("Effects/LanternGore");
            ef.Parameters["alphaValue"].SetValue((600 - gore.timeLeft) / 600f);
            ef.Parameters["tex0"].SetValue(texG);
            ef.Parameters["environmentLight"].SetValue(new Vector4(cg.R * 255f, cg.G * 255f, cg.B * 255f, 255 - gore.alpha) / 255f);
            ef.CurrentTechnique.Passes["Test"].Apply();
            float alp = (255 - gore.alpha) / 255f;
            cg = new Color(cg.R / 255f * alp, cg.G / 255f * alp, cg.B / 255f * alp, alp);

            Main.spriteBatch.Draw(tex, gore.position + new Vector2(gore.Width / 2f, gore.Height / 2f) - Main.screenPosition, null, cg, gore.rotation, tex.Size() / 2, gore.scale, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(texB, gore.position + new Vector2(gore.Width / 2f, gore.Height / 2f) - Main.screenPosition, null, cg, gore.rotation, tex.Size() / 2, gore.scale, SpriteEffects.None, 0);
        }
    }
    public class ShaderLanternGore
    {
        public static void Load()
        {
            On.Terraria.Main.DrawGore += DrawShaderLantern;
        }
        public static void UnLoad()
        {
            //On.Terraria.Main.DrawGore -= DrawShaderLantern;
        }
        private static void DrawShaderLantern(On.Terraria.Main.orig_DrawGore orig, Terraria.Main self)
        {
            foreach(Gore gore in Main.gore)
            {
                if(gore.ModGore is DissolveGore dGore)
                {
                    dGore.DrawDissolve(gore);
                }
            }
            orig.Invoke(self);
        }
    }
}
