using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.ExampleModule.VFX;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class HardmodeItem : ModItem
{
    //protected override bool CloneNewInstances => true;
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 40;
        Item.useAnimation = 40;
    }

    public override bool? UseItem(Player player)
    {
        if (!Main.hardMode) { Main.NewText("Activated Hardmode"); Main.hardMode = true; }
        else { Main.NewText("Deactivated Hardmode"); Main.hardMode = false; }
        //VFXManager.Add(new WhiteDust() { position = Main.MouseWorld });

        return true;
    }
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        if (!Main.hardMode) { tooltips.Add(new TooltipLine(Everglow.Instance, "IsNotHardmode", "Not in Hardmode")); }
        else { tooltips.Add(new TooltipLine(Everglow.Instance, "IsHardmode", "In Hardmode")); }
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }
}