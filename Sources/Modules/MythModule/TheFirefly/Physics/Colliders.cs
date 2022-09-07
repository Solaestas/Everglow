using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Physics
{
    internal interface ICollider2D
    {
        Vector3 GetSDFWithGradient(Vector2 position);
        bool Intersect(ICollider2D other);
        bool Contains(Vector2 position);
    }


    internal class AABBCollider2D : ICollider2D
    {
        public Vector2 Center
        {
            get;
            set;
        }
        public Vector2 Size
        {
            get;
            set;
        }
        private Vector3 sdgBox(Vector2 p, Vector2 b)
        {
            Vector2 w = new Vector2(Math.Abs(p.X), Math.Abs(p.Y)) - b;
            Vector2 s = new Vector2(p.X < 0.0 ? -1 : 1, p.Y < 0.0 ? -1 : 1);
            float g = Math.Max(w.X, w.Y);
            Vector2 q = new Vector2(Math.Max(w.X, 0.0f), Math.Max(w.Y, 0.0f));
            float l = q.Length();
            var v = s * ((g > 0.0) ? (q / l) : ((w.X > w.Y) ? new Vector2(1, 0) : new Vector2(0, 1)));
            return new Vector3((g > 0.0) ? l : g, v.X, v.Y);
        }
        public Vector3 GetSDFWithGradient(Vector2 pos)
        {
            sdgBox(new Vector2(0, -2), new Vector2(3, 3));
            return sdgBox(pos - Center, Size);
        }

        public bool Intersect(ICollider2D other)
        {
            return false;
        }

        public bool Contains(Vector2 position)
        {
            return position.X > Center.X - Size.X && position.X < Center.X + Size.X 
                && position.Y > Center.Y - Size.Y && position.Y < Center.Y + Size.Y;
        }
    }

    internal class TileTriangleCollider2D : ICollider2D
    {
        public Vector2 Center
        {
            get;
            set;
        }
        public Vector2 Size
        {
            get;
            set;
        }

        public Vector2 LineDir;
        private Vector3 sdgBox(Vector2 p, Vector2 b)
        {
            Vector2 w = new Vector2(Math.Abs(p.X), Math.Abs(p.Y)) - b;
            Vector2 s = new Vector2(p.X < 0.0 ? -1 : 1, p.Y < 0.0 ? -1 : 1);
            float g = Math.Max(w.X, w.Y);
            Vector2 q = new Vector2(Math.Max(w.X, 0.0f), Math.Max(w.Y, 0.0f));
            float l = q.Length();
            var v = s * ((g > 0.0) ? (q / l) : ((w.X > w.Y) ? new Vector2(1, 0) : new Vector2(0, 1)));
            return new Vector3((g > 0.0) ? l : g, v.X, v.Y);
        }

        private Vector3 sdgSmoothMin(Vector3 a, Vector3 b, float k)
        {
            float h = Math.Max(k - Math.Abs(a.X - b.X), 0.0f);
            float m = 0.25f * h * h / k;
            float n = 0.50f * h / k;
            var v = Vector2.Lerp(new Vector2(a.Y, a.Z), new Vector2(b.Y, b.Z), (a.X < b.X) ? n : 1.0f - n);
            return new Vector3(Math.Min(a.X, b.X) - m, v.X, v.Y);
        }

        private float cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        private Vector3 sdgLine(Vector2 p, Vector2 d)
        {
            // 直线的SDF
            d.Normalize();
            Vector2 proj = Vector2.Dot(d, p) * d;
            Vector2 N = p - proj;
            float x = Math.Sign(-cross(d, p)) * N.Length();
            N = N.SafeNormalize(new Vector2(-d.Y, d.X));
            return new Vector3(x, N.X, N.Y);
        }

        //private Vector3 sdgSegment(in Vector2 p, in Vector2 a, in Vector2 b)
        //{
        //    // 直线的SDF
        //    Vector2 proj = Vector2.Dot(d, p) / d.Length() * d;
        //    Vector2 N = p - proj;
        //    float x = Math.Sign(cross(d, p)) * N.Length();
        //    return new Vector3(x, N.X, N.Y);
        //}

        public Vector3 GetSDFWithGradient(Vector2 pos)
        {
            Vector2 p = pos - Center;
            Vector3 box = sdgBox(p, Size);
            Vector3 line = sdgLine(p, LineDir);
            if (box.X > line.X)
            {
                return box;
            }
            else
            {
                return line;
            }
        }

        public bool Intersect(ICollider2D other)
        {
            return false;
        }

        public bool Contains(Vector2 position)
        {
            return GetSDFWithGradient(position).X < 0;
        }
    }
}
