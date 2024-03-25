using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class LightBulb : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 12;
		NPCSpawnManager.RegisterNPC(Type);
	}

	public override void SetDefaults()
	{
		NPC.width = 90;
		NPC.height = 90;
		NPC.lifeMax = 30;
		NPC.damage = 8;
		NPC.defense = 2;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.value = 100;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath4;

	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Poisoned, 600);
	}
	public override void FindFrame(int frameHeight)
	{
		switch (State)
		{
			case (int)NPCState.Sleep:
				{

					NPC.frame.Y = (int)(NPC.frameCounter / 8 % 4 + 8) * frameHeight;
					break;
				}
			case (int)NPCState.charge:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 6 % 12) * frameHeight;
					break;
				}
			case (int)NPCState.Attack:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 6 % 12) * frameHeight;
					break;
				}
			case (int)NPCState.Cooldown:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 8 % 4 + 8) * frameHeight;
					break;
				}

		}
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		YggdrasilTownBiome YggdrasilTownBiome = ModContent.GetInstance<YggdrasilTownBiome>();
		if (!YggdrasilTownBiome.IsBiomeActive(Main.LocalPlayer))
			return 0f;
		return 3f;
	}

	public override void OnSpawn(IEntitySource source)
	{
		State = (int)NPCState.Sleep;
	}
	int State;

	private enum NPCState
	{
		Sleep,
		charge,
		Attack,
		Cooldown
	}

	public override void AI()
	{
		Lighting.AddLight(NPC.Center, 0.6f, 0.6f, 0);
		switch (State)
		{
			case (int)NPCState.Sleep:
				{
					NPC.frameCounter++;
					NPC.TargetClosest();
					NPC.ai[0] = 0;
					if (NPC.HasValidTarget && Main.player[NPC.target].Distance(NPC.Center) <= 750)
					{
						State = (int)NPCState.charge;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;

					}
					break;
				}
			case (int)NPCState.charge:
				{
					NPC.frameCounter++;
					NPC.ai[0]++;
					if ((NPC.ai[0] % 18) == 0)
					{
						State = (int)NPCState.Attack;
						NPC.ai[0] = 0;
					}
					break;
				}
			case (int)NPCState.Attack:
				{
					if (NPC.ai[0] == 0)
					{
						for (int i = 0; i < 6; i++)
						{
							Vector2 v = new Vector2(5f, 0);
							v = v.RotatedBy(-Math.PI * i / 5).RotatedByRandom(Math.PI / 10);
							if (v.Y > 0)
							{
								v.Y = -v.Y;
							}
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 40), v, ModContent.ProjectileType<LightSeed>(), 6, 0);
						}
					}
					NPC.ai[0]++;
					NPC.frameCounter++;
					NPC.TargetClosest();

					if ((NPC.ai[0] % 30) == 0)
					{
						State = (int)NPCState.Cooldown;
					}
					break;
				}
			case (int)NPCState.Cooldown:
				{
					NPC.frameCounter++;
					NPC.ai[0]++;

					NPC.TargetClosest();
					if (NPC.HasValidTarget && Main.player[NPC.target].Distance(NPC.Center) > 750 || (NPC.ai[0] % 160) == 0)
					{
						State = (int)NPCState.Sleep;
						NPC.ai[0] = 0;
					}
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
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D glow = ModAsset.LightBulb_glow.Value;
		spriteBatch.Draw(glow, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, NPC.frame.Y, 160, 90), new Color(1f, 1f, 1f, 0), NPC.rotation, new Vector2(80, 45), NPC.scale, SpriteEffects.None, 0);
		base.PostDraw(spriteBatch, screenPos, drawColor);
	}
}
