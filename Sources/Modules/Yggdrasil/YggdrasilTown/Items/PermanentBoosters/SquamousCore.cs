using Everglow.Yggdrasil.Common;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.PermanentBoosters;

/// <summary>
/// Permanently increase life max by 30.
/// </summary>
public class SquamousCore : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.PermanentBoosters;

    public static readonly int SquamousCoreMax = 1;
    public static readonly int SquamousCoreLife = 30;

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(30, 1);

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 4;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.LifeCrystal);
        Item.width = 30;
        Item.height = 26;
        Item.rare = ItemRarityID.Blue;
        Item.value = 15000;
        Item.maxStack = Item.CommonMaxStack;
        Item.useStyle = ItemUseStyleID.EatFood;
    }

    public override bool? UseItem(Player player) => player.GetModPlayer<YggdrasilPlayer>().UseSquamousCore();
}