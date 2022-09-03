using Everglow.Sources.Commons.Function.Vertex;

using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    public class AcytaeaLaserBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Laser");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "红光束");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 800;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        private int ControlP = -1;
        private float St = 0;
        private int AIMNpc = -1;
        private float Radius = 0;

        public override void AI()
        {
            if (Projectile.timeLeft < 60)
            {
                St *= 0.96f;
            }
            else
            {
                St += 40f;
            }
            if (AIMNpc < 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<NPCs.Acytaea>())
                    {
                        AIMNpc = i;
                        break;
                    }
                }
                if (AIMNpc != -1)
                {
                    Vector2 Vv = Vector2.Normalize(Main.npc[AIMNpc].Center - Projectile.Center).RotatedBy(Math.PI / 2d) * 1;
                    Projectile.velocity = Vv;
                    Radius = (Main.npc[AIMNpc].Center - Projectile.Center).Length();
                }
            }
            if (AIMNpc != -1)
            {
                if (Main.npc[AIMNpc].type == ModContent.NPCType<NPCs.Acytaea>() && Main.npc[AIMNpc].active)
                {
                    Projectile.velocity += Vector2.Normalize(Main.npc[AIMNpc].Center - Projectile.Center) * 1 / Radius;
                }
            }
            if (Projectile.timeLeft == 750)
            {
                Vector2 vp = Vector2.One;
                for (int f = 0; f < 200; f++)
                {
                    if (Main.npc[f].type == ModContent.NPCType<NPCs.Acytaea>())
                    {
                        vp = Main.npc[f].Center - Projectile.Center;
                        break;
                    }
                }

                float Rot = (float)Math.Atan2(vp.Y, vp.X) + (float)(Math.PI / 2d);
                ControlP = Projectile.NewProjectile(null, Projectile.Center, new Vector2(34, 0).RotatedBy(Projectile.ai[0]), ModContent.ProjectileType<AcytaeaLaser>(), 100, 3, Main.LocalPlayer.whoAmI, Rot);
            }
            if (ControlP != -1)
            {
                if (Main.projectile[ControlP].type == ModContent.ProjectileType<AcytaeaLaser>() && Main.projectile[ControlP].active)
                {
                    Main.projectile[ControlP].Center = Projectile.Center;
                }
            }
            St *= 0.99f;
        }

        public override void Kill(int timeLeft)
        {
        }

        private Effect ef2;

        public override void PostDraw(Color lightColor)
        {
            /*for (int z = 0; z < 7; z++)
            {
                if (Rota[z] == 0)
                {
                    Rota[z] = Main.rand.NextFloat(0, 6.283f);
                }
                if (Sca[z] == 0)
                {
                    Sca[z] = Main.rand.NextFloat(0.3f, 1.1f);
                }
                if (ARota[z] == 0)
                {
                    ARota[z] = Main.rand.NextFloat(0, 6.283f) / 40f;
                }
                Rota[z] += ARota[z];
            }
            float Timer = Projectile.timeLeft / 15f + 6;
            for (int z = 0; z < 7; z++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<Vertex2D> Vx = new List<Vertex2D>();

                for (int h = 0; h < 60; h++)
                {
                    float MinCosZ = (float)(2.4 - Math.Cos(Math.PI)) / 2.4f;
                    Vector2 vBla = new Vector2(120 * MinCosZ, 0).RotatedBy(Timer - h * 0.1f);
                    vBla.Y *= 0.3f;
                    Vector2 vb = Projectile.Center + (vBla + new Vector2(0, -St / 80f * Sca[z])).RotatedBy(0.4 + Rota[z]);
                    Vector2 vCla = new Vector2(120 * MinCosZ, 0).RotatedBy(Timer - (float)(Math.PI / 30d) - h * 0.1f);
                    vCla.Y *= 0.3f;
                    Vector2 vc = Projectile.Center + (vCla + new Vector2(0, -St / 80f * Sca[z])).RotatedBy(0.4 + Rota[z]);
                    Color color3 = new Color(255, 255, 255, 0);
                    if (Projectile.timeLeft < 255)
                    {
                        color3 = new Color(Projectile.timeLeft, Projectile.timeLeft, Projectile.timeLeft, 0);
                    }
                    if (Projectile.timeLeft > 945)
                    {
                        color3 = new Color(1200 - Projectile.timeLeft, 1200 - Projectile.timeLeft, 1200 - Projectile.timeLeft, 0);
                    }
                    Vx.Add(new Vertex2D(vc - Main.screenPosition + new Vector2(0, 0), color3, new Vector3((h + 1) / 60f, 0, 0)));
                    Vx.Add(new Vertex2D(vb - Main.screenPosition + new Vector2(0, 0), color3, new Vector3((h) / 60f, 0, 0)));
                    Vx.Add(new Vertex2D(Projectile.Center - Main.screenPosition + new Vector2(0, 0) + new Vector2(0, St / 160f * Sca[z]).RotatedBy(0.4 + Rota[z]), color3, new Vector3(0.5f, 1, 0)));
                }
                Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Projectiles/AcytaeaTornado5").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }*/
            ef2 = ModContent.Request<Effect>("MythMod/Effects/ef3/SpherePerspective3").Value;
            List<Vertex2D> triangleList2 = new List<Vertex2D>();
            int radius = (int)(St / 80f);//sss
            triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(radius, radius), Color.White, new Vector3(-1, 1, 0)));
            triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(radius, -radius), Color.White, new Vector3(-1, -1, 0)));
            triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(-radius, -radius), Color.White, new Vector3(1, -1, 0)));

            triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(radius, radius), Color.White, new Vector3(-1, 1, 0)));
            triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(-radius, -radius), Color.White, new Vector3(1, -1, 0)));
            triangleList2.Add(new Vertex2D(Projectile.Center - new Vector2(-radius, radius), Color.White, new Vector3(1, 1, 0)));

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            RasterizerState originalState2 = Main.graphics.GraphicsDevice.RasterizerState;
            // 干掉注释掉就可以只显示三角形栅格
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //rasterizerState.FillMode = FillMode.WireFrame;
            //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

            var projection2 = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model2 = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
            // 把变换和所需信息丢给shader
            ef2.Parameters["uTransform"].SetValue(model2 * projection2);
            ef2.Parameters["circleCenter"].SetValue(new Vector3(0, 0, -2));
            ef2.Parameters["radiusOfCircle"].SetValue(1f);
            ef2.Parameters["uTime"].SetValue((float)Main.time * 0.02f);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/VisualTextures/RedBall2").Value;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            ef2.CurrentTechnique.Passes[0].Apply();
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);
            Main.graphics.GraphicsDevice.RasterizerState = originalState2;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}