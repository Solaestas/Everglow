using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using Everglow.Commons.NetUtils;
using Everglow.Commons.CustomTiles;
using Everglow.Yggdrasil.Common.Elevator.Tiles;
using static Terraria.NPC.NPCNameFakeLanguageCategoryPassthrough;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class CrimsonProtestant : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 6;
		NPCSpawnManager.RegisterNPC(Type);
	}
	public override void SetDefaults()
	{

		NPC.width = 54;
		NPC.height = 42;
		NPC.lifeMax = 200;
		NPC.damage = 60;
		NPC.defense = 8;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.5f;
		NPC.value = 100;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath4;

		AnimationType = NPCID.Tim;
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
	bool HasSpell=false;
	public override void AI()
	{

		NPC.TargetClosest();
		NPC.velocity.X *= 0.93f;
		if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
			NPC.velocity.X = 0f;


		if (NPC.ai[0] == 0f)
			NPC.ai[0] = 400;


		if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)
		{
			NPC.position += NPC.netOffset;;

			SoundEngine.PlaySound(SoundID.Item8, NPC.position);
			for (int i = 0; i < 50; i++)
			{

				int num71 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3));
				Dust dust = Main.dust[num71];
				dust.velocity *= 3f;
				if (Main.dust[num71].scale > 1f)
					Main.dust[num71].noGravity = true;

			}
			
			NPC.position -= NPC.netOffset;
			NPC.position.X = NPC.ai[2] * 16f - (float)(NPC.width / 2) + 8f;
			NPC.position.Y = NPC.ai[3] * 16f - (float)NPC.height;
			NPC.netOffset *= 0f;
			NPC.velocity.X = 0f;
			NPC.velocity.Y = 0f;
			NPC.ai[2] = 0f;
			NPC.ai[3] = 0f;
			HasSpell = false;
			SoundEngine.PlaySound(SoundID.Item8, NPC.position);
			for (int i = 0; i < 50; i++)
			{

				int num80 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3));
				Dust dust = Main.dust[num80];
				dust.velocity *= 3f;
				if (Main.dust[num80].scale > 1f)
					Main.dust[num80].noGravity = true;

			}
		}
		if (NPC.ai[0] >= 420 && Main.netMode != 1)
		{
			NPC.ai[0] = 0f;
			int targetTileX = (int)Main.player[NPC.target].Center.X / 16;
			int targetTileY = (int)Main.player[NPC.target].Center.Y / 16;
			Vector2 chosenTile = Vector2.Zero;
			if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
			{
				NPC.ai[1] = 15f;
				NPC.ai[2] = chosenTile.X;
				NPC.ai[3] = chosenTile.Y;
			}

			NPC.netUpdate = true;
		}

		NPC.ai[0]++;
		if (NPC.ai[1] > 0f)
		{
			NPC.ai[1] -= 1f;
			if (NPC.ai[1] == 0)
			{

				if (!HasSpell && Main.netMode != 1)
				{
					for (int i = 1; i <= 3; i++)
					{

						Vector2 Position = new Vector2(new Vector2(0, -50).RotatedBy(-Math.PI * i * 2 / 3).X,
													   new Vector2(0, -50).RotatedBy(-Math.PI * i * 2 / 3).Y);
						int Spell=NPC.NewNPC(NPC.GetSpawnSourceForProjectileNPC(), (int)(NPC.Center.X + (int)Position.X), (int)(NPC.Center.Y + Position.Y), ModContent.NPCType<CrimsonSpell>(), 0, NPC.whoAmI, 115 * i +60 );
						Main.npc[Spell].velocity = Vector2.Normalize(Position.RotatedBy(Math.PI/ 2))*3;
					}

					HasSpell = true;
					NPC.ai[1] = 115;

				}

			}
		}
		if (Main.rand.NextBool(5))
		{
			int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, 27, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default(Color), 1.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity.X *= 0.5f;
			Main.dust[dust].velocity.Y = -2f;
		}

		NPC.position -= NPC.netOffset;
		return;
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void OnKill()
	{

	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		//TODO 掉落物
	}
}
