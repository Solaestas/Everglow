using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using SubworldLibrary;
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
                //if (SubWorldModule.SubworldSystem.IsActive<MothWorld>())
                //{
                //    SubWorldModule.SubworldSystem.Exit();
                //}
                //else
                //{
                //    if (!SubWorldModule.SubworldSystem.Enter<MothWorld>())
                //    {
                //        Main.NewText("Fail!");
                //    }
                //}
                if (SubworldSystem.Enter<MothWorld>())
                {
                    SubworldSystem.Exit();
                }
                else
                {
                    if (!SubworldSystem.Enter<MothWorld>())
                    {
                        Main.NewText("Fail!");
                    }
                }
            }
            return base.UseItem(player);
        }
    }
}