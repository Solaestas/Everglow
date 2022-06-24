using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.Genshin.Projectiles
{
    public class PrimordialJadeWingedSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 180;
        }
        public override ModProjectile Clone(Projectile projectile)
        {
            var clone = base.Clone(projectile) as PrimordialJadeWingedSpear;
            return clone;
        }
        private Vector3 HeadPos;
        private Vector3 TailPos;
        private Vector3 Axis;
        private float size = 1000;
        /// <summary>
        /// 3D向量orig绕axis旋转theta
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="orig"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public Vector3 Pivot(Vector3 orig, Vector3 axis,  double theta)
        {
            axis = Vector3.Normalize(axis);
            return orig * (float)(Math.Cos(theta)) + (float)(1 - Math.Cos(theta)) * Vector3.Dot(axis, orig) * axis + Vector3.Cross(axis, orig) * (float)(Math.Sin(theta));
        }
        Vector2[] OldOutsidepos = new Vector2[60];
        Vector2[] OldInsidepos = new Vector2[60];
        public override void AI()
        {
            if(Projectile.timeLeft == 180)
            {
                HeadPos = new Vector3(-40, -60, 50);
                TailPos = new Vector3(20, 18, -10);
                Axis = new Vector3(1, -1, -0.43f);
            }
            HeadPos = Pivot(HeadPos, Axis, 0.05);
            TailPos = Pivot(TailPos, Axis, 0.05);
            if(Main.mouseLeft)
            {
                Projectile.timeLeft = 60;
            }
            OldOutsidepos[0] = Get2DDrawPos(HeadPos * 1.2f - TailPos * 0.2f, size);
            for (int f = OldOutsidepos.Length - 1; f > 0; f--)
            {
                OldOutsidepos[f] = OldOutsidepos[f - 1];
            }

            OldInsidepos[0] = Get2DDrawPos(HeadPos * 0.7f + TailPos * 0.3f, size);
            for (int f = OldInsidepos.Length - 1; f > 0; f--)
            {
                OldInsidepos[f] = OldInsidepos[f - 1];
            }
        }
        /// <summary>
        /// 透视投影
        /// </summary>
        /// <param name="Pos"></param>
        /// <param name="Size"></param>
        public Vector2 Get2DDrawPos(Vector3 Pos, float Size)
        {
            Player player = Main.player[Projectile.owner];
            Pos.Z += 400;
            return new Vector2(Pos.X / Pos.Z, Pos.Y / Pos.Z) * Size + player.Center - Main.screenPosition;
        }
        /// <summary>
        /// 通过给出的头尾两点和轴向量求出绘制面的四点(左右前后)
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetFourPoint(Vector3 head, Vector3 tail, Vector3 axis)
        {
            Vector3 DrawLeftPos = Pivot(head - tail, axis, -Math.PI / 2d) * 0.5f + (head + tail) * 0.5f;
            Vector3 DrawRightPos = Pivot(head - tail, axis, Math.PI / 2d) * 0.5f + (head + tail) * 0.5f;
            Vector3 DrawHeadPos = HeadPos;
            Vector3 DrawTailPos = TailPos;

            List<Vector2> fourPoint = new List<Vector2>();
            Vector2 Left = Get2DDrawPos(DrawLeftPos, size);
            Vector2 Right = Get2DDrawPos(DrawRightPos, size);
            Vector2 Head = Get2DDrawPos(DrawHeadPos, size);
            Vector2 Tail = Get2DDrawPos(DrawTailPos, size);
            fourPoint.Add(Left);
            fourPoint.Add(Right);
            fourPoint.Add(Head);
            fourPoint.Add(Tail);
            return fourPoint;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            List<Vector2> DrawPoint = GetFourPoint(HeadPos, TailPos, Axis);
            List<Vertex2D> Triangles = new List<Vertex2D>();
            Color light = Lighting.GetColor((int)(player.Center.X / 16d) , (int)(player.Center.Y / 16d));
            Triangles.Add(new Vertex2D(DrawPoint[1], light, new Vector3(1, 0, 0)));
            Triangles.Add(new Vertex2D(DrawPoint[2], light, new Vector3(0, 0, 0)));
            Triangles.Add(new Vertex2D(DrawPoint[3], light, new Vector3(1, 1, 0)));

            Triangles.Add(new Vertex2D(DrawPoint[0], light, new Vector3(0, 1, 0)));
            Triangles.Add(new Vertex2D(DrawPoint[2], light, new Vector3(0, 0, 0)));
            Triangles.Add(new Vertex2D(DrawPoint[3], light, new Vector3(1, 1, 0)));

            Texture2D t0 = MythContent.QuickTexture("Genshin/Projectiles/PrimordialJadeWingedSpear");
            Main.graphics.GraphicsDevice.Textures[0] = t0;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Triangles.ToArray(), 0, Triangles.Count / 3);

            List<Vertex2D> Trails = new List<Vertex2D>();
            Color pureGreen = new Color(0, 255, 0, 0);
            Color pureRed = new Color(255, 0, 0, 0);
            for (int i = 1; i < OldOutsidepos.Length - 2; i += 1)
            {
                if(OldOutsidepos[i + 1] == Vector2.Zero)
                {
                    break;
                }
                Trails.Add(new Vertex2D(OldOutsidepos[i], pureGreen, new Vector3(1, 0, 0)));
                Trails.Add(new Vertex2D(OldOutsidepos[i + 1], pureGreen, new Vector3(0, 0, 0)));
                Trails.Add(new Vertex2D(OldInsidepos[i], pureGreen, new Vector3(1, 1, 0)));

                Trails.Add(new Vertex2D(OldInsidepos[i], pureRed, new Vector3(0, 1, 0)));
                Trails.Add(new Vertex2D(OldInsidepos[i + 1], pureRed, new Vector3(1, 1, 0)));
                Trails.Add(new Vertex2D(OldOutsidepos[i + 1], pureRed, new Vector3(0, 0, 0)));
            }

            Texture2D t1 = MythContent.QuickTexture("Genshin/Projectiles/PureGreen");
            Main.graphics.GraphicsDevice.Textures[0] = t1;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Trails.ToArray(), 0, Trails.Count / 3);
            return false;
        }
    }
}
