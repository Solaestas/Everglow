using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.FeatureFlags;
using Everglow.Sources.Modules.ZYModule.Commons.Core;

using Everglow.Sources.Modules.ZYModule.Commons.Function;
namespace Everglow.Sources.Modules.ZYModule.Visuals.ScreenShaders;

internal class ScreenShaderManager : IModule
{
    private Dictionary<string, ScreenShader> screenShaders = new Dictionary<string, ScreenShader>();
    private ScreenShader defaultShader;
    public int rtIndex;
    public RenderTarget2D Current => rtIndex == 0 ? Main.screenTarget : Main.screenTargetSwap;
    public RenderTarget2D Next => rtIndex == 1 ? Main.screenTarget : Main.screenTargetSwap;
    public ScreenShader this[string name]
    {
        get
        {
            return screenShaders[name];
        }
        set
        {
            screenShaders[name] = value;
        }
    }
    public static ScreenShaderManager Instance => Everglow.ModuleManager.GetModule<ScreenShaderManager>();
    public static void Add(string name, ScreenShader shader)
    {
        Instance.screenShaders.Add(name, shader);
    }
    public static void Activate(string name)
    {
        Instance.screenShaders[name].active = true;
    }
    public static void Deactivate(string name)
    {
        Instance.screenShaders[name].active = false;
    }
    public string Name => "ScreenShaderManager";
    public void Load()
    {
        defaultShader = new ScreenShader(EffectType.Default, ScreenParameter.None);
        Everglow.HookSystem.AddMethod(Update, Sources.Commons.Core.CallOpportunity.PostUpdateEverything);
        Everglow.HookSystem.AddMethod(Render, Sources.Commons.Core.CallOpportunity.PostDrawFilter);
    }

    public void Unload()
    {
        
    }
    public void Switch()
    {
        rtIndex = (rtIndex + 1) % 2;
    }
    public void Update()
    {
        foreach (var shader in screenShaders.Values)
        {
            if (shader.active)
            {
                shader.opacity = MathUtils.Approach(shader.opacity, 1, 0.05f);
            }
            else
            {
                shader.opacity = MathUtils.Approach(shader.opacity, 0, 0.05f);
            }
            if (shader.opacity != 0)
            {
                ++shader.time;
            }
            else
            {
                shader.time = 0;
            }
        }
    }
    public void Render()
    {
        var enableShaders = screenShaders.Values.Where(s => s.opacity != 0);
        if (!enableShaders.Any())
        {
            return;
        }
        var sb = Main.spriteBatch;
        var gd = Main.instance.GraphicsDevice;
        rtIndex = 0;

        sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
        gd.SetRenderTarget(Main.screenTarget);
        gd.Clear(Color.Transparent);
        sb.Draw((Texture2D)gd.GetRenderTargets()[0].RenderTarget, Vector2.Zero, Color.White);
        gd.Clear(Color.Transparent);

        foreach (var shader in enableShaders)
        {
            gd.SetRenderTarget(Next);
            gd.Clear(Color.Transparent);
            shader.AutoSetParameters();
            shader.SetParameters();
            shader.effectPass.Apply();
            sb.Draw(Current, Vector2.Zero, Color.White);
            Switch();
        }

        gd.SetRenderTarget(null);
        gd.Clear(Color.Transparent);
        defaultShader.effectPass.Apply();
        sb.Draw(Current, Vector2.Zero, Color.White);

        sb.End();
    }
}
