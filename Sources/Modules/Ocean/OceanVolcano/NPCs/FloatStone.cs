using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	public class FloatStone : ModNPC
	{
		private int num1 = 0;
		private int num2;
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("火山浮石");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "火山浮石");
		}
		public override void SetDefaults()
		{
			base.NPC.damage = 90;
			base.NPC.width = 50;
			base.NPC.height = 50;
			base.NPC.defense = 65;
			base.NPC.lifeMax = 4000;
			base.NPC.knockBackResist = 0.4f;
			base.NPC.alpha = 0;
			base.NPC.lavaImmune = true;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = true;
            base.NPC.aiStyle = -1;
        }
		public override void AI()
		{
			num1 += 1;
            Player player = Main.player[Main.myPlayer];
            Vector2 playerposition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
			float num4 = player.position.X - vector.X;
            float num5 = player.position.Y - vector.Y;
            float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
            if(NPC.ai[3] <= 0)
            {
                NPC.velocity += new Vector2(num4, num5) / num6 * 0.1f;
                if (NPC.velocity.Length() < 3)
                {
                    NPC.velocity *= 1.05f;
                }
                if (NPC.velocity.Length() > 5)
                {
                    NPC.velocity *= 0.95f;
                }
            }
            else
            {
                NPC.ai[3] -= 1;
                NPC.velocity += new Vector2(NPC.ai[1], NPC.ai[2]);
            }
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneVolcano)
			{
				return 0.6f;
			}
            else
            {
                return 0f;
            }
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
            Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/火山浮石发光部分").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/火山浮石发光部分").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.NPC.alpha, 97 - base.NPC.alpha, 97 - base.NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/火山浮石发光部分"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
		public override void HitEffect(NPC.HitInfo hit)
        {
			SoundEngine.PlaySound(SoundID.Item10, new Vector2(base.NPC.position.X, base.NPC.position.Y));
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            for (int j = 0; j < 3; j++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.NPC.life <= 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
				for (int j = 0; j < 25; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
                float scaleFactor = (float)(Main.rand.Next(-8, 8) / 100f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块1"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块2"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块3"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块4"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                int num3 = 0;
            }
        }
        public override void OnKill()
        {
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.FlameEdge>(), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.LavaStone>(), Main.rand.Next(1, 4), false, 0, false, false);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
		}
	}
}
