using Everglow.Myth.Common;
using Terraria.Localization;

namespace Everglow.Myth.TheFirefly.NPCs.Bosses;

public class MothSummonEffect : ModNPC
{
	private bool Start = false;
	private Vector2 Cent;
	private Vector2 Acc;
	private float Ome = 0;
	private float kx = 1;
	private int AimN = -1;
	public override string Texture => "Terraria/Images/NPC_0";

	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("");
			}

	public override void SetDefaults()
	{
		NPC.width = 10;
		NPC.height = 10;
		NPC.aiStyle = -1;
		NPC.friendly = false;
		NPC.damage = 0;
		NPC.behindTiles = true;
		NPC.aiStyle = -1;
		NPC.alpha = 0;
		NPC.lifeMax = 1;
		NPC.dontTakeDamage = true;
		NPC.dontCountMe = true;
		NPC.scale = 1f;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPCID.Sets.TrailingMode[NPC.type] = 0;
		NPCID.Sets.TrailCacheLength[NPC.type] = 40;
	}

	public override void AI()
	{
		if (++NPC.ai[0] > 180)
			NPC.active = false;

		if (!Start)
		{
			NPC.velocity = new Vector2(Main.rand.NextFloat(0, 10f), 0).RotatedByRandom(6.28);
			Acc = new Vector2(Main.rand.NextFloat(0, 0.35f), 0).RotatedByRandom(6.28);
			Cent = NPC.Center;
			NPC.position += new Vector2(0, Main.rand.NextFloat(350f, 500f)).RotatedByRandom(6.28);
			Ome = Main.rand.NextFloat(-0.16f, 0.16f);
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].type == ModContent.NPCType<CorruptMoth>())
				{
					if (i > NPC.whoAmI)
					{
						NPC.active = false;
						break;
					}
				}
				/*
                    if (Main.npc[i].type == ModContent.NPCType<NPCs.CorruptMoth.EvilPackBreak>())
                    {
                        NPC.position = new Vector2(0, -Main.rand.NextFloat(0f, 30f)).RotatedByRandom(6.28) + Main.npc[i].Center;
                        break;
                    }*/
			}
			if (AimN == -1)
			{
				for (int f = 0; f < 200; f++)
				{
					if (Main.npc[f].type == ModContent.NPCType<CorruptMoth>())
					{
						AimN = f;
						break;
					}
				}
			}
			Start = true;
		}
		if (AimN != -1)
			Cent = Main.npc[AimN].Center;
		Vector2 v0 = Cent - NPC.Center;
		if (v0.Length() >= 15)
		{
			Vector2 v = Cent - (NPC.Center + NPC.velocity * 30);
			Vector2 v2 = v / v.Length() * 0.05f * (float)(1 + Math.Log(v.Length() + 1));

			Acc *= 0.95f;
			NPC.velocity += Acc + v2;
			NPC.velocity = NPC.velocity.RotatedBy(Ome);
			Ome *= 0.96f;
			kx = 20 - v0.Length() / 12f;
			if (kx < 1)
				kx = 1;
		}
		else
		{
			NPC.velocity *= 0.8f;
			kx--;
			if (kx <= 1)
			{
				NPC.active = false;
				;
			}
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return false;
	}

	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var bars = new List<Vertex2D>();
		int width = (int)(2 * kx);
		for (int i = 1; i < NPC.oldPos.Length - 1; ++i)
		{
			if (NPC.oldPos[i] == Vector2.Zero)
				break;

			var normalDir = NPC.oldPos[i - 1] - NPC.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)NPC.oldPos.Length;
			var color = new Color(0, 70, 255, 0);

			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(NPC.oldPos[i] + normalDir * width + new Vector2(4, 35) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
			bars.Add(new Vertex2D(NPC.oldPos[i] + normalDir * -width + new Vector2(4, 35) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
		}

		var triangleList = new List<Vertex2D>();

		if (bars.Count > 2)
		{
			triangleList.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(NPC.velocity) * 5, Color.White, new Vector3(0, 0.5f, 1));
			triangleList.Add(bars[1]);
			triangleList.Add(vertex);
			for (int i = 0; i < bars.Count - 2; i += 2)
			{
				triangleList.Add(bars[i]);
				triangleList.Add(bars[i + 2]);
				triangleList.Add(bars[i + 1]);

				triangleList.Add(bars[i + 1]);
				triangleList.Add(bars[i + 2]);
				triangleList.Add(bars[i + 3]);
			}
			Texture2D t = MythContent.QuickTexture("TheFirefly/NPCs/Bosses/MeteroD");
			Main.graphics.GraphicsDevice.Textures[0] = t;
			if (triangleList.Count > 3)
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
		}
	}
}