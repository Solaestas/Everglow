using Everglow.Commons.CustomTiles;
using Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk_New.DTiles;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk_New;


public class TuskWall : ModNPC
{
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 0;
		NPC.width = 1;
		NPC.height = 1;
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
	//ai0:高度
	public override void AI()
	{
		if (NPC.ai[2] == 0)
		{
			NPC.ai[2] = 1;
			TileSystem.Instance.AddTile(new WallDBlock { npc = NPC, id = 0, dir = NPC.direction });
			TileSystem.Instance.AddTile(new WallDBlock { npc = NPC, id = 1, dir = NPC.direction });
		}

		if (NPC.ai[0] < 880)
		{
			for (int h = 0; h < 20; h++)
			{
				int k = Dust.NewDust(NPC.Center + new Vector2(-150, -20), 300, 30, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 3.3f));
				Main.dust[k].noGravity = true;
				Main.dust[k].velocity = new Vector2(0, -Main.rand.NextFloat(0f, 5f));
			}
		}

		if (NPC.ai[1] < 60)
			NPC.ai[1]++;
		else
		{
			if (NPC.ai[0] < 880)
			{
				TuskModPlayer.ScreenShake(10, NPC.Center);
				NPC.ai[0] += 10;
			}
		}
	}
	public override bool CheckActive()
	{
		return false;
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		SpriteEffects spriteEffects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
		Color color = NPC.GetAlpha(drawColor);
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/TuskWallLeft").Value;
		Main.spriteBatch.Draw(t, NPC.Center + new Vector2(0, 880 - NPC.ai[0] + 30) - Main.screenPosition, new Rectangle(0, 0, t.Width, (int)NPC.ai[0]), color, NPC.rotation, new Vector2(t.Width / 2f, t.Height), 1f, spriteEffects, 0f);
		return false;
	}
}
