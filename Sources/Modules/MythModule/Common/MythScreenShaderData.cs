using Terraria.Graphics.Shaders;

namespace Everglow.Sources.Modules.MythModule.Common
{
    public class MythScreenShaderData : ScreenShaderData
    {
        public MythScreenShaderData(string passName) : base(passName)
        {
        }
        public MythScreenShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
        {
        }
        public override void Apply()
        {
            base.Apply();
        }
    }
}