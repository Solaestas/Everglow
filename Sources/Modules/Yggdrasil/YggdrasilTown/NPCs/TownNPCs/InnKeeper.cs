using Everglow.Commons.Coroutines;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.TownNPCAttack;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

[AutoloadHead]
public class InnKeeper : TownNPC_LiveInYggdrasil
{
	public bool Attacking0 = false;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 9;
	}

	public override void SetDefaults()
	{
		StandFrame = new Rectangle(0, 0, 48, 56);
		NPC.frame = StandFrame;
		FrameHeight = 56;
		Attacking0 = false;
		base.SetDefaults();
	}

	public override void TryAttack()
	{
		float minDis = 100;
		float minDis2 = 400;
		if (ThreatenTarget >= 0 && ThreatenTarget < 200)
		{
			NPC npc = Main.npc[ThreatenTarget];
			Vector2 distance = npc.Center - NPC.Center;
			for (int i = 0; i < 8; i++)
			{
				Vector2 checkPos = npc.Center;
				switch (i)
				{
					case 0:
						checkPos = npc.TopLeft;
						break;
					case 1:
						checkPos = npc.Top;
						break;
					case 2:
						checkPos = npc.TopRight;
						break;
					case 3:
						checkPos = npc.Left;
						break;
					case 4:
						checkPos = npc.Right;
						break;
					case 5:
						checkPos = npc.BottomLeft;
						break;
					case 6:
						checkPos = npc.Bottom;
						break;
					case 7:
						checkPos = npc.BottomRight;
						break;
				}
				if ((checkPos - NPC.Center).Length() < distance.Length())
				{
					distance = checkPos - NPC.Center;
				}
			}
			if (Main.rand.NextFloat(TotalThreaten) > 0.3f)
			{
				if (distance.Length() < minDis2)
				{
					BehaviorsCoroutines.Enqueue(new Coroutine(Attack2()));
					return;
				}
			}
			if (distance.Length() < minDis)
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack()));
				return;
			}
			else if (distance.Length() < minDis2)
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack2()));
				return;
			}
		}
	}

	public override void WalkFrame()
	{
		NPC.frameCounter += Math.Abs(NPC.velocity.X);
		if (NPC.frameCounter > 4)
		{
			NPC.frame.Y += FrameHeight;
			NPC.frameCounter = 0;
		}
		if (NPC.frame.Y > 7 * FrameHeight)
		{
			NPC.frame.Y = 0;
		}
	}

	public override void CheckInSuitableArea()
	{
		if (SubworldSystem.Current is not YggdrasilWorld)
		{
			return;
		}
		AnchorForBehaviorPos = YggdrasilTownCentralSystem.TownTopLeftWorldCoord.ToTileCoordinates() + new Point(358, 156);
		bool safe = false;
		var homePoint = AnchorForBehaviorPos;
		NPC.homeless = false;
		NPC.homeTileX = homePoint.X;
		NPC.homeTileY = homePoint.Y;

		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X > AnchorForBehaviorPos.X - 100 && npcTilePos.X < AnchorForBehaviorPos.X + 100)
		{
			if (npcTilePos.Y > AnchorForBehaviorPos.Y - 20 && npcTilePos.Y < AnchorForBehaviorPos.Y + 20)
			{
				safe = true;
			}
		}
		if (!safe)
		{
			TeleportHome();
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!Attacking0)
		{
			Texture2D texMain = ModAsset.InnKeeper.Value;
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.height - NPC.frame.Height) * 0.5f;
			Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
		else
		{
			Texture2D texMain = ModAsset.InnKeeper_Attack.Value;
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, -8);
			Main.spriteBatch.Draw(texMain, drawPos, null, drawColor, NPC.rotation, texMain.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
		return false;
	}

	public IEnumerator<ICoroutineInstruction> Attack()
	{
		bool safe = false;
		if (ThreatenTarget is >= 0 and < 200)
		{
			safe = true;
		}
		if (!safe)
		{
			EndAIPiece();
			yield break;
		}
		NPC target = Main.npc[ThreatenTarget];
		if (target.Center.X > NPC.Center.X)
		{
			NPC.direction = 1;
		}
		if (target.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(-NPC.direction, 0), ModContent.ProjectileType<Georg_Hammer>(), 150, 4, Main.myPlayer, NPC.whoAmI);
		for (int t = 0; t < 60; t++)
		{
			NPC.velocity.X *= 0;
			Attacking0 = true;
			yield return new SkipThisFrame();
		}
		Attacking0 = false;
		EndAIPiece();
	}

	public IEnumerator<ICoroutineInstruction> Attack2()
	{
		bool safe = false;
		if (ThreatenTarget is >= 0 and < 200)
		{
			safe = true;
		}
		if (!safe)
		{
			EndAIPiece();
			yield break;
		}
		NPC target = Main.npc[ThreatenTarget];
		if (target.Center.X > NPC.Center.X)
		{
			NPC.direction = 1;
		}
		if (target.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (target.Center - NPC.Center).NormalizeSafe(), ModContent.ProjectileType<Georg_Hook>(), 100, 4, Main.myPlayer, NPC.whoAmI);
		for (int t = 0; t < 100; t++)
		{
			NPC.velocity.X *= 0;
			Attacking0 = true;
			yield return new SkipThisFrame();
		}
		Attacking0 = false;
		EndAIPiece();
	}
}