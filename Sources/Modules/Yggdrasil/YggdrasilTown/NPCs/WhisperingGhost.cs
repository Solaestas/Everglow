using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class WhisperingGhost : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 5;
		YggdrasilTownNPC.RegisterYggdrasilTownNPC(Type);
	}
	public override void SetDefaults()
	{

		NPC.width = 81;
		NPC.height = 63;
		NPC.lifeMax = 40;
		NPC.damage = 12;
		NPC.defense = 4;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.15f;
		NPC.value = 60;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath4;
		NPC.noGravity = true;
		NPC.noTileCollide = true;


	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Silenced, 180);
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		YggdrasilTownBiome YggdrasilTownBiome = ModContent.GetInstance<YggdrasilTownBiome>();
		if (!YggdrasilTownBiome.IsBiomeActive(Main.LocalPlayer))
			return 0f;
		return 3f;
	}
	public override void AI()
	{
		NPC.velocity = new Vector2(0, -1);
		float timeValue = (float)(Main.time * 0.035f);
		Vector2 toAim = Vector2.Zero;
		NPC.TargetClosest();
		Player target = Main.player[NPC.target];
		Vector2 aimTarget = target.Center;
		if (Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
		{
            toAim = aimTarget - NPC.Center + new Vector2(0, MathF.Sin(timeValue) * 100);
			NPC.velocity = Vector2.Normalize(toAim) * NPC.velocity.Length()*1.2f;
			if (NPC.velocity.Length() >= 1.75f)
			{
				NPC.velocity = Vector2.Normalize(NPC.velocity) * 1.75f;
			}
		}
		else
		{
			toAim = new Vector2(MathF.Cos(0.5f* timeValue), MathF.Sin(0.75f* timeValue));
			NPC.velocity = Vector2.Normalize(toAim) * NPC.velocity.Length() * 0.9f;
			if (NPC.velocity.Length() <= 0.75f)
			{
				NPC.velocity = Vector2.Normalize(NPC.velocity) *0.75f;
			}
		}
		if(Collision.SolidTiles(NPC.position, NPC.width, NPC.height))
		{
			NPC.velocity += new Vector2(0, -0.1f);
		}

		


	}
	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void OnKill()
	{

	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.Common(ItemID.Megaphone,50));
	}
}
