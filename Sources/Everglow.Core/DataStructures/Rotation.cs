namespace Everglow.Core.DataStructures;

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
			angle = AngleWrap(value);
			cos = (float)Math.Cos(value);
			sin = (float)Math.Sin(value);
		}
	}
	public float HFilpAngle => angle switch
	{
		< 0 => -MathHelper.Pi - angle,
		_ => MathHelper.Pi - angle
	};
	public float VFilpAngle => -angle;
	public float Cos
	{
		get => cos;
		set
		{
			angle = (float)Math.Acos(value);
			cos = value;
			sin = (float)Math.Sin(angle);
		}
	}
	public float Sin
	{
		get => sin;
		set
		{
			angle = (float)Math.Asin(value);
			cos = (float)Math.Cos(value);
			sin = value;
		}
	}
	public Vector2 XAxis => new(cos, sin);
	public Vector2 YAxis => new(-sin, cos);
	public Matrix RotationMatrix => Matrix.CreateRotationZ(angle);
	public Rotation()
	{
		angle = 0;
		cos = 1;
		sin = 0;
	}
	public Rotation(float angle)
	{
		this.angle = AngleWrap(angle);
		cos = (float)Math.Cos(angle);
		sin = (float)Math.Sin(angle);
	}
	public void Reset()
	{
		angle = 0;
		cos = 1;
		sin = 0;
	}
	public static float AngleWrap(float angle)
	{
		while (angle >= MathHelper.Pi)
		{
			angle -= MathHelper.TwoPi;
		}

		while (angle < -MathHelper.Pi)
		{
			angle += MathHelper.TwoPi;
		}

		return angle;
	}
	#region Lerp
	/// <summary>
	/// 角度线性插值，选择就近的旋转方向
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public Rotation Lerp(Rotation to, float value)
	{
		if (Math.Abs(angle - to.angle) > MathHelper.Pi)
		{
			to.angle -= Math.Sign(to.angle) * MathHelper.TwoPi;
		}
		return angle * (1 - value) + to * value;
	}
	/// <summary>
	/// 角度线性插值，指定方向与圈数
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <param name="value"></param>
	/// <param name="direction">顺时针 ： 1， 逆时针 ： -1</param>
	/// <param name="turns">圈数</param>
	/// <returns></returns>
	public Rotation Lerp(Rotation to, float value, int direction = 1, int turns = 0)
	{
		while (direction * (to - angle) < MathHelper.TwoPi * turns)
		{
			to += MathHelper.TwoPi * direction;
		}

		return angle * (1 - value) + to * value;
	}
	#endregion
	public float Distance(Rotation rotation)
	{
		float dis = Math.Abs(angle - rotation.angle);
		return dis >= MathHelper.Pi ? MathHelper.TwoPi - dis : dis;
	}
	/// <summary>
	/// 选择就近的旋转方向
	/// </summary>
	/// <param name="target"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public Rotation Approach(Rotation target, float value)
	{
		float dis = Math.Abs(angle - target.angle);
		bool clockwise = dis < MathHelper.Pi;
		if ((clockwise ? dis : MathHelper.TwoPi - dis) <= value)
		{
			return target;
		}
		return new Rotation(angle + value * (clockwise ^ angle > target.angle ? 1 : -1));
	}
	#region Operator
	public static Rotation operator *(Rotation rot, float angle) => new(rot.angle * angle);
	public static Rotation operator *(float angle, Rotation rot) => rot * angle;
	public static Rotation operator *(Rotation a, Rotation b) => new(a.angle * b.angle);
	public static Rotation operator /(Rotation rot, float angle) => new(rot.angle / angle);
	public static Rotation operator /(float angle, Rotation rot) => new(angle / rot.angle);
	public static Vector2 operator *(Rotation rot, Vector2 vec) => new(vec.X * rot.Cos - vec.Y * rot.sin, vec.X * rot.Sin + vec.Y * rot.cos);
	public static Vector2 operator *(Vector2 vec, Rotation rot) => rot * vec;
	public static Vector2 operator /(Vector2 vec, Rotation rot) => new(vec.X * rot.Cos + vec.Y * rot.sin, -vec.X * rot.Sin + vec.Y * rot.cos);
	public static Rotation operator +(Rotation rot, float angle)
	{
		return new Rotation(rot.angle + angle);
	}
	public static Rotation operator +(float angle, Rotation rot) => rot + angle;
	public static Rotation operator +(Rotation a, Rotation b) => new(a.angle + b.angle);
	public static Rotation operator -(Rotation rot, float angle)
	{
		return new Rotation(rot.angle - angle);
	}
	public static Rotation operator -(float angle, Rotation rot)
	{
		return new Rotation(angle - rot.angle);
	}
	public static Rotation operator -(Rotation a, Rotation b) => new(a.angle - b.angle);
	public static Rotation operator -(Rotation rot) => new(-rot.angle);
	public static explicit operator float(Rotation rot) => rot.Angle;
	public static explicit operator Vector2(Rotation rot) => rot.XAxis;
	public static implicit operator Rotation(float v) => new(v);
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
