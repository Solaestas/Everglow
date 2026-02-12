namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class FlappedHangGlider : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Tools;

    public const float AccelerationX = 0.1f;
    public const float VelocityXMAX = 10f;
    public const float VelocityYMAX = 2f;
    public const int DurabilityMax = 120;
    public const int DurabilityRecoverCooldownMax = 45;

    public int Durability { get; private set; }

    public int DurabilityRecoverCooldown { get; private set; }

    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 28;
        Item.scale = 1.4f;

        Item.holdStyle = ItemHoldStyleID.HoldUp;
        Item.value = 0;
        Item.rare = ItemRarityID.Gray;

        Durability = DurabilityMax;
        DurabilityRecoverCooldown = 0;
    }

    public override void HoldItem(Player player)
    {
        // Check if player is on the ground
        if (TileUtils.PlatformCollision(player.Bottom + new Vector2(0, 2)))
        {
            if (DurabilityRecoverCooldown <= 0)
            {
                Durability = DurabilityMax;
            }
            else
            {
                DurabilityRecoverCooldown--;
            }
        }

        // Check hang glider durability
        if (Durability < 0)
        {
            Item.holdStyle = ItemHoldStyleID.None;
            return;
        }
        else
        {
            Item.holdStyle = ItemHoldStyleID.HoldUp;
        }

        if (player.pulley || player.controlDown)
        {
            return;
        }

        // Manage velocity X
        if (MathF.Abs(player.velocity.Y) > 0)
        {
            Durability--;
            DurabilityRecoverCooldown = DurabilityRecoverCooldownMax;
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

    public override void HoldStyle(Player player, Rectangle heldItemFrame)
    {
        if (!player.pulley)
        {
            player.itemRotation = player.direction * 0;
            player.itemLocation.X = player.Center.X - 30 * player.direction;
            player.itemLocation.Y = player.Center.Y + 0f;
        }
    }
}