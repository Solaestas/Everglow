namespace Everglow.Commons.TileHelper;

/// <summary>
/// 此种物品用于标记,手持时右键绳节点物块可挂绳
/// </summary>
public abstract class CableTileItem : ModItem
{
    /// <summary>
    /// 必填项,物块种类
    /// </summary>
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    /// <summary>
    /// 必填项,物块种类
    /// </summary>
    public virtual int TileType { get; set; }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(TileType);
    }

    public override bool? UseItem(Player player)
    {
        return base.UseItem(player);
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }
}