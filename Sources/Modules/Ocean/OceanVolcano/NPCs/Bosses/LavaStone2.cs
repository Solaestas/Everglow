using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs.VolCano
{
    [AutoloadBossHead]
    public class LavaStone2 : ModNPC
	{
		private int num1 = 0;
		private int num2;
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("熔岩巨石怪");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "熔岩巨石怪");
		}
		public override void SetDefaults()
		{
			base.NPC.damage = 120;
			base.NPC.width = 100;
			base.NPC.height = 100;
			base.NPC.defense = 120;
			base.NPC.lifeMax = 9000;
			base.NPC.knockBackResist = 0;
			base.NPC.alpha = 0;
			base.NPC.lavaImmune = true;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = true;
            base.NPC.aiStyle = -1;
            NPC.boss = false;
        }
        private int Xa = 0;
        public override void AI()
        {
            if (NPC.ai[3] <= 0)
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
                NPC.ai[3] -= 1;
                NPC.velocity += new Vector2(NPC.ai[1], NPC.ai[2]);
            }
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            Player player = Main.player[Main.myPlayer];
            Vector2 playerposition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
            if (!mplayer.ZoneVolcano)
            {
                float num4 = player.position.X - vector.X;
                float num5 = player.position.Y - vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                NPC.velocity += new Vector2(num4, num5) / num6 * 0.1f;
                if (NPC.velocity.Length() < 15)
                {
                    NPC.velocity *= 1.05f;
                }
                if (NPC.velocity.Length() > 17)
                {
                    NPC.velocity *= 0.95f;
                }
                NPC.velocity.Y += 1f;
                NPC.damage = 6000;
            }
            else
            {
                if(Xa < 600 && Xa > 0)
                {
                    float num4 = player.position.X - vector.X;
                    float num5 = player.position.Y - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    NPC.velocity += new Vector2(num4, num5) / num6 * 0.04f * ((NPC.Center - player.Center).Length() / 300 + 1);
                    if (NPC.velocity.Length() < 9)
                    {
                        NPC.velocity *= 1.01f;
                    }
                    if (NPC.velocity.Length() > 11)
                    {
                        NPC.velocity *= 0.98f;
                    }
                    NPC.damage = 120;
                }
                if (Xa >= 600 && Xa < 800)
                {
                    float num4 = player.position.X - vector.X;
                    float num5 = (player.position.Y - 100) - vector.Y;
                    float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                    NPC.velocity += new Vector2(num4, num5) / num6 * 0.04f * ((NPC.Center - (player.Center + new Vector2(0, -100))).Length() / 30 + 1);
                    if (NPC.velocity.Length() < 2)
                    {
                        NPC.velocity *= 1.01f;
                    }
                    if (NPC.velocity.Length() > 3)
                    {
                        NPC.velocity *= 0.98f;
                    }
                    NPC.damage = 120;
                }
                if(Xa > 800)
                {
                    Xa = 0;
                }
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
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/VolCano/熔岩巨石怪2Glow").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/VolCano/熔岩巨石怪2Glow").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.NPC.alpha, 97 - base.NPC.alpha, 97 - base.NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/VolCano/熔岩巨石怪2Glow"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
		public override void HitEffect(NPC.HitInfo hit)
        {
			SoundEngine.PlaySound(SoundID.Item10, new Vector2(base.NPC.position.X, base.NPC.position.Y));
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 54, hit.HitDirection, -1f, 0, default(Color), 1f);
            }
            for (int j = 0; j < 3; j++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 6, hit.HitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                    Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor5, ModContent.Find<ModGore>("Everglow/火山浮石碎块" + (i % 5 + 1).ToString()).Type, 1f);
                }
                for (int i = 0; i < 5; i++)
                {
                    float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                    Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor5, ModContent.Find<ModGore>("Everglow/火山浮石碎块" + (i % 2 + 6).ToString()).Type, 1f);
                    NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Everglow.Ocean.NPCs.火山浮石>(), 0, 0, Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.Next(0, 60), 255);
                }
                if (NPC.CountNPCS(ModContent.NPCType<Everglow.Ocean.NPCs.熔岩巨石怪2>()) <= 1)
                {
                    Color messageColor = Color.MediumPurple;
                    Main.NewText(Language.GetTextValue("熔岩巨石怪已被打败!"), messageColor);
                    if (Main.expertMode)
                    {
                        Item.NewItem(null, (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.LavaStoneTreasureBag>(), 1, false, 0, false, false);
                    }
                    if (Main.maxTilesX == 4200)
                    {
                        for (int i = (int)(Main.maxTilesX * 0.62f + 445); i < (int)(Main.maxTilesX * 0.62f + 455); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 110); j < (int)(Main.maxTilesY * 0.67f - 60); j++)
                            {
                                if (Main.tile[i, j].TileType == ModContent.TileType<Everglow.Ocean.Tiles.LavaStone>() || Main.tile[i, j].TileType == ModContent.TileType<Everglow.Ocean.Tiles.Basalt>())
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
                                if (Main.tile[i, j].TileType == ModContent.TileType<Everglow.Ocean.Tiles.LavaStone>() || Main.tile[i, j].TileType == ModContent.TileType<Everglow.Ocean.Tiles.Basalt>())
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
                                if (Main.tile[i, j].TileType == ModContent.TileType<Everglow.Ocean.Tiles.LavaStone>() || Main.tile[i, j].TileType == ModContent.TileType<Everglow.Ocean.Tiles.Basalt>())
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
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
		}
	}
}
