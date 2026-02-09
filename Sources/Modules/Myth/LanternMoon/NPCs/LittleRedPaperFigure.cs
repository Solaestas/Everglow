using Everglow.Myth.LanternMoon.Buffs;

namespace Everglow.Myth.LanternMoon.NPCs;

public class LittleRedPaperFigure : LanternMoonNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 8;
	}

	public enum BehaviorState
	{
		Fighter,
		Fly,
		Attach,
	}

	public int State;

	public int Timer;

	public int ClosingTimeMax = 300;

	public Vector2 SummonPos = Vector2.zeroVector;

	public override void SetDefaults()
	{
		NPC.damage = 25;
		NPC.lifeMax = 150;
		NPC.npcSlots = 1f;
		NPC.width = 38;
		NPC.height = 52;
		NPC.defense = 8;
		NPC.value = 200;
		NPC.aiStyle = NPCAIStyleID.Fighter;
		NPC.knockBackResist = 1.5f;
		NPC.dontTakeDamage = false;
		NPC.HitSound = SoundID.NPCHit3;
		LanternMoonScore = 2f;
	}

	public override void FindFrame(int frameHeight)
	{
		Timer++;
		NPC.frame.Width = 52;
		NPC.frame.Height = 52;
		if (State == (int)BehaviorState.Fighter)
		{
			NPC.frameCounter++;
			NPC.frame.X = 0;
			Main.npcFrameCount[NPC.type] = 8;
			if (NPC.frameCounter >= 8)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += NPC.frame.Height;
				if (NPC.frame.Y >= NPC.frame.Height * Main.npcFrameCount[NPC.type])
				{
					NPC.frame.Y = 0;
				}
			}
		}
		else if (State == (int)BehaviorState.Fly)
		{
			if (Timer % 2 == 0)
			{
				NPC.frameCounter++;
			}
			NPC.frame.X = 52;
			Main.npcFrameCount[NPC.type] = 8;
			if (NPC.frameCounter >= 8)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += NPC.frame.Height;
				if (NPC.frame.Y >= NPC.frame.Height * Main.npcFrameCount[NPC.type])
				{
					NPC.frame.Y = 0;
				}
			}
		}
		else if (State == (int)BehaviorState.Attach)
		{
			NPC.frame.X = 104;
			NPC.frame.Y = 0;
			NPC.frameCounter = 0;
		}
	}

	public override void AI()
	{
		NPC.TargetClosest(false);
		if (NPC.target == -1)
		{
			return;
		}
		Player player = Main.player[NPC.target];
		if (player.Center.X > NPC.Center.X)
		{
			NPC.direction = 1;
		}
		else
		{
			NPC.direction = -1;
		}
		Lighting.AddLight(NPC.Center, 0.4f, 0.05f, 0.05f);
		if (State == (int)BehaviorState.Fighter)
		{
			NPC.aiStyle = NPCAIStyleID.Fighter;
		}
		else if (State == (int)BehaviorState.Fly)
		{
			NPC.aiStyle = NPCAIStyleID.StarCell;
			NPC.noGravity = true;
			NPC.rotation = 0;
		}
		else if (State == (int)BehaviorState.Attach)
		{
			NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
			NPC.knockBackResist = 0f;
			NPC.Center = player.Center - Vector2.UnitY * 9 - (player.direction == 1 ? 2 * Vector2.UnitX : -2 * Vector2.UnitX);
			NPC.noGravity = true;
			NPC.rotation = 0;
		}
	}

	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		if (State == (int)BehaviorState.Fighter)
		{
			State = (int)BehaviorState.Fly;
		}
	}

	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
	{
		if (State == (int)BehaviorState.Fighter)
		{
			State = (int)BehaviorState.Fly;
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit)
	{
		if (State == (int)BehaviorState.Fighter)
		{
			State = (int)BehaviorState.Fly;
		}
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		if (State == (int)BehaviorState.Attach)
		{
			target.AddBuff(ModContent.BuffType<PaperObstructed>(), 59);
		}
		if (State != (int)BehaviorState.Attach)
		{
			State = (int)BehaviorState.Attach;
		}
	}

	public override void OnKill()
	{
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Player target = Main.player[NPC.target];
		NPC.spriteDirection = NPC.direction;
		Texture2D texture = ModAsset.LittleRedPaperFigure.Value;
		if (State != (int)BehaviorState.Attach && !target.creativeGodMode)
		{
			spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}
		if (State == (int)BehaviorState.Attach)
		{
			spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, target.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		return false;
	}
}