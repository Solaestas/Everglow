using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Core.VFX;
using Terraria.GameContent.Shaders;
using Everglow.Sources.Commons.Function.Curves;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public abstract class ClubProj : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 1;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;

            Projectile.DamageType = DamageClass.Melee;
            SetDef();
            trailVecs = new Queue<Vector2>(trailLength + 1);
        }
        public virtual void SetDef()
        {

        }
        /// <summary>
        /// 角速度
        /// </summary>
        internal float Omega = 0;
        /// <summary>
        /// 角加速度
        /// </summary>
        internal float Beta = 0.003f;
        /// <summary>
        /// 最大角速度(受近战攻速影响)
        /// </summary>
        internal float MaxOmega = 0.3f;
        /// <summary>
        /// 伤害半径
        /// </summary>
        internal float HitLength = 32f;
        /// <summary>
        /// 扭曲强度
        /// </summary>
        internal float WarpValue = 0.6f;
        /// <summary>
        /// 命中敌人后对于角速度的削减率(会根据敌人的击退抗性而再次降低)
        /// </summary>
        internal float StrikeOmegaDecrease = 0.9f;
        /// <summary>
        /// 命中敌人后最低剩余角速度(默认40%,即0.4)
        /// </summary>
        internal float MinStrikeOmegaDecrease = 0.4f;
        /// <summary>
        /// 内部音效播放计时器
        /// </summary>
        internal float AudioTimer = 3.14159f;
        /// <summary>
        /// 内部参数，用来计算伤害
        /// </summary>
        internal int DamageStartValue = 0;
        /// <summary>
        /// 拖尾长度
        /// </summary>
        internal int trailLength = 10;
        /// <summary>
        /// 是否正在攻击
        /// </summary>
        internal bool isAttacking = false;
        /// <summary>
        /// 拖尾
        /// </summary>
        internal Queue<Vector2> trailVecs;
        public virtual BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }
        public virtual string TrailShapeTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/Melee";
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            float power = Math.Max(StrikeOmegaDecrease - MathF.Pow(target.knockBackResist / 4f, 3), MinStrikeOmegaDecrease);

            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
            float ShakeStrength = Omega;
            Omega *= power;
            damage = (int)(damage / power);
            Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12f * ShakeStrength, 100)).RotatedByRandom(6.283);
            hitDirection = target.Center.X > Main.player[Projectile.owner].Center.X ? 1 : -1;
            knockback *= Omega * 3;
        }
        public virtual void UpdateSound()
        {
            AudioTimer -= Omega;
            if (AudioTimer <= 0)
            {
                //SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Clubs/Projectiles/Club_wood").WithPitchOffset(-1 + Omega * 3f), Projectile.Center);
                SoundEngine.PlaySound(((SoundID.DD2_MonkStaffSwing.WithPitchOffset(-1 + Omega * 3f)).WithVolumeScale(1 - Omega)), Projectile.Center);
                AudioTimer = MathF.PI;
            }
        }
        public override void AI()
        {
            if (DamageStartValue == 0)
            {
                DamageStartValue = Projectile.damage;
                Projectile.damage = 0;
            }
            //造成伤害等于原伤害*转速*3.334
            Projectile.damage = Math.Max((int)(DamageStartValue * Omega * 3.334), 1);

            Player player = Main.player[Projectile.owner];
            Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
            MouseToPlayer = Vector2.Normalize(MouseToPlayer) * 15f;
            Vector2 vT0 = Main.MouseWorld - player.MountedCenter;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(vT0.Y, vT0.X) - Math.PI / 2d));
            Projectile.Center = player.MountedCenter + MouseToPlayer;
            Projectile.spriteDirection = player.direction;
            Projectile.localNPCHitCooldown = (int)(MathF.PI / (Math.Max(Omega, 0.157)));
            //这个受击冷却是个麻烦的问题                                                                          
            //旋转一周打两次，理论结果是Pi/Omega
            //存在角加速过程
            //localNPCHitCooldown一旦命中就会开始计时，以当时的localNPCHitCooldown值倒计时。
            //这个计时器还没归零，下一击已然命中。则这一击失效。
            //如果设计极短，又会重复判断
            //而且还考虑到怪物会动

            Projectile.rotation += Omega;
            if (HasContinueUsing())
            {
                float MeleeSpeed = player.GetAttackSpeed(Projectile.DamageType);
                if (Omega < MeleeSpeed * MaxOmega)
                {
                    Omega += Beta * MeleeSpeed;
                }
                if (Projectile.timeLeft < 22)
                {
                    Projectile.timeLeft = 22;
                }
            }
            else
            {
                Omega *= 0.9f;
                if (Projectile.timeLeft > 22)
                {
                    Projectile.timeLeft = 22;
                }
            }
            Vector2 HitRange = new Vector2(HitLength, HitLength * Projectile.spriteDirection).RotatedBy(Projectile.rotation) * Projectile.scale;
            trailVecs.Enqueue(HitRange);
            if (trailVecs.Count > trailLength)
            {
                trailVecs.Dequeue();
            }

            if (player.dead)
            {
                Projectile.Kill();
            }
            player.heldProj = Projectile.whoAmI;
            UpdateSound();
            ProduceWaterRipples(new Vector2(HitLength * Projectile.scale));
        }
        public bool HasContinueUsing()
        {
            Player player = Main.player[Projectile.owner];
            if (player.controlUseItem && !player.dead)
            {
                if(player.HeldItem.shoot == Projectile.type)
                {
                    return true;
                }
                
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0f);
            for (int i = 0; i < 5; i++)
            {
                float alp = Omega / 0.4f;
                Color color2 = new Color((int)(lightColor.R * (5 - i) / 5f * alp), (int)(lightColor.G * (5 - i) / 5f * alp), (int)(lightColor.B * (5 - i) / 5f * alp), (int)(lightColor.A * (5 - i) / 5f * alp));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition , null, color2, Projectile.rotation - i * 0.75f * Omega, texture.Size() / 2f, Projectile.scale, effects, 0f);
            }
            DrawTrail();
            PostPreDraw();
            return false;
        }
        public virtual void PostPreDraw()
        {

        }
        public virtual void DrawTrail()
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
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
            }
            bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
            bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = TrailAlpha(factor);
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

            Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/ClubTrail");
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
            Vector4 lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
            lightColor.W = 0.7f * Omega;
            MeleeTrail.Parameters["Light"].SetValue(lightColor);
            MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawWarp(VFXBatch spriteBatch)
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

                float d = trail[i].ToRotation() + 3.14f + 1.57f;
                if (d > 6.28f)
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
                    var MidValue = (1 - dir) / (1 - dir + dir1);
                    var MidPoint = MidValue * trail[i] + (1 - MidValue) * trail[i - 1];
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                }
                if (dir1 - dir > 0.5)
                {
                    var MidValue = (1 - dir1) / (1 - dir1 + dir);
                    var MidPoint = MidValue * trail[i - 1] + (1 - MidValue) * trail[i];
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                }

                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir,Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
            }
            bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
            bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);

                float d = trail[i].ToRotation() + 3.14f + 1.57f;
                if (d > 6.28f)
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
                    var MidValue = (1 - dir) / (1 - dir + dir1);
                    var MidPoint = MidValue * trail[i] + (1 - MidValue) * trail[i - 1];
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                }
                if (dir1 - dir > 0.5)
                {
                    var MidValue = (1 - dir1) / (1 - dir1 + dir);
                    var MidPoint = MidValue * trail[i - 1] + (1 - MidValue) * trail[i];
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(1, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(0, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
                }

                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, Omega * WarpValue, 0, 1), new Vector3(factor, 1, 1)));
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - trail[i] * Projectile.scale * 1.1f, new Color(dir, Omega * WarpValue, 0, 1), new Vector3(factor, 0, 1)));
            }

            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/Warp").Value, bars, PrimitiveType.TriangleStrip);
        }
        public virtual float TrailAlpha(float factor)
        {
            float w;
            w = MathHelper.Lerp(0f, 1, factor);
            return w;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            Vector2 HitRange = new Vector2(HitLength, HitLength * Projectile.spriteDirection).RotatedBy(Projectile.rotation) * Projectile.scale;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - HitRange, Projectile.Center + HitRange, 2 * HitLength / 32f * Omega / 0.3f, ref point))
            {
                return true;
            }
            return false;
        }
        private void ProduceWaterRipples(Vector2 beamDims)
        {
            WaterShaderData shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
            float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
            Vector2 HitRange = new Vector2(HitLength, -HitLength).RotatedBy(Projectile.rotation) * Projectile.scale;
            Vector2 ripplePos = Projectile.Center + HitRange;
            Vector2 ripplePosII = Projectile.Center - HitRange;
            Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
            shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation + MathHelper.Pi / 2f);
            shaderData.QueueRipple(ripplePosII, waveData, beamDims, RippleShape.Square, Projectile.rotation + MathHelper.Pi / 2f);
        }
    }
}
