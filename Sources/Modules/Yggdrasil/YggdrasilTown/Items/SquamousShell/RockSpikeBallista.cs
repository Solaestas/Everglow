using Everglow.Commons.Weapons.CrossBow;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.SquamousShell;

public class RockSpikeBallista : CrossBowItem
{
	public override void SetDef()
	{
		Item.width = 108;
		Item.height = 38;
		Item.rare = ItemRarityID.White;
		Item.value = 4200;

		Item.useTime = 24;
		Item.useAnimation = 24;
		Item.shootSpeed = 21f;
		Item.damage = 18;
		Item.knockBack = 8f;
		CrossBowProjType = ModContent.ProjectileType<RockSpikeBallista_Proj>();
	}
}
