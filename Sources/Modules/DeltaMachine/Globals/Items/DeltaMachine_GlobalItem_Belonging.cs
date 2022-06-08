using Everglow.Sources.Modules.DeltaMachine.Contents.Items;

namespace Everglow.Sources.Modules.DeltaMachine.Globals.Items
{
    /// <summary>
    /// 戴尔塔机器模块: 全局物品设置.物品所属确定.
    /// </summary>
    public class DeltaMachine_GlobalItem_Belonging : GlobalItem
    {
        public bool IsDeltaMachineItem = false;

        public override void SetDefaults( Item item )
        {
            if ( item.ModItem != null && item.ModItem.GetType( ).GetInterfaces( ).Contains( typeof( IDeltaItem ) ) )
            {
                item.GetGlobalItem( this ).IsDeltaMachineItem = true;
            }
            base.SetDefaults( item );
        }
    }
}