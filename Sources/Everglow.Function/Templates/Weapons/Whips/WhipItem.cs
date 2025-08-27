using static Terraria.GameContent.Prefixes.PrefixLegacy;

namespace Everglow.Commons.Templates.Weapons.Whips;

public abstract class WhipItem : ModItem
{
    public override string LocalizationCategory => Utilities.LocalizationUtils.Categories.SummonWeapons;

    public float WhipLength;

    public override void SetDefaults()
    {
        Item.knockBack = 2f;
        SetDef();
        DefaultToWhip(Item.damage, Item.knockBack, Item.shootSpeed);
    }

    /// <summary>
    /// These tags should be filled: shoot, shootSpeed, useAnimation, damage, rare, value, width, height
    /// </summary>
    public virtual void SetDef()
    {
    }

    public override bool? UseItem(Player player)
    {
        if (player.autoReuseGlove)
        {
            Item.autoReuse = true;
            return true;
        }
        Item.autoReuse = false;
        return true;
    }

    public void DefaultToWhip(int dmg, float kb, float shootspeed)
    {
        Player player = Main.LocalPlayer;
        Item.autoReuse = false;
        if (player.autoReuseGlove)
        {
            Item.autoReuse = true;
        }

        Item.autoReuse = false;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Item152;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.noUseGraphic = true;
        Item.damage = dmg;
        Item.knockBack = kb;
        Item.shootSpeed = shootspeed;
        ItemSets.SwordsHammersAxesPicks[Item.type] = true;
    }
}