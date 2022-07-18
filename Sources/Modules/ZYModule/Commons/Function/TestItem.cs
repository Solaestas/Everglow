using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Test;
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
        Item.autoReuse = true;
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
        VFXManager.Instance.Add(new WhiteDust() { position = Main.MouseWorld });
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().Register();
    }

}
