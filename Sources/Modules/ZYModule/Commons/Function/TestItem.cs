using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Test;
using Everglow.Sources.Modules.ZYModule.Visuals.ScreenShaders;

using Terraria.Audio;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class TestItem : ModItem
{
    private class TestShader : ScreenShader
    {
        public TestShader() : base(EffectType.Test, ScreenParameter.uTime | ScreenParameter.uOpacity | ScreenParameter.uResolution | ScreenParameter.uNoise)
        {
        }
        public override void Update()
        {
            if(time >= 120)
            {
                active = false;
            }
        }
    }
    protected override bool CloneNewInstances => true;
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetStaticDefaults()
    {
        soundStyles = typeof(SoundID).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.GetValue(null) is SoundStyle)
            .Select(f => (SoundStyle)f.GetValue(null))
            .ToList();
        enumerator = soundStyles.GetEnumerator();
        var shader = new TestShader();
        ScreenShaderManager.Add("Test", shader);
        shader.effectParameters["uColorBar"].SetValue(TextureType.WhiteGreenBar.GetValue(false));
    }
    public override void SetDefaults()
    {
        Item.useAnimation = 10;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
    }
    [CloneByReference]
    private List<SoundStyle> soundStyles = new List<SoundStyle>();
    [CloneByReference]
    private IEnumerator<SoundStyle> enumerator;
    public override bool CanUseItem(Player player)
    {
        //if (enumerator.MoveNext())
        //{
        //    SoundEngine.PlaySound(enumerator.Current, player.Center);
        //    Main.NewText(enumerator.Current.SoundPath);
        //}
        //else
        //{
        //    enumerator.Reset();
        //    Main.NewText("Over!");
        //}
        //if (!ScreenShaderManager.Instance["Test"].active)
        //{
        //    ScreenShaderManager.Activate("Test");
        //}
        //else
        //{
        //    ScreenShaderManager.Deactivate("Test");
        //}
        VFXManager.Instance.Add(new WhiteDust() { position = Main.MouseWorld});

        return true;
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }

}
