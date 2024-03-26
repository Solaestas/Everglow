using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles;
using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Everglow.Commons.Skeleton2D;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class Bulbling : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 8;
		NPCSpawnManager.RegisterNPC(Type);
	}
	public override void SetDefaults()
	{
		NPC.width = 16;
		NPC.height = 16;
		NPC.aiStyle = -1;
		NPC.damage = 13;
		NPC.defense = 2;
		NPC.lifeMax = 24;
		NPC.HitSound = SoundID.NPCHit3;
		NPC.DeathSound = SoundID.NPCDeath3;
		NPC.noGravity = true;
		NPC.noTileCollide = false;
		NPC.knockBackResist = 1f;
		NPC.value = 90;

	}
	public override void OnSpawn(IEntitySource source)
	{
		State = (int)NPCState.Sleep;
	}
	int State;

	private enum NPCState
	{
		Sleep,
		Dash,
		Rest,
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		YggdrasilTownBiome YggdrasilTownBiome = ModContent.GetInstance<YggdrasilTownBiome>();
		if (!YggdrasilTownBiome.IsBiomeActive(Main.LocalPlayer))
			return 0f;
		return 3f;
	}

	public override void FindFrame(int frameHeight)
	{

		switch (State)
		{
			case (int)NPCState.Dash:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 5 % 8) * frameHeight;
					break;
				}
			case (int)NPCState.Sleep:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 5 % 7 + 1) * frameHeight;
					break;
				}
			case (int)NPCState.Rest:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 5 % 7 + 1) * frameHeight;
					break;
				}
		}

	}
	public override void AI()
	{
		switch (State)
		{
			case (int)NPCState.Sleep:
				{
					NPC.frameCounter++;
					NPC.TargetClosest();
					NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(0, -0.001f), 0.5f);
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
					if (NPC.HasValidTarget && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1)
							 && Main.player[NPC.target].Distance(NPC.Center) <= 750)
					{
						State = (int)NPCState.Dash;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
					}
					break;
				}
			case (int)NPCState.Dash:
				{
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center;
					NPC.velocity = Vector2.Normalize(toAim) * 10;
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
					
					NPC.frameCounter++;
					if (NPC.frameCounter>=5)
					{
						State = (int)NPCState.Rest;
						NPC.frameCounter = 0;
					}

					break;
				}
			case (int)NPCState.Rest:
				{
					NPC.velocity *= 0.96f;
					NPC.frameCounter++;
					if (NPC.velocity.Length() <= 0.01f)
					{
						NPC.velocity *= 0;
						State = (int)NPCState.Dash;
						NPC.frameCounter=0;
						NPC.TargetClosest();
						if (NPC.HasValidTarget && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1)
										   && Main.player[NPC.target].Distance(NPC.Center) > 1000)
						{
							State = (int)NPCState.Sleep;
							NPC.frameCounter = 0;
						}
					}
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
					break;
				}


		}

	}
	public override void OnKill()
	{

	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		//TODO 掉落物
	}
}