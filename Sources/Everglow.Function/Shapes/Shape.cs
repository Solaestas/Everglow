using static Everglow.Commons.Shapes.Shape;

namespace Everglow.Commons.Shapes
{
	public class Shape
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
	public class NormalConvex2D : IConvex<Vector2>, ISAT<Vector2>, IEPA<Vector2>
	{
		public static NormalConvex2D Create(Span<Vector2> vs)
		{
			return new NormalConvex2D()
			{
				vertex = ShapeCollision.QuickHull(vs.ToArray()).ToArray()
			};
		}
		protected Vector2[] vertex;
		public virtual ReadOnlySpan<Vector2> ConvexVertex()
		{
			return new ReadOnlySpan<Vector2>(vertex);
		}
		public virtual Vector2 FurthestPoint(Vector2 dir)
		{
			Vector2 center = Vector2.Zero;
			foreach (Vector2 v in vertex)
			{
				center += v;
			}
			center /= vertex.Length;
			Vector2 result = vertex[0];
			Vector2 Vr = result - center;
			float d = Vector2.Dot(Vr, dir);
			for (int i = 1; i < vertex.Length; i++)
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
		public virtual List<Vector2> GetAxes()
		{
			List<Vector2> axes = new();
			for(int i = 0; i < vertex.Length; i++)
			{
				axes.Add(vertex[i] - vertex[(i + 1) % vertex.Length]);
			}
			return axes;
		}
		public virtual void Project(Vector2 axis, out float min, out float max)
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
		public virtual void Move(Vector2 movement)
		{
			Matrix transform = Matrix.CreateTranslation(new Vector3(movement, 0));
			Transform(transform);
		}
		public virtual void Scale(Vector2? baseCenter, float scale)
		{
			if (baseCenter == null)
			{
				baseCenter = Vector2.Zero;
				for (int i = 0; i < vertex.Length; i++)
				{
					baseCenter += vertex[i];
				}
				baseCenter /= vertex.Length;
			}
			Matrix transform = Matrix.CreateTranslation(new Vector3(baseCenter.Value, 0)) * Matrix.CreateScale(scale, scale, 1) * Matrix.CreateTranslation(new Vector3(-baseCenter.Value, 0));
			Transform(transform);
		}
		public virtual void Rotate(Vector2? baseCenter, float radian)
		{
			if (baseCenter == null)
			{
				baseCenter = Vector2.Zero;
				for (int i = 0; i < vertex.Length; i++)
				{
					baseCenter += vertex[i];
				}
				baseCenter /= vertex.Length;
			}
			Matrix transform = Matrix.CreateTranslation(new Vector3(baseCenter.Value, 0)) * Matrix.CreateRotationZ(radian) * Matrix.CreateTranslation(new Vector3(-baseCenter.Value, 0));
			Transform(transform);
		}
		public virtual void Transform(Matrix transform)
		{
			for (int i = 0; i < vertex.Length; i++)
			{
				vertex[i] = Vector2.Transform(vertex[i], transform);
			}
		}
	}
	public class Circle : NormalConvex2D
	{
		public Circle(Vector2 center,float radius,int samplingNumber=8)
		{
			Center = center;
			Radius = radius;
			vertex = new Vector2[samplingNumber];
			for (int i = 0; i < samplingNumber; i++)
			{
				float f = MathHelper.TwoPi / samplingNumber * i;
				vertex[i] = Center + new Vector2((float)Math.Cos(f), (float)Math.Sin(f)) * radius;
			}
		}
		private float radius;
		public Vector2 Center;
		public int SamplingNumber
		{
			get => vertex.Length;
			set
			{
				int samplingNumber = Math.Max(value, 3);
				vertex = new Vector2[samplingNumber];
				for (int i = 0; i < samplingNumber; i++)
				{
					float f = MathHelper.TwoPi / samplingNumber * i;
					vertex[i] = Center + new Vector2((float)Math.Cos(f), (float)Math.Sin(f)) * radius;
				}
			}
		}
		public float Radius
		{
			get => radius;
			set => radius = Math.Max(value, float.Epsilon);
		}
		public override Vector2 FurthestPoint(Vector2 dir)
		{
			return Center + Vector2.Normalize(dir) * radius;
		}
		public override void Project(Vector2 axis, out float min, out float max)
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
		public override void Transform(Matrix transform)
		{
			base.Transform(transform);
			Center = Vector2.Transform(Center, transform);
		}
	}
	public class Triangle : NormalConvex2D
	{
		public Triangle(Vector2 v1, Vector2 v2, Vector2 v3, bool checkClockWise = false)
		{
			if (checkClockWise)
			{
				vertex = ShapeCollision.QuickHull(new Vector2[] { v1, v2, v3 }).ToArray();
			}
			else
			{
				vertex[0] = v1;
				vertex[1] = v2;
				vertex[2] = v3;
			}
		}
	}
	public class Rectangle : NormalConvex2D
	{
		private float width, height;
		public Vector2 LeftTop => vertex[0];
		public Vector2 RightTop => vertex[1];
		public Vector2 RightBottom => vertex[2];
		public Vector2 LeftBottom => vertex[3];
		public float Width
		{
			get => width;
			set
			{
				if (value == 0)
				{
					throw new ArgumentException("Can't be 0!");
				}
				width = value;
				vertex[1].X = vertex[0].X + value;
				vertex[2].X = vertex[0].X + value;
				if (width < 0)
				{
					(vertex[0], vertex[1]) = (vertex[1], vertex[0]);
					(vertex[2], vertex[3]) = (vertex[3], vertex[2]);
				}
			}
		}
		public float Height
		{
			get => height;
			set
			{
				if (value == 0)
				{
					throw new ArgumentException("Can't be 0!");
				}
				height = value;
				vertex[2].Y = vertex[0].Y + value;
				vertex[3].Y = vertex[0].Y + value;
				if (height < 0)
				{
					(vertex[0], vertex[3]) = (vertex[3], vertex[0]);
					(vertex[1], vertex[2]) = (vertex[2], vertex[1]);
				}
			}
		}
		public Rectangle(Vector2 pos, float width, float height)
		{
			this.width = width;
			this.height = height;
			vertex[0] = pos;
			vertex[1] = vertex[0] + new Vector2(width, 0);
			vertex[2] = vertex[0]+ new Vector2(width, height);
			vertex[3] = vertex[0] + new Vector2(0, height);
		}
		public static explicit operator Microsoft.Xna.Framework.Rectangle(Rectangle rect)
		{
			return new Microsoft.Xna.Framework.Rectangle((int)rect.vertex[0].X, (int)rect.vertex[0].Y, (int)rect.width, (int)rect.height);
		}
	}
}
