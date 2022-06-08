using Everglow.Sources.Modules.DeltaMachine.Contents.Items;

namespace Everglow.Sources.Modules.DeltaMachine.Globals.Items
{
    /// <summary>
    /// 戴尔塔机器模块: 全局物品设置.物品品级.
    /// </summary>
    public class DeltaMachine_GlobalItem_DeltaQuality : GlobalItem
    {
        /// <summary>
        /// 物品品级.
        /// </summary>
        public DeltaQuality Quality;

        public override void SetDefaults( Item item )
        {
            item.GetGlobalItem( this ).Quality = DeltaQuality.Circular;
            base.SetDefaults( item );
        }

        public override bool InstancePerEntity => true;
        public override GlobalItem Clone( Item from, Item to )
        {
            GlobalItem _gItem = base.Clone( from, to );
            ( _gItem as DeltaMachine_GlobalItem_DeltaQuality ).Quality = Quality;
            return _gItem;
        }
    }
}