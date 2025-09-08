namespace Everglow.Yggdrasil.YggdrasilTown.Items.Mounts;

public class JellyBubbleItem : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Mounts;

    public override void SetDefaults()
    {
        Item.width = 64;
        Item.height = 64;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.RaiseLamp;

        Item.UseSound = SoundID.Item81;
        Item.noMelee = true;
        Item.mountType = ModContent.MountType<JellyBubble>();

        Item.value = Item.buyPrice(silver: 63, copper: 50);
        Item.rare = ItemRarityID.Blue;
    }
}