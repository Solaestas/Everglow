namespace Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures
{
    [DebuggerDisplay("Angle = {angle}")]
    public struct Rotation : IComparable, IComparable<Rotation>, IConvertible, IEquatable<Rotation>, ISpanFormattable, IFormattable
    {
        private float angle;
        private float cos;
        private float sin;
        public float Angle
        {
            get => angle;
            set
            {
                angle = MathUtils.AngleWrap(value);
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
        public Vector2 XAxis => new Vector2(cos, sin);
        public Vector2 YAxis => new Vector2(-sin, cos);
        public Matrix RotationMatrix => Matrix.CreateRotationZ(angle);
        public Rotation()
        {
            angle = 0;
            cos = 1;
            sin = 0;
        }
        public Rotation(float angle)
        {
            this.angle = MathUtils.AngleWrap(angle);
            cos = MathUtils.Cos(angle);
            sin = MathUtils.Sin(angle);
        }
        public Rotation Lerp(Rotation to, float t) => Lerp(this, to, t);
        public static Rotation Lerp(Rotation from, Rotation to, float t)
        {
            int sign = Math.Sign(from.angle);
            sign = sign == 0 ? 1 : sign;
            to *= sign;
            to += to < from * sign - MathHelper.Pi ? MathHelper.TwoPi : 0;
            to *= sign;
            return from * (1 - t) + to * t;
        }
        public Rotation Lerp(Rotation to, float t, int dir, int n = 0) => Lerp(this, to, t, dir, n);
        public static Rotation Lerp(Rotation from, Rotation to, float t, int dir, int n = 0)
        {
            while (dir * (to - from) < MathHelper.TwoPi * n)
            {
                to += MathHelper.TwoPi * dir;
            }

            return from * (1 - t) + to * t;
        }
        #region Operator
        public static Rotation operator *(Rotation rot, float angle) => new Rotation(rot.angle * angle);
        public static Rotation operator *(float angle, Rotation rot) => rot * angle;
        public static Rotation operator *(Rotation a, Rotation b) => new Rotation(a.angle * b.angle);
        public static Rotation operator /(Rotation rot, float angle) => new Rotation(rot.angle / angle);
        public static Rotation operator /(float angle, Rotation rot) => new Rotation(angle / rot.angle);
        public static Vector2 operator *(Rotation rot, Vector2 vec) => new Vector2(vec.X * rot.Cos - vec.Y * rot.sin, vec.X * rot.Sin + vec.Y * rot.cos);
        public static Vector2 operator *(Vector2 vec, Rotation rot) => rot * vec;
        public static Vector2 operator /(Vector2 vec, Rotation rot) => new Vector2(vec.X * rot.Cos + vec.Y * rot.sin, -vec.X * rot.Sin + vec.Y * rot.cos);
        public static Rotation operator +(Rotation rot, float angle)
        {
            return new Rotation(rot.angle + angle);
        }
        public static Rotation operator +(float angle, Rotation rot) => rot + angle;
        public static Rotation operator +(Rotation a, Rotation b) => new Rotation(a.angle + b.angle);
        public static Rotation operator -(Rotation rot, float angle)
        {
            return new Rotation(rot.angle - angle);
        }
        public static Rotation operator -(float angle, Rotation rot)
        {
            return new Rotation(angle - rot.angle);
        }
        public static Rotation operator -(Rotation a, Rotation b) => new Rotation(a.angle - b.angle);
        public static Rotation operator -(Rotation rot) => new Rotation(-rot.angle);
        public static explicit operator float(Rotation rot) => rot.Angle;
        public static explicit operator Vector2(Rotation rot) => rot.XAxis;
        public static implicit operator Rotation(float v) => new Rotation(v);
        #endregion
        #region Compare
        public static bool operator ==(Rotation a, Rotation b) => a.angle == b.angle;
        public static bool operator !=(Rotation a, Rotation b) => a.angle != b.angle;
        public bool Equals(Rotation other)
        {
            return other.angle == angle;
        }
        public override bool Equals(object obj)
        {
            return obj is Rotation rotation && Equals(rotation);
        }
        public int CompareTo(Rotation other)
        {
            return angle.CompareTo(other.angle);
        }

        public int CompareTo(object obj)
        {
            return angle.CompareTo(obj);
        }
        public static bool operator <(Rotation left, Rotation right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Rotation left, Rotation right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Rotation left, Rotation right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Rotation left, Rotation right)
        {
            return left.CompareTo(right) >= 0;
        }

        #endregion
        #region Convert
        public TypeCode GetTypeCode()
        {
            return angle.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToBoolean(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToByte(provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToChar(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToDateTime(provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToDecimal(provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToDouble(provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToInt64(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToSByte(provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToSingle(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return angle.ToString(provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)angle).ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToUInt16(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToUInt32(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)angle).ToUInt64(provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return angle.ToString(format, formatProvider);
        }
        #endregion
        public override int GetHashCode()
        {
            return angle.GetHashCode();
        }
        public override string ToString()
        {
            return angle.ToString();
        }
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            return angle.TryFormat(destination, out charsWritten, format, provider);
        }
    }
}
