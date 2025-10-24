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

	public sealed override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 24;

		Item.DamageType = DamageClass.MeleeNoSpeed;
		Item.knockBack = 2.5f;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useAnimation = 25;
		Item.useTime = 25;
		Item.UseSound = SoundID.Item1;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		Item.shootSpeed = 16f;

		SetCustomDefaults();
	}

	/// <summary>
	/// This is where you set all your item's properties, such as width, damage, shootSpeed, defense, etc.
	/// <br/>Same to <see cref="ModItem.SetDefaults"/>. Looking for base values in <see cref="YoyoItem.SetDefaults"/>.
	/// </summary>
	/// <remarks>
	/// Necessary defaults:
	/// <br/><see cref="Item.shoot"/>
	/// <br/><see cref="Item.damage"/>
	/// <br/><see cref="Item.rare"/>
	/// <br/><see cref="Item.value"/>
	/// </remarks>
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