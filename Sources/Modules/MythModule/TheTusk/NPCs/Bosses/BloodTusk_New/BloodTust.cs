using System;
using Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk_New
{
    /*
    [AutoloadBossHead]
    public  class BloodTust_New : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Tusk");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "鲜血獠牙");
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk",
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            string tex = "It was just a wolf tooth, dropped to the Crimson when its owner was defeated by a hero, gradually corrupted by the power of Cthulhu and granted mentality.";
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                tex = "原本只是一颗狼牙,在它的主人被勇士讨伐时掉落至猩红之地,逐渐为克苏鲁的力量所沾染,有了自己的意识";
            }
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson, 
				new FlavorTextBestiaryInfoElement(tex)
            });
        }
        public override void SetDefaults()
        {
            NPC.behindTiles = true;
            NPC.damage = 0;
            NPC.width = 40;
            NPC.height = 142;
            NPC.defense = 15;
            NPC.lifeMax = 7800;
            
            if (Main.expertMode)
            {
                NPC.lifeMax = 10200;
                NPC.defense = 20;
            }
            if (Main.masterMode)
            {
                NPC.lifeMax = 13400;
                NPC.defense = 25;
            }
            for (int i = 0; i < NPC.buffImmune.Length; i++)
            {
                NPC.buffImmune[i] = true;
            }
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 2, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.DD2_SkeletonHurt;
            NPC.DeathSound = SoundID.DD2_SkeletonDeath;
            NPC.dontTakeDamage = true;
            Music = Common.MythContent.QuickMusic("TuskTension");
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            bool flag = NPC.life <= 0;
            if (!flag)
            {
                float num = NPC.life / (float)NPC.lifeMax;
                bool flag2 = num > 1f;
                if (flag2)
                {
                    num = 1f;
                }
                int num2 = (int)(36f * num);
                float num3 = position.X - 18f * scale;
                float num4 = position.Y;
                bool flag3 = Main.player[Main.myPlayer].gravDir == -1f;
                if (flag3)
                {
                    num4 -= Main.screenPosition.Y;
                    num4 = Main.screenPosition.Y + Main.screenHeight - num4;
                }
                float num5 = 0f;
                float num6 = 255f;
                num -= 0.1f;
                bool flag4 = (double)num > 0.5;
                float num7;
                float num8;
                if (flag4)
                {
                    num7 = 255f;
                    num8 = 255f * (1f - num) * 2f;
                }
                else
                {
                    num7 = 255f * num * 2f;
                    num8 = 255f;
                }
                float num9 = 0.95f;
                num8 *= num9;
                num7 *= num9;
                num6 *= num9;
                bool flag5 = num8 < 0f;
                if (flag5)
                {
                    num8 = 0f;
                }
                bool flag6 = num8 > 255f;
                if (flag6)
                {
                    num8 = 255f;
                }
                bool flag7 = num7 < 0f;
                if (flag7)
                {
                    num7 = 0f;
                }
                bool flag8 = num7 > 255f;
                if (flag8)
                {
                    num7 = 255f;
                }
                bool flag9 = num6 < 0f;
                if (flag9)
                {
                    num6 = 0f;
                }
                bool flag10 = num6 > 255f;
                if (flag10)
                {
                    num6 = 255f;
                }
                Color color = new Color((byte)num8, (byte)num7, (byte)num5, (byte)num6);
                bool flag11 = num2 < 3;
                if (flag11)
                {
                    num2 = 3;
                }
                bool flag12 = num2 < 38;
                if (flag12)
                {
                    bool flag13 = num2 < 40;
                    if (flag13)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + num2 * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(2, 0, 2, TextureAssets.Hb2.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    bool flag14 = num2 < 38;
                    if (flag14)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + (num2 + 2) * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(num2 + 2, 0, 36 - num2 - 2, TextureAssets.Hb2.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    bool flag15 = num2 > 2;
                    if (flag15)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, num2 - 2, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    bool flag16 = num2 > 18;//分血条
                    if (flag16)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X + (num2 - 2) * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(32, 0, 2, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                }
                else
                {
                    bool flag16 = num2 < 40;
                    if (flag16)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + num2 * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(num2, 0, 36 - num2, TextureAssets.Hb2.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    bool flag17 = num2 > 18;//分血条
                    if (flag17)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, num2, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedTusk, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public bool reallyStart = false;
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Bleeding, 120);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (!reallyStart)
            {
                reallyStart = true;
                if (!Main.dedServ)
                {
                    Music = Common.MythContent.QuickMusic("TuskFighting");
                }
                Main.NewText("The Tusk has awoken!", 175, 75, 255);
                //NPC.NewNPC(null, (int)NPC.Center.X - 800, (int)NPC.Center.Y - 200, ModContent.NPCType<TuskWallLeft>());
               // NPC.NewNPC(null, (int)NPC.Center.X + 800, (int)NPC.Center.Y - 200, ModContent.NPCType<TuskWallRight>());
            }
        }





        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
    }*/
}
