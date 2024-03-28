using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Everglow.Commons.Shapes.Shape;

namespace Everglow.Commons.Shapes
{
	internal class Shape
	{
		public interface ISAT<T>
		{
			public List<T> GetAxes();
			public void Project(T axis, out float min, out float max);
		}
		public interface IGJK<T>
		{
			public T FurthestPoint(T dir);
		}
		public interface IEPA<T>:IGJK<T>
		{

		}
		public interface IConvex<T>
		{
			public ReadOnlySpan<T> ConvexVertex();
		}
	}
	public class NormalConvex2D : IConvex<Vector2>, IEPA<Vector2>
	{
		public NormalConvex2D(IEnumerable<Vector2> vs)
		{
			vertex = ShapeCollision.QuickHull(vs).ToArray();
		}
		protected readonly Vector2[] vertex;
		public ReadOnlySpan<Vector2> ConvexVertex()
		{
			return new ReadOnlySpan<Vector2>(vertex);
		}
		public Vector2 FurthestPoint(Vector2 dir)
		{
			Vector2 center = Vector2.Zero;
			vertex.ForEach(v => center += v);
			center /= vertex.Count;
			Vector2 result = vertex[0];
			Vector2 Vr = result - center;
			float d = Vector2.Dot(Vr, dir);
			for (int i = 1; i < vertex.Count; i++)
			{
				Vector2 Vi = vertex[i] - center;
				float di = Vector2.Dot(Vi, dir);
				if (di > d)
				{
					d = di;
					result = vertex[i];
				}
			}
			return result;
		}
	}
	internal class Circle : IConvex<Vector2>, ISAT<Vector2>, IEPA<Vector2>
	{
		private uint samplingNumber = 8;
		private float radius;
		public Vector2 Center;
		public uint SamplingNumber
		{
			get => samplingNumber;
			set => samplingNumber = Math.Max(value, 3);
		}
		public float Radius
		{
			get => radius;
			set => radius = Math.Max(value, float.Epsilon);
		}
		public ReadOnlySpan<Vector2> ConvexVertex()
		{
			Vector2[] result = new Vector2[samplingNumber];
			for (int i = 0; i < samplingNumber; i++)
			{
				float f = MathHelper.TwoPi / samplingNumber * i;
				result[i] = Center + new Vector2((float)Math.Cos(f), (float)Math.Sin(f)) * radius;
			}
			return new ReadOnlySpan<Vector2>(result);
		}
		public Vector2 FurthestPoint(Vector2 dir)
		{
			return Center + Vector2.Normalize(dir) * radius;
		}
		public List<Vector2> GetAxes()
		{
			var samplings = ConvexVertex();
			List<Vector2> axes = new();
			for (int i = 1; i < samplingNumber; i++)
			{
				axes.Add(samplings[i] - samplings[i - 1]);
			}
			axes.Add(samplings[0] - samplings[^1]);
			return axes;
		}
		public void Project(Vector2 axis, out float min, out float max)
		{
			min = float.MaxValue;
			max = float.MinValue;
			foreach (Vector2 point in ConvexVertex())
			{
				float projection = Vector2.Dot(point, axis);
				if (projection < min)
				{
					min = projection;
				}
				if (projection > max)
				{
					max = projection;
				}
			}
		}
		public static explicit operator NormalConvex2D(Circle circle)
		{
			return new NormalConvex2D(circle.ConvexVertex());
		}
	}
	internal class Triangle : IConvex<Vector2>, ISAT<Vector2>, IEPA<Vector2>
	{
		private readonly Vector2[] vertex = new Vector2[3];
		public ReadOnlySpan<Vector2> ConvexVertex()
		{
			return new ReadOnlySpan<Vector2>(vertex);
		}
		public Vector2 FurthestPoint(Vector2 dir)
		{
			Vector2 center = (vertex[0] + vertex[1] + vertex[2]) / 3;
			Vector2 result = vertex[0];
			Vector2 Vr = result - center;
			float d = Vector2.Dot(Vr, dir);
			for (int i = 1; i < 3; i++)
			{
				Vector2 Vi = vertex[i] - center;
				float di = Vector2.Dot(Vi, dir);
				if (di > d)
				{
					d = di;
					result = vertex[i];
				}
			}
			return result;
		}
		public List<Vector2> GetAxes()
		{
			return new List<Vector2>() { vertex[1] - vertex[0], vertex[2] - vertex[1], vertex[0] - vertex[2] };
		}
		public void Project(Vector2 axis, out float min, out float max)
		{
			min = float.MaxValue;
			max = float.MinValue;
			foreach (Vector2 point in vertex)
			{
				float projection = Vector2.Dot(point, axis);
				if (projection < min)
				{
					min = projection;
				}
				if (projection > max)
				{
					max = projection;
				}
			}
		}
		public static explicit operator NormalConvex2D(Triangle triangle)
		{
			return new NormalConvex2D(triangle.ConvexVertex());
		}
	}
	public class Rectangle : IConvex<Vector2>, ISAT<Vector2>, IEPA<Vector2>
	{
		private Vector2 leftTop;
		private float width, height;
		public Vector2 LeftTop => leftTop;
		public Vector2 RightTop => leftTop + new Vector2(width, 0);
		public Vector2 LeftBottom => leftTop + new Vector2(0, height);
		public Vector2 RightBottom => leftTop + new Vector2(width, height);
		public Rectangle(Vector2 pos, float width, float height)
		{
			leftTop = pos;
			this.width = width;
			this.height = height;
		}
		public Vector2 FurthestPoint(Vector2 dir)
		{
			Vector2 center = leftTop + new Vector2(width, height) / 2;
			Vector2 result = leftTop;
			Vector2 Vr = result - center;
			float d = Vector2.Dot(Vr, dir);
			foreach (Vector2 v in ConvexVertex())
			{
				Vector2 Vi = v - center;
				float di = Vector2.Dot(Vi, dir);
				if (di > d)
				{
					d = di;
					result = v;
				}
			}
			return result;
		}
		public List<Vector2> GetAxes()
		{
			return new List<Vector2>()
			{
				RightTop-LeftTop,
				RightBottom-RightTop,
				LeftBottom-RightBottom,
				LeftTop-LeftBottom
			};
		}
		public void Project(Vector2 axis, out float min, out float max)
		{
			min = float.MaxValue;
			max = float.MinValue;
			foreach (Vector2 point in ConvexVertex())
			{
				float projection = Vector2.Dot(point, axis);
				if (projection < min)
				{
					min = projection;
				}
				if (projection > max)
				{
					max = projection;
				}
			}
		}
		public ReadOnlySpan<Vector2> ConvexVertex()
		{
			Vector2[] array = new Vector2[] { LeftTop, RightTop, RightBottom, LeftBottom };
			return new ReadOnlySpan<Vector2>(array);
		}
		public static explicit operator Microsoft.Xna.Framework.Rectangle(Rectangle rect)
		{
			return new Microsoft.Xna.Framework.Rectangle((int)rect.leftTop.X, (int)rect.leftTop.Y, (int)rect.width, (int)rect.height);
		}
		public static explicit operator NormalConvex2D(Rectangle rect)
		{
			return new NormalConvex2D(new Vector2[] { rect.LeftTop, rect.RightTop, rect.RightBottom, rect.LeftBottom });
		}
	}
}
