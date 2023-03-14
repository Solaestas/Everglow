using Everglow.Myth.TheFirefly.Items.Accessories;
using Everglow.Myth.TheFirefly.Projectiles;
using Terraria.Localization;

namespace Everglow.Myth.TheFirefly.Items.Weapons;

public class MothYoyo : ModItem
{
	FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
	public override void SetStaticDefaults()
	{
		// These are all related to gamepad controls and don't seem to affect anything else
		ItemID.Sets.Yoyo[Item.type] = true;
		ItemID.Sets.GamepadExtraRange[Item.type] = 15;
		ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		
	}

	public override void SetDefaults()
	{
		
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.width = 24;
		Item.height = 24;
		Item.useAnimation = 25;
		Item.useTime = 25;
		Item.shootSpeed = 16f;
		Item.knockBack = 2.5f;
		Item.damage = 25;
		Item.rare = ItemRarityID.Green;

		Item.DamageType = DamageClass.Melee;
		Item.channel = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.UseSound = SoundID.Item1;
		Item.value = 2300;
		Item.shoot = ModContent.ProjectileType<MothYoyoProjectile>();
	}

	private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Terrible, PrefixID.Dull, PrefixID.Shameful, PrefixID.Annoying, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy };

	public override bool AllowPrefix(int pre)
	{
		if (Array.IndexOf(unwantedPrefixes, pre) > -1)
			return false;
		return true;
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		if (MothEye.LocalOwner != null && MothEye.LocalOwner.TryGetModPlayer(out MothEyePlayer mothEyePlayer))
		{
			if (mothEyePlayer.MothEyeEquipped && fireflyBiome.IsBiomeActive(Main.LocalPlayer) && Main.hardMode)
			{
				tooltips.AddRange(new TooltipLine[]
				{
					new(ModIns.Mod, "MothEyeBonusText", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeBonusText")),
					new(ModIns.Mod, "MothEyeYoyoBonus", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MEyeBonusTextMothYoyo")),
				});
			}
		}
	}
	public override void AddRecipes()
	{
	}
}