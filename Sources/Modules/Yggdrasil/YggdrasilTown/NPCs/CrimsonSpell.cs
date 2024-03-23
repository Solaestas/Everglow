using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class CrimsonSpell : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 6;
		NPCSpawnManager.RegisterNPC(Type);
	}
	public override void SetDefaults()
	{
		NPC.width = 16;
		NPC.height = 16;
		NPC.aiStyle = -1;
		NPC.damage = 30;
		NPC.defense = 0;
		NPC.lifeMax = 1;
		NPC.HitSound = SoundID.NPCHit3;
		NPC.DeathSound = SoundID.NPCDeath3;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.knockBackResist = 0f;
		NPC.alpha = 255;
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
	float Master;
	float Timer;
	public override void OnSpawn(IEntitySource source)
	{
		
		Timer  = NPC.ai[1];
	}
	public override bool? CanBeHitByProjectile(Projectile projectile)
	{
		if (NPC.ai[1] > 0)
			return false;
		return base.CanBeHitByProjectile(projectile);
	}
	public override bool CanBeHitByNPC(NPC attacker)
	{
		if (NPC.ai[1] > 0)
			return false;
		return base.CanBeHitByNPC(attacker);
	}
	public override bool? CanBeHitByItem(Player player, Item item)
	{
		if (NPC.ai[1] > 0)
			return false;
		return base.CanBeHitByItem(player, item);
	}
	public override void AI()
	{
		NPC.ai[1]--;
		int num80 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 27, 0f, 0f, 100, default(Color), Main.rand.Next(1, 3));
		Dust dust = Main.dust[num80];
		dust.velocity *= 0f;
		if (Main.dust[num80].scale > 1f)
			Main.dust[num80].noGravity = true;
		if (NPC.ai[1] > 0)
		{

			
			NPC Master = Main.npc[(int)NPC.ai[0]];
			Vector2 unit = Vector2.Normalize(Master.Center - NPC.Center) * 9 / 50;
			NPC.velocity += unit;
		}
		else if (NPC.ai[1] == 0)
		{
			NPC.TargetClosest();
			NPC.velocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 4;
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
