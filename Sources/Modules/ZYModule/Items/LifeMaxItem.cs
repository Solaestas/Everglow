using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.ExampleModule.VFX;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class LifeMaxItem : ModItem
{
    //protected override bool CloneNewInstances => true;
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetDefaults()
    {
        Item.accessory = true;
    }
    public override void UpdateInventory(Player player)
    {
        if (player.statLife < player.statLifeMax || player.statLife < player.statLifeMax2)
        {
            player.statLife = player.statLifeMax;
            player.statLife = player.statLifeMax2;
            player.dead = false;
        }
        base.UpdateInventory(player);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.statLife < player.statLifeMax || player.statLife < player.statLifeMax2)
        {
            player.statLife = player.statLifeMax;
            player.statLife = player.statLifeMax2;
            player.dead = false;
        }
        base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanUseItem(Player player)
    {

        //VFXManager.Add(new WhiteDust() { position = Main.MouseWorld });

        return false;
    }
    public override void AddRecipes()
    {
        //CreateRecipe().Register();
    }
}