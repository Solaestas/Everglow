using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.TileModule;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class TestItem : ModItem
{
    public override string Texture => "Terraria/Images/UI/Wires_0";
    public override void SetDefaults()
    {
        Item.useAnimation = 10;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
    }
    public class TestBlock : Block
    {
        public TestBlock(Vector2 position, Vector2 size) : base(position, size)
        {
            velocity = new Vector2(0, 1);
        }
    }
    public override bool CanUseItem(Player player)
    {
        var block = new TestBlock(Main.MouseWorld, Vector2.One * 32);
        TileSystem.AddTile(block);
        return true;
    }

}
