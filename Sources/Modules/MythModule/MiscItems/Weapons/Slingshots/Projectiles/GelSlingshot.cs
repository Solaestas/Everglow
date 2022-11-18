using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    internal class GelSlingshot : SlingshotProjectile
    {
        public override void SetDef()
        {
            ShootProjType = ModContent.ProjectileType<GelBall>();
        }
        public override void DrawString()
        {
            Player player = Main.player[Projectile.owner];
            Color drawColor = Lighting.GetColor((int)(Projectile.Center.X / 16.0), (int)(Projectile.Center.Y / 16.0));
            float DrawRot;
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                DrawRot = Projectile.rotation - MathF.PI / 4f;
            }
            else
            {
                DrawRot = Projectile.rotation - MathF.PI * 0.25f;
            }
            Vector2 HeadCenter = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot);
            if (player.direction == -1)
            {
                HeadCenter = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d);
            }
            HeadCenter += Projectile.Center - Main.screenPosition;
            Vector2 SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
            Vector2 SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Vector2.Normalize(SlingshotStringHead) * Power * 0.2625f;
            if (player.direction == -1)
            {
                SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
                SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(SlingshotStringHead) * Power * 0.2625f;
            }
            SlingshotStringTail += Projectile.Center - Main.screenPosition;
            Vector2 Head1 = HeadCenter + Utils.SafeNormalize(HeadCenter.RotatedBy(Math.PI / 8 + DrawRot), Vector2.Zero) * SplitBranchDis;
            Vector2 Head2 = HeadCenter - Utils.SafeNormalize(HeadCenter.RotatedBy(Math.PI / 8 + DrawRot), Vector2.Zero) * SplitBranchDis;
            if (player.direction == -1)
            {
                Head1 = HeadCenter + Utils.SafeNormalize(HeadCenter.RotatedBy(Math.PI / 8 * 5 + DrawRot), Vector2.Zero) * SplitBranchDis;
                Head2 = HeadCenter - Utils.SafeNormalize(HeadCenter.RotatedBy(Math.PI / 8 * 5 + DrawRot), Vector2.Zero) * SplitBranchDis;
            }
            Color Light = new Color(Power / 120f, Power / 260f, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            DrawTexLine(Head1, SlingshotStringTail, 1, drawColor, MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/GelString"));
            DrawTexLine(Head2, SlingshotStringTail, 1, drawColor, MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/GelString"));
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, float width, Color color, Texture2D tex)
        {
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * width;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            vertex2Ds.Add(new Vertex2D(StartPos + Width, color, new Vector3(0, 0, 0)));
            vertex2Ds.Add(new Vertex2D(StartPos - Width, color, new Vector3(0, 1, 0)));

            vertex2Ds.Add(new Vertex2D(EndPos + Width, color, new Vector3(1, 0, 0)));
            vertex2Ds.Add(new Vertex2D(EndPos - Width, color, new Vector3(1, 1, 0)));

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);
        }
    }
}
