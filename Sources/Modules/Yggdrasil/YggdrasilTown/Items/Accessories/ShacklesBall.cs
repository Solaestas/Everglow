namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class ShacklesBall : ModItem
{
	public const float EnduranceBonus = 0.1f;
	public const float MoveSpeedReduction = 0.15f;
	public const float JumpSpeedReduction = 0.15f;
	public const float JumpHeightReduction = 0.07f;

	public override void SetDefaults()
	{
		Item.accessory = true;
		Item.width = 28;
		Item.height = 20;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(platinum: 0, gold: 1);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.endurance += EnduranceBonus;
		player.moveSpeed -= MoveSpeedReduction;
		player.jumpSpeedBoost -= Player.jumpSpeed * JumpSpeedReduction;
		player.jump = (int)(player.jump * (1 - JumpHeightReduction));
		player.buffImmune[BuffID.Slow] = true;
	}
}