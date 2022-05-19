using Everglow.Sources.Modules.ZY.InvasionModule;
namespace Everglow.Sources.Modules.ZY.Common;

internal class TestItem : ModItem
{
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetDefaults()
    {
        Item.useAnimation = 10;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
    }
    public override bool CanUseItem(Player player)
    {
        if (Main.invasionType != 0)
        {
            InvasionSystem.InvasionEnd();
        }else
        {
            InvasionSystem.InvasionBegin<TestInvasion>();
        }

        return true;
    }

}
