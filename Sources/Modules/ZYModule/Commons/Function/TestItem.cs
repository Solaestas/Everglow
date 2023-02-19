﻿using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.ExampleModule.VFX;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class TestItem : ModItem
{
    protected override bool CloneNewInstances => true;
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetDefaults()
    {
        Item.useAnimation = 10;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = false;
    }
    public override bool CanUseItem(Player player)
    {

        VFXManager.Add(new WhiteDust() { position = Main.MouseWorld });

        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }
}