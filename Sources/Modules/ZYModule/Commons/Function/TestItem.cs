using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
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
    public class TestCircle : DCircle
    {
        public TestCircle(Circle circle, float miu) : base(circle, miu)
        {
        }
    }
    public class TestPlat : DPlatform
    {
        public TestPlat(Vector2 pos, Vector2 vel, float width, Rotation rot) : base(pos, vel, width, rot)
        {
        }
    }
    public override bool CanUseItem(Player player)
    {
        //var block = new TestBlock(Main.MouseWorld, Vector2.One * 256);
        //block.Velocity = Vector2.UnitX * -1;
        //TileSystem.AddTile(block);
        var plat = new TestPlat(Main.MouseWorld, Vector2.Zero, 200, new Rotation(-MathHelper.PiOver2));
        TileSystem.AddTile(plat);
        //var circle = new TestCircle(new Circle(Main.MouseWorld, 256), 1);
        //TileSystem.AddTile(circle);
        return true;
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }

}
