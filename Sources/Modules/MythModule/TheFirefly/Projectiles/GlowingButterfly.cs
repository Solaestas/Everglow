using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class GlowingButterfly : ModProjectile
    {
        FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.netImportant = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = ItemUseStyleID.Swing;
            if (MothEye.LocalOwner != null && MothEye.LocalOwner.TryGetModPlayer(out MothEyePlayer mothEyePlayer))
            {
                if (mothEyePlayer.MothEyeEquipped && fireflyBiome.IsBiomeActive(Main.LocalPlayer) && Main.hardMode)
                {
                    Projectile.timeLeft = 400;
                }
                else
                {
                    Projectile.timeLeft = 100;
                }
            }
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.alpha = 255;
        }

        private float Ome = 0;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (MothEye.LocalOwner != null && MothEye.LocalOwner.TryGetModPlayer(out MothEyePlayer mothEyePlayer))
            {
                
                if (mothEyePlayer.MothEyeEquipped && fireflyBiome.IsBiomeActive(Main.LocalPlayer) && Main.hardMode)
                {
                    if (y == 0)
                    {
                        Projectile.timeLeft = Main.rand.Next(135, 185);
                        Projectile.frame = Main.rand.Next(6);
                        Ome = Main.rand.NextFloat(-0.02f, 0.02f);
                        Projectile.scale = Main.rand.NextFloat(0.6f, 1.0f);
                        y = ItemUseStyleID.Swing;
                    }
                    if (Projectile.timeLeft > 100 && Projectile.alpha >= 8)
                    {
                        Projectile.alpha -= 4;
                    }
                    if (Projectile.timeLeft <= 66)
                    {
                        Projectile.alpha += 4;
                    }
                    if (Projectile.alpha < 100)
                    {
                        Projectile.friendly = true;
                    }
                    else
                    {
                        Projectile.friendly = false;
                    }
                }
                else
                {
                    if (y == 0)
                    {
                        Projectile.timeLeft = Main.rand.Next(85, 135);
                        Projectile.frame = Main.rand.Next(6);
                        Ome = Main.rand.NextFloat(-0.02f, 0.02f);
                        Projectile.scale = Main.rand.NextFloat(0.6f, 1.0f);
                        y = ItemUseStyleID.Swing;
                    }
                    if (Projectile.timeLeft > 50 && Projectile.alpha >= 8)
                    {
                        Projectile.alpha -= 8;
                    }
                    if (Projectile.timeLeft <= 33)
                    {
                        Projectile.alpha += 8;
                    }
                    if (Projectile.alpha < 50)
                    {
                        Projectile.friendly = true;
                    }
                    else
                    {
                        Projectile.friendly = false;
                    }
                }
            }
            
            //Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.75);
            Projectile.velocity = Projectile.velocity.RotatedBy(Ome);
            Ome += Math.Sign(Ome) * 0.001f;
            if (Projectile.frame != 5)
            {
                Projectile.velocity *= 1.04f;
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }
            if (Collision.SolidCollision((Projectile.Center - Projectile.velocity), 1, 1))
            {
                Projectile.tileCollide = true;
            }
            Stre = Math.Clamp((100 - Projectile.timeLeft) / 10f, 0, 1f);

            if (Projectile.timeLeft % 5 == 0)
            {
                if (Projectile.frame != 5)
                {
                    Projectile.frame++;
                }
                else
                {
                    if (Main.rand.NextFloat(0, 7) >= Projectile.velocity.Length())
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
            Projectile.velocity.Y *= 0.96f;
            if (Projectile.timeLeft % 6 == 0)
            {
                //int num89 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 113, 0,0, 0, default, 0.6f);
                int num90 = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, ModContent.DustType<BlueGlowAppear>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.4f, 1.2f));
                //Main.dust[num89].velocity = Projectile.velocity * 0.5f;
                Main.dust[num90].velocity = Projectile.velocity * 0.5f;
            }
            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
            Visuals();
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.04f;

            // Fix overlap with other minions
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];

                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            // Starting search distance
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            //Projectile.friendly = foundTarget;
        }

        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            // Default movement parameters (here for attacking)
            float speed = 8f;
            float inertia = 20f;

            if (foundTarget)
            {
                // Minion has a target: attack (here, fly towards the enemy)
                if (distanceFromTarget > 40f)
                {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
            {
                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 600f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 9f;
                    inertia = 60f;
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = 4f;
                    inertia = 80f;
                }

                if (distanceToIdlePosition > 20f)
                {
                    // The immediate range around the player (when it passively floats about)

                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
        }

        private void Visuals()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0, 0.05f * (255 - Projectile.alpha) / 255f, 0.12f * (255 - Projectile.alpha) / 255f));
        }

        private int y = 0;
        private float Stre;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.alpha > 180)
            {
                return;
            }
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default, 0.6f * Stre);
            }
            for (int i = 0; i < 6; i++)
            {
                int num90 = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f) * Stre);
                Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283) * Stre;
                Main.dust[num90].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color((255 - Projectile.alpha), (255 - Projectile.alpha), (255 - Projectile.alpha), (255 - Projectile.alpha) / 3);
        }

        /*private Effect ef;
         public override void PostDraw(Color lightColor)
         {
             Main.spriteBatch.End();
             Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
             List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
             ef = (Effect)ModContent.Request<Effect>("MythMod/Effects/TrailB2").Value;
             for (int i = 1; i < Projectile.oldPos.Length; ++i)
             {
                 if (Projectile.oldPos[i] == Vector2.Zero) break;

                 int width = 30;
                 var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                 normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                 var factor = i / (float)Projectile.oldPos.Length;
                 var color = Color.Lerp(Color.White, Color.Blue, factor);
                 var w = MathHelper.Lerp(1f, 0.05f, factor);

                 bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(17, 17) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                 bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(17, 17) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
             }

             List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();

             if (bars.Count > 2)
             {
                 triangleList.Add(bars[0]);
                 var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                 RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                 var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                 var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                 ef.Parameters["uTransform"].SetValue(model * projection);
                 ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + Projectile.ai[0]);
                 Texture2D Blue = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBlue").Value;
                 Texture2D Shape = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                 Texture2D Mask = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/IceTrace").Value;
                 Main.graphics.GraphicsDevice.Textures[0] = Blue;
                 Main.graphics.GraphicsDevice.Textures[1] = Shape;
                 Main.graphics.GraphicsDevice.Textures[2] = Mask;
                 Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                 Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                 Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                 ef.CurrentTechnique.Passes[0].Apply();
                 Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                 Main.graphics.GraphicsDevice.RasterizerState = originalState;
                 Main.spriteBatch.End();
                 Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
             }
         }
         private struct CustomVertexInfo : IVertexType
         {
             private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
             {
                 new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                 new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                 new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
             });
             public Vector2 Position;
             public Color Color;
             public Vector3 TexCoord;

             public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
             {
                 this.Position = position;
                 this.Color = color;
                 this.TexCoord = texCoord;
             }

             public VertexDeclaration VertexDeclaration
             {
                 get
                 {
                     return _vertexDeclaration;
                 }
             }
         }*/
    }
}