using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LightSeeker;

[AutoloadEquip(EquipType.Legs)]
public class ShadowlessBoots : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Armor;

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(3);

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 26;
        Item.value = 2500;
        Item.rare = ItemRarityID.Green;
        Item.defense = 2;
    }

    public override void UpdateEquip(Player player)
    {
        player.moveSpeed += 0.1f;
    }

    public override void AddRecipes()
    {
    }
}