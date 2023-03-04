﻿using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using IL.Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.LunarFlare
{
    internal class LunarFlareArray : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.LunarFlareBook)
            {
                Projectile.timeLeft = player.itemTime + 99;
                if (Timer < 99)
                {
                    Timer++;
                }
                if (Timer % 5 == 0)
                {
                    SubStars.Add(new SubStar(Projectile, SubStars.Count)
                    {
                        scalemax = Main.rand.NextFloat(2.4f, 3.6f),
                        scalelagger = true,
                        drawdelay = Main.rand.Next(25)
                    });
                }
            }
            else
            {
                Timer--;
                if (Timer < 0)
                {
                    Projectile.Kill();
                }
            }
            Projectile.scale = Timer / 99f;
            Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;

            player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
            Vector2 vTOMouse = Main.MouseWorld - player.Center;
            player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
            Projectile.rotation = player.fullRotation;

            if (Lighting.Mode != Terraria.Graphics.Light.LightMode.Color && Lighting.Mode != Terraria.Graphics.Light.LightMode.White)
            {
                return;
            }
            RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;
            Projectile.velocity = RingPos;
            for (int x = (int)(-Timer * 3.5f); x <= Timer * 3.5f; x += 8)
            {
                for (int y = (int)(-Timer * 3.5f); y <= Timer * 3.5f; y += 8)
                {
                    Vector2 AddRange = new Vector2(x, y);
                    if (AddRange.Length() < Timer * 3.5f)
                    {
                        Vector2 tPos = Projectile.Center + AddRange;
                        Tile tile = Main.tile[(int)(tPos.X / 16f), (int)(tPos.Y / 16f)];
                        if (tile.WallType == 0)
                        {
                            tile.WallType = (ushort)ModContent.WallType<Walls.NightEffectWall>();
                        }
                    }
                }
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.9f);
            for (int x = 0; x <= 4; x++)
            {
                Lighting.AddLight(Projectile.Center + new Vector2(0, Timer).RotatedBy(x / 2d * Math.PI - Main.timeForVisualEffects * 0.03f), 0.5f, 0.5f, 0.9f);
            }
            for (int x = 0; x <= 12; x++)
            {
                Lighting.AddLight(Projectile.Center + new Vector2(0, Timer * (1.4f + 0.9f * (x % 2))).RotatedBy(x / 6d * Math.PI + Main.timeForVisualEffects * 0.03f), 0.5f, 0.5f, 0.9f);
            }
            SubStars.ForEach(sub => sub.Update());
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            SubStars.ForEach(sub => sub.SendExtraAI(writer));
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            SubStars.ForEach(sub => sub.ReceiveExtraAI(reader));
        }

        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;
        internal List<SubStar> SubStars = new();
        internal class SubStar
        {
            static Texture2D Texture;
            const float k = 1;
            Projectile parent;
            internal int index;
            internal float rotation;
            internal float scale;
            internal float scalemax;
            internal bool scalelagger;
            internal int drawdelay;
            public SubStar(Projectile parent, int index)
            {
                //缓存父弹幕以便于确认中心位置
                this.parent = parent;
                //自身索引
                this.index = index;
                //在非服务器上请求图片
                if (Main.netMode != NetmodeID.Server)
                {
                    Texture ??= ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Projectiles/LunarFlare/Star", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                }
            }
            internal void SendExtraAI(BinaryWriter writer)
            {
                writer.Write(index);
                writer.Write(rotation);
                writer.Write(scale);
                writer.Write(scalemax);
                writer.Write(scalelagger);
                writer.Write(drawdelay);
            }
            internal void ReceiveExtraAI(BinaryReader reader)
            {
                index = reader.ReadInt32();
                rotation = reader.ReadSingle();
                scale = reader.ReadSingle();
                scalemax = reader.ReadSingle();
                scalelagger = reader.ReadBoolean();
                drawdelay = reader.ReadInt32();
            }
            internal void Update()
            {
                //如果不绘制则暂停更新
                if (drawdelay > 0)
                {
                    //减短绘制延迟
                    drawdelay--;
                    //如果绘制延迟结束重置随机起始角度
                    if (drawdelay == 0)
                    {
                        rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    }
                    return;
                }
                //更新旋转角度
                rotation += MathHelper.TwoPi / 120;
                if (scalelagger)
                {
                    //1秒内扩大到最大
                    scale += scalemax / 60;
                    //抵达最大值时兼并,变更scale变化方向
                    if (scale >= scalemax)
                    {
                        scale = scalemax;
                        scalelagger = false;
                    }
                }
                else
                {
                    //1秒内抵达最小
                    scale -= scalemax / 60;
                    //抵达最小时兼并,变更scale变化方向,重置随机最大尺寸
                    if (scale <= 0)
                    {
                        scale = 0;
                        scalelagger = true;
                        scalemax = Main.rand.NextFloat(2.4f, 3.6f);
                    }
                }
                //联机防异常的数值夹
                scale = Math.Clamp(scale, 0, scalemax);
                //scale为0说明已经完成依次绘制,赋予随机绘制延迟
                if (scale == 0)
                {
                    drawdelay = Main.rand.Next(15, 60);
                }
            }
            internal void Draw()
            {
                //绘制延迟(不应绘制)或者图片异常(不能绘制)时不绘制
                if (drawdelay > 0 || Texture is null)
                {
                    return;
                }
                //获得持有者玩家对象
                Player player = Main.player[parent.owner];
                //已玩家中心计算螺旋位置,基础偏移16像素(避免被玩家遮挡),每偏向外延展8*k像素
                Vector2 debugvector = player.MountedCenter + new Vector2(0, index * k * 8 + 16).RotatedBy(index * k) - Main.screenPosition;
                Main.spriteBatch.Draw(Texture,
                    debugvector,
                    null,
                    Color.Silver,
                    rotation,
                    Texture.Size() / 2,
                    scale,
                    SpriteEffects.None,
                    0);
            }
        }
    }
    internal class StarrySkySystem : ModSystem
    {
        public override void OnModLoad()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Everglow.HookSystem.AddMethod(DrawStarrySky, Commons.Core.CallOpportunity.PostDrawBG);
            }
        }

        public void DrawStarrySky()
        {
            if (Lighting.Mode != Terraria.Graphics.Light.LightMode.Color && Lighting.Mode != Terraria.Graphics.Light.LightMode.White)
            {
                return;
            }
            if (Main.WaveQuality < 3)
            {
                return;
            }
            //从RT池子里抓3个
            var renderTargets = Everglow.RenderTargetPool.GetRenderTarget2DArray(4);
            RenderTarget2D screen = renderTargets.Resource[0];
            RenderTarget2D StarryTarget = renderTargets.Resource[1];
            RenderTarget2D blackTarget = renderTargets.Resource[2];
            RenderTarget2D StarrySkyTarget = renderTargets.Resource[3];

            Effect Starry = MythContent.QuickEffect("Effects/StarrySkyZone");
            //保存原图
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
            graphicsDevice.SetRenderTarget(screen);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            //绘制黑域
            graphicsDevice.SetRenderTarget(blackTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LunarFlare/BlackSky");

            //抓捕缓存substar
            List<LunarFlareArray.SubStar> stars = new();
            foreach (Projectile p in Main.projectile)
            {
                if (p.type == ModContent.ProjectileType<LunarFlareArray>())
                {
                    Player player = Main.player[p.owner];
                    Vector2 drawCenter = player.Center + p.velocity - Main.screenPosition;
                    DrawShadowArea(tex, drawCenter, MathF.Sqrt(p.scale) * 0.5f);
                    stars.AddRange((p.ModProjectile as LunarFlareArray).SubStars);
                }
            }
            Main.spriteBatch.End();


            //绘制星空域
            graphicsDevice.SetRenderTarget(StarrySkyTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            tex = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LunarFlare/StarrySky");
            Main.spriteBatch.Draw(tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            //TODO:@SliverMoon把星星绘制在这里
            //绘制substar
            stars.ForEach(star => star.Draw());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);


            //在StarryTarget用Shader实现星空

            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Starry.Parameters["uTransform"].SetValue(projection);
            Starry.Parameters["tex2"].SetValue(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Perlin"));
            Starry.Parameters["uTime"].SetValue((float)(Main.timeForVisualEffects * 0.005f));
            Starry.Parameters["tex1"].SetValue(StarrySkyTarget);
            Starry.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.SetRenderTarget(StarryTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Draw(blackTarget, Vector2.Zero, Color.White);

            Main.spriteBatch.End();

            graphicsDevice.SetRenderTarget(Main.screenTarget);
            graphicsDevice.Clear(Color.Transparent);
            //叠加
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
            Main.spriteBatch.Draw(StarryTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            renderTargets.Release();
        }
        public static void DrawShadowArea(Texture2D tex, Vector2 drawCenter, float Scale)
        {
            Main.spriteBatch.Draw(tex, drawCenter, null, Color.White, 0, tex.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}