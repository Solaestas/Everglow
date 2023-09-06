//using MythMod.UI.YinYangLife;
using SubworldLibrary;
using Terraria.Localization;

namespace Everglow.Ocean.Items
{
    public class TerrariaContinent : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terraria Continent (Not working)");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "泰拉瑞亚大陆 (无效)");
            //Tooltip.SetDefault("Return you to Terraria from OceanContinent");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "送你回归泰拉瑞亚大陆");
        }
        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 120;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item105;
            Item.consumable = false;
        }
        public override bool? UseItem(Player player)
        {
			if (player.itemAnimation == player.itemAnimationMax)
			{
				if (SubworldSystem.IsActive<WorldGeneration.OceanWorld.OceanWorld>())
					SubworldSystem.Exit();
				
			}
			return SubworldSystem.IsActive<WorldGeneration.OceanWorld.OceanWorld>(); // TEST THIS AND CHANGE IF NEEDED
			//Common.Systems.SubWorld.TranTerr = true;
			//return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24.0f, 0.0f);
        }
    }
}
