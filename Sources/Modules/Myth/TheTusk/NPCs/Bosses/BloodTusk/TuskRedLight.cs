using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class TuskRedLight : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙致命池");
	}
	private Vector2[] V = new Vector2[10];
	private Vector2[] VMax = new Vector2[10];
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 0;
		NPC.width = 10;
		NPC.height = 40;
		NPC.defense = 0;
		NPC.lifeMax = 5;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.aiStyle = -1;
		NPC.alpha = 255;
		NPC.lavaImmune = true;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.dontTakeDamage = true;
	}
	private int wait = 480;
	public override void AI()
	{
		if (wait == 480)
		{
			NPC.localAI[0] = 0;
			wait--;
		}
		wait--;
		if (wait == 0)
			NPC.active = false;
		V[3].Y += 1;
		VMax[0] = new Vector2(0, 84);
		NPC.TargetClosest(false);
		NPC.localAI[0] += 1;
		Player player = Main.player[NPC.target];
		if (NPC.velocity.Length() <= 0.5f && V[3].Y > 4)
		{
			if (V[3].Y > 3 && NPC.velocity.Length() < 5)
			{
				int k = Dust.NewDust(NPC.Bottom + new Vector2(Main.rand.Next(-100, 100), 0), 0, 0, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 2.3f));
				Main.dust[k].noGravity = true;
				int kk = Dust.NewDust(NPC.Bottom + new Vector2(Main.rand.Next(-100, 100), 0), 0, 0, DustID.VampireHeal, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 2.3f));
				Main.dust[kk].noGravity = true;
			}
		}
		if (NPC.localAI[0] % 30 == 8)
			NPC.NewNPC(null, (int)(NPC.Center.X + Main.rand.Next(-120, 120)), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
		if (NPC.localAI[0] % 20 == 0)
			NPC.NewNPC(null, (int)(NPC.Center.X + Main.rand.Next(-120, 120)), (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
		if (NPC.localAI[0] % 60 == 10)
		{
			int Ax = Main.rand.Next(-120, 120);
			int m = NPC.NewNPC(null, (int)NPC.Center.X + Ax, (int)NPC.Center.Y + 60, ModContent.NPCType<LittleMouth>());
			Main.npc[m].velocity = new Vector2(-Math.Sign(Ax) * Main.rand.NextFloat(2f, 5f), Main.rand.NextFloat(-8f, -5f));
		}
	}
	public override void OnHitPlayer(Player player, int damage, bool crit)
	{
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}
}
