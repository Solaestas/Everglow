using Terraria.Localization;

namespace Everglow.Myth.Bosses.Acytaea.NPCs;

[AutoloadBossHead]
public class FakeAcytaea : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Acytaea");
		/*Main.npcFrameCount[NPC.type] = 50;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;*/
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雅斯塔亚");
	}

	private bool canDespawn = false;

	public override bool CheckActive()
	{
		return canDespawn;
	}

	public override void SetDefaults()
	{
		NPC.friendly = false;
		NPC.width = 40;
		NPC.height = 56;
		NPC.aiStyle = -1;
		NPC.damage = 0;
		NPC.defense = 100;
		NPC.lifeMax = 275000;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.knockBackResist = 0.5f;
		NPC.friendly = false;
		NPC.dontTakeDamage = true;
		NPC.noTileCollide = true;
		NPC.boss = true;
	}

	private int AIMNPC = -1;

	public override void AI()
	{
		if (AIMNPC == -1)
		{
			for (int f = 0; f < 200; f++)
			{
				if (Main.npc[f].type == ModContent.NPCType<Acytaea>())
				{
					AIMNPC = f;
					break;
				}
			}
		}
		if (AIMNPC != -1)
		{
			NPC.position = Main.npc[AIMNPC].position;
			NPC.life = Main.npc[AIMNPC].life;
			NPC.lifeMax = Main.npc[AIMNPC].lifeMax;
			if (Main.npc[AIMNPC].life <= 0 || Main.npc[AIMNPC].active == false || Main.npc[AIMNPC].type != ModContent.NPCType<Acytaea>() || !Main.npc[AIMNPC].boss || Acytaea.BossIndex == 0)
			{
				if (NPC.active)
					NPC.active = false;
			}
		}
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return NPC.AnyNPCs(NPC.type) ? 0f : 0f;
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
}