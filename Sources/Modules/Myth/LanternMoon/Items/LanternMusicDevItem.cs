using Everglow.Myth.LanternMoon.LanternCommon;
using Terraria.Audio;

namespace Everglow.Myth.LanternMoon.Items;

public class LanternMusicDevItem : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.HoldUp;
	}

	public override bool CanUseItem(Player player)
	{
		LanternMoonMusicSystem musicSystem = ModContent.GetInstance<LanternMoonMusicSystem>();
		musicSystem.PlayMusic(ModAsset.LanternMoonMusic_15_Melody_Head_Mod);
		//musicSystem.StartMusic(ModAsset.LanternMoonMusic_Pre15_Percussion_Loop_Mod, 300);
		//musicSystem.FadeMusic(ModAsset.LanternMoonMusic_15_Melody_Loop_Mod, 1 / 120f);
		return true;
	}
}