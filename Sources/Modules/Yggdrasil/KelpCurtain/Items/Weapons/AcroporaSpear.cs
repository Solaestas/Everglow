using Everglow.Sources.Modules.MEACModule.Projectiles;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons
{
	public class AcroporaSpear : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 116;
			Item.height = 132;
			Item.useAnimation = 5;
			Item.useTime = 5;
			Item.shootSpeed = 5f;
			Item.knockBack = 5.5f;
			Item.damage = 34; //Original: Item.damage = 30
			Item.rare = ItemRarityID.Green;

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.value = Item.sellPrice(gold: 1);
		}
		public override bool CanUseItem(Player player)
		{
			if (base.CanUseItem(player))
			{
				if (Main.myPlayer == player.whoAmI)
				{
					if (player.altFunctionUse != 2)
						Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.AcroporaSpear>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
					else//右键
					{
					}
				}
				return false;
			}
			return base.CanUseItem(player);
		}
	}
}
