using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Shaders;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public abstract class MeleeProj : ModProjectile, IWarpProjectile
    {
        public Projectile projectile { get => Projectile; }
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 15;
            projectile.aiStyle = -1;
            projectile.timeLeft = 30;
            projectile.extraUpdates = 1;
            projectile.scale = 1f;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.DamageType = DamageClass.Melee;
            SetDef();
            TrailVec = new Vector2[TrailLength];
        }
        public virtual void SetDef()
        {

        }
        public Player player => Main.player[projectile.owner];
        internal int attackType = 0;
        internal int maxAttackType = 3;
        internal Vector2 mainVec;
        internal Vector2[] TrailVec;
        internal int TrailLength = 40;
        internal int Timer = 0;
        internal bool isAttacking = false;
        internal bool UseTrail = true;
        /// <summary>
        /// 能否穿墙攻击敌人
        /// </summary>
        internal bool CanIgnoreTile = false;
        /// <summary>
        /// 是否为长柄武器
        /// </summary>
        internal bool longHandle = false;
        internal float disFromPlayer = 6;
        internal string shadertype = "Trail0";
        public bool isRightClick = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(mainVec);
            writer.Write(disFromPlayer);
            writer.Write(projectile.spriteDirection);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            mainVec = reader.ReadVector2();
            disFromPlayer = reader.ReadSingle();
            projectile.spriteDirection = reader.ReadInt32();
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = target.Center.X > Main.player[projectile.owner].Center.X ? 1 : -1;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public float GetMeleeSpeed(Player player, float max = 100)
        {
            return Math.Min((player.GetAttackSpeed(DamageClass.Melee) - 1) * 100, max);
        }
        public override void AI()
        {
            player.heldProj = projectile.whoAmI;
            player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = true;
            projectile.Center = player.Center + Utils.SafeNormalize(mainVec, Vector2.One) * disFromPlayer;
            isAttacking = false;
            player.headRotation += 1.57f;
            projectile.timeLeft++;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, mainVec.ToRotation() - 1.57f);
            Attack();
            Timer++;
            if (!isAttacking)
            {
                if (!isRightClick)
                {
                    if (!player.controlUseItem)
                    {
                        End();
                    }
                }
                else
                {
                    if (!player.controlUseTile)
                    {
                        End();
                    }
                }
            }
            if (isAttacking)
            {
                player.direction = projectile.spriteDirection;
            }
            if (UseTrail)
            {
                for (int i = TrailVec.Length - 1; i > 0; --i)
                    TrailVec[i] = TrailVec[i - 1];
                TrailVec[0] = mainVec;
            }
            else//清空！
            {
                TrailVec = new Vector2[TrailLength];
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
            Vector2 ripplePos = projectile.Center + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(mainVec.ToRotation());
            Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
            shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, mainVec.ToRotation());
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.TileActionAttempt cut = new Terraria.Utils.TileActionAttempt(DelegateMethods.CutTiles);
            Vector2 beamStartPos = projectile.Center;
            Vector2 beamEndPos = beamStartPos + mainVec;
            Terraria.Utils.PlotTileLine(beamStartPos, beamEndPos, projectile.width * projectile.scale, cut);
        }
        public void ScreenShake(int time)
        {
            //震屏没写
            //Main.player[projectile.owner].GetModPlayer<EffectPlayer>().screenShake = time;
        }
        public void NextAttackType()
        {
            Timer = 0;
            attackType++;
            if (attackType > maxAttackType)
                attackType = 0;
        }
        public virtual void End()
        {
            Player player = Main.player[projectile.owner];
            projectile.Kill();
            player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            if (isAttacking && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + mainVec * projectile.scale * (longHandle ? 0.2f : 0.1f), projectile.Center + mainVec * projectile.scale, projectile.height, ref point))
                if (Collision.CanHitLine(projectile.Center, 1, 1, targetHitbox.TopLeft(), targetHitbox.Width, targetHitbox.Height) || CanIgnoreTile)
                    return true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DrawTrail(lightColor);
            DrawSelf(Main.spriteBatch, lightColor);
            return false;
        }
       
        public virtual void DrawSelf(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            float exScale = 1;
            if (longHandle)
            {
                exScale += 1f;
            }
            Vector2 origin = new Vector2(longHandle ? tex.Width / 2 : 5, tex.Height / 2);
            Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor), mainVec.ToRotation(), origin, new Vector2(exScale * mainVec.Length() / tex.Width, 1.2f) * projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public virtual float TrailAlpha(float factor)
        {
            float w;
            if (factor > 0.5f)
                w = MathHelper.Lerp(0.5f, 0.7f, factor);
            else
                w = MathHelper.Lerp(0f, 0.5f, factor * 2f);
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
            
            List<VertexInfo> bars = new List<VertexInfo>();
            float counts = 0;
            for (int i = 0; i < TrailVec.Length; i++)
                if (TrailVec[i] != Vector2.Zero)
                    counts++;
            for (int i = 0; i < TrailVec.Length; i++)
            {
                if (TrailVec[i] == Vector2.Zero)
                    continue;
                float factor = 1 - (float)i / counts;
                float w = TrailAlpha(factor);
                if (!longHandle)
                {
                    bars.Add(new VertexInfo(projectile.Center + TrailVec[i] * 0.15f * projectile.scale, new Vector3(factor, 1, 0f),Color.White));
                    bars.Add(new VertexInfo(projectile.Center + TrailVec[i] * projectile.scale, new Vector3(factor, 0, w), Color.White));
                }
                else
                {
                    bars.Add(new VertexInfo(projectile.Center + TrailVec[i] * 0.3f * projectile.scale, new Vector3(factor, 1, 0f), Color.White));
                    bars.Add(new VertexInfo(projectile.Center + TrailVec[i] * projectile.scale, new Vector3(factor, 0, w), Color.White));
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
            
            Effect MeleeTrail= ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/MeleeTrail",ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            //Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

            if (bars.Count >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawWarp()
        {
            List<VertexInfo> bars = new List<VertexInfo>();
            float counts = 0;
            for (int i = 0; i < TrailVec.Length; i++)
                if (TrailVec[i] != Vector2.Zero)
                    counts++;
            for (int i = 0; i < TrailVec.Length; i++)
            {
                if (TrailVec[i] == Vector2.Zero)
                    continue;
                float factor = 1 - (float)i / counts;
                float w = 1f;
                float d = (TrailVec[i]).ToRotation() + 1.57f;
                float dir = d / MathHelper.TwoPi;
                bars.Add(new VertexInfo(projectile.Center - Main.screenPosition, new Vector3(factor, 1, w), new Color(dir, w, 0, 1)));
                bars.Add(new VertexInfo(projectile.Center - Main.screenPosition + TrailVec[i] * projectile.scale * 1.1f, new Vector3(factor, 0, w), new Color(dir, w, 0, 1)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);            Effect KEx= ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp",ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/Warp").Value;//扭曲用贴图
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            KEx.CurrentTechnique.Passes[0].Apply();
            if (bars.Count >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        /// <summary>
        /// 根据鼠标位置锁定玩家方向
        /// </summary>
        /// <param name="player"></param>
        public void LockPlayerDir(Player player)
        {
            if (projectile.owner == Main.myPlayer)
            {
                projectile.spriteDirection = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                player.direction = projectile.spriteDirection;
            }
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
            v = Vector3.Transform(v,Matrix.CreateRotationX(-rot1));
            if (rot2 != 0)
                v = Vector3.Transform(v, Matrix.CreateRotationZ(rot2));
            float k = -viewZ / (v.Z - viewZ);
            return k * new Vector2(v.X, v.Y);
        }

        public void AttSound(SoundStyle sound)
        {
            SoundEngine.PlaySound(sound, projectile.Center);
        }
        public struct VertexInfo : IVertexType
        {
            private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color,0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate,0)

            });
            public Vector2 Position;
            public Color Color;
            public Vector3 TexCoord;
            public VertexInfo(Vector2 position, Vector3 texCoord, Color color)
            {
                Position = position;
                TexCoord = texCoord;
                Color = color;
            }
            public VertexDeclaration VertexDeclaration
            {
                get => _vertexDeclaration;
            }
        }
    }
}
