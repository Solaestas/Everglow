using Everglow.Commons.Coroutines;
using Humanizer;
using MathNet.Numerics;
using SubworldLibrary;
using Terraria.DataStructures;
using static Everglow.Commons.Utilities.NPCUtils;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

public abstract class TownNPC_LiveInYggdrasil : ModNPC
{
	/// <summary>
	/// Control specific behaviors; Control by _townNPCGeneralCoroutine.
	/// </summary>
	public CoroutineManager _townNPCBehaviorCoroutine = new CoroutineManager();

	/// <summary>
	/// Manage behavior pool.
	/// </summary>
	public CoroutineManager _townNPCGeneralCoroutine = new CoroutineManager();

	/// <summary>
	/// Behaviors pool, waiting for _townNPCBehaviorCoroutine to execute.
	/// </summary>
	public Queue<Coroutine> BehaviorsCoroutines = new Queue<Coroutine>();
	public bool Talking = false;
	public bool Sit = false;
	public bool Idle = true;
	public int ThreatenTarget = -1;
	public int FrameHeight = 56;

	/// <summary>
	/// A scalars for the total threaten value;
	/// </summary>
	public float TotalThreaten = 0f;

	/// <summary>
	/// A vector2 for total threaten direction;
	/// </summary>
	public Vector2 TotalThreatenDirection = Vector2.zeroVector;

	public Rectangle StandFrame = new Rectangle(0, 0, 10, 10);

	/// <summary>
	/// HomePos, force attach his or her home to this coordinate.
	/// </summary>
	public Point AnchorForBehaviorPos = YggdrasilTownCentralSystem.TownTopLeftWorldCoord.ToTileCoordinates();

	private int aiMainCount = 0;

	public override void SetDefaults()
	{
		NPC.townNPC = true;
		NPC.width = 18;
		NPC.height = 40;
		NPC.aiStyle = -1;
		NPC.damage = 100;
		NPC.defense = 100;
		NPC.lifeMax = 250;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.friendly = true;
		NPC.knockBackResist = 0;
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return 0;
	}

	public override void OnSpawn(IEntitySource source)
	{
		_townNPCGeneralCoroutine.StartCoroutine(new Coroutine(BehaviorControl()));
		base.OnSpawn(source);
	}

	public override bool PreAI()
	{
		aiMainCount = 0;
		return base.PreAI();
	}

	public override void AI()
	{
		_townNPCBehaviorCoroutine.Update();
		_townNPCGeneralCoroutine.Update();
		if (!Talking)
		{
			CheckDangers();
			if (BehaviorsCoroutines.Count <= 0)
			{
				Idle = true;
				if (Peace())
				{
					if (Sit)
					{
						BehaviorsCoroutines.Enqueue(new Coroutine(SitDown(Main.rand.Next(60, 90))));
					}
					else
					{
						if (Main.rand.NextBool())
						{
							BehaviorsCoroutines.Enqueue(new Coroutine(Stand(Main.rand.Next(60, 90))));
						}
						else
						{
							BehaviorsCoroutines.Enqueue(new Coroutine(Walk(Main.rand.Next(60, 900))));
						}
					}
				}
				else if (CanEscape())
				{
					Sit = false;
					if (TotalThreaten < 1 && Main.rand.NextBool(2))
					{
						TryAttack();
					}
					else
					{
						BehaviorsCoroutines.Enqueue(new Coroutine(Walk(Main.rand.Next(60, 90))));
					}
				}
				else
				{
					Sit = false;
					TryAttack();
				}
			}
		}
		else
		{
			if (!CheckTalkingPlayer())
			{
				Talking = false;
			}
		}

		if (aiMainCount == 0)
		{
			_townNPCGeneralCoroutine.StartCoroutine(new Coroutine(BehaviorControl()));
		}

		// ai[0] = 5 is a magic number represent to a sitting town npc.
		// so we should prevent other npc from sitting the same chair.
		if (Sit)
		{
			NPC.aiStyle = 7;
			NPC.ai[0] = 5;
		}
		else
		{
			NPC.aiStyle = -1;
			NPC.ai[0] = 0;
		}
	}

	public virtual bool Peace()
	{
		if (TotalThreaten > 0.005)
		{
			return false;
		}
		return true;
	}

	public virtual bool CanEscape()
	{
		// Main.NewText(TotalThreaten);
		// Main.NewText(MathF.Abs(TotalThreatenDirection.X), Color.Red);
		if (!CanContinueWalk(NPC))
		{
			NPC.frame = StandFrame;
			return false;
		}
		if (TotalThreaten > MathF.Abs(TotalThreatenDirection.X) * 3f)
		{
			NPC.frame = StandFrame;
			return false;
		}
		if(!SafeAndInSuitable())
		{
			return false;
		}
		return true;
	}

	public virtual bool SafeAndInSuitable()
	{
		if(SubworldSystem.Current is not YggdrasilWorld)
		{
			return true;
		}
		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X < AnchorForBehaviorPos.X - 90 && TotalThreatenDirection.X < 0)
		{
			NPC.frame = StandFrame;
			return false;
		}
		if (npcTilePos.X > AnchorForBehaviorPos.X + 90 && TotalThreatenDirection.X > 0)
		{
			NPC.frame = StandFrame;
			return false;
		}
		return true;
	}

	public virtual bool CheckTalkingPlayer()
	{
		for (int j = 0; j < 255; j++)
		{
			if (Main.player[j].active && Main.player[j].talkNPC == NPC.whoAmI)
			{
				if (Main.player[j].position.X + Main.player[j].width / 2 < NPC.position.X + NPC.width / 2)
				{
					NPC.direction = -1;
				}
				else
				{
					NPC.direction = 1;
				}
				return true;
			}
		}
		return false;
	}

	public virtual void CheckDuplicatedSelf()
	{
		foreach (NPC npc in Main.npc)
		{
			if (npc != null && npc.type == Type && npc.active && npc != NPC)
			{
				npc.active = false;
			}
			if (NPC.CountNPCS(Type) <= 1)
			{
				break;
			}
		}
	}

	public virtual void TryAttack()
	{
	}

	/// <summary>
	/// Analysis surrounding threaten sources.
	/// </summary>
	public virtual void CheckDangers()
	{
		ThreatenTarget = -1;
		TotalThreatenDirection = Vector2.zeroVector;
		TotalThreaten = 0f;
		float minDis = 300;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter && npc != NPC)
			{
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
				if (distance.Length() < 300 && npc.damage > 0)
				{
					TotalThreatenDirection += (-distance.NormalizeSafe() / (distance.Length() + 1)) * 1f * npc.damage;
					TotalThreaten += (-distance.NormalizeSafe() / (distance.Length() + 1)).Length() * 1f * npc.damage;
					if (distance.Length() < minDis)
					{
						minDis = distance.Length();
						ThreatenTarget = npc.whoAmI;
					}
				}
			}
		}
	}

	public virtual void CheckInSuitableArea()
	{
		if (SubworldSystem.Current is not YggdrasilWorld)
		{
			return;
		}
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

	public virtual void TeleportHome()
	{
		NPC.Center = AnchorForBehaviorPos.ToWorldCoordinates() + new Vector2(0, 48);
	}

	public virtual void EndAIPiece()
	{
		Idle = true;
		BehaviorsCoroutines.Dequeue();
	}

	/// <summary>
	/// Inheritable
	/// </summary>
	/// <returns></returns>
	public virtual IEnumerator<ICoroutineInstruction> BehaviorControl()
	{
		while (true)
		{
			CheckInSuitableArea();
			CheckDuplicatedSelf();
			aiMainCount++;
			if (BehaviorsCoroutines.Count > 0 && Idle)
			{
				Idle = false;
				_townNPCBehaviorCoroutine.StartCoroutine(BehaviorsCoroutines.First());
			}
			if (aiMainCount >= 2)
			{
				yield break;
			}
			yield return new SkipThisFrame();
		}
	}

	/// <summary>
	/// Sitting silently; Inheritable
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public virtual IEnumerator<ICoroutineInstruction> SitDown(int time)
	{
		Sit = true;
		for (int t = 0; t < time; t++)
		{
			NPC.frame = new Rectangle(0, 616, 48, FrameHeight);
			NPC.velocity.X = 0;
			NPC.spriteDirection = NPC.direction;
			if (!Peace())
			{
				if (TotalThreatenDirection.X > 0)
				{
					NPC.direction = 1;
				}
				else
				{
					NPC.direction = -1;
				}
				break;
			}
			yield return new SkipThisFrame();
		}
		Sit = false;
		EndAIPiece();
	}

	/// <summary>
	/// Standing silently; Inheritable
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public virtual IEnumerator<ICoroutineInstruction> Stand(int time)
	{
		for (int t = 0; t < time; t++)
		{
			NPC.frame = StandFrame;
			NPC.velocity.X = 0;
			NPC.spriteDirection = NPC.direction;
			if (!Peace())
			{
				if (TotalThreatenDirection.X > 0)
				{
					NPC.direction = 1;
				}
				else
				{
					NPC.direction = -1;
				}
				break;
			}
			yield return new SkipThisFrame();
		}
		EndAIPiece();
	}

	/// <summary>
	/// Walking, include escape under duress, jump in front of a protruding tile, door opening(closing), check chair for sitting; Inheritable
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public virtual IEnumerator<ICoroutineInstruction> Walk(int time)
	{
		NPC.direction = ChooseDirection(NPC);
		for (int t = 0; t < time; t++)
		{
			// Too far from home will trigger teleportation.
			CheckWalkBound();
			if (TotalThreatenDirection.X > 0.32f)
			{
				NPC.direction = 1;
			}
			if (TotalThreatenDirection.X < -0.32f)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			float speed = 1.2f;
			if (TotalThreatenDirection.Length() > 0.5f)
			{
				speed += TotalThreatenDirection.Length() - 0.5f;
			}
			if (!CanEscape())
			{
				break;
			}
			WalkFrame();
			NPC.velocity.X = NPC.direction * speed;
			if (!CanContinueWalk(NPC))
			{
				break;
			}
			if (Talking)
			{
				break;
			}

			if (Main.rand.NextBool(24))
			{
				if (CheckSit(NPC))
				{
					Sit = true;
					break;
				}
			}
			TryOpenDoor(NPC);
			TryCloseDoor(NPC);
			yield return new SkipThisFrame();
		}
		NPC.velocity.X *= 0;
		EndAIPiece();
	}

	public virtual void CheckWalkBound()
	{
		if (SubworldSystem.Current is not YggdrasilWorld)
		{
			return;
		}
		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X < AnchorForBehaviorPos.X - 90)
		{
			NPC.direction = 1;
		}
		if (npcTilePos.X > AnchorForBehaviorPos.X + 90)
		{
			NPC.direction = -1;
		}
	}

	/// <summary>
	/// Only trigger when walking
	/// </summary>
	public virtual void WalkFrame()
	{
		NPC.frameCounter += Math.Abs(NPC.velocity.X);
		if (NPC.frameCounter > 4)
		{
			NPC.frame.Y += FrameHeight;
			NPC.frameCounter = 0;
		}
		if (NPC.frame.Y > 9 * FrameHeight)
		{
			NPC.frame.Y = 0;
		}
	}

	public override bool CheckActive()
	{
		return false;
	}

	public override bool CanChat()
	{
		return true;
	}

	public override string GetChat()
	{
		Talking = true;
		return base.GetChat();
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D texMain = ModAsset.Resturateur.Value;
		Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.height - NPC.frame.Height + 8) * 0.5f;
		Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		return false;
	}
}