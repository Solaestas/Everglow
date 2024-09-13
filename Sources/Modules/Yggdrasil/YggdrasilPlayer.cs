using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil;
internal class YggdrasilPlayer : ModPlayer
{
	/// <summary>
	/// 6*30
	/// </summary>
	public int ConsumedCongealedBloodExtractive { get; private set; }
	/// <summary>
	/// 3*40
	/// </summary>
	public int ConsumedJadeGlazeFruit { get; private set; }
	/// <summary>
	/// 60*2
	/// </summary>
	public int ConsumedStarPollen { get; private set; }
	public override bool CloneNewInstances => true;
	public override void Load()
	{
		MonoModHooks.Add(typeof(PlayerLoader).GetMethod(nameof(PlayerLoader.ResetMaxStatsToVanilla), BindingFlags.Public | BindingFlags.Static), Hook_ResetMaxStatsToVanilla);
	}
	static void Hook_ResetMaxStatsToVanilla(Action<Player> orig, Player player)
	{
		int lifeFix1 = 0, lifeFix2 = 0, manaFix1 = 0, manaFix2 = 0;
		if (player.TryGetModPlayer(out YggdrasilPlayer modPlayer))
		{
			lifeFix1 = modPlayer.ConsumedCongealedBloodExtractive;
			lifeFix2 = modPlayer.ConsumedJadeGlazeFruit;
			manaFix1 = modPlayer.ConsumedStarPollen;
		}
		if (SubworldSystem.IsActive<YggdrasilWorld>())
		{
			player.statLifeMax = 100 + player.ConsumedLifeCrystals * 4 + player.ConsumedLifeFruit * 1 + lifeFix1 * 30 + lifeFix2 * 40;
			player.statManaMax = 20 + player.ConsumedManaCrystals * 4 + manaFix1 * 2;
		}
		else
		{
			player.statLifeMax = 100 + player.ConsumedLifeCrystals * 20 + player.ConsumedLifeFruit * 5 + lifeFix1 * 6 + lifeFix2 * 8;
			player.statManaMax = 20 + player.ConsumedManaCrystals * 20 + (int)(manaFix1 * 0.4);
		}
	}
	public bool UseCongealedBloodExtractive()
	{
		if (ConsumedCongealedBloodExtractive < 6)
		{
			ConsumedCongealedBloodExtractive++;
			return true;
		}
		return false;
	}
	public bool UseJadeGlazeFruit()
	{
		if (ConsumedJadeGlazeFruit < 3)
		{
			ConsumedJadeGlazeFruit++;
			return true;
		}
		return false;
	}
	public bool UseStarPollen()
	{
		if (ConsumedStarPollen < 60)
		{
			ConsumedStarPollen++;
			return true;
		}
		return false;
	}
	public override void SaveData(TagCompound tag)
	{
		tag[nameof(ConsumedCongealedBloodExtractive)] = ConsumedCongealedBloodExtractive;
		tag[nameof(ConsumedJadeGlazeFruit)] = ConsumedJadeGlazeFruit;
		tag[nameof(ConsumedStarPollen)] = ConsumedStarPollen;
	}
	public override void LoadData(TagCompound tag)
	{
		if(tag.TryGet(nameof(ConsumedCongealedBloodExtractive),out int life1))
		{
			ConsumedCongealedBloodExtractive = life1;
		}
		if(tag.TryGet(nameof(ConsumedJadeGlazeFruit),out int life2))
		{
			ConsumedJadeGlazeFruit = life2;
		}
		if(tag.TryGet(nameof(ConsumedStarPollen),out int mana1))
		{
			ConsumedStarPollen = mana1;
		}
	}
	public override bool CanUseItem(Item item)
	{
		switch (item.type)
		{
			case ItemID.LifeCrystal:
				return Player.consumedLifeCrystals < 30;
			case ItemID.LifeFruit:
				return Player.consumedLifeFruit < 20;
			case ItemID.ManaCrystal:
				return Player.consumedManaCrystals < 20;
		}
		return base.CanUseItem(item);
	}
}
