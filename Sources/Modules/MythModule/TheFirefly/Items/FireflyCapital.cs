using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Everglow.Sources.Modules.WorldModule;

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
            if (player.itemAnimation == player.itemAnimationMax)
            {
                //TestCode
                if (WorldManager.Activing<MothWorld>())
                {
                    WorldManager.TryBack();
                }
                else
                {
                    WorldManager.TryEnter<MothWorld>();
                }
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .Register();
        }
    }
}