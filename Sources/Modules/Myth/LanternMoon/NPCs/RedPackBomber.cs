using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.NPCs;


public class RedPackBomber : ModNPC
{
	private bool A = true;
	private int num10;
	private int num11;
	private int num12;
	private int num13;
	private int num14;
	private int num15;
	private int num16;
	private int num17;
	private int num18;
	private int num19;
	private int num20;
	private int num21;
	private int num22;
	private int num23;
	private int num24;
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Redpack Bomber");
		Main.npcFrameCount[NPC.type] = 4;
		
	}
	public override void SetDefaults()
	{
		NPC.damage = 100;
		NPC.lifeMax = 300;
		NPC.npcSlots = 14f;
		NPC.width = 62;
		NPC.height = 74;
		NPC.defense = 0;
		NPC.value = 4000;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.6f;
		NPC.dontTakeDamage = false;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit3;
	}
	private int A2 = 0;
	public override void AI()
	{
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		NPC.rotation = NPC.velocity.X / 30f;
		A2 += 1;
		Vector2 v = player.Center + new Vector2((float)Math.Sin(A2 / 60f) * 1000f, (float)Math.Sin((A2 + 200) / 60f) * 50f - 300) - NPC.Center;
		if (NPC.velocity.Length() < 14f)
			NPC.velocity += v / v.Length() * 0.5f;
		NPC.velocity *= 0.96f;
		NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
		/*if(Math.Abs(NPC.velocity.X) > 5 && A2 % 32 == 1)
            {
                Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y + 30f, NPC.velocity.X / 3f, NPC.velocity.Y * 0.25f + 1.5f, mod.ProjectileType("红包"), 45, 0f, Main.myPlayer, 0f, 0f);
            }*/
		if (Main.dayTime)
		{
			NPC.noTileCollide = true;
			NPC.velocity.Y += 1;
		}
		if (A2 % 32 < 8)
			NPC.frame.Y = 0;
		if (A2 % 32 >= 8 && A2 % 32 < 16)
			NPC.frame.Y = 36;
		if (A2 % 32 >= 16 && A2 % 32 < 24)
			NPC.frame.Y = 72;
		if (A2 % 32 >= 24)
			NPC.frame.Y = 108;
	}
	public override void OnHitPlayer(Player player, int damage, bool crit)
	{
	}
	public override void HitEffect(int hitDirection, double damage)
	{

	}
}
