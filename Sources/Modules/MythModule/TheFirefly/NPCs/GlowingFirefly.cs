using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs
{
    public class GlowingFirefly : ModNPC
    {
        private Matrix outerProduct(Vector4 v1, Vector4 v2)
        {
            Matrix matrix;
            matrix.M11 = v1.X * v2.X;
            matrix.M12 = v1.X * v2.Y;
            matrix.M13 = v1.X * v2.Z;
            matrix.M14 = v1.X * v2.W;

            matrix.M21 = v1.Y * v2.X;
            matrix.M22 = v1.Y * v2.Y;
            matrix.M23 = v1.Y * v2.Z;
            matrix.M24 = v1.Y * v2.W;

            matrix.M31 = v1.Z * v2.X;
            matrix.M32 = v1.Z * v2.Y;
            matrix.M33 = v1.Z * v2.Z;
            matrix.M34 = v1.Z * v2.W;

            matrix.M41 = v1.W * v2.X;
            matrix.M42 = v1.W * v2.Y;
            matrix.M43 = v1.W * v2.Z;
            matrix.M44 = v1.W * v2.W;
            return matrix;
        }

        private Vector3 V4ToV3(Vector4 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        private Vector3 G_prime(Vector3 x, float dt, Vector3 fixedPoint, float elasticity, float restLength)
        {
            var pos = new Vector3(NPC.Center, 0f);
            var vel = new Vector3(NPC.velocity, 0f);

            var offset = (x - fixedPoint);
            var length = offset.Length();
            var unit = offset / length;
            Vector3 force = -elasticity * (length - restLength) * unit + new Vector3(0, 1, 0);
            Vector4 term2 = Vector4.Transform(new Vector4(x - pos - dt * vel, 0), Matrix.Identity * 1f / (dt * dt));
            return V4ToV3(term2) - force;
        }

        private Matrix G_Hessian(Vector3 x, float dt, Vector3 fixedPoint, float elasticity, float restLength)
        {
            var offset = (x - fixedPoint);
            var length = (x - fixedPoint).Length();
            var length2 = (x - fixedPoint).LengthSquared();

            var span = outerProduct(new Vector4(offset, 0), new Vector4(offset, 0));
            var term1 = span * elasticity / length2;
            var term2 = (Matrix.Identity - span / length2) * elasticity * (1 - restLength / length);
            return Matrix.Identity * 1f / (dt * dt) + term1 + term2;
        }

        private static Vector4 SolveAxB(in Matrix A, Vector4 b)
        {
            Matrix AInv = Matrix.Invert(A);
            return Vector4.Transform(b, AInv);
        }

        private Vector3 NewtonsMethod(float dt, Vector3 fixedPoint, float elasticity, float restLength)
        {
            Vector3 x = new Vector3(NPC.Center, 0);
            for (int i = 0; i < 10; i++)
            {
                var gp = new Vector4(G_prime(x, dt, fixedPoint, elasticity, restLength), 0);
                var deltaX = V4ToV3(SolveAxB(G_Hessian(x, dt, fixedPoint, elasticity, restLength), -gp));
                x += deltaX;
                if (deltaX.LengthSquared() < 1e-3)
                {
                    break;
                }
            }
            return x;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
            MothLandGlobalNPC.RegisterMothLandNPC(Type);
        }
        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 20;
            NPC.height = 20;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 1f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.catchItem = ModContent.ItemType<Items.GlowingFirefly>();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
            if(!fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                return 0f;
            }
            return 0.3f;
        }
        public override void AI()
        {
            float deltaTime = 1f;
            Vector3 xx = NewtonsMethod(deltaTime, new Vector3(Main.LocalPlayer.Center, 0), 0.3f, 100 );
            var oldPos = NPC.Center;
            NPC.velocity = (new Vector2(xx.X, xx.Y) - oldPos) / deltaTime;
            return;

            Player player = Main.player[NPC.FindClosestPlayer()];
            if(NPC.ai[1] > 0)
            {
                if(NPC.ai[0] < 4)
                {
                    NPC.ai[0] += 0.18f;
                }
                else
                {
                    NPC.ai[0] += 0.5f;
                }
                
                if (NPC.ai[0] >= 8f)
                {
                    NPC.ai[0] = 4f;
                }
                NPC.velocity.Y = 0f;
                UpdateMove();
            }
            else
            {
                if ((player.Center - NPC.Center).Length() < 80f || NPC.life != NPC.lifeMax) 
                {
                    NPC.ai[1] = 1f;
                }
                foreach(NPC same in Main.npc)
                {
                    if(same.type == NPC.type)
                    {
                        if((same.Center - NPC.Center).Length() < 60)
                        {
                            if(same.ai[1] > 0)
                            {
                                NPC.ai[1] = 1;
                                NPC.ai[2] = Main.rand.Next(100);
                                AimPos = new Vector2(0, Main.rand.NextFloat(12f, 220f)).RotatedByRandom(6.283) + NPC.Center;
                                break;
                            }
                        }
                    }
                }
            }
        }
        Vector2 AimPos = Vector2.Zero;
        private void UpdateMove()
        {
            NPC.ai[2] += 1;
            if(NPC.ai[2] % 40 == 0)
            {
                Vector2 vNext = new Vector2(0, Main.rand.NextFloat(12f, 220f)).RotatedByRandom(6.283) + NPC.Center + Vector2.Normalize(NPC.Center - Main.player[Player.FindClosest(NPC.Center, 0, 0)].Center) * 6 + new Vector2(0, -6);
                while(!Collision.CanHit(NPC.Center,0,0, vNext,0,0) || Main.tile[(int)(vNext.X / 16), (int)(vNext.Y / 16)].LiquidAmount != 0)
                {
                    vNext = new Vector2(0, Main.rand.NextFloat(12f, 220f)).RotatedByRandom(6.283) + NPC.Center + Vector2.Normalize(NPC.Center - Main.player[Player.FindClosest(NPC.Center, 0, 0)].Center) * 6 + new Vector2(0, -6);
                }
                AimPos = vNext;

            }
            if((NPC.Center - AimPos).Length() >= 20)
            {
                NPC.velocity = Vector2.Normalize(AimPos - NPC.Center) * 1f;
            }
            else
            {
                NPC.velocity *= 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tx = MythContent.QuickTexture("TheFirefly/NPCs/GlowingFirefly");
            Texture2D tg = MythContent.QuickTexture("TheFirefly/NPCs/GlowingFireflyGlow");
            Vector2 vector = new Vector2(tx.Width / 2f, tx.Height / (float)Main.npcFrameCount[NPC.type] / 2f);

            Color color0 = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
            Color color1 = new Color(255, 255, 255, 0);
            Main.spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, new Rectangle(0, 50 * (int)NPC.ai[0], 46, 50), color0, NPC.rotation, vector, NPC.scale, effects, 0f);
            Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition, new Rectangle(0, 50 * (int)NPC.ai[0], 46, 50), color1, NPC.rotation, vector, NPC.scale, effects, 0f);

            return false;
        }
        public override void OnKill()
        {
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Clentaminator_Blue, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 0, default, 0.6f);
            }
            for (int i = 0; i < 6; i++)
            {
                int index = Dust.NewDust(NPC.position - new Vector2(8), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f));
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283);
                Main.dust[index].noGravity = true;
            }
            base.OnKill();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MothScaleDust>(), 1, 1, 1));
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC.scale = Main.rand.NextFloat(0.83f, 1.17f);
            base.OnSpawn(source);
        }
        public override bool? CanBeCaughtBy(Item item, Player player)
        {
            return true;
        }
        public override void OnCaughtBy(Player player, Item item, bool failed)
        {
            Item.NewItem(NPC.GetSource_FromThis(), NPC.Center,0,0, ModContent.ItemType<Items.GlowingFirefly>(),1);
            NPC.active = false;
            base.OnCaughtBy(player, item, failed);
        }
    }
}
