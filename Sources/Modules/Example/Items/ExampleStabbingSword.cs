using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Example.Projectiles;

namespace Everglow.Example.Items
{
	public class ExampleStabbingSword : StabbingSwordItem
	{
		public int HoldingTimer = 0;

		public override void SetCustomDefault()
		{
			StaminaCost += 0.3f;
			Item.damage = 3;
			Item.knockBack = 1f;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 12, 0);
			Item.shoot = ModContent.ProjectileType<ExampleStabbingSword_Pro>();
			PowerfulStabDamageFlat = 4f;
			PowerfulStabProj = ModContent.ProjectileType<ExampleStabbingSword_Pro_Stab>();
		}

		public override void HoldItem(Player player)
		{
			// if (HoldingTimer == 0)
			// {
			// ExampleStabbingSword_Config config = new ExampleStabbingSword_Config()
			// {
			// Owner = player,
			// Visible = true,
			// Active = true,
			// };
			// Ins.VFXManager.Add(config);
			// }
			// HoldingTimer++;
			base.HoldItem(player);
		}
	}
}