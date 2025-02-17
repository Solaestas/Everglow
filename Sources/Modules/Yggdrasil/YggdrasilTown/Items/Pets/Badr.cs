using Everglow.Yggdrasil.YggdrasilTown.Buffs.Pets;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Pets;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Pets;

public class Badr : ModItem
{
	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.WispinaBottle);
		Item.UseSound = SoundID.Item83;
		Item.shoot = ModContent.ProjectileType<BadrProj>();
		Item.buffType = ModContent.BuffType<BadrBuff>();

		Item.value = Item.sellPrice(gold: 5);
		Item.rare = ItemRarityID.Orange;
	}

	public override void UseStyle(Player player, Rectangle heldItemFrame)
	{
		if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
		{
			player.AddBuff(Item.buffType, 3600, true);
		}
	}
}