using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Twilight;

[AutoloadEquip(EquipType.Legs)]
public class TwilightWoodLeggings : ModItem
{
	public const int MoveSpeedBonus = 5;
	public const int JumpSpeedBonus = 5;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = Item.buyPrice(silver: 37, copper: 50);
		Item.rare = ItemRarityID.White;
		Item.defense = 2;
	}

	public override void UpdateEquip(Player player)
	{
		player.moveSpeed += MoveSpeedBonus / 100f;
		player.jumpSpeedBoost += JumpSpeedBonus / 100f;

		if (player.velocity.Y == 0 && player.velocity.Length() >= 1E-05f)
		{
			Dust.NewDust(player.position + new Vector2(player.width / 2, player.height), 1, 1, ModContent.DustType<TwilightCrystalDust>(), player.velocity.X * 0.5f, player.velocity.Y * 0.5f);
		}
	}
}