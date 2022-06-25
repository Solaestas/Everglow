using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public class ScaleWingBladeProj : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 3;
            trailLength = 20;
            shadertype = "Trail";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
        }
        public override float TrailAlpha(float factor)
        {
            return base.TrailAlpha(factor) * 1.5f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;

            float texWidth = 85;//转换成水平贴图时候的宽度
            float texHeight = 30;//转换成水平贴图时候的高度
            float Size = 0.95f;//放大的几何倍数
            double baseRotation = 0.79;//这个是刀刃倾斜度与水平的夹角

            float exScale = 1;
            if (longHandle)
            {
                exScale += 1f;
            }
            Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

            Vector2 Zoom = new Vector2(exScale * mainVec.Length() / tex.Width, 1.2f) * projectile.scale;

            double ProjRotation = mainVec.ToRotation() + Math.PI / 4;

            float QuarterSqrtTwo = 0.35355f;

            Vector2 drawCenter = projectile.Center - Main.screenPosition;
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

            if (Main.player[Projectile.owner].direction == -1)
            {
                sourceTopLeft = sourceBottomLeft;
                sourceTopRight = sourceBottomRight;
                sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
            }

            List<Vertex2D> vertex2Ds = new List<Vertex2D>
                {
                    new Vertex2D(drawCenter + TopLeft, projectile.GetAlpha(lightColor), new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, projectile.GetAlpha(lightColor), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),
                    new Vertex2D(drawCenter + BottomLeft, projectile.GetAlpha(lightColor), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),

                    new Vertex2D(drawCenter + BottomRight, projectile.GetAlpha(lightColor), new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + BottomLeft, projectile.GetAlpha(lightColor), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, projectile.GetAlpha(lightColor), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
                };

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void Attack()
        {
            Player player = Main.player[Projectile.owner];
            useTrail = true;
            if (attackType == 0)
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer == 30)
                    AttSound(SoundID.Item1);
                if (timer >30&&timer< 50)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.25f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
                }
                if (timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 1)
            {
                if (timer == 0)
                {
                    LockPlayerDir(player);
                    useTrail = false;
                    Projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.6f;
                }
                if (timer == 1)
                    AttSound(SoundID.Item1);
                if (timer < 20)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.22f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, -0.6f);
                }
                if (timer > 40)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                if (timer == 0 )
                {
                    LockPlayerDir(player);
                    useTrail = false;
                    Projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.1f;
                }
                if (timer < 60)
                {
                    isAttacking = true;
                    if (timer % 15 == 0)
                    {
                        AttSound(SoundID.Item1);
                        useTrail = false;
                    }
                    if (timer % 30 < 15)
                    {
                        Projectile.rotation += Projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, Projectile.rotation, 1f);
                    }
                    else
                    {
                        Projectile.rotation -= Projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, Projectile.rotation, -1f);
                    }

                }
                if (timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 3)
            {
                if (timer < 30)//前摇
                {
                    useTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.7f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += Projectile.DirectionFrom(player.Center) * 3;
                    Projectile.rotation = mainVec.ToRotation();
                }
                if (timer > 30 && timer < 40)
                {
                    isAttacking = true;
                    Projectile.rotation += Projectile.spriteDirection * 0.55f;
                    mainVec = Vector2Elipse(120, Projectile.rotation, -1.2f);
                }
                if (timer == 30)
                    AttSound(SoundID.Item1);
                if (timer > 30 && timer < 50)
                {

                }
                if (timer > 100)
                {
                    NextAttackType();
                }
            }
            if (isAttacking)
                for (int i = 1; i < 4; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center + i * mainVec / 3, 20, 20, 172, 0, 0, 0, default, 1.5f);
                    d.velocity *= 0;
                    d.noGravity = true;
                }
        }
    }
}

