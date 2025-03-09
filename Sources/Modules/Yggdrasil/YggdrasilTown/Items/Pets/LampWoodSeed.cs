using Everglow.Yggdrasil.YggdrasilTown.Buffs.Pets;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Pets;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Pets;

public class LampWoodSeed : ModItem
{
	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.WispinaBottle);
		Item.width = 28;
		Item.height = 50;

		Item.UseSound = SoundID.Item83;
		Item.shoot = ModContent.ProjectileType<LampWoodSeedProj>();
		Item.buffType = ModContent.BuffType<LampWoodSeedBuff>();

		Item.value = Item.sellPrice(gold: 1, silver: 50);
		Item.rare = ItemRarityID.Blue;
	}

	public override void UseStyle(Player player, Rectangle heldItemFrame)
	{
		if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
		{
			player.AddBuff(Item.buffType, 2, true);
		}
	}
}