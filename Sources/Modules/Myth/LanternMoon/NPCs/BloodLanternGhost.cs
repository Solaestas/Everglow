using Everglow.Myth.LanternMoon.Gores;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.NPCs;

public class BloodLanternGhost : LanternMoonNPC
{
	public Vector2 StayPosition = Vector2.zeroVector;

	public int Timer;

	public int TeleportTime = 300;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 8;
	}

	public override void SetDefaults()
	{
		NPC.damage = 90;
		NPC.lifeMax = 558;
		NPC.npcSlots = 2.5f;
		NPC.width = 60;
		NPC.height = 60;
		NPC.defense = 24;
		NPC.value = 200;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.8f;
		NPC.dontTakeDamage = false;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit3;
		LanternMoonScore = 12f;
	}

	public override void OnSpawn(IEntitySource source)
	{
		NPC.frame.Y = 0;
	}

	public override void FindFrame(int frameHeight)
	{
		NPC.frameCounter++;
		frameHeight = 122;
		if (NPC.frameCounter >= 6)
		{
			NPC.frameCounter = 0;
			NPC.frame.Y += frameHeight;
			if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
			{
				NPC.frame.Y = 0;
			}
		}
	}

	public override void AI()
	{
		if (Main.dayTime)
		{
			NPC.velocity.Y += 1;
			return;
		}
		Timer++;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		if (StayPosition == Vector2.zeroVector || Timer == 1)
		{
			Vector2 toPlayer = player.Center - NPC.Center;
			toPlayer = -toPlayer.NormalizeSafe() * Main.rand.NextFloat(180, 420);
			toPlayer = toPlayer.RotatedByRandom(MathHelper.PiOver4);
			if (toPlayer.Y > 0)
			{
				toPlayer.Y *= -1;
			}
			StayPosition = player.Center + toPlayer;
		}
		if(Timer > 1)
		{
			NPC.Center = Vector2.Lerp(NPC.Center, StayPosition, 0.02f);
		}
		NPC.velocity *= 0.95f;
		if (Timer > TeleportTime)
		{
			TeleportTime = Main.rand.Next(300, 500);
			Timer = 0;
		}
	}

	public override void OnKill()
	{
		for (int g = 0; g < 8; g++)
		{
			Vector2 vel = new Vector2(MathF.Sqrt(Main.rand.NextFloat()) * 8f, 0).RotatedByRandom(MathHelper.TwoPi);
			string texturePath = ModAsset.BloodLanternGhost_Gore_0_Mod;
			if (texturePath is not null)
			{
				texturePath = texturePath.Remove(texturePath.Length - 1, 1);
				texturePath += g;
			}
			var gore = new NormalGore
			{
				Velocity = vel,
				Position = NPC.Center + vel,
				Texture = ModContent.Request<Texture2D>(texturePath).Value,
				RotateSpeed = Main.rand.NextFloat(-0.2f, 0.2f),
				Scale = Main.rand.NextFloat(0.8f, 1.2f),
				MaxTime = Main.rand.Next(300, 340),
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
			};
			Ins.VFXManager.Add(gore);
		}
		base.OnKill();
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		SpriteEffects effects = SpriteEffects.None;
		NPC.spriteDirection = NPC.direction;
		if (NPC.spriteDirection == 1)
		{
			effects = SpriteEffects.FlipHorizontally;
		}
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, effects, 0);
		return false;
	}
}