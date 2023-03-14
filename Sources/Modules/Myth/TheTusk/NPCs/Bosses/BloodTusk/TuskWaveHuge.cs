using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class TuskWaveHuge : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Tusk Spike");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙潜伏");
	}
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

	private int wait = 450;
	private float heig = 1;
	public override void AI()
	{
		if (NPC.velocity.Length() < 0.5f)
		{
			wait--;
			heig = 1;
			if (wait > 390)
				heig = (450 - wait) / 60f;
			if (wait < 60)
				heig = wait / 60f;
			if (wait <= 0)
				NPC.active = false;
			if (wait == 100)
			{
				if (NPC.CountNPCS(ModContent.NPCType<HugeMouth>()) == 0)
					SoundEngine.PlaySound(SoundID.Roar, NPC.Bottom);
				int m = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y + 150, ModContent.NPCType<HugeMouth>());
				Main.npc[m].velocity = new Vector2(NPC.ai[0] * Main.rand.NextFloat(11f, 12f), NPC.ai[1] * Main.rand.NextFloat(-16f, -14f));
				TuskModPlayer tmplayer = Main.player[Main.myPlayer].GetModPlayer<TuskModPlayer>();
				tmplayer.Shake = 4; // 摇
			}
		}
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var BackBase = new List<VertexBase.CustomVertexInfo>();
		Color color = Lighting.GetColor((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16));
		float index = 1;
		for (int x = -300; x < 300; x += 4)
		{
			float DelY = Math.Clamp((float)(Math.Sin(wait / 18d + x / 16d) * 0.9f + Math.Sin(wait / 3.6d + x / 8d) * 0.7f), -1, 2) * 5 + 10;//复合的正弦函数
			if (x < -260)
				index = (x + 300) / 40f;
			if (x > 260)
				index = (300 - x) / 40f;
			BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(x, 0) - Main.screenPosition, color, new Vector3(0, 1, 0)));
			BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(x + 4, -DelY * index * heig) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(x + 4, 0) - Main.screenPosition, color, new Vector3(1, 1, 0)));


			BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(x, -DelY * index * heig) - Main.screenPosition, color, new Vector3(0, 0, 0)));
			BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(x + 4, -DelY * index * heig) - Main.screenPosition, color, new Vector3(1, 0, 0)));
			BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(x, 0) - Main.screenPosition, color, new Vector3(0, 1, 0)));

		}
		Texture2D thang = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/BloodWave").Value;
		Main.graphics.GraphicsDevice.Textures[0] = thang;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, BackBase.ToArray(), 0, BackBase.Count / 3);
		return false;
	}
}

