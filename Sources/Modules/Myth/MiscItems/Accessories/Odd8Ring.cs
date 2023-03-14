namespace Everglow.Myth.MiscItems.Accessories;

public class Odd8Ring : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 22;
		Item.value = 1375;
		Item.accessory = true;
		Item.rare = ItemRarityID.Green;
	}
	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		Odd8RingEquiper o8RE = player.GetModPlayer<Odd8RingEquiper>();
		o8RE.Odd8Enable = true;
	}
}
class Odd8RingEquiper : ModPlayer
{
	public bool Odd8Enable = false;
	public override void ResetEffects()
	{
		Odd8Enable = false;
	}
	public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
	{
		if (Odd8Enable)
		{
			if (damage < 8)
				GoldStrike = 1;
			damage = Math.Max(8, damage);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
	{
		if (Odd8Enable)
		{
			if (damage < 8)
				GoldStrike = 1;
			damage = Math.Max(8, damage);
		}
	}
	public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
	{
		if (Odd8Enable)
		{
			if (damage < 8)
				GoldStrike = 1;
			damage = Math.Max(8, damage);
		}
	}
	public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
	{

		if (Odd8Enable)
		{
			if (damage < 8)
				GoldStrike = 1;
			damage = Math.Max(8, damage);
		}
	}
	public static int GoldStrike = 0;
	public override void Load()
	{
		On_CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
	}
	private int CombatText_NewText_Rectangle_Color_string_bool_bool(On_CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
	{
		if (GoldStrike > 0)
		{
			color = new Color(255, 235, 0);
			GoldStrike--;
		}
		return orig(location, color, text, dramatic, dot);
	}
}
