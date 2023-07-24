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
    public class LavaStone2 : ModNPC
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
			base.npc.width = 100;
			base.npc.height = 100;
			base.npc.defense = 120;
			base.npc.lifeMax = 9000;
			base.npc.knockBackResist = 0;
			base.npc.alpha = 0;
			base.npc.lavaImmune = true;
			base.npc.noGravity = true;
			base.npc.noTileCollide = true;
            base.npc.aiStyle = -1;
            npc.boss = false;
        }
        private int Xa = 0;
        public override void AI()
        {
            if (npc.ai[3] <= 0)
            {
                if (Xa == 0)
                {
                    Xa = Main.rand.Next(1, 799);
                }
                num1 += 1;
                Xa += 1;
            }
            else
            {
                npc.ai[3] -= 1;
                npc.velocity += new Vector2(npc.ai[1], npc.ai[2]);
            }
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
                if(Xa < 600 && Xa > 0)
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
                if(Xa > 800)
                {
                    Xa = 0;
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
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/VolCano/熔岩巨石怪2Glow").Width, (float)(base.mod.GetTexture("NPCs/VolCano/熔岩巨石怪2Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.npc.alpha, 97 - base.npc.alpha, 97 - base.npc.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/VolCano/熔岩巨石怪2Glow"), vector2, new Rectangle?(base.npc.frame), color, base.npc.rotation, vector, 1f, effects, 0f);
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
                for (int i = 0; i < 10; i++)
                {
                    float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                    Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor5, base.mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 5 + 1).ToString()), 1f);
                }
                for (int i = 0; i < 5; i++)
                {
                    float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                    Gore.NewGore(base.npc.position, base.npc.velocity * scaleFactor5, base.mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 2 + 6).ToString()), 1f);
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("火山浮石"), 0, 0, Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.Next(0, 60), 255);
                }
                if (NPC.CountNPCS(mod.NPCType("熔岩巨石怪2")) <= 1)
                {
                    Color messageColor = Color.MediumPurple;
                    Main.NewText(Language.GetTextValue("熔岩巨石怪已被打败!"), messageColor);
                    if (Main.expertMode)
                    {
                        Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, mod.ItemType("LavaStoneTreasureBag"), 1, false, 0, false, false);
                    }
                    if (Main.maxTilesX == 4200)
                    {
                        for (int i = (int)(Main.maxTilesX * 0.62f + 445); i < (int)(Main.maxTilesX * 0.62f + 455); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 110); j < (int)(Main.maxTilesY * 0.67f - 60); j++)
                            {
                                if (Main.tile[i, j].type == mod.TileType("LavaStone") || Main.tile[i, j].type == mod.TileType("Basalt"))
                                {
                                    WorldGen.KillTile(i, j, false, false, true);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int i = (int)(Main.maxTilesX * 0.665f + 445); i < (int)(Main.maxTilesX * 0.665f + 455); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 140); j < (int)(Main.maxTilesY * 0.67f - 90); j++)
                            {
                                if (Main.tile[i, j].type == mod.TileType("LavaStone") || Main.tile[i, j].type == mod.TileType("Basalt"))
                                {
                                    WorldGen.KillTile(i, j, false, false, true);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int i = (int)(Main.maxTilesX * 0.69f + 445); i < (int)(Main.maxTilesX * 0.69f + 455); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 180); j < (int)(Main.maxTilesY * 0.67f - 120); j++)
                            {
                                if (Main.tile[i, j].type == mod.TileType("LavaStone") || Main.tile[i, j].type == mod.TileType("Basalt"))
                                {
                                    WorldGen.KillTile(i, j, false, false, true);
                                }
                            }
                        }
                    }
                    if (!MythWorld.downedVol)
                    {
                        MythWorld.downedVol = true;
                        Color messageColor2 = Color.OrangeRed;
                        Main.NewText(Language.GetTextValue("火山密室顶部崩塌"), messageColor2);
                    }
                }
            }
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		}
	}
}
