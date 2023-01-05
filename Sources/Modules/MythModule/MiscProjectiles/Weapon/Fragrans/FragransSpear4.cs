using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Fragrans
{
	public class FragransSpear4 : ModProjectile
    {
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 150f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Dosn't Shine");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "月色无芒");

        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.  
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.width = 80;
            Projectile.height = 80;
        }
        private bool fi = true;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            float duration = player.itemAnimationMax * 1.2f; // Define the duration the projectile will exist in frames
            player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

            // Reset projectile time left if necessary
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = (int)duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

            float halfDuration = duration * 0.5f;
            float progress;

            if (Projectile.timeLeft < halfDuration + 2 && !max)
            {
                //Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity * 15f, ModContent.ProjectileType<TuskSpear3>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                max = true;
            }
            // Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation. 
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }
            // Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            // Apply proper rotation to the sprite.
            if (Projectile.spriteDirection == -1)
            {
                // If sprite is facing left, rotate 45 degrees
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // If sprite is facing right, rotate 135 degrees
                Projectile.rotation += MathHelper.ToRadians(135f);
            }
            float dx = Projectile.timeLeft / (duration);
            if (Projectile.timeLeft > 6)
            {
                player.velocity = Projectile.velocity * 300f * dx * dx * dx * dx;
            }

            // Avoid spawning dusts on dedicated servers
            if (!Main.dedServ)
            {
            }
            if (fi)
            {
                sTp = player.Center;
                fi = false;
            }
            MiscProjectiles.Weapon.Fragrans.Fragrans.IMMUNE = 15;
            return false; // Don't execute vanilla AI.
        }
        private Effect ef;
        private bool max = false;
        private int pre = 0;
        private Vector2[] vpos = new Vector2[15];
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float a = Main.rand.NextFloat(0, 100f);
            Player player = Main.player[Projectile.owner];
            pre--;
            if (pre > -3)
            {
            }
            for (int y = 0; y < 10; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscDusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 3; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscDusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Weapon.Fragrans.Fragrans>()] < 1)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Weapon.Fragrans.Fragrans>(), 0, 0, player.whoAmI, 0, 0);
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
        }
        private Vector2 sTp;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            // 把所有的点都生成出来，按照顺序
            int width = 60;
            if (Projectile.timeLeft < 30)
            {
                width = Projectile.timeLeft * 3;
            }
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft == duration - 8)
            {
                Vector2 vp = (player.Center - sTp).RotatedBy(Math.PI / 2d);
                float Ve = vp.X * Projectile.velocity.X + vp.Y * Projectile.velocity.Y;
                if (Math.Abs(Ve) <= 5f)
                {
                    for (int i = 1; i < Projectile.oldPos.Length; ++i)
                    {
                        vpos[i] = Projectile.oldPos[i];
                    }
                }
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                if (vpos[i] != Vector2.Zero)
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    Vector2 deltaPos = Projectile.position - vpos[1];
                    bars.Add(new Vertex2D(vpos[i] + normalDir * width + new Vector2(47) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(vpos[i] + normalDir * -width + new Vector2(47) + deltaPos, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
                else
                {
                    var normalDir = Projectile.velocity;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)Projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(47), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(47), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                Vector2 vo = (bars[0].position - bars[1].position) / (bars[0].position - bars[1].position).Length();
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + vo.RotatedBy(-Math.PI / 2d) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                ef.Parameters["uTime"].SetValue(0);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine2").Value;
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
