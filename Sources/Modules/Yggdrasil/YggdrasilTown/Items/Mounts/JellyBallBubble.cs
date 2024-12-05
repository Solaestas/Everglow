namespace Everglow.Yggdrasil.YggdrasilTown.Items.Mounts;

public class JellyBallBubble : ModMount
{
	public const int Distance = 16;

	public const float GravityAcceleration = 5f;
	public const float BuoyancyAcceleration = 8f;

	public override void SetStaticDefaults()
	{
		// Movement
		MountData.heightBoost = 20;
		MountData.flightTimeMax = int.MaxValue - 1;
		MountData.fatigueMax = int.MaxValue - 1;
		MountData.fallDamage = 0f;
		MountData.usesHover = true;

		MountData.runSpeed = 11f;
		MountData.dashSpeed = 8f;
		MountData.acceleration = 0.19f;

		MountData.jumpHeight = 5;
		MountData.jumpSpeed = 4f;
		MountData.blockExtraJumps = true;
		MountData.constantJump = false;

		// Misc
		MountData.buff = ModContent.BuffType<Buffs.JellyBallBubbleMount>();

		// Visual Effects
		MountData.spawnDust = ModContent.DustType<Dusts.JellyBallSpark>();
		MountData.spawnDustNoGravity = true;

		// Frame data and player offsets
		MountData.totalFrames = 4;
		MountData.playerYOffsets = Enumerable.Repeat(20, MountData.totalFrames).ToArray();
		MountData.xOffset = 13;
		MountData.yOffset = -12;
		MountData.playerHeadOffset = 22;
		MountData.bodyFrame = 3;

		// Standing
		MountData.standingFrameCount = 4;
		MountData.standingFrameDelay = 12;
		MountData.standingFrameStart = 0;

		// Running
		MountData.runningFrameCount = 4;
		MountData.runningFrameDelay = 12;
		MountData.runningFrameStart = 0;

		// Flying
		MountData.flyingFrameCount = 4;
		MountData.flyingFrameDelay = 12;
		MountData.flyingFrameStart = 0;

		// In-air
		MountData.inAirFrameCount = 4;
		MountData.inAirFrameDelay = 12;
		MountData.inAirFrameStart = 0;

		// Idle
		MountData.idleFrameCount = 4;
		MountData.idleFrameDelay = 12;
		MountData.idleFrameStart = 0;
		MountData.idleFrameLoop = true;

		// Swim
		MountData.swimFrameCount = MountData.inAirFrameCount;
		MountData.swimFrameDelay = MountData.inAirFrameDelay;
		MountData.swimFrameStart = MountData.inAirFrameStart;

		if (Main.netMode != NetmodeID.Server)
		{
			MountData.textureWidth = MountData.backTexture.Width();
			MountData.textureHeight = MountData.backTexture.Height();
		}
	}

	public override void UpdateEffects(Player player)
	{
		// Update the max speed of mount to player's max speed
		MountData.runSpeed = player.maxRunSpeed;
		MountData.dashSpeed = player.maxRunSpeed;
		MountData.acceleration = 0.19f;

		// Buoyancy simulation
		var bottomTilePosition = FindSentryRestingSpot(player, player.MountedCenter, out int xPosition, out int yPosition);
		var playerIsNotMoving = !(player.controlLeft || player.controlRight || player.controlUp || player.controlDown);
		if (playerIsNotMoving)
		{
			MountData.acceleration = 0.05f;
			var distanceToBottomTile = bottomTilePosition.Y - player.MountedCenter.Y;
			var offset = MathF.Abs(MountData.heightBoost) / (16f * 2f);
			var buoyancyFact = 1 - distanceToBottomTile.SmoothStep((Distance - offset) * 16f, (Distance + offset) * 16f);
			var acceleration = (GravityAcceleration - BuoyancyAcceleration * buoyancyFact) * 0.1f;
			player.velocity.Y += acceleration;
		}

		// Make positions visualizable
		if (Main.timeForVisualEffects % 3 == 0)
		{
			Dust.NewDust(player.MountedCenter, 2, 2, DustID.Cloud, 0, 0, 1);
			Dust.NewDust(bottomTilePosition, 1, 1, DustID.Torch, 0, 0, 1);
		}
	}

	public static Vector2 FindSentryRestingSpot(Player player, Vector2 position, out int worldX, out int worldY)
	{
		static void LimitPointToPlayerReachableArea(Player player, ref Vector2 pointPoisition)
		{
			Vector2 center = player.Center;
			Vector2 vector = pointPoisition - center;
			float num = Math.Abs(vector.X);
			float num2 = Math.Abs(vector.Y);
			float num3 = 1f;
			if (num > 960f)
			{
				float num4 = 960f / num;
				if (num3 > num4)
				{
					num3 = num4;
				}
			}

			if (num2 > 600f)
			{
				float num5 = 600f / num2;
				if (num3 > num5)
				{
					num3 = num5;
				}
			}

			Vector2 vector2 = vector * num3;
			pointPoisition = center + vector2;
		}

		Vector2 pointPoisition = position;
		LimitPointToPlayerReachableArea(player, ref pointPoisition);
		int num = (int)pointPoisition.X / 16;
		int i = (int)pointPoisition.Y / 16;
		worldX = num * 16 + 8;

		for (;
			i < Main.maxTilesY - 10
			 && Main.tile[num, i] != null
			 && !WorldGen.SolidTile2(num, i)
			 && Main.tile[num - 1, i] != null
			 && !WorldGen.SolidTile2(num - 1, i)
			 && Main.tile[num + 1, i] != null
			 && !WorldGen.SolidTile2(num + 1, i);
			i++)
		{
		}

		worldY = i * 16;

		return new(worldX, worldY);
	}
}