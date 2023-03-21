using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.Curves;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Everglow.Sources.Commons.Core.VFX;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public abstract class MeleeProj : ModProjectile, IWarpProjectile,IBloomProjectile
    {
        
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 15;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = DamageClass.Melee;
            SetDef();
            trailVecs = new Queue<Vector2>(trailLength + 1);
        }
        public virtual void SetDef()
        {

        }
        internal int attackType = 0;
        internal int maxAttackType = 3;
        internal Vector2 mainVec;
        internal Queue<Vector2> trailVecs;
        internal int trailLength = 40;
        internal int timer = 0;

        /// <summary>
        /// 是否采用自己的DrawWarp
        /// </summary>
        internal bool selfWarp = false;
        internal bool isAttacking = false;
        internal bool useTrail = true;
        /// <summary>
        /// 断开左键是否自动结束攻击
        /// </summary>
        internal bool AutoEnd = true;
        /// <summary>
        /// 绑定AutoEnd参数,用于判定上次攻击结束前是否按下鼠标左键从而实现连击
        /// </summary>
        internal bool HasContinueLeftClick = false;
        /// <summary>
        /// 允许长按左键?(一般情况用来做重击)
        /// </summary>
        internal bool CanLongLeftClick = false;
        /// <summary>
        /// 绑定CanLongLeftClick,用于判定重击
        /// </summary>
        internal int Clicktimer = 0;
        /// <summary>
        /// 绑定CanLongLeftClick,用于判定重击所需要的蓄力时长
        /// </summary>
        internal int ClickMaxtimer = 90;
        /// <summary>
        /// 能否穿墙攻击敌人
        /// </summary>
        internal bool CanIgnoreTile = false;
        /// <summary>
        /// 是否为长柄武器
        /// </summary>
        internal bool longHandle = false;

        internal float drawScaleFactor = 1f;

        internal float disFromPlayer = 6;
        internal string shadertype = "Trail0";
        public bool isRightClick = false;
        public Player Player => Main.player[Projectile.owner];
        public Vector2 MainVec_WithoutGravDir
        {
            get
            {
                Vector2 vec = mainVec;
                if (Player.gravDir == -1)
                    vec.Y *= -1;
                return vec;
            }
        }
        public Vector2 MouseWorld_WithoutGravDir
        {
            get
            {
                Vector2 vec = Main.MouseWorld;
                if (Player.gravDir == -1)
                {
                    vec = WrapY(vec);
                }
                return vec;
            }
        }
        public Vector2 ProjCenter_WithoutGravDir
        {
            get
            {
                Vector2 vec = Projectile.Center;
                if (Player.gravDir == -1)
                {
                    vec = WrapY(vec);
                }
                return vec;
            }
        }
        private Vector2 WrapY(Vector2 vec)
        {
            vec.Y -= Main.screenPosition.Y;
            float d = vec.Y - Main.screenHeight / 2;
            vec.Y -= 2 * d;
            vec.Y += Main.screenPosition.Y;
            return vec;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(mainVec);
            writer.Write(disFromPlayer);
            //writer.Write(Projectile.spriteDirection);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mainVec = reader.ReadVector2();
            disFromPlayer = reader.ReadSingle();
            //Projectile.spriteDirection = reader.ReadInt32();
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = target.Center.X > Main.player[Projectile.owner].Center.X ? 1 : -1;
        }
        public float GetMeleeSpeed(Player player, float max = 100)
        {
            return Math.Min((player.GetAttackSpeed(DamageClass.Melee) - 1) * 100, max);
        }
        public override void AI()
        {
            Player.heldProj = Projectile.whoAmI;
            Player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = true;
            Projectile.Center = Player.Center + Utils.SafeNormalize(mainVec, Vector2.One) * disFromPlayer;
            isAttacking = false;

            Projectile.timeLeft++;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, mainVec.ToRotation() - 1.57f);
            Attack();
            timer++;
            if (!isAttacking)
            {
                if (!isRightClick)
                {
                    bool IsEnd = AutoEnd ? (!Player.controlUseItem || Player.dead) : Player.dead;
                    if (IsEnd)
                    {
                        End();
                    }
                }
                else
                {
                    bool IsEnd = AutoEnd ? (!Player.controlUseTile || Player.dead) : Player.dead;
                    if (IsEnd)
                    {
                        End();
                    }
                }
            }
            if (!HasContinueLeftClick && timer > 15)//大于1/60s即判定为下一击继续
            {
                if (Main.mouseLeft)
                {
                    HasContinueLeftClick = true;
                }
            }
            if (isAttacking)
            {
                Player.direction = Projectile.spriteDirection;
            }
            if (useTrail)
            {
                trailVecs.Enqueue(mainVec);
                if (trailVecs.Count > trailLength)
                {
                    trailVecs.Dequeue();
                }
            }
            else//清空！
            {
                trailVecs.Clear();
            }
            if(CanLongLeftClick)
            {
                if(Main.mouseLeft)
                {
                    Clicktimer++;
                }
                else
                {
                    Clicktimer = 0;
                }
            }
            ProduceWaterRipples(new Vector2(mainVec.Length(), 30));
        }
        public virtual void Attack()
        {

        }
        public override bool ShouldUpdatePosition() => false;

        private void ProduceWaterRipples(Vector2 beamDims)
        {
            WaterShaderData shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
            float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
            Vector2 ripplePos = Projectile.Center + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(mainVec.ToRotation());
            Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
            shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, mainVec.ToRotation());
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.TileActionAttempt cut = new Terraria.Utils.TileActionAttempt(DelegateMethods.CutTiles);
            Vector2 beamStartPos = Projectile.Center;
            Vector2 beamEndPos = beamStartPos + mainVec;
            Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
        }
        public void ScreenShake(int time)
        {
            //震屏没写
            //Main.player[projectile.owner].GetModPlayer<EffectPlayer>().screenShake = time;
        }
        public void NextAttackType()
        {
            if (!isAttacking && !AutoEnd)
            {
                if (Clicktimer >= ClickMaxtimer)
                {
                    LeftLongThump();
                    End();
                }
                if (!isRightClick)
                {
                    if (!HasContinueLeftClick || Player.dead)
                    {
                        Player player = Main.player[Projectile.owner];
                        End();
                        player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
                    }
                }
                else
                {
                    if (!Player.controlUseTile || Player.dead)
                    {
                        Player player = Main.player[Projectile.owner];
                        End();
                        player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
                    }
                }
            }
            HasContinueLeftClick = false;
            timer = 0;
            attackType++;
            if (attackType > maxAttackType)
            {
                attackType = 0;
            }
            
        }
        public virtual void LeftLongThump()
        {

        }
        public virtual void End()
        {
            Player player = Main.player[Projectile.owner];
            TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
            player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
            player.fullRotation = 0;
            player.legRotation = 0;
            Tplayer.HeadRotation = 0;
            Tplayer.HideLeg = false;
            player.legPosition = Vector2.Zero;
            Projectile.Kill();
            player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            if (isAttacking && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ProjCenter_WithoutGravDir + MainVec_WithoutGravDir * Projectile.scale * (longHandle ? 0.2f : 0.1f), ProjCenter_WithoutGravDir + MainVec_WithoutGravDir * Projectile.scale, Projectile.height, ref point))
            {
                if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, targetHitbox.TopLeft(), targetHitbox.Width, targetHitbox.Height) || CanIgnoreTile)
                {
                    return true;
                }
            }

            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawTrail(lightColor);
            DrawSelf(Main.spriteBatch, lightColor);
            return false;
        }

        public virtual void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth = 10, float HorizontalHeight = 10, float DrawScale = 0.9f, string GlowPath = "", double DrawRotation = 0.7854)
        {
            Player player = Main.player[Projectile.owner];
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            //float exScale = longHandle ? 2 : 1;
            //Vector2 origin = new Vector2(longHandle ? tex.Width / 2 : 5, tex.Height / 2);
            //Main.spriteBatch.Draw(tex, ProjCenter_WithoutGravDir - Main.screenPosition, null, Projectile.GetAlpha(lightColor), MainVec_WithoutGravDir.ToRotation(), origin, new Vector2(exScale * mainVec.Length() / tex.Width, 1.2f) * Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            float texWidth = HorizontalWidth;//转换成水平贴图时候的宽度
            float texHeight = HorizontalHeight;//转换成水平贴图时候的高度
            float Size = DrawScale;//放大的几何倍数
            double baseRotation = DrawRotation;//这个是刀刃倾斜度与水平的夹角,尽量不要是别的数值

            float exScale = 1;
            if (longHandle)
            {
                exScale += 1f;
            }
            Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * drawScaleFactor* mainVec.Length() / tex.Width, 1.2f) * Projectile.scale;

            double ProjRotation = MainVec_WithoutGravDir.ToRotation() + Math.PI / 4;

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = ProjCenter_WithoutGravDir - Main.screenPosition;
            Vector2 CenterMoveByPlayerRotation = new Vector2(6 * player.direction, -player.height * player.gravDir) - new Vector2(0, -player.height * player.gravDir).RotatedBy(player.fullRotation);
            Vector2 drawCenter2 = drawCenter - CenterMoveByPlayerRotation;

            Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * Size;
            Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * Size;

            Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
            ITexNormal.X /= tex.Width;
            ITexNormal.Y /= tex.Height;
            Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
            JTexNormal.X /= tex.Width;
            JTexNormal.Y /= tex.Height;

            Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = Vector2.Normalize(INormal) * origin.Y - Vector2.Normalize(JNormal) * origin.X;
            Vector2 TopRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) + Vector2.Normalize(INormal) * origin.Y;
            Vector2 BottomLeft = -Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y) - Vector2.Normalize(JNormal) * origin.X;
            Vector2 BottomRight = Vector2.Normalize(JNormal) * (JNormal.Length() * 2 - origin.X) - Vector2.Normalize(INormal) * (INormal.Length() * 2 - origin.Y);


            Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
            Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
            Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

            if (Player.direction * Player.gravDir == -1)
            {
                sourceTopLeft = sourceBottomLeft;
                sourceTopRight = sourceBottomRight;
                sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            }

            List<Vertex2D> vertex2Ds = new List<Vertex2D>
            {
                    new Vertex2D(drawCenter2 + TopLeft, lightColor, new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

                    new Vertex2D(drawCenter2 + BottomLeft, lightColor, new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + BottomRight, lightColor, new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, lightColor, new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
            };

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);

            if (GlowPath != "")
            {
                vertex2Ds = new List<Vertex2D>
                {
                    new Vertex2D(drawCenter2 + TopLeft, new Color(255,255,255,0), new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter2 + BottomLeft, new Color(255,255,255,0), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, new Color(255,255,255,0), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),

                    new Vertex2D(drawCenter2 + BottomLeft, new Color(255,255,255,0), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + BottomRight, new Color(255,255,255,0), new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, new Color(255,255,255,0), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
                };
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/" + GlowPath).Value; ;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public virtual float TrailAlpha(float factor)
        {
            float w;
            w = MathHelper.Lerp(0f, 1, factor);
            return w;
        }
        public virtual string TrailShapeTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/Melee";
        }
        public virtual string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
        }
        public virtual BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }
        public virtual void DrawTrail(Color color)
        {
            List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(trailVecs.ToList());//平滑
            List<Vector2> SmoothTrail = new List<Vector2>();
            for (int x = 0; x < SmoothTrailX.Count - 1; x++)
            {
                SmoothTrail.Add(SmoothTrailX[x]);
            }
            if (trailVecs.Count != 0)
            {
                SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
            }

            int length = SmoothTrail.Count;
            if (length <= 3)
            {
                return;
            }
            Vector2[] trail = SmoothTrail.ToArray();
            List<Vertex2D> bars = new List<Vertex2D>();

            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = TrailAlpha(factor);
                if (!longHandle)
                {
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.15f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
                }
                else
                {
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.3f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                    bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            Effect MeleeTrail = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            //Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        Vector2 r = Vector2.One;
        public void DrawWarp(VFXBatch spriteBatch)
        {
            if(selfWarp)
            {
                return;
            }
            List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(trailVecs.ToList());//平滑
            List<Vector2> SmoothTrail = new List<Vector2>();
            for (int x = 0; x < SmoothTrailX.Count - 1; x++)
            {
                SmoothTrail.Add(SmoothTrailX[x]);
            }
            if (trailVecs.Count != 0)
            {
                SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
            }
            int length = SmoothTrail.Count;
            if (length <= 3)
            {
                return;
            }
            Vector2[] trail = SmoothTrail.ToArray();
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1f;
                float d = trail[i].ToRotation()+3.14f+1.57f;
                if(d>6.28f)
                {
                    d -= 6.28f;
                }
                float dir = d / MathHelper.TwoPi;

                float dir1 = dir;
                if (i > 0)
                {
                    float d1 = trail[i - 1].ToRotation() + 3.14f + 1.57f;
                    if (d1 > 6.28f)
                    {
                        d1 -= 6.28f;
                    }
                    dir1 = d1 / MathHelper.TwoPi;
                }

                if (dir - dir1 > 0.5)
                {
                    var midValue = (1 - dir) / (1 - dir + dir1);
                    var midPoint = midValue * trail[i] + (1 - midValue) * trail[i - 1];
                    var oldFactor = (i - 1) / (length - 1f);
                    var midFactor = midValue * factor + (1 - midValue) * oldFactor;
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
                }
                if (dir1 - dir > 0.5)
                {
                    var midValue = (1 - dir1) / (1 - dir1 + dir);
                    var midPoint = midValue * trail[i - 1] + (1 - midValue) * trail[i];
                    var oldFactor = (i - 1) / (length - 1f);
                    var midFactor = midValue * oldFactor + (1 - midValue) * factor;
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
                }
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, w, 0, 1), new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, w, 0, 1), new Vector3(factor, 0, w)));
            }
            
            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/Warp").Value,bars,PrimitiveType.TriangleStrip);
        }
        /// <summary>
        /// 根据鼠标位置锁定玩家方向
        /// </summary>
        /// <param name="player"></param>
        public void LockPlayerDir(Player player)
        {
            Projectile.spriteDirection = Main.MouseWorld.X > player.Center.X ? 1 : -1;
            player.direction = Projectile.spriteDirection;
        }
        /// <summary>
        /// 圆的透视投影
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="rot0"></param>
        /// <param name="rot1"></param>
        /// <param name="rot2"></param>
        /// <param name="viewZ"></param>
        /// <returns></returns>
        public static Vector2 Vector2Elipse(float radius, float rot0, float rot1, float rot2 = 0, float viewZ = 1000)
        {
            Vector3 v = Vector3.Transform(Vector3.UnitX, Matrix.CreateRotationZ(rot0)) * radius;
            v = Vector3.Transform(v, Matrix.CreateRotationX(-rot1));
            if (rot2 != 0)
            {
                v = Vector3.Transform(v, Matrix.CreateRotationZ(-rot2));
            }
            float k = -viewZ / (v.Z - viewZ);
            return k * new Vector2(v.X, v.Y);
        }
        public float GetAngToMouse()
        {
            Vector2 vec = (MouseWorld_WithoutGravDir - Main.player[Projectile.owner].Center);
            if (vec.X < 0)
                vec = -vec;
            return -vec.ToRotation();
        }
        public void AttSound(SoundStyle sound)
        {
            SoundEngine.PlaySound(sound, Projectile.Center);
        }
        public void DrawBloom()
        {
            DrawTrail(Color.White);
        }
    }
}