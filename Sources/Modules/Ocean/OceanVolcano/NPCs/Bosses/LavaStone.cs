using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs.VolCano
{
    [AutoloadBossHead]
    public class LavaStone : ModNPC
	{
		private int num1 = 0;
		private int num2;
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔岩巨石怪");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "熔岩巨石怪");
		}
		public override void SetDefaults()
		{
			base.npc.damage = 120;
			base.npc.width = 212;
			base.npc.height = 186;
			base.npc.defense = 150;
			base.npc.lifeMax = 90000;
			base.npc.knockBackResist = 0;
			base.npc.alpha = 0;
			base.npc.lavaImmune = true;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
            base.npc.aiStyle = -1;
            npc.boss = true;
        }
        private int Xa = 0;
        private bool st = true;
        private Vector2[] v = new Vector2[16];
        private bool canDespawn;
        public override bool CheckActive()
        {
            return this.canDespawn;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Main.myPlayer];
            if (!player.active || player.dead)
            {
                canDespawn = true;
            }
            else
            {
                canDespawn = false;
            }
            if (npc.life < npc.lifeMax * 0.5)
            {
                if (st)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                        Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor5, base.mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 5 + 1).ToString()), 1f);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                        Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor5, base.mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 2 + 6).ToString()), 1f);
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("LavaStone2"), 0, 0, Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.Next(0, 60), 255);
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("火山浮石"), 0, 0, Main.rand.NextFloat(-0.15f,0.15f), Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.Next(0,60), 255);
                    }
                    st = false;
                }
                int li = 0;
                for(int u = 0;u < 200;u++)
                {
                    if(Main.npc[u].type == mod.NPCType("LavaStone2"))
                    {
                        li += Main.npc[u].life;
                    }
                }
                npc.life = li;
                npc.position = player.position + new Vector2(0, 2000);
                return false;
            }
            return true;
        }
        public override void AI()
        {
            if (npc.life >= npc.lifeMax * 0.5)
            {
                num1 += 1;
                Xa += 1;
                MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
                Player player = Main.player[Main.myPlayer];
                Vector2 playerposition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                Vector2 vector = new Vector2(base.npc.position.X + (float)base.npc.width * 0.5f, base.npc.position.Y + (float)base.npc.height * 0.5f);
                if (!mplayer.ZoneVolcano)
                {
                    float num4 = player.position.X - vector.X;
                    float num5 = player.position.Y - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    npc.velocity += new Vector2(num4, num5) / num6 * 0.1f;
                    if (npc.velocity.Length() < 15)
                    {
                        npc.velocity *= 1.05f;
                    }
                    if (npc.velocity.Length() > 17)
                    {
                        npc.velocity *= 0.95f;
                    }
                    npc.velocity.Y += 1f;
                    npc.damage = 6000;
                }
                else
                {
                    if (Xa < 600)
                    {
                        float num4 = player.position.X - vector.X;
                        float num5 = player.position.Y - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        npc.velocity += new Vector2(num4, num5) / num6 * 0.04f * ((npc.Center - player.Center).Length() / 300 + 1);
                        if (npc.velocity.Length() < 9)
                        {
                            npc.velocity *= 1.01f;
                        }
                        if (npc.velocity.Length() > 11)
                        {
                            npc.velocity *= 0.98f;
                        }
                        npc.damage = 120;
                    }
                    if (Xa >= 600 && Xa < 800)
                    {
                        float num4 = player.position.X - vector.X;
                        float num5 = (player.position.Y - 100) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        npc.velocity += new Vector2(num4, num5) / num6 * 0.04f * ((npc.Center - (player.Center + new Vector2(0, -100))).Length() / 30 + 1);
                        if (npc.velocity.Length() < 2)
                        {
                            npc.velocity *= 1.01f;
                        }
                        if (npc.velocity.Length() > 3)
                        {
                            npc.velocity *= 0.98f;
                        }
                        npc.damage = 120;
                    }
                    if (Xa >= 800 && Xa < 1400)
                    {
                        if (Xa == 801)
                        {
                            for (int l = 0; l < 7; l++)
                            {
                                v[l] = new Vector2(Main.rand.Next(1, 80), 0).RotatedByRandom(Math.PI * 2);
                            }
                        }
                        float num4 = player.position.X - vector.X;
                        float num5 = (player.position.Y - 100) - vector.Y;
                        float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                        npc.velocity += new Vector2(num4, num5) / num6 * 0.04f * ((npc.Center - (player.Center + new Vector2(0, -100))).Length() / 30 + 1);
                        if (npc.velocity.Length() < 0)
                        {
                            npc.velocity *= 1.01f;
                        }
                        if (npc.velocity.Length() > 0.05)
                        {
                            npc.velocity *= 0.98f;
                        }
                        for (int l = 0; l < 7; l++)
                        {
                            if (Xa % 6 == 0)
                            {
                                Vector2 v2 = new Vector2(v[l].X, v[l].Y) / v[l].Length() * 6f;
                                int u = Projectile.NewProjectile(npc.Center.X + v[l].X, npc.Center.Y + v[l].Y, v2.X, v2.Y, mod.ProjectileType("烟火"), 100, 2f, Main.myPlayer, 0f, 1);
                                Main.projectile[u].friendly = false;
                                Main.projectile[u].timeLeft = 30;
                            }
                        }
                        npc.damage = 120;
                    }
                    if (Xa > 1400)
                    {
                        Xa = 0;
                    }
                }
            }
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.npc.Center.X, base.npc.Center.Y);
            Vector2 vector = new Vector2((float)(Main.npcTexture[base.npc.type].Width / 2), (float)(Main.npcTexture[base.npc.type].Height / Main.npcFrameCount[base.npc.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/VolCano/熔岩巨石怪Glow").Width, (float)(base.mod.GetTexture("NPCs/VolCano/熔岩巨石怪Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.npc.alpha, 97 - base.npc.alpha, 97 - base.npc.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/VolCano/熔岩巨石怪Glow"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
        }
		public override void HitEffect(int hitDirection, double damage)
        {
			Main.PlaySound(2, (int)base.npc.position.X, (int)base.npc.position.Y, 10, 1f, 0f);
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            for (int j = 0; j < 3; j++)
            {
                Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.npc.life <= 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
				for (int j = 0; j < 25; j++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
                float scaleFactor = (float)(Main.rand.Next(-8, 8) / 100f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块1"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块2"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块3"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块4"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/火山浮石碎块5"), 1f);
                int num3 = 0;
            }
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		}
	}
}
