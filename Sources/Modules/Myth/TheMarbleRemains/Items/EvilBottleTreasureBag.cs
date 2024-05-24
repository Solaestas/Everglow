namespace Everglow.Myth.TheMarbleRemains.Items
{
    public class EvilBottleTreasureBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Treasure Bag");
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            //DisplayName.AddTranslation(GameCulture.Chinese, "宝藏袋");
			//Tooltip.AddTranslation(GameCulture.Chinese, "右键点击打开");
		}
        //public override int BossBagNPC => mod.NPCType("EvilBottle");
        public override void SetDefaults()
		{
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 9;
            Item.expert = true;
            //this.BossBagNPC = mod.NPCType("EvilBottle");
        }
		public override bool CanRightClick()
		{
			return true;
		}
        public override void RightClick(Player player)
        {
			var entitySource = player.GetSource_OpenItem(Type);
			int type = 0;
            switch (Main.rand.Next(1, 9))
            {
                case 1:
                    type = Mod.Find<ModItem>("DarkStaff").Type;
                    break;
                case 2:
                    type = Mod.Find<ModItem>("EvilBomb").Type;
                    break;
                case 3:
                    type = Mod.Find<ModItem>("EvilRing").Type;
                    break;
                case 4:
                    type = Mod.Find<ModItem>("EvilSlingshot").Type;
                    break;
                case 5:
                    type = Mod.Find<ModItem>("EvilSword").Type;
                    break;
                case 6:
                    type = Mod.Find<ModItem>("ShadowYoyo").Type;
                    break;
                case 7:
                    type = Mod.Find<ModItem>("EvilShadowBlade").Type;
                    break;
                case 8:
                    type = Mod.Find<ModItem>("GeometryEvil").Type;
                    break;
            }
            player.QuickSpawnItem(entitySource, type, 1);
        }
	}
}
