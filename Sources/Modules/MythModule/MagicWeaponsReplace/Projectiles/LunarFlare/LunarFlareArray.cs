using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.YggdrasilModule.Common;

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

            RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;
            Projectile.velocity = RingPos;
            for (int x = (int)(-Timer * 3.5f); x <= Timer * 3.5f; x += 8)
            {
                for (int y = (int)(-Timer * 3.5f); y <= Timer * 3.5f; y += 8)
                {
                    Vector2 AddRange = new Vector2(x, y);
                    if(AddRange.Length() < Timer * 3.5f)
                    {
                        Vector2 tPos = Projectile.Center + AddRange;
                        Tile tile = Main.tile[(int)(tPos.X / 16f), (int)(tPos.Y / 16f)];
                        if(tile.WallType == 0)
                        {
                            tile.WallType = (ushort)ModContent.WallType<Walls.NightEffectWall>();
                        }
                    }
                }
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.9f);
            for (int x = 0; x <= 4; x ++)
            {
                Lighting.AddLight(Projectile.Center + new Vector2(0, Timer).RotatedBy(x / 2d * Math.PI - Main.timeForVisualEffects * 0.03f), 0.5f, 0.5f, 0.9f);
            }
            for (int x = 0; x <= 12; x++)
            {
                Lighting.AddLight(Projectile.Center + new Vector2(0, Timer * (1.4f + 0.9f * (x % 2))).RotatedBy(x / 6d * Math.PI + Main.timeForVisualEffects * 0.03f), 0.5f, 0.5f, 0.9f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;
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

            foreach (Projectile p in Main.projectile)
            {
                if (p.type == ModContent.ProjectileType<LunarFlareArray>())
                {
                    Player player = Main.player[p.owner];
                    Vector2 drawCenter = player.Center + p.velocity - Main.screenPosition;
                    DrawShadowArea(tex, drawCenter, MathF.Sqrt(p.scale) * 0.5f);
                }
            }
            Main.spriteBatch.End();


            //绘制星空域
            graphicsDevice.SetRenderTarget(StarrySkyTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            tex = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LunarFlare/StarrySky");
            //TODO:@SliverMoon把星星绘制在这里
            Main.spriteBatch.Draw(tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
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