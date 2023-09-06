using Everglow.Ocean.UIs;
using SubworldLibrary;
using Terraria.Localization;

namespace Everglow.Ocean.Items
{
    public class OceanContinent : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ocean World (Not working)");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "海洋世界 (无效)");
            //Tooltip.SetDefault("Sends you to Ocean World");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "送你离开这个大陆, 来到真正的海边");
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
				if (!SubworldSystem.IsActive<WorldGeneration.OceanWorld.OceanWorld>())
					SubworldSystem.Enter<WorldGeneration.OceanWorld.OceanWorld>();
				else
				{
					if (!SubworldSystem.Enter<WorldGeneration.OceanWorld.OceanWorld>())
						Main.NewText("Fail!");
				}
			}
			return !SubworldSystem.IsActive<WorldGeneration.OceanWorld.OceanWorld>(); // TEST THIS AND CHANGE IF NEEDED
			//if (Main.ActiveWorldFileData.Path.Contains("OcEaNMyTh"))
			//{
			//    return true;
			//}
			//Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Typeless.Transport>(), 0, 0);
			//return true;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24.0f, 0.0f);
        }
    }
}
