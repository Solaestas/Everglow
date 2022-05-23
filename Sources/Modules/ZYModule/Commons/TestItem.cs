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
        //if (Main.invasionType != 0)
        //{
        //    InvasionSystem.InvasionEnd();
        //}else
        //{
        //    InvasionSystem.InvasionBegin<TestInvasion>();
        //}
        if (Everglow.HookSystem.DisableDrawBackground)
        {
            Everglow.HookSystem.DisableDrawBackground = false;
        }
        else
        {
            Everglow.HookSystem.DisableDrawBackground = true;
        }

        if (Everglow.HookSystem.DisableDrawSkyAndHell)
        {
            Everglow.HookSystem.DisableDrawSkyAndHell = false;
        }
        else
        {
            Everglow.HookSystem.DisableDrawSkyAndHell = true;
        }
        return true;
    }

}
