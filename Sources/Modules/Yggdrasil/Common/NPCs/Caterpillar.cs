using Everglow.Commons.DataStructures;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public abstract class Caterpillar : ModNPC
{
	public override void SetStaticDefaults()
	{

	}
	public struct Segment
	{ 
		public float Rotation;
		public int Index;
		public bool Flip;
		public Vector2 SelfPosition;
	}

	public int SegmentCount = 10;
	public override void SetDefaults()
	{
		NPCID.Sets.DontDoHardmodeScaling[Type] = true;//必须要设置一个这个否则专家大师模式属性翻倍三倍

		NPC.width = 40;
		NPC.height = 40;
		NPC.lifeMax = 16;

		NPC.damage = 22;
		NPC.defense = 8;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.2f;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.noGravity = true;
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

	}
	public override void AI()
	{
		
	}
	public override void OnKill()
	{
	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{

	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		
		return false;
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot)
	{
		return base.CanHitPlayer(target, ref cooldownSlot);
	}
	public override bool? CanBeHitByProjectile(Projectile projectile)
	{
		return projectile.Colliding(projectile.Hitbox, NPC.Hitbox);
	}
}
