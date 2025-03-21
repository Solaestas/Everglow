using Terraria.GameContent.Events;

namespace Everglow.Commons.Utilities;

public static class PlayerUtils
{
	public static List<string> GetDevPlayerNames() =>
	[
		/*Mr. Skirt | DXTST*/
		"Mr. Skirt", "Mr Skirt", "DXTsT", "CXUtk",
		/*Dirty Octopus | 脏渔歌*/
		"Dirty Octopus", "脏渔歌",
		/*u_silver | 新萌の绿草*/
		"u_silver", "usilver", "新萌の绿草", "Uin",
		/*ju_zhang | 太阳照常升起*/
		"ju_zhang", "太阳照常升起", "ju zhang",
		/*ye_you | 夜谷紫幽*/
		"ye_you", "ye you", "dsfgasdfg", "Solaestas", "夜谷紫幽",
		/*Slime1024 | 凝胶 | TheGelatum*/
		"Slime1024", "Slime_1024", "凝胶", "TheGelatum",
		/*Omnielement | 万象元素*/
		"Omnielement", "Element Of All", "万象元素",
		/*yiyang223*/
		"yiyang223", "yiyang",
		/*Lacewing*/
		"Lacewing", "lyc-lacewing",
		/*JSDA Ling*/
		"JSDA Ling", "JDSA Ling",
		/*Jack Lyh*/
		"Jack Lyh",
		/*Cyrillya | crapsky223*/
		"Cyril", "Cyrillya", "crapsky223",
		/*FelixYang777*/
		"FelixYang777", "Felix Yang",
		/*Colin Weiss*/
		"Colin Weiss", "ColinWeiss",
		/*SilverMoon | 942293328*/
		"SilverMoon", "942293328", "Silverymoon",
		/*Setnour6*/
		"Setnour6",
		/*Nomis*/
		"Nomis", "NomisPrime",
		/*DomesticFoxcy*/
		"DomesticFoxcy", "DomesticFoxy",
		/*Plantare*/
		"Plantare", "世纪小花",
		/*Drawn_Lemon | Cloudea*/
		"Drawn_Lemon", "DrawnLemon", "Drawn",
		/* Others */
		"青枫", "陌林", "京墨", "鸭子ceo",
	];

	/// <summary>
	/// Sets the player's arm position to align with the mouse position, with an optional offset.
	/// </summary>
	/// <param name="player">The player instance to modify.</param>
	/// <param name="offset">An optional float value to adjust the rotation by. Defaults to 0f.</param>
	/// <returns>The modified player instance.</returns>
	public static Player SetArmToFitMousePosition(this Player player, float offset = 0f)
	{
		var rotation = (Main.MouseWorld - player.MountedCenter).ToRotation() * player.gravDir + offset * player.direction - MathHelper.PiOver2;

		// Set the player's composite arm to be in front, fully stretched, and rotated to the calculated rotation.
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);

		return player;
	}

	/// <summary>
	/// Calculate <see cref="Player.slotsMinions"/> manually, should only be used in <see cref="ModItem.Shoot(Player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float)"/>
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public static float GetSlotsMinions(this Player player) => Main.projectile.Where(x => x.owner == player.whoAmI && x.active && x.minion).Sum(x => x.minionSlots);

	/// <summary>
	/// Check if the player is in pillar zone
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public static bool InCelestialPillarZone(this Player player) => player.ZoneTowerStardust || player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula;

	/// <summary>
	/// Check if the player is in event
	/// </summary>
	/// <param name="player"></param>
	/// <param name="checkBloodMoon"></param>
	/// <returns></returns>
	public static bool AnyEvent(this Player player, bool checkBloodMoon = false)
	{
		if (Main.invasionType > InvasionID.None && Main.invasionProgressNearInvasion)
		{
			return true;
		}
		if (player.InCelestialPillarZone())
		{
			return true;
		}
		if (DD2Event.Ongoing && player.ZoneOldOneArmy)
		{
			return true;
		}
		if ((player.ZoneOverworldHeight || player.ZoneSkyHeight) && (Main.eclipse || Main.pumpkinMoon || Main.snowMoon))
		{
			return true;
		}
		if ((player.ZoneOverworldHeight || player.ZoneSkyHeight) && Main.bloodMoon && checkBloodMoon)
		{
			return true;
		}
		return false;
	}
}