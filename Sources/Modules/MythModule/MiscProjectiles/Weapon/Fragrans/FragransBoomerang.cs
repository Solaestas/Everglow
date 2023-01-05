using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Fragrans
{
    class FragransBoomerang : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 54;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        private bool Hit = true;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Hit)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 v = new Vector2(0, -17).RotatedBy(Main.rand.NextFloat(-2f, 2f));
                    int num2 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBoomerangBrake>(), Projectile.damage / 3 * 2, 0.2f, Main.LocalPlayer.whoAmI, 0f, 0f);
                    Main.projectile[num2].timeLeft = 200;
                    Main.projectile[num2].hostile = false;
                    Main.projectile[num2].friendly = true;
                }

                Hit = false;
            }
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Weapon.Fragrans.Fragrans>()] < 1)
            {
                //Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Weapon.Fragrans.Fragrans>(), 0, 0, player.whoAmI, 0, 0);
                player.AddBuff(ModContent.BuffType<MiscBuffs.Fragrans.MoonAndFragrans>(), 300);
            }
            else
            {
                MiscProjectiles.Weapon.Fragrans.Fragrans.Reset = 300;
                if (target.type == NPCID.TargetDummy)
                {
                    MiscProjectiles.Weapon.Fragrans.Fragrans.Dummy = true;
                }
            }
            for (int y = 0; y < 6; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 2; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.Fragrans>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 7.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int i = 0; i < 4; i++)
            {
                float ad = 0.3f;
                Vector2 velocity = new Vector2(0, Main.rand.NextFloat(4f, 9f));
                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), target.Center, velocity.RotatedByRandom(6.28) * Main.rand.NextFloat(0.85f, 1.15f), ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransMagicBeta>(), Projectile.damage / 2, knockback, player.whoAmI, ad * Main.rand.NextFloat(0.0f, 0.35f));
            }
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBomb>(), Projectile.damage / 2, knockback, player.whoAmI, 0);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
            if (Projectile.timeLeft > 2850)
            {
                if (Projectile.ai[1] != 0)
                {
                    return true;
                }
                Projectile.soundDelay = 10;
                if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                {
                    Projectile.velocity.X = oldVelocity.X * -0.9f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                {
                    Projectile.velocity.Y = oldVelocity.Y * -0.9f;
                }
            }
            return false;
        }
        float[] OldRo = new float[12];
        public override void AI()
        {
            OldRo[0] = Projectile.rotation;
            for (int i = 11; i > 0; i--)
            {
                OldRo[i] = OldRo[i - 1];
            }
            float num7 = Projectile.velocity.Length();
            Player p = Main.player[Projectile.owner];
            Projectile.rotation += 0.05f * num7;
            float num6 = (float)Math.Sqrt((p.Center.X - Projectile.Center.X) * (p.Center.X - Projectile.Center.X) + (p.Center.Y - Projectile.Center.Y) * (p.Center.Y - Projectile.Center.Y));
            if (Projectile.timeLeft <= 2850)
            {
                if (num7 < 5f)
                {
                    Projectile.velocity *= 1.05f;
                }
                if (num7 > 6f)
                {
                    Projectile.velocity *= 0.98f;
                }
                int num3 = (int)Player.FindClosest(Projectile.Center, 1, 1);
                Projectile.velocity = Projectile.velocity * 0.98f + (p.Center - Projectile.Center) / num6 * 3.5f;
                Projectile.tileCollide = false;
            }
            else
            {
                if (num7 < 27f)
                {
                    Projectile.velocity *= 1.05f;
                }
                if (num7 > 27.5f)
                {
                    Projectile.velocity *= 0.98f;
                }
                /*if (Projectile.timeLeft > 2985)
                {
                    Projectile.tileCollide = false;
                }
                else
                {
                    Projectile.tileCollide = true;
                }*/
            }
            if (num6 < 40 && num7 < 10)
            {
                Projectile.timeLeft = 0;
            }
            if (DelOme < 120)
            {
                DelOme += 3;
            }
            if (Main.rand.Next(6) == 1)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = Projectile.velocity * 0.5f;
            }
            if (Main.rand.Next(18) == 1)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = Projectile.velocity * 0.5f;
            }
            /*Player player = Main.LocalPlayer;
            if(player.wet)
            {
                Projectile.ignoreWater = true;
            }
            else
            {
                Projectile.ignoreWater = false;
            }*/
        }

        public override void Kill(int timeLeft)
        {
        }
        private int DelOme = 0;
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
            Texture2D tg = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Fragrans/FragransBoomerangGlow").Value;
            Main.spriteBatch.Draw(tg, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(34f, 39f), Projectile.scale, SpriteEffects.None, 0);
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
                Vector2 DrawPos = Projectile.oldPos[i] + new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                Color c0 = Lighting.GetColor((int)(DrawPos.X / 16f), (int)(DrawPos.Y / 16f));
                if (i % 3 == 0)
                {
                    Main.spriteBatch.Draw(texture, DrawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(c0.R * (10 - i) / 10f), (int)(c0.G * (10 - i) / 10f), (int)(c0.B * (10 - i) / 10f), (int)(255 * (10 - i) / 10f)), OldRo[i], new Vector2(34f, 39f), Projectile.scale, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tg, DrawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * (10 - i) / 10f), (int)(255 * (10 - i) / 10f), (int)(255 * (10 - i) / 10f), 0), OldRo[i], new Vector2(34f, 39f), Projectile.scale, SpriteEffects.None, 0);
                }
                Lighting.AddLight(DrawPos, (float)(255 - Projectile.alpha) * 0.4f / 500f * (10 - i) * 155f / 255f, (float)(255 - Projectile.alpha) * 0.4f / 500f * (10 - i) * 122f / 255f, (float)(255 - Projectile.alpha) * 0.4f / 500f * (10 - i) * 23f / 255f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            // 把所有的点都生成出来，按照顺序
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = (int)(120 - omega * 480);
            if (width <= 10)
            {
                width = 10;
            }
            if (width <= 30)
            {
                DelOme = width;
            }
            if (width > DelOme)
            {
                width = DelOme;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                //spriteBatch.Draw(Main.magicPixel, Projectile.oldPos[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);


                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);



                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(27, 27), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(27, 27), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                // 把变换和所需信息丢给shader
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline2").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/IceTrace").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

                ef.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
