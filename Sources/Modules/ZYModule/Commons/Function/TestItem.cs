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
        public override void OnCollision(AABB aabb, Direction dir)
        {
            this.velocity = Vector2.UnitX * 15;
        }
    }
    public class TestPlat : DPlatform
    {
        public TestPlat(Vector2 position, Vector2 velocity, float width, Rotation rotation, float miu) : base(position, velocity, width, rotation, miu)
        {
        }

        public TestPlat(Vector2 position, Vector2 velocity, float width, Rotation rotation, Rotation angularVelocity, float miu) : base(position, velocity, width, rotation, angularVelocity, miu)
        {
        }

        int timer = 30;
        public override void AI()
        {
            //timer++;
            //if ((timer % 240) < 120)
            //{
            //    angularVelocity = 0;
            //}
            //else
            //{
            //    angularVelocity = -0;
            //}
        }
    }
    public override bool CanUseItem(Player player)
    {
        //var block = new TestBlock(Main.MouseWorld, new Vector2(1000, 16));
        //block.Velocity = Vector2.UnitX * -10;
        //TileSystem.AddTile(block);
        var plat = new TestPlat(Main.MouseWorld, -Vector2.UnitY, 200, -MathHelper.PiOver2, 0, 1);
        //plat.miu = 1;
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
