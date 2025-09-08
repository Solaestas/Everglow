using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.LightningMechanism;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class UnderwaterLightningMechanism_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public int State = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<UnderwaterLightningMechanism>());
		Item.width = 18;
		Item.height = 36;
		Item.value = 8000;
	}

	public Vector2 OldPos = default(Vector2);

	public override void HoldItem(Player player)
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			// if (OldPos == default(Vector2))
			// {
			// OldPos = Main.MouseWorld;
			// }
			// else
			// {
			// Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<UnderwaterLightningMechanism_Lightning>(), 250, 5f, player.whoAmI, 0, 2);
			// UnderwaterLightningMechanism_Lightning uLLML = proj.ModProjectile as UnderwaterLightningMechanism_Lightning;
			// if (uLLML is not null)
			// {
			// uLLML.StartPos = OldPos;
			// uLLML.EndPos = Main.MouseWorld;
			// OldPos = Main.MouseWorld;
			// }
			// }

			State++;
			if (State >= 2)
			{
				State = 0;
			}
			if (State == 1)
			{
				Item.DefaultToPlaceableTile(ModContent.TileType<UnderwaterLightningMechanism_H>());
				Item.width = 18;
				Item.height = 36;
			}
			else
			{
				Item.DefaultToPlaceableTile(ModContent.TileType<UnderwaterLightningMechanism>());
				Item.width = 18;
				Item.height = 36;
			}
		}
		base.HoldItem(player);
	}
}