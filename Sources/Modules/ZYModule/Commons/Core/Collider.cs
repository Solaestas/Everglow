namespace Everglow.Sources.Modules.ZYModule.Commons.Core;

public static class CollisionUtils
{
    public const float Epsilon = 1e-4f;
    public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;
    public static Vector2 Abs(this Vector2 v) => new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
    public static CRectangle LineToAABB(Vector2 start, Vector2 end)
    {
        Vector2 min = Vector2.Min(start, end);
        Vector2 max = Vector2.Max(start, end);
        return new CRectangle(min, Vector2.Max(Vector2.One, max - min));
    }
    public static bool Intersect(float min1, float max1, float min2, float max2) => !(min1 >= max2 || min2 >= max1);
    public static bool LineIntersect(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out float factor)
    {
        factor = -1;
        if (!LineToAABB(start1, end1).Colliding(LineToAABB(start2, end2)))
        {
            return false;
        }

        factor = (end2 - start2).Cross(start1 - start2);
        float u2 = (end1 - start1).Cross(start1 - start2);
        float denom = (end2 - start2).Cross(end1 - start1);
        if (denom < Epsilon)
        {
            return false;
        }

        factor /= denom;
        u2 /= denom;
        if (0 < factor && factor < 1 && 0 < u2 && u2 < 1)
        {
            return true;
        }
        return false;
    }
    public static bool LineIntersect(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 point)
    {
        point = new Vector2();
        if (!LineToAABB(start1, end1).Colliding(LineToAABB(start2, end2)))
        {
            return false;
        }

        float u1 = (end2 - start2).Cross(start1 - start2);
        float u2 = (end1 - start1).Cross(start1 - start2);
        float denom = (end2 - start2).Cross(end1 - start1);
        if (denom < Epsilon)
        {
            return false;
        }

        u1 /= denom;
        u2 /= denom;
        if (0 < u1 && u1 < 1 && 0 < u2 && u2 < 1)
        {
            point = start1 + u1 * (end1 - start1);
            return true;
        }
        return false;
    }
    public static Vector2 Normalize_S(this Vector2 vector)
    {
        if (vector == Vector2.Zero)
        {
            return Vector2.UnitX;
        }
        else
        {
            return Vector2.Normalize(vector);
        }
    }
    public static Vector2 NormalLine(this Vector2 vec) => new Vector2(-vec.Y, vec.X).Normalize_S();
    public static float Projection(Vector2 dir, Vector2 vec)
    {
        return Vector2.Dot(dir.Normalize_S(), vec);
    }
    public static List<float> Projection(Vector2 dir, params Vector2[] vec)
    {
        List<float> vs = new List<float>(vec.Length);
        for (int i = 0; i < vec.Length; i++)
        {
            vs.Add(Projection(dir, vec[i]));
        }
        return vs;
    }
    public static List<float> Projection(Vector2 dir, IEnumerable<Vector2> vec)
    {
        List<float> vs = new List<float>();
        foreach (var v in vec)
        {
            vs.Add(Projection(dir, v));
        }
        return vs;
    }
    public static Vector2 MinValue(this IEnumerable<Vector2> vector2s)
    {
        var it = vector2s.GetEnumerator();
        Vector2 v = it.Current;
        while (it.MoveNext())
        {
            v = Vector2.Min(it.Current, v);
        }
        return v;
    }
    public static Vector2 MaxValue(this IEnumerable<Vector2> vector2s)
    {
        var it = vector2s.GetEnumerator();
        Vector2 v = it.Current;
        while (it.MoveNext())
        {
            v = Vector2.Max(it.Current, v);
        }
        return v;
    }
    internal static CRectangle ToCRectangle(this Rectangle rectangle) => new CRectangle(rectangle.TopLeft(), rectangle.Size());
}
public interface ICollider
{
    public bool Colliding(ICollider collider);
    public CRectangle AABB
    {
        get;
    }
}
public class CPoint : ICollider
{
    public Vector2 pos;

    public CPoint(Vector2 pos)
    {
        this.pos = pos;
    }

    public CRectangle AABB => new CRectangle(pos, Vector2.One);

    public bool Colliding(ICollider collider)
    {
        if (collider is null)
        {
            return false;
        }
        if (!collider.AABB.Contain(pos))
        {
            return false;
        }
        if (collider is CRectangle)
        {
            return true;
        }
        else if (collider is CCircle circle)
        {
            return circle.pos.Distance(pos) < circle.radius;
        }
        else if (collider is CPolygon polygon)
        {
            var edges = polygon.Edges;
            int sign = Math.Sign(edges[0].Cross(pos - polygon[0]));
            for (int i = 1; i < edges.Length; i++)
            {
                if (sign != Math.Sign(edges[i].Cross(pos - polygon[i])))
                {
                    return false;
                }
            }
            return true;
        }
        else if (collider is Colliders colliders)
        {
            foreach (var c in colliders.colliders)
            {
                if (collider.Colliding(c))
                {
                    return true;
                }
            }
        }
        else
        {
            return collider.Colliding(this);
        }
        return false;
    }
}
public class CRectangle : ICollider
{
    public Vector2 pos;
    public Vector2 size;
    public Vector2 TopLeft
    {
        get
        {
            return pos;
        }
        set
        {
            if (value.X > pos.X + size.X || value.Y > pos.Y + size.Y)
            {
                throw new ArgumentException("TopLeft set value is not correct", nameof(value));
            }

            size += pos - value;
            pos = value;
        }
    }

    public Vector2 TopRight
    {
        get
        {
            return pos + new Vector2(size.X, 0);
        }
        set
        {
            if (value.X < pos.X || value.Y > pos.Y + size.Y)
            {
                throw new ArgumentException("TopRight set value is not correct", nameof(value));
            }

            size.X += value.X - pos.X - size.X;
            size.Y += pos.Y - value.Y;
            pos = value - new Vector2(size.X, 0);
        }
    }

    public Vector2 BottomLeft
    {
        get
        {
            return pos + new Vector2(0, size.Y);
        }
        set
        {
            if (value.X > pos.X + size.X || value.Y < pos.Y)
            {
                throw new ArgumentException("BottomLeft set value is not correct", nameof(value));
            }

            size.X += pos.X - value.X;
            size.Y += value.Y - pos.Y;
            pos = value - new Vector2(0, size.Y);
        }
    }

    public Vector2 BottomRight
    {
        get
        {
            return pos + size;
        }
        set
        {
            if (value.X < pos.X || value.Y < pos.Y)
            {
                throw new ArgumentException("BottomRight set value is not correct", nameof(value));
            }

            size = value - pos;
        }
    }
    public Vector2 Size
    {
        get => size;
        set => size = value;
    }
    public Vector2 Center
    {
        get
        {
            return pos + size / 2;
        }
        set
        {
            pos = value - size / 2;
        }
    }


    public Vector2[] Edges
    {
        get
        {
            return new Vector2[] { pos, pos + new Vector2(size.X, 0), pos + new Vector2(0, size.Y), pos + size };
        }
    }

    public Rectangle Rectangle => new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
    public CRectangle AABB => this;
    public CRectangle()
    {
    }
    public CRectangle(float x, float y, float width, float height)
    {
        pos = new Vector2(x, y);
        size = new Vector2(width, height);
    }
    public CRectangle(Vector2 pos, Vector2 size)
    {
        this.pos = pos;
        this.size = size;
    }
    public bool Contain(Vector2 point)
    {
        return point.X > pos.X && point.X < pos.X + size.X && point.Y > pos.Y && point.Y < pos.Y + size.Y;
    }
    public bool Colliding(ICollider collider)
    {
        if (collider is null)
        {
            return false;
        }

        if (!collider.AABB.Colliding(this))
        {
            return false;
        }

        //矩形和矩形已经检验
        if (collider is CRectangle)
        {
            return true;
        }

        if (collider is CCircle circle)
        {
            return Colliding(circle);
        }

        if (collider is CPolygon polygon)
        {
            return Colliding(polygon);
        }

        return collider.Colliding(this);
        ;
    }
    /// <summary>
    /// ()
    /// </summary>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public bool Colliding(CRectangle rectangle)
    {
        if (rectangle is null)
        {
            return false;
        }

        if (rectangle.pos.X >= pos.X + size.X || rectangle.pos.X + rectangle.size.X <= pos.X ||
            rectangle.pos.Y >= pos.Y + size.Y || rectangle.pos.Y + rectangle.size.Y <= pos.Y)
        {
            return false;
        }

        return true;
    }
    public bool Colliding(CCircle circle)
    {
        if (circle is null)
        {
            return false;
        }

        Vector2 or = (circle.pos - (pos + size / 2)).Abs();
        if (or.X > size.X / 2)
        {
            if (or.Y > size.Y / 2)
            {
                return or.Distance(size / 2) < circle.radius;
            }
            else
            {
                return or.X - size.X / 2 < circle.radius;
            }
        }
        else
        {
            return or.Y - size.Y / 2 < circle.radius;
        }
    }
    public bool Colliding(CPolygon polygon)
    {
        foreach (var nor in polygon.Normals)
        {
            var p1 = CollisionUtils.Projection(nor, polygon.Vs);
            var p2 = CollisionUtils.Projection(nor, Edges);
            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
            {
                return false;
            }
        }
        foreach (var e in Edges)
        {
            Vector2 nor = e.NormalLine();
            var p1 = CollisionUtils.Projection(nor, polygon.Vs);
            var p2 = CollisionUtils.Projection(nor, Edges);
            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
            {
                return false;
            }
        }
        return true;
    }
}
public class CCircle : ICollider
{
    public Vector2 pos;
    public float radius;
    public CRectangle AABB => new CRectangle(pos - new Vector2(radius), pos + new Vector2(radius));
    public CCircle()
    {
    }

    public CCircle(Vector2 pos, float radius)
    {
        this.pos = pos;
        this.radius = radius;
    }

    public bool Colliding(ICollider collider)
    {
        if (collider is null)
        {
            return false;
        }

        if (!collider.AABB.Colliding(AABB))
        {
            return false;
        }

        if (collider is CRectangle rectangle)
        {
            return rectangle.Colliding(this);
        }

        if (collider is CCircle circle)
        {
            return Colliding(circle);
        }

        if (collider is CPolygon polygon)
        {
            return Colliding(polygon);
        }

        return collider.Colliding(this);
        ;
    }
    public bool Colliding(CRectangle rectangle)
    {
        if (rectangle is null)
        {
            return false;
        }

        return rectangle.Colliding(this);
    }
    public bool Colliding(CCircle circle)
    {
        if (circle is null)
        {
            return false;
        }

        return pos.Distance(circle.pos) < radius + circle.radius;
    }
    public bool Colliding(CPolygon polygon)
    {
        foreach (var nor in polygon.Normals)
        {
            var p1 = CollisionUtils.Projection(nor, polygon.Vs);
            float p = CollisionUtils.Projection(nor, pos);
            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p - radius, p + radius))
            {
                return false;
            }
        }
        return true;
    }
}
public class CTriangle : CPolygon
{
    internal Vector2 vertexA;
    internal Vector2 vertexB;
    internal Vector2 vertexC;
    public Vector2 VertexA
    {
        get => vertexA;
        set
        {
            vertexA = value;
            Initialize();
        }
    }
    public Vector2 VertexB
    {
        get => vertexB;
        set
        {
            vertexB = value;
            Initialize();
        }
    }
    public Vector2 VertexC
    {
        get => vertexC;
        set
        {
            vertexC = value;
            Initialize();
        }
    }

    public CTriangle()
    {
        vertices = new Vertices(3);
        Initialize();
    }

    public CTriangle(Vector2 vertexA, Vector2 vertexB, Vector2 vertexC)
    {
        this.vertexA = vertexA;
        this.vertexB = vertexB;
        this.vertexC = vertexC;
        vertices = new Vertices(vertexA, vertexB, vertexC);
        Initialize();
    }
}
public class CLineSegment : CPolygon
{
    internal Vector2 start;
    internal Vector2 end;
    internal float width;
    public Vector2 Start
    {
        get => start;
        set
        {
            start = value;
            vertices[0] = start + Normal * width;
            vertices[1] = start - Normal * width;
            Initialize();
        }
    }
    public Vector2 End
    {
        get => end;
        set
        {
            end = value;
            vertices[0] = end + Normal * width;
            vertices[1] = end - Normal * width;
            Initialize();
        }
    }
    public float Width
    {
        get => width;
        set
        {
            width = value;
            Vector2 normal = Normal;
            vertices = new Vertices(start + normal * width, start - normal * width, end + normal * width, end - normal * width);
            Initialize();
        }
    }
    public Vector2 Normal => new Vector2(end.Y - Start.Y, Start.X - end.X).Normalize_S();
    public CLineSegment()
    {
        vertices = new Vertices(4);
        Initialize();
    }

    public CLineSegment(Vector2 start, Vector2 end, float width)
    {
        Vector2 normal = (end - start).NormalLine();
        vertices = new Vertices(start + normal * width, start - normal * width, end + normal * width, end - normal * width);
        this.start = start;
        this.end = end;
        this.width = width;
        Initialize();
    }

    public Vector2 StartToEnd => end - Start;
    public float Length => StartToEnd.Length();
}
public class CPolygon : ICollider
{
    protected Vertices vertices;
    protected Vector2[] normals;

    public CRectangle AABB => vertices.AABB;

    public CPolygon()
    {
    }
    public CPolygon(IEnumerable<Vector2> vertices)
    {
        this.vertices = new Vertices(vertices);
        Initialize();
    }
    public CPolygon(params Vector2[] vertices)
    {
        this.vertices = new Vertices(vertices);
        Initialize();
    }
    public Vector2 this[int index]
    {
        get => vertices[index];
        set
        {
            vertices[index] = value;
            Initialize();
        }
    }
    public Vertices Vs => vertices;
    public Vector2[] Normals => normals;
    public Vector2[] Edges
    {
        get
        {
            Vector2[] edges = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                int next = (i + 1) % vertices.Count;
                edges[i] = vertices[next] - vertices[i];
            }
            return edges;
        }
    }
    public void RemoveAt(int index)
    {
        if (vertices.Count == 3)
        {
            throw new Exception("Triangle Can't Remove vertex");
        }

        vertices.RemoveAt(index);
        Initialize();
    }
    protected void Initialize()
    {
        if (!vertices.IsConvex())
        {
            return;
        }

        normals = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            normals[i] = (vertices[i] - vertices[i + 1 == vertices.Count ? 0 : i + 1]).NormalLine();
        }

    }
    public bool Colliding(ICollider collider)
    {
        if (collider is null)
        {
            return false;
        }

        if (!collider.AABB.Colliding(AABB))
        {
            return false;
        }

        if (collider is CRectangle rectangle)
        {
            return rectangle.Colliding(this);
        }
        if (collider is CCircle circle)
        {
            return circle.Colliding(AABB);
        }
        if (collider is CPolygon polygon)
        {
            return Colliding(polygon);
        }
        return collider.Colliding(this);
    }
    public bool Colliding(CPolygon polygon)
    {
        foreach (var nor in normals)
        {
            var p1 = CollisionUtils.Projection(nor, vertices);
            var p2 = CollisionUtils.Projection(nor, polygon.Vs);
            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
            {
                return false;
            }
        }
        foreach (var nor in polygon.Normals)
        {
            var p1 = CollisionUtils.Projection(nor, vertices);
            var p2 = CollisionUtils.Projection(nor, polygon.Vs);
            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
            {
                return false;
            }
        }
        return true;
    }
}
public class Colliders : ICollider
{
    public ICollider[] colliders;
    public CRectangle AABB
    {
        get
        {
            if (colliders is null || colliders.Length == 0)
            {
                return null;
            }

            CRectangle aabb = colliders[0].AABB;
            for (int i = 1; i < colliders.Length; i++)
            {
                CRectangle rect = colliders[i].AABB;
                Vector2[] vs = new Vector2[] { aabb.TopLeft, aabb.BottomRight, rect.TopLeft, rect.BottomRight };
                aabb = new CRectangle(vs.Min(), vs.Max());
            }
            return aabb;
        }
    }
    public Colliders(int capacity)
    {
        colliders = new ICollider[capacity];
    }
    public Colliders(ICollider[] colliders)
    {
        this.colliders = colliders;
    }

    public bool Colliding(ICollider collider)
    {
        foreach (ICollider collider2 in colliders)
        {
            if (collider.Colliding(collider2))
            {
                return true;
            }
        }
        return false;
    }
}
public class Vertices : List<Vector2>
{
    public Vertices() : base() { }
    public Vertices(int capacity) : base(capacity) { }
    public Vertices(IEnumerable<Vector2> vs) : base(vs) { }
    public Vertices(params Vector2[] vs) : base(vs) { }
    public CRectangle AABB
    {
        get
        {
            return new CRectangle(this.MinValue(), this.MaxValue());
        }
    }
    public bool IsConvex()
    {
        if (Count < 3)
        {
            return false;
        }

        if (Count == 3)
        {
            return true;
        }

        for (int i = 0; i < Count; i++)
        {
            int next = i + 1 < Count ? i + 1 : 0;
            Vector2 edge = this[next] - this[i];
            for (int j = 0; j < Count; j++)
            {
                if (i == j || next == j)
                {
                    continue;
                }

                Vector2 line = this[j] - this[j];
                if (line.Cross(edge) < 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

