using Everglow.Sources.Modules.DeltaMachine.Globals.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.DeltaMachine.Contents.Items.Tools
{
    /// <summary>
    /// 戴尔塔机器模块: 物品.节能装置.
    /// </summary>
    public class EnergySavingDevice : ModItem , IDeltaItem
    {
        public override void SetStaticDefaults( )
        {
            DisplayName.SetDefault( "节能装置" );
            Tooltip.SetDefault( "" +
                "允许你将其装载在 [永恒雪山]\n" +
                "公司所出产的武器上以减少能耗." );
            base.SetStaticDefaults( );
        }
        public override void SetDefaults( )
        {
            Item.width = 24;
            Item.height = 28;
            Item.rare = ItemRarityID.LightPurple;
            Item.maxStack = 1;
            Item.GetGlobalItem<DeltaMachine_GlobalItem_DeltaQuality>( ).Quality = DeltaQuality.Square;
            base.SetDefaults( );
        }
    }
}