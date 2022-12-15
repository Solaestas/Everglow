using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk
{
    public class LargeTusk : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk Spike"); //Large Tusk Spike
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙刺");
        }

        private Vector2 V = Vector2.Zero;
        private Vector2 VMax = Vector2.Zero;
        private Vector2 VBase = Vector2.Zero;
        private Vector2 VBaseType1 = Vector2.Zero;
        private Vector2 VBaseType2 = Vector2.Zero;
        private Vector2 VBaseType3 = Vector2.Zero;
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
            NPC.timeLeft = 4000;
        }

        private int wait = 90;
        private bool squ = false;
        private bool Down = true;
        private bool Spr = false;
        public override void AI()
        {
            if (VBaseType1 == Vector2.Zero)
            {
                VBaseType1 = new Vector2(0, Main.rand.NextFloat(-24, -18)).RotatedBy(Main.rand.NextFloat(-0.7f, 0.7f));
                VBaseType2 = new Vector2(0, Main.rand.NextFloat(-15, -7)).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
                VBaseType3 = new Vector2(0, Main.rand.NextFloat(-34, -25)).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
            }
            VMax = new Vector2(0, 84);
            NPC.TargetClosest(false);

            Player player = Main.player[NPC.target];
            if (NPC.collideX && Down)
            {
                NPC.active = false;
            }
            if (NPC.velocity.Length() <= 0.5f && !squ)//鼓包
            {
                if (wait >= 60 && wait < 90)
                {
                    VBase = VBase * 0.9f + VBaseType1 * 0.1f;
                }
                if (wait >= 30 && wait < 60)
                {
                    VBase = VBase * 0.9f + VBaseType2 * 0.1f;
                }
                if (wait >= 0 && wait < 30)
                {
                    VBase = VBase * 0.9f + VBaseType3 * 0.1f;
                }
                if (wait < 2)
                {
                    if (!Spr)
                    {
                        Spr = true;
                        for (int f = 0; f < 6; f++)
                        {
                            Vector2 vd = new Vector2(0, Main.rand.NextFloat(-3f, -1f)).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f));
                            Dust.NewDust(NPC.Bottom - new Vector2(4, -6), 0, 0, DustID.Blood, vd.X, vd.Y, 0, default, Main.rand.NextFloat(1f, 2f));
                        }
                    }
                }
            }
            if (NPC.velocity.Length() <= 0.5f && squ)
            {
                if (VBase.Y <= 0)
                {
                    VBase.Y *= 0.9f;
                }
            }
            if (NPC.velocity.Length() <= 0.5f && NPC.alpha > 0 && !squ)
            {
                if (Main.tile[(int)(NPC.Bottom.X / 16d), (int)(NPC.Bottom.Y / 16d)].IsHalfBlock && Down)
                {
                    Down = false;
                    NPC.position.Y += 16;
                }
                startFight = true;
                if (NPC.alpha == 255)
                {
                    RamInt = Main.rand.Next(6);
                }
                V = VMax;
                NPC.alpha -= 25;
            }
            if (NPC.alpha <= 0)
            {
                NPC.alpha = 0;
                wait--;
                if (wait == 20)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath11.WithVolumeScale(.4f), NPC.Bottom);
                }
            }
            if (wait <= 0 && !squ)
            {
                NPC.damage = 60;
                if (Main.expertMode)
                {
                    NPC.damage = 80;
                }
                if (Main.masterMode)
                {
                    NPC.damage = 100;
                }
                V *= 0.6f;
                /*if ((player.Center - NPC.Center).Length() < 400)
                {
                    MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
                    mplayer.MinaShake = 4;
                    float Str = 1;
                    if ((player.Center - NPC.Center).Length() > 100)
                    {
                        Str = 100 / (player.Center - NPC.Center).Length();
                    }
                    mplayer.ShakeStrength = Str;
                }*/
                if (V.Y <= 0.5f)
                {
                    squ = true;
                }
            }
            if (squ)
            {
                V.Y += 0.9f;

                if (V.Y > 80)
                {
                    NPC.alpha += 15;
                    if (NPC.alpha > 240)
                    {
                        NPC.active = false;
                    }
                }
                if (V.Y > 40)
                {
                    NPC.damage = 0;
                }
            }
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Bleeding, 120);
        }
        private bool startFight = false;
        private int RamInt = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!startFight || NPC.velocity.Length() >= 0.5f)
            {
                return false;
            }
            List<VertexBase.CustomVertexInfo> BackBase = new List<VertexBase.CustomVertexInfo>();
            Color color = Lighting.GetColor((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16));
            float index = (84 - V.Y) / 84f;
            BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(-600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(0, 1, 0)));
            BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(0, 5) + VBase - Main.screenPosition, color, new Vector3(0.5f, 0, 0)));
            BackBase.Add(new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(1, 1, 0)));
            Texture2D thang = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/LargeTuskBase").Value;
            Main.graphics.GraphicsDevice.Textures[0] = thang;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, BackBase.ToArray(), 0, BackBase.Count / 3);


            List<VertexBase.CustomVertexInfo> Vx2 = new List<VertexBase.CustomVertexInfo>
            {
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(-12, 5) - Main.screenPosition, color, new Vector3(0, index, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(12, V.Y - 79) - Main.screenPosition, color, new Vector3(1, 0, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(12, 5) - Main.screenPosition, color, new Vector3(1, index, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(-12, V.Y - 79) - Main.screenPosition, color, new Vector3(0, 0, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(12, V.Y - 79) - Main.screenPosition, color, new Vector3(1, 0, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(-12, 5) - Main.screenPosition, color, new Vector3(0, index, 0))
            };
            thang = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/Tuskplus" + RamInt.ToString()).Value;
            Main.graphics.GraphicsDevice.Textures[0] = thang;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);


            List<VertexBase.CustomVertexInfo> ForeBase = new List<VertexBase.CustomVertexInfo>
            {
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(-600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(0, 1, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(0, 5) + VBase - Main.screenPosition, color, new Vector3(0.5f, 0, 0)),
                new VertexBase.CustomVertexInfo(NPC.Bottom + new Vector2(600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(1, 1, 0))
            };

            thang = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/LargeTuskDrag").Value;
            Main.graphics.GraphicsDevice.Textures[0] = thang;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, ForeBase.ToArray(), 0, ForeBase.Count / 3);
            return false;
        }
    }
}
