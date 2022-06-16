using Terraria.Audio;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class TestItem : ModItem
{
    protected override bool CloneNewInstances => true;
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetStaticDefaults()
    {
        soundStyles = typeof(SoundID).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.GetValue(null) is SoundStyle)
            .Select(f => (SoundStyle)f.GetValue(null))
            .ToList();
        enumerator = soundStyles.GetEnumerator();
    }
    public override void SetDefaults()
    {
        Item.useAnimation = 10;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
    }
    [CloneByReference]
    private List<SoundStyle> soundStyles = new List<SoundStyle>();
    [CloneByReference]
    private IEnumerator<SoundStyle> enumerator;
    public override bool CanUseItem(Player player)
    {
        SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, player.Center);
        return true;
        if (enumerator.MoveNext())
        {
            SoundEngine.PlaySound(enumerator.Current, player.Center);
            Main.NewText(enumerator.Current.SoundPath);
        }
        else
        {
            enumerator.Reset();
            Main.NewText("Over!");
        }
        return true;
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }

}
