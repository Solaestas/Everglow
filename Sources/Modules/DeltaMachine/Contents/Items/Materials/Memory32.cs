using Everglow.Sources.Modules.DeltaMachine.Globals.Items;

namespace Everglow.Sources.Modules.DeltaMachine.Contents.Items.Materials
{
    /// <summary>
    /// 戴尔塔机器模块: 物品.32G内存条.
    /// </summary>
    public class Memory32 : ModItem , IDeltaItem
    {
        public override void SetStaticDefaults( )
        {
            DisplayName.SetDefault( "内存条 32G" );
            Tooltip.SetDefault( "" +
                "允许你将其装载在 [永恒雪山]\n" +
                "公司所出产的武器上以增加计算能力." );
            base.SetStaticDefaults( );
        }
        public override void SetDefaults( )
        {
            Item.width = 48;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = 4;
            base.SetDefaults( );
        }
    }
}