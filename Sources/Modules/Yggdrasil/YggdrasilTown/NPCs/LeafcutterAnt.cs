using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class LeafcutterAnt : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 5;
		NPCSpawnManager.RegisterNPC(Type);
	}
	public override void SetDefaults()
	{

		NPC.width = 50;
		NPC.height = 30;
		NPC.lifeMax = 80;
		NPC.damage = 25;
		NPC.defense = 10;
		NPC.friendly = false;
		NPC.aiStyle = 3;
		NPC.knockBackResist = 0.5f;
		NPC.value = 100;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath4;
		AIType = NPCID.WalkingAntlion;
	}
	public override void FindFrame(int frameHeight)
	{
		if (NPC.ai[0] == 0f)
		{
			NPC.rotation = 0f;
			if (NPC.velocity.Y == 0f)
				NPC.spriteDirection = NPC.direction;
			else if (NPC.velocity.Y < 0f)
				NPC.frameCounter = 0;

			NPC.frameCounter += Math.Abs(NPC.velocity.X) * 1.1f;
			if (NPC.frameCounter < 6)
				NPC.frame.Y = 0;
			else if (NPC.frameCounter < 12)
				NPC.frame.Y = frameHeight;
			else if (NPC.frameCounter < 18)
				NPC.frame.Y = frameHeight * 2;
			else if (NPC.frameCounter < 24)
				NPC.frame.Y = frameHeight * 3;
			else if (NPC.frameCounter < 32)
				NPC.frame.Y = frameHeight * 4;
			else
				NPC.frameCounter = 0;
		}
		else if (NPC.ai[0] == 1f)
		{
			NPC.frameCounter = 0;
			if (NPC.ai[1] < 10f)
				NPC.frame.Y = frameHeight * 5;
			else if (NPC.ai[1] < 20f)
				NPC.frame.Y = frameHeight * 6;
			else
				NPC.frame.Y = frameHeight * 7;
		}
		else if (NPC.ai[0] == 5f)
		{
			NPC.frameCounter = 0;
			if (NPC.ai[1] < 10f)
				NPC.frame.Y = frameHeight * 7;
			else if (NPC.ai[1] < 20)
				NPC.frame.Y = frameHeight * 6;
			else
				NPC.frame.Y = frameHeight * 5;
		}
		else
		{
			NPC.frameCounter = 0;
			NPC.frame.Y = frameHeight * 7;
		}
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Poisoned, 600);
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
		NPC.scale = Main.rand.NextFloat(0.75f, 0.9f);
	}

	public override void OnKill()
	{

	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		//TODO 掉落物
	}
}
