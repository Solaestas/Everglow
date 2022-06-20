using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
    public class FireflyCapital : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            player.position = new Vector2(mothLand.FireflyCenterX, mothLand.FireflyCenterY) * 16;
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .Register();
        }
    }
}
