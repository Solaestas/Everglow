namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class HangGlider : ModItem
{
	public const float AccelerationX = 0.1f;
	public const float VelocityXMAX = 10f;
	public const float VelocityYMAX = 2f;

	public override void SetDefaults()
	{
		Item.width = 54;
		Item.height = 28;
		Item.scale = 1.4f;

		Item.holdStyle = ItemHoldStyleID.HoldUp;

		Item.value = 0;
		Item.rare = ItemRarityID.Gray;
	}

	public override void HoldItem(Player player)
	{
		if (!player.pulley && !player.controlDown)
		{
			// Manage velocity X
			if (MathF.Abs(player.velocity.Y) > 0)
			{
				var targetVelocityX = player.velocity.X + player.direction * AccelerationX;
				if (MathF.Sign(targetVelocityX) != player.direction || MathF.Abs(targetVelocityX) <= VelocityXMAX)
				{
					player.velocity.X = targetVelocityX;
				}
			}

			// Manage velocity Y
			if (player.gravDir == -1f)
			{
				if (player.velocity.Y < -VelocityYMAX)
				{
					player.velocity.Y = -VelocityYMAX;
				}
			}
			else if (player.velocity.Y > VelocityYMAX)
			{
				player.velocity.Y = VelocityYMAX;
			}
		}
	}

	public override void HoldStyle(Player player, Rectangle heldItemFrame)
	{
		if (!player.pulley)
		{
			player.itemRotation = player.direction * 0;
			player.itemLocation.X = player.Center.X - 30 * player.direction;
			player.itemLocation.Y = player.Center.Y + 0f;
			player.fallStart = (int)(player.position.Y / 16f);
		}
	}
}