using Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk_New;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs
{
	public class Tester : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tester");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "测试");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 24;
			Item.maxStack = 999;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = 1;
			Item.useTime = Item.useAnimation = 30;
		}
		public override bool CanUseItem(Player player)
		{
			NPC npc = NPC.NewNPCDirect(null, (int)Main.MouseWorld.X - 800, (int)Main.MouseWorld.Y, ModContent.NPCType<TuskWall>());
			npc.direction = 1;
			npc = NPC.NewNPCDirect(null, (int)Main.MouseWorld.X + 800, (int)Main.MouseWorld.Y, ModContent.NPCType<TuskWall>());
			npc.direction = -1;
			return base.CanUseItem(player);
		}
	}
}
