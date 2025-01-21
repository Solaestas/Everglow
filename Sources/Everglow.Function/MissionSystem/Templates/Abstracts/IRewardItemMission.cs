using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

public interface IRewardItemMission : IMissionReward
{
	public List<Item> RewardItems { get; }

	public static string SourceContext => typeof(IRewardItemMission).FullName;

	public void GiveReward()
	{
		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(SourceContext), item, item.stack);
		}
	}

	public void Load(TagCompound tag)
	{
		MissionBase.LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	public string GetRewardString() => string.Join(' ', RewardItems.ConvertAll(i => ItemDrawer.Create(i.type, i.stack, new Color(196, 241, 255))));
}