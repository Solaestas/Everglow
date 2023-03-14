using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class CrimsonTusk1 : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
	}
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 50;
		if (Main.expertMode)
			NPC.damage = 75;
		if (Main.masterMode)
			NPC.damage = 125;
		NPC.width = 30;
		NPC.height = 30;
		NPC.defense = 0;
		NPC.lifeMax = 5;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.aiStyle = -1;
		NPC.alpha = 0;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.dontTakeDamage = true;
	}
	private bool Stop = false;
	private bool Start = true;
	private int St = 60;
	public override void AI()
	{
		if (Start)
		{
			NPC.velocity = new Vector2(0, -0.5f).RotatedBy(NPC.ai[0] * NPC.ai[1] / 4d);
			NPC.frame = new Rectangle(0, 0, 14, 0);
			Start = false;
		}
		NPC.localAI[0] += 1;
		if (NPC.localAI[0] <= 124)
			NPC.frame = new Rectangle(0, 0, 14, (int)(NPC.localAI[0] / 2f));
		else
		{
			if (NPC.velocity.Length() < 10)
				NPC.velocity *= 45f;
			else
			{
				St--;
				if (St < 0)
					NPC.noTileCollide = false;
			}
		}
		if (NPC.collideX || NPC.collideY)
		{
			NPC.noTileCollide = true;
			NPC.velocity = NPC.oldVelocity;
			Stop = true;
		}
		else
		{
			NPC.rotation = (float)(Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + Math.PI / 2d);
		}
		if (Stop)
		{

			if (NPC.velocity.Length() < 0.01f)
			{
				NPC.alpha += 15;
				if (NPC.alpha > 240)
					NPC.active = false;
			}
			else
			{
				NPC.velocity *= 0.5f;
			}
		}
	}
}
