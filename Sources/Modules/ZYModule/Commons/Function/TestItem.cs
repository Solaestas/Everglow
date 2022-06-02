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
    //public class TestPlat : RotatedPlat
    //{
    //    public TestPlat(Vector2 position)
    //    {
    //        rotation = MathHelper.PiOver2;
    //        this.position = position;
    //        width = 300;
    //    }
    //}
    public class TestBlock : DBlock
    {
        public TestBlock(Vector2 position, Vector2 size) : base(position, size)
        {
        }
    }
    public override bool CanUseItem(Player player)
    {
        var block = new TestBlock(Main.MouseWorld, Vector2.One * 256);
        block.Velocity = Vector2.UnitX * -1;
        //var plat = new TestPlat(Main.MouseWorld);
        TileSystem.AddTile(block);
        return true;
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }

}
