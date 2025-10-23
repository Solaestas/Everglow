using Terraria.GameContent.Creative;
using Terraria.GameContent.Prefixes;

namespace Everglow.Commons.Templates.Weapons.Yoyos;

public abstract class YoyoItem : ModItem
{
	public override string LocalizationCategory => Utilities.LocalizationUtils.Categories.MeleeWeapons;

	public override void SetStaticDefaults()
	{
		ItemID.Sets.Yoyo[Item.type] = true; // Used to increase the gamepad range when using Strings.
		ItemID.Sets.GamepadExtraRange[Item.type] = 15; // Increases the gamepad range. Some vanilla values: 4 (Wood), 10 (Valor), 13 (Yelets), 18 (The Eye of Cthulhu), 21 (Terrarian).
		ItemID.Sets.GamepadSmartQuickReach[Item.type] = true; // Unused, but weapons that require aiming on the screen are in this set.
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 24;
		Item.useAnimation = 25;
		Item.useTime = 25;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;
		Item.shootSpeed = 16f;
		Item.knockBack = 2.5f;
		Item.DamageType = DamageClass.MeleeNoSpeed;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item1;
		Item.DamageType = DamageClass.Melee;
		SetCustomDefaults();
	}

	/// <summary>
	/// Default Values:
	/// ItemID.Sets.Yoyo[Item.type] = true;<br></br>
	/// ItemID.Sets.GamepadExtraRange[Item.type] = 15;<br></br>
	/// ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;<br></br>
	/// CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;<br></br>
	/// Item.useStyle = ItemUseStyleID.Shoot;<br></br>
	/// Item.width = 24;<br></br>
	/// Item.height = 24;<br></br>
	/// Item.noUseGraphic = true;<br></br>
	/// Item.UseSound = SoundID.Item1;<br></br>
	/// Item.DamageType = DamageClass.Melee;<br></br>
	/// Item.channel = true;<br></br>
	/// Item.useAnimation = 5;<br></br>
	/// Item.useTime = 5;<br></br>
	/// Item.shootSpeed = 0f;<br></br>
	/// Item.knockBack = 5f;<br></br>
	/// Item.noMelee = true;<br></br>
	/// ItemID.Sets.Yoyo[Item.type] = true;<br></br><br></br>
	/// Necessary fields: value, shoot, damage, rare;<br></br>
	/// Vanilla Reference Price: Wooden Yoyo_100, Rally_5000, Malaise_10000, Hel-Fire_40000, Terrarian_100000
	/// </summary>
	public virtual void SetCustomDefaults()
	{
	}

	/// <summary>
	/// Only allows prefixes that are appropriate for yoyos.
	/// <br/>Most of yoyos: <see cref="PrefixLegacy.Prefixes.PrefixesForBoomeransAndChakrums"/>
	/// <br/>Terrarian: <see cref="PrefixLegacy.Prefixes.PrefixesForBoomeransAndChakrums_TerrarianYoyo"/>
	/// </summary>
	/// <param name="pre"></param>
	/// <returns></returns>
	public override bool AllowPrefix(int pre) => PrefixLegacy.Prefixes.PrefixesForBoomeransAndChakrums.Any(p => p == pre);
}