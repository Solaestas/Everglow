using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk
{
	public class CrimsonTusk : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
		}
		public override void SetDefaults()
		{
			NPC.behindTiles = true;
			NPC.damage = 20;
			if (Main.expertMode)
				NPC.damage = 50;
			if (Main.masterMode)
				NPC.damage = 80;
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
			NPC.noTileCollide = false;
			NPC.dontTakeDamage = true;
		}
		private bool Stop = false;
		public override void AI()
		{
			NPC.velocity.Y += 0.15f;
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
}
