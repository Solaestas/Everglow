﻿using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Curves;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Shaders;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class CrystalClub_fly : ModProjectile, IWarpProjectile
    {
        /// <summary>
        /// 角速度
        /// </summary>
        internal float Omega = 0;
        /// <summary>
        /// 角加速度
        /// </summary>
        internal float Beta = 0.005f;
        /// <summary>
        /// 最大角速度(受近战攻速影响)
        /// </summary>
        internal float MaxOmega = 0.5f;
        /// <summary>
        /// 伤害半径
        /// </summary>
        internal float HitLength = 32f;
        /// <summary>
        /// 命中敌人后对于角速度的削减率(会根据敌人的击退抗性而再次降低)
        /// </summary>
        internal float StrikeOmegaDecrease = 0.9f;
        /// <summary>
        /// 命中敌人后最低剩余角速度(默认40%,即0.4)
        /// </summary>
        internal float MinStrikeOmegaDecrease = 0.4f;
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
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 1;
            Projectile.timeLeft = 580;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;

            Projectile.DamageType = DamageClass.Melee;

            trailVecs = new Queue<Vector2>(trailLength + 1);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float Rnd = Main.rand.NextFloat(6.283f);
            for(int d = 0;d < 9;d++)
            {
                Vector2 v0 = new Vector2(0, 0.7f).RotatedBy(d / 4.5 * Math.PI + Rnd) * 5;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, v0, ProjectileID.CrystalShard, Projectile.damage / 2, Projectile.knockBack * 0.1f, Projectile.owner);
                p.scale = 1.6f;
            }
            for (int d = 0; d < 9; d++)
            {
                Vector2 v0 = new Vector2(0, 1.2f).RotatedBy((d + 0.5) / 4.5 * Math.PI + Rnd) * 5;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, v0, ProjectileID.CrystalShard, Projectile.damage / 2, Projectile.knockBack * 0.1f, Projectile.owner);
                p.scale = 2.4f;
            }
            SoundEngine.PlaySound(SoundID.Shatter.WithPitchOffset(Main.rand.NextFloat(0.2f, 0.9f)), Projectile.Center);
            int type = 0;

            for (int d = 0; d < 50; d += 1)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        type = DustID.BlueCrystalShard; break;
                    case 1:
                        type = DustID.PinkCrystalShard; break;
                    case 2:
                        type = DustID.PurpleCrystalShard; break;
                }
                float scale = Main.rand.NextFloat(0.4f, 2.1f);
                Dust D = Dust.NewDustDirect(Projectile.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, scale);
                D.noGravity = true;
                D.velocity = new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(6.283) * scale;
            }
            for (int d = 0; d < 120; d += 1)
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        type = ModContent.DustType<Slingshots.Dusts.SapphireDust>(); break;
                    case 1:
                        type = ModContent.DustType<Slingshots.Dusts.EmeraldDust>(); break;
                    case 2:
                        type = ModContent.DustType<Slingshots.Dusts.EmeraldDust>(); break;
                    case 3:
                        type = ModContent.DustType<Slingshots.Dusts.EmeraldDust>(); break;
                }
                float scale = Main.rand.NextFloat(0.4f, 1.1f);
                Dust D = Dust.NewDustDirect(Projectile.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, scale);
                D.noGravity = true;
                D.velocity = new Vector2(0, Main.rand.NextFloat(20f)).RotatedByRandom(6.283) * scale;
            }
            Projectile.Kill();
        }
        public BlendState TrailBlendState()
        {
            return BlendState.AlphaBlend;
        }
        public string TrailShapeTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/Melee";
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            float power = Math.Max(StrikeOmegaDecrease - MathF.Pow(target.knockBackResist / 4f, 3), MinStrikeOmegaDecrease);

            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
            float ShakeStrength = Omega * 0.04f;
            Omega *= power;
            damage = (int)(damage / power);
            Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12f * ShakeStrength, 100)).RotatedByRandom(6.283);
            hitDirection = target.Center.X > Main.player[Projectile.owner].Center.X ? 1 : -1;
            knockback *= Omega * 3;
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
            if(Projectile.timeLeft >= 550)
            {
                Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
                MouseToPlayer = Vector2.Normalize(MouseToPlayer) * 15f;
                Vector2 vT0 = Main.MouseWorld - player.MountedCenter;
                Projectile.Center = player.MountedCenter + MouseToPlayer;
                Projectile.spriteDirection = player.direction;
                if(Projectile.timeLeft == 550)
                {
                    Projectile.velocity = Utils.SafeNormalize(vT0, Vector2.Zero) * 15;
                    Projectile.friendly = true;
                }
            }
            if (Projectile.timeLeft < 550 && Projectile.timeLeft > 500)
            {
                Projectile.velocity *= 0.985f;
            }
            if (Projectile.timeLeft < 500)
            {
                Projectile.velocity *= 0.96f;
                if(Projectile.timeLeft > 20 && Omega < 0.1f)
                {
                    Projectile.timeLeft = 20;
                }
            }
            Projectile.localNPCHitCooldown = (int)(MathF.PI / (Math.Max(Omega, 0.157)));

            Projectile.rotation += Omega;
            float MeleeSpeed = player.GetAttackSpeed(Projectile.DamageType);
            if (Projectile.timeLeft > 550)
            {
                if (Omega < MeleeSpeed * MaxOmega)
                {
                    Omega += Beta * MeleeSpeed * 4f;
                }
            }
            else
            {
                if (Projectile.timeLeft < 500)
                {
                    Omega *= 0.98f;
                }
                else
                {
                    if (Omega < MeleeSpeed * MaxOmega + 0.2f)
                    {
                        Omega += Beta * MeleeSpeed * 0.04f;
                    }
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

            ProduceWaterRipples(new Vector2(HitLength * Projectile.scale));
            float distance = 200f;
            
            if(target == -1)
            {
                foreach (var npc in Main.npc)
                {
                    if (npc.active)
                    {
                        if (npc.CanBeChasedBy())
                        {
                            if (!npc.dontTakeDamage)
                            {
                                if (!npc.friendly)
                                {
                                    if ((npc.Center - Projectile.Center).Length() < distance)
                                    {
                                        target = npc.whoAmI;
                                        distance = (npc.Center - Projectile.Center).Length();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(target >= 0)
            {
                if(!Main.npc[target].active)
                {
                    target = -1;
                    return;
                }
                Vector2 addV = Utils.SafeNormalize(Main.npc[target].Center - Projectile.Center, Vector2.Zero);
                Projectile.velocity = addV * Projectile.velocity.Length() * 0.15f + Projectile.velocity * 0.9f;

                if (Projectile.timeLeft > 100)
                {
                    if (Omega < MaxOmega)
                    {
                        Omega += Beta * 1f;
                    }
                }
            }
        }
        internal int target = -1;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            float colorValue = Omega / 0.4f;
            Color color = new Color(colorValue, colorValue, colorValue, colorValue);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0f);
            for (int i = 0; i < 5; i++)
            {
                float alp = Omega / 0.4f;
                Color color2 = new Color((int)((5 - i) / 5f * alp), (int)((5 - i) / 5f * alp), (int)((5 - i) / 5f * alp), 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation - i * 0.75f * Omega, texture.Size() / 2f, Projectile.scale, effects, 0f);
            }
            DrawTrail();
            PostPreDraw();
            return false;
        }

        public void PostPreDraw()
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
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2;
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w * 2f * Omega)));
            }
            bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
            bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.1f * Projectile.scale, Color.Black, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * Projectile.scale, Color.Black, new Vector3(factor, 0, w * 2f * Omega)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

            Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/CrystalClubTrail");
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(MythContent.QuickTexture("MiscItems/Weapons/Clubs/Projectiles/CrystalClub_fly"));
            Vector4 lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
            lightColor.W = 0.7f * Omega;
            MeleeTrail.Parameters["Light"].SetValue(lightColor);
            MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawTrail()
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

            float fade = Omega * 0.6f + 0.1f;
            Vector4 lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
            Color color2 = new Color(fade * 0.7f * lightColor.X, fade * 0.1f * lightColor.Y, fade * 0.4f * lightColor.Z, fade * 1.4f * lightColor.W);
            if(Projectile.timeLeft < 20)
            {
                color2 *= Projectile.timeLeft / 20f;
            }
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
            }
            bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
            bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
            color2 = new Color(fade * 0.1f * lightColor.X, fade * 0.5f * lightColor.Y, fade * 0.7f * lightColor.Z, fade * 1.4f * lightColor.W);
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MiscItems/Weapons/Clubs/Projectiles/CrystalClub_trail");

            lightColor.W = 0.7f * Omega;

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void DrawWarp(VFXBatch spriteBatch)
        {
            float warpvalue = Omega * 0.3f;
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
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(0, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(1, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                }
                if (dir1 - dir > 0.5)
                {
                    var MidValue = (1 - dir1) / (1 - dir1 + dir);
                    var MidPoint = MidValue * trail[i - 1] + (1 - MidValue) * trail[i];
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(1, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + MidPoint * Projectile.scale * 1.1f, new Color(0, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                }

                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
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
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(0, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(1, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                }
                if (dir1 - dir > 0.5)
                {
                    var MidValue = (1 - dir1) / (1 - dir1 + dir);
                    var MidPoint = MidValue * trail[i - 1] + (1 - MidValue) * trail[i];
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(1, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(1, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(0, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                    bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - MidPoint * Projectile.scale * 1.1f, new Color(0, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
                }

                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, new Color(dir, warpvalue, 0, 1), new Vector3(factor, 1, 1)));
                bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition - trail[i] * Projectile.scale * 1.1f, new Color(dir, warpvalue, 0, 1), new Vector3(factor, 0, 1)));
            }

            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/Warp").Value, bars, PrimitiveType.TriangleStrip);
        }
        public float TrailAlpha(float factor)
        {
            float w;
            w = MathHelper.Lerp(0f, 1, factor);
            return w;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            Vector2 HitRange = new Vector2(HitLength, HitLength * Projectile.spriteDirection).RotatedBy(Projectile.rotation) * Projectile.scale;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - HitRange, Projectile.Center + HitRange, 10 * HitLength / 32f * Omega / 0.3f, ref point) && Projectile.timeLeft < 550)
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
