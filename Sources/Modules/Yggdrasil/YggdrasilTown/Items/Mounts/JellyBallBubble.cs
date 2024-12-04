namespace Everglow.Yggdrasil.YggdrasilTown.Items.Mounts;

public class JellyBallBubble : ModMount
{
	public const int Distance = 16;

	public const float GravityAcceleration = 5f;
	public const float BuoyancyAcceleration = 8f;

	public override void SetStaticDefaults()
	{
		// Movement
		MountData.heightBoost = 20; // Height between the mount and the ground
		MountData.flightTimeMax = int.MaxValue - 1; // The amount of time in frames a mount can be in the state of flying.
		MountData.fatigueMax = int.MaxValue - 1;
		MountData.fallDamage = 0f;
		MountData.usesHover = true;
		MountData.runSpeed = 11f; // The speed of the mount
		MountData.dashSpeed = 8f; // The speed the mount moves when in the state of dashing.
		MountData.acceleration = 0.19f; // The rate at which the mount speeds up.
		MountData.jumpHeight = 5; // How high the mount can jump.
		MountData.jumpSpeed = 4f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is pressed.
		MountData.blockExtraJumps = true; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
		MountData.constantJump = false; // Allows you to hold the jump button down.

		// Misc
		MountData.buff = ModContent.BuffType<Buffs.JellyBallBubbleMount>(); // The ID number of the buff assigned to the mount.

		// Visual Effects
		MountData.spawnDust = ModContent.DustType<Dusts.JellyBallSpark>(); // The ID of the dust spawned when mounted or dismounted.
		MountData.spawnDustNoGravity = true;

		// Frame data and player offsets
		MountData.totalFrames = 4; // Amount of animation frames for the mount
		MountData.playerYOffsets = Enumerable.Repeat(20, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
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
		bool playerIsNotMoving = !(player.controlLeft
			|| player.controlRight
			|| player.controlUp
			|| player.controlDown
			|| player.controlHook);

		// TODO: Update the maxSpeed on X axis to player's maxSpeed

		var position = FindSentryRestingSpot(player, player.MountedCenter, out int xPosition, out int yPosition);
		if (playerIsNotMoving)
		{
			float distance = position.Y - player.MountedCenter.Y;

			var offset = 0.5f;
			var min = (Distance - offset) * 16f;
			var max = (Distance + offset) * 16f;
			var buoyancyFact = 1 - distance.SmoothStep(min, max);

			var acceleration = GravityAcceleration - BuoyancyAcceleration * buoyancyFact;
			player.velocity.Y += acceleration * 0.1f;
		}

		// TODO: Delete development testing code
		// Make bottom tile position visualizable
		Dust.NewDust(position, 1, 1, DustID.Torch, 0, 0, 1);
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