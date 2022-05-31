namespace Everglow.Sources.Modules.ZYModule.Commons.Core
{
    public struct AABB
    {
        public Vector2 position;
        public Vector2 size;
        public float Top
        {
            get
            {
                return position.Y;
            }
            set
            {
                Debug.Assert(value <= position.Y + size.Y);

                size.Y = position.Y + size.Y - value;
                position.Y = value;
            }
        }
        public float Bottom
        {
            get
            {
                return position.Y + size.Y;
            }
            set
            {
                Debug.Assert(value >= position.Y);

                size.Y = value - position.Y;
            }
        }
        public float Left
        {
            get
            {
                return position.X;
            }
            set
            {
                Debug.Assert(value <= position.X + size.X);

                size.X = position.X + size.X - value;
                position.X = value;
            }
        }
        public float Right
        {
            get
            {
                return position.X + size.X;
            }
            set
            {
                Debug.Assert(value >= position.X);

                size.X = value - position.X;
            }
        }
        public float Width
        {
            get
            {
                return size.X;
            }
            set
            {
                size.X = value;
            }
        }
        public float Height
        {
            get
            {
                return size.Y;
            }
            set
            {
                size.Y = value;
            }
        }
        /// <summary>
        /// 右下角顶点位置不变，移动左上角顶点
        /// </summary>
        public Vector2 TopLeft
        {
            get
            {
                return position;
            }
            set
            {
                Debug.Assert(value.X <= position.X + size.X && value.Y <= position.Y + size.Y);

                size += position - value;
                position = value;
            }
        }
        public Vector2 TopRight
        {
            get
            {
                return position + new Vector2(size.X, 0);
            }
            set
            {
                Debug.Assert(value.X >= position.X && value.Y <= position.Y + size.Y);

                size.X += value.X - position.X - size.X;
                size.Y += position.Y - value.Y;
                position = value - new Vector2(size.X, 0);
            }
        }
        public Vector2 BottomLeft
        {
            get
            {
                return position + new Vector2(0, size.Y);
            }
            set
            {
                Debug.Assert(value.X <= position.X + size.X && value.Y >= position.Y);

                size.X += position.X - value.X;
                size.Y += value.Y - position.Y;
                position = value - new Vector2(0, size.Y);
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                return position + size;
            }
            set
            {
                Debug.Assert(value.X >= position.X && value.Y >= position.Y);

                size = value - position;
            }
        }
        public Vector2 Center
        {
            get
            {
                return position + size / 2;
            }
            set
            {
                position = value - size / 2;
            }
        }

        public AABB(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        public AABB(Vector2 position, float sizeX, float sizeY)
        {
            this.position = position;
            this.size = new Vector2(sizeX, sizeY);
        }
        public AABB(float x, float y, float sizeX, float sizeY)
        {
            this.position = new Vector2(x, y);
            this.size = new Vector2(sizeX, sizeY);
        }
        public override string ToString()
        {
            return $"position = {{{position.X}, {position.Y}}} size = {{{size.X}, {size.Y}}}";
        }
    }

    public struct Edge
    {
        public Vector2 begin;
        public Vector2 end;
        public Vector2 BeginToEnd => end - begin;
        public Vector2 NormalLine => new Vector2(end.Y - begin.Y, begin.X - end.Y);
        public Edge(Vector2 begin, Vector2 end)
        {
            this.begin = begin;
            this.end = end;
        }

        public override string ToString()
        {
            return $"begin = {{{begin.X}, {begin.Y}}} end = {{{end.X}, {end.Y}}}";
        }
    }


    public struct Rotation
    {
        private float angle;
        private float cos;
        private float sin;
        public float Angle
        {
            get => angle;
            set
            {
                angle = value;
                cos = MathUtils.Cos(value);
                sin = MathUtils.Sin(value);
            }
        }
        public float Cos
        {
            get => cos;
            set
            {
                angle = (float)Math.Acos(value);
                cos = value;
                sin = MathUtils.Sin(angle);
            }
        }
        public float Sin
        {
            get => sin;
            set
            {
                angle = (float)Math.Asin(value);
                cos = MathUtils.Sin(angle);
                sin = value;
            }
        }
        public Rotation(float angle)
        {
            this.angle = angle;
            cos = MathUtils.Cos(angle);
            sin = MathUtils.Sin(angle);
        }
        public static Rotation operator +(Rotation rot, float angle)
        {
            return new Rotation(rot.angle + angle);
        }
        public static Rotation operator +(float angle, Rotation rot) => rot + angle;
        public static Rotation operator -(Rotation rot, float angle)
        {
            return new Rotation(rot.angle - angle);
        }
        public static Rotation operator -(float angle, Rotation rot)
        {
            return new Rotation(angle - rot.angle);
        }
        public static Rotation operator -(Rotation rot) => new Rotation(-rot.angle);
        public static implicit operator float(Rotation rot) => rot.Angle;
    }
}
