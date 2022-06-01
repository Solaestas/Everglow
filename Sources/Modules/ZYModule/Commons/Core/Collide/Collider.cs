using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
public abstract class Collider
{
    public abstract Vector2 Position { get; set; }
    public abstract AABB AABB { get; }
    public abstract bool Collision(Collider other, bool newCheck = true);
    public virtual Collider Clone()
    {
        return (Collider)MemberwiseClone();
    }
}
public class CPoint : Collider
{
    public CPoint(Vector2 position)
    {
        Position = position;
    }

    public CPoint()
    {
    }

    public override Vector2 Position { get; set; }
    public override AABB AABB => new AABB(Position, 1, 1);
    public override bool Collision(Collider other, bool newCheck = true)
    {
        if (newCheck && !other.AABB.Contain(Position))
        {
            return false;
        }

        if (other is CPoint)
        {
            return other.Position == Position;
        }
        else if (newCheck)
        {
            return other.Collision(this, false);
        }
        Everglow.Instance.Logger.Warn($"未定义的碰撞：{other.GetType()} 与 {this.GetType()}");
        return false;
    }
    public override Collider Clone()
    {
        return new CPoint(Position);
    }
}

public class CAABB : Collider
{
    public AABB aabb;

    public CAABB()
    {
    }
    public CAABB(Vector2 position, Vector2 size)
    {
        aabb = new AABB(position, size);
    }
    public CAABB(AABB aabb)
    {
        this.aabb = aabb;
    }

    public override Vector2 Position { get => aabb.position; set => aabb.position = value; }
    public override AABB AABB => aabb;
    public override bool Collision(Collider other, bool newCheck = true)
    {
        if (newCheck && !other.AABB.Intersect(aabb))
        {
            return false;
        }

        if (other is CPoint point)
        {
            return aabb.Contain(point.Position);
        }
        else if (other is CAABB)
        {
            return true;//AABB已经在粗碰撞检测过
        }
        else if (newCheck)
        {
            return other.Collision(this, false);
        }
        return false;
    }
    public override Collider Clone()
    {
        return new CAABB(aabb);
    }
}

public class CEdge : Collider
{
    public Edge edge;

    public CEdge()
    {
    }

    public CEdge(Edge edge)
    {
        this.edge = edge;
    }

    public override Vector2 Position { get => edge.begin; set => edge.begin = value; }
    public override AABB AABB => edge.ToAABB();

    public override bool Collision(Collider other, bool newCheck = true)
    {
        if (newCheck && !other.AABB.Intersect(edge))
        {
            return false;
        }

        if (other is CPoint)
        {
            float min = Math.Min(edge.begin.X, edge.end.X);
            float max = Math.Max(edge.begin.X, edge.end.X);
            return min <= other.Position.X && other.Position.X <= max && 
                (edge.begin - other.Position).Cross(edge.end - other.Position) < CollisionUtils.Epsilon;
        }
        else if (other is CAABB)
        {
            return true;
        }
        else if (other is CEdge edge)
        {
            return edge.edge.Intersect(this.edge);
        }
        else if (newCheck)
        {
            return other.Collision(this, false);
        }
        return false;
    }
    public override Collider Clone()
    {
        return new CEdge(edge);
    }
}
//public class CLine : ICollider
//{
//    public Vector2 begin;
//    public Vector2 end;

//    public CLine()
//    {
//    }

//    public CLine(Vector2 begin, Vector2 end)
//    {
//        this.begin = begin;
//        this.end = end;
//    }

//    public CRectangle AABB => CollisionUtils.LineToAABB(begin, end);

//    public bool Colliding(ICollider collider)
//    {
//        if (collider == null)
//        {
//            return false;
//        }
//        else if (collider is CPoint point)
//        {
//            if (Math.Abs((begin - point.pos).Cross(end - point.pos)) < CollisionUtils.Epsilon)
//            {
//                return true;
//            }
//        }
//        else if (collider is CRectangle rect)
//        {
//            Vector2 to = end - begin;
//            bool? side = null;
//            foreach (var p in rect.Vertices.Select(p => p - begin))
//            {
//                float n = to.Cross(p);
//                if (n == 0)//重合视作不相交
//                {
//                    continue;
//                }
//                bool t = n > 0;
//                if (side is null)
//                {
//                    side = t;
//                }
//                else if (side != t)
//                {
//                    return true;
//                }
//            }
//        }
//        else if (collider is CCircle circle)
//        {
//            Vector2 to = end - begin;
//            float dis = Math.Abs(to.Y * circle.pos.X - to.X * circle.pos.Y + end.X * begin.Y - begin.X * end.Y) / to.Length();
//            return dis < circle.radius;
//        }
//        else if (collider is CPolygon polygon)
//        {
//            throw new NotImplementedException();
//        }
//        else if (collider is Colliders colliders)
//        {
//            foreach (var c in colliders.colliders)
//            {
//                if (Colliding(c))
//                {
//                    return true;
//                }
//            }
//        }
//        else if (collider is CLine line)
//        {
//            return CollisionUtils.LineIntersect(line.begin, line.end, begin, end, out float _);
//        }
//        else
//        {
//            return collider.Colliding(this);
//        }
//        return false;
//    }
//}
//public class CPoint : ICollider
//{
//    public Vector2 pos;

//    public CPoint(Vector2 pos)
//    {
//        this.pos = pos;
//    }

//    public CRectangle AABB => new CRectangle(pos, Vector2.One);

//    public bool Colliding(ICollider collider)
//    {
//        if (collider is null)
//        {
//            return false;
//        }
//        if (!collider.AABB.Contain(pos))
//        {
//            return false;
//        }
//        if (collider is CRectangle)
//        {
//            return true;
//        }
//        else if (collider is CCircle circle)
//        {
//            return circle.pos.Distance(pos) < circle.radius;
//        }
//        else if (collider is CPolygon polygon)
//        {
//            var edges = polygon.Edges;
//            int sign = Math.Sign(edges[0].Cross(pos - polygon[0]));
//            for (int i = 1; i < edges.Length; i++)
//            {
//                if (sign != Math.Sign(edges[i].Cross(pos - polygon[i])))
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
//        else if (collider is Colliders colliders)
//        {
//            foreach (var c in colliders.colliders)
//            {
//                if (collider.Colliding(c))
//                {
//                    return true;
//                }
//            }
//        }
//        else
//        {
//            return collider.Colliding(this);
//        }
//        return false;
//    }
//}
//public class CRectangle : ICollider
//{
//    public Vector2 pos;
//    public Vector2 size;
//    public Vector2 TopLeft
//    {
//        get
//        {
//            return pos;
//        }
//        set
//        {
//            if (value.X > pos.X + size.X || value.Y > pos.Y + size.Y)
//            {
//                throw new ArgumentException("TopLeft set value is not correct", nameof(value));
//            }

//            size += pos - value;
//            pos = value;
//        }
//    }

//    public Vector2 TopRight
//    {
//        get
//        {
//            return pos + new Vector2(size.X, 0);
//        }
//        set
//        {
//            if (value.X < pos.X || value.Y > pos.Y + size.Y)
//            {
//                throw new ArgumentException("TopRight set value is not correct", nameof(value));
//            }

//            size.X += value.X - pos.X - size.X;
//            size.Y += pos.Y - value.Y;
//            pos = value - new Vector2(size.X, 0);
//        }
//    }

//    public Vector2 BottomLeft
//    {
//        get
//        {
//            return pos + new Vector2(0, size.Y);
//        }
//        set
//        {
//            if (value.X > pos.X + size.X || value.Y < pos.Y)
//            {
//                throw new ArgumentException("BottomLeft set value is not correct", nameof(value));
//            }

//            size.X += pos.X - value.X;
//            size.Y += value.Y - pos.Y;
//            pos = value - new Vector2(0, size.Y);
//        }
//    }

//    public Vector2 BottomRight
//    {
//        get
//        {
//            return pos + size;
//        }
//        set
//        {
//            if (value.X < pos.X || value.Y < pos.Y)
//            {
//                throw new ArgumentException("BottomRight set value is not correct", nameof(value));
//            }

//            size = value - pos;
//        }
//    }
//    public Vector2 Size
//    {
//        get => size;
//        set => size = value;
//    }
//    public Vector2 Center
//    {
//        get
//        {
//            return pos + size / 2;
//        }
//        set
//        {
//            pos = value - size / 2;
//        }
//    }


//    public Vertices Vertices => new Vertices() { pos, pos + new Vector2(size.X, 0), pos + new Vector2(0, size.Y), pos + size };

//    public Rectangle Rectangle => new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
//    public CRectangle AABB => this;
//    public CRectangle()
//    {
//    }
//    public CRectangle(float x, float y, float width, float height)
//    {
//        pos = new Vector2(x, y);
//        size = new Vector2(width, height);
//    }
//    public CRectangle(Vector2 pos, Vector2 size)
//    {
//        this.pos = pos;
//        this.size = size;
//    }
//    public bool Contain(Vector2 point)
//    {
//        return point.X > pos.X && point.X < pos.X + size.X && point.Y > pos.Y && point.Y < pos.Y + size.Y;
//    }
//    public bool Colliding(ICollider collider)
//    {
//        if (collider is null)
//        {
//            return false;
//        }

//        if (!collider.AABB.Colliding(this))
//        {
//            return false;
//        }

//        //矩形和矩形已经检验
//        if (collider is CRectangle)
//        {
//            return true;
//        }

//        if (collider is CCircle circle)
//        {
//            return Colliding(circle);
//        }

//        if (collider is CPolygon polygon)
//        {
//            return Colliding(polygon);
//        }

//        return collider.Colliding(this);
//    }
//    /// <summary>
//    /// ()
//    /// </summary>
//    /// <param name="rectangle"></param>
//    /// <returns></returns>
//    public bool Colliding(CRectangle rectangle)
//    {
//        if (rectangle is null)
//        {
//            return false;
//        }

//        if (rectangle.pos.X >= pos.X + size.X || rectangle.pos.X + rectangle.size.X <= pos.X ||
//            rectangle.pos.Y >= pos.Y + size.Y || rectangle.pos.Y + rectangle.size.Y <= pos.Y)
//        {
//            return false;
//        }

//        return true;
//    }
//    public bool Colliding(CCircle circle)
//    {
//        if (circle is null)
//        {
//            return false;
//        }

//        Vector2 or = (circle.pos - (pos + size / 2)).Abs();
//        if (or.X > size.X / 2)
//        {
//            if (or.Y > size.Y / 2)
//            {
//                return or.Distance(size / 2) < circle.radius;
//            }
//            else
//            {
//                return or.X - size.X / 2 < circle.radius;
//            }
//        }
//        else
//        {
//            return or.Y - size.Y / 2 < circle.radius;
//        }
//    }
//    public bool Colliding(CPolygon polygon)
//    {
//        foreach (var nor in polygon.Normals)
//        {
//            var p1 = CollisionUtils.Projection(nor, polygon.Vs);
//            var p2 = CollisionUtils.Projection(nor, Vertices);
//            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
//            {
//                return false;
//            }
//        }
//        foreach (var e in Vertices)
//        {
//            Vector2 nor = e.NormalLine();
//            var p1 = CollisionUtils.Projection(nor, polygon.Vs);
//            var p2 = CollisionUtils.Projection(nor, Vertices);
//            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//public class CCircle : ICollider
//{
//    public Vector2 pos;
//    public float radius;
//    public CRectangle AABB => new CRectangle(pos - new Vector2(radius), pos + new Vector2(radius));
//    public CCircle()
//    {
//    }

//    public CCircle(Vector2 pos, float radius)
//    {
//        this.pos = pos;
//        this.radius = radius;
//    }

//    public bool Colliding(ICollider collider)
//    {
//        if (collider is null)
//        {
//            return false;
//        }

//        if (!collider.AABB.Colliding(AABB))
//        {
//            return false;
//        }

//        if (collider is CRectangle rectangle)
//        {
//            return rectangle.Colliding(this);
//        }

//        if (collider is CCircle circle)
//        {
//            return Colliding(circle);
//        }

//        if (collider is CPolygon polygon)
//        {
//            return Colliding(polygon);
//        }

//        return collider.Colliding(this);
//        ;
//    }
//    public bool Colliding(CRectangle rectangle)
//    {
//        if (rectangle is null)
//        {
//            return false;
//        }

//        return rectangle.Colliding(this);
//    }
//    public bool Colliding(CCircle circle)
//    {
//        if (circle is null)
//        {
//            return false;
//        }

//        return pos.Distance(circle.pos) < radius + circle.radius;
//    }
//    public bool Colliding(CPolygon polygon)
//    {
//        foreach (var nor in polygon.Normals)
//        {
//            var p1 = CollisionUtils.Projection(nor, polygon.Vs);
//            float p = CollisionUtils.Projection(nor, pos);
//            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p - radius, p + radius))
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//public class CTriangle : CPolygon
//{
//    internal Vector2 vertexA;
//    internal Vector2 vertexB;
//    internal Vector2 vertexC;
//    public Vector2 VertexA
//    {
//        get => vertexA;
//        set
//        {
//            vertexA = value;
//            Initialize();
//        }
//    }
//    public Vector2 VertexB
//    {
//        get => vertexB;
//        set
//        {
//            vertexB = value;
//            Initialize();
//        }
//    }
//    public Vector2 VertexC
//    {
//        get => vertexC;
//        set
//        {
//            vertexC = value;
//            Initialize();
//        }
//    }

//    public CTriangle()
//    {
//        vertices = new Vertices(3);
//        Initialize();
//    }

//    public CTriangle(Vector2 vertexA, Vector2 vertexB, Vector2 vertexC)
//    {
//        this.vertexA = vertexA;
//        this.vertexB = vertexB;
//        this.vertexC = vertexC;
//        vertices = new Vertices(vertexA, vertexB, vertexC);
//        Initialize();
//    }
//}
//public class CLineSegment : CPolygon
//{
//    internal Vector2 start;
//    internal Vector2 end;
//    internal float width;
//    public Vector2 Start
//    {
//        get => start;
//        set
//        {
//            start = value;
//            vertices[0] = start + Normal * width;
//            vertices[1] = start - Normal * width;
//            Initialize();
//        }
//    }
//    public Vector2 End
//    {
//        get => end;
//        set
//        {
//            end = value;
//            vertices[0] = end + Normal * width;
//            vertices[1] = end - Normal * width;
//            Initialize();
//        }
//    }
//    public float Width
//    {
//        get => width;
//        set
//        {
//            width = value;
//            Vector2 normal = Normal;
//            vertices = new Vertices(start + normal * width, start - normal * width, end + normal * width, end - normal * width);
//            Initialize();
//        }
//    }
//    public Vector2 Normal => new Vector2(end.Y - Start.Y, Start.X - end.X).Normalize_S();
//    public CLineSegment()
//    {
//        vertices = new Vertices(4);
//        Initialize();
//    }

//    public CLineSegment(Vector2 start, Vector2 end, float width)
//    {
//        Vector2 normal = (end - start).NormalLine();
//        vertices = new Vertices(start + normal * width, start - normal * width, end + normal * width, end - normal * width);
//        this.start = start;
//        this.end = end;
//        this.width = width;
//        Initialize();
//    }

//    public Vector2 StartToEnd => end - Start;
//    public float Length => StartToEnd.Length();
//}
//public class CPolygon : ICollider
//{
//    protected Vertices vertices;
//    protected Vector2[] normals;

//    public CRectangle AABB => vertices.AABB;

//    public CPolygon()
//    {
//    }
//    public CPolygon(IEnumerable<Vector2> vertices)
//    {
//        this.vertices = new Vertices(vertices);
//        Initialize();
//    }
//    public CPolygon(params Vector2[] vertices)
//    {
//        this.vertices = new Vertices(vertices);
//        Initialize();
//    }
//    public Vector2 this[int index]
//    {
//        get => vertices[index];
//        set
//        {
//            vertices[index] = value;
//            Initialize();
//        }
//    }
//    public Vertices Vs => vertices;
//    public Vector2[] Normals => normals;
//    public Vector2[] Edges
//    {
//        get
//        {
//            Vector2[] edges = new Vector2[vertices.Count];
//            for (int i = 0; i < vertices.Count; i++)
//            {
//                int next = (i + 1) % vertices.Count;
//                edges[i] = vertices[next] - vertices[i];
//            }
//            return edges;
//        }
//    }
//    public void RemoveAt(int index)
//    {
//        vertices.RemoveAt(index);
//        Initialize();
//    }
//    protected void Initialize()
//    {
//        if (!vertices.IsConvex())
//        {
//            throw new Exception($"{this} is not convex");
//        }

//        normals = new Vector2[vertices.Count];
//        for (int i = 0; i < vertices.Count; i++)
//        {
//            normals[i] = (vertices[i] - vertices[i + 1 == vertices.Count ? 0 : i + 1]).NormalLine();
//        }

//    }
//    public bool Colliding(ICollider collider)
//    {
//        if (collider is null)
//        {
//            return false;
//        }

//        if (!collider.AABB.Colliding(AABB))
//        {
//            return false;
//        }

//        if (collider is CRectangle rectangle)
//        {
//            return rectangle.Colliding(this);
//        }
//        if (collider is CCircle circle)
//        {
//            return circle.Colliding(AABB);
//        }
//        if (collider is CPolygon polygon)
//        {
//            return Colliding(polygon);
//        }
//        return collider.Colliding(this);
//    }
//    public bool Colliding(CPolygon polygon)
//    {
//        foreach (var nor in normals)
//        {
//            var p1 = CollisionUtils.Projection(nor, vertices);
//            var p2 = CollisionUtils.Projection(nor, polygon.Vs);
//            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
//            {
//                return false;
//            }
//        }
//        foreach (var nor in polygon.Normals)
//        {
//            var p1 = CollisionUtils.Projection(nor, vertices);
//            var p2 = CollisionUtils.Projection(nor, polygon.Vs);
//            if (!CollisionUtils.Intersect(p1.Min(), p1.Max(), p2.Min(), p2.Max()))
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//public class Colliders : ICollider
//{
//    public ICollider[] colliders;
//    public CRectangle AABB
//    {
//        get
//        {
//            if (colliders is null || colliders.Length == 0)
//            {
//                return null;
//            }

//            CRectangle aabb = colliders[0].AABB;
//            for (int i = 1; i < colliders.Length; i++)
//            {
//                CRectangle rect = colliders[i].AABB;
//                Vector2[] vs = new Vector2[] { aabb.TopLeft, aabb.BottomRight, rect.TopLeft, rect.BottomRight };
//                aabb = new CRectangle(vs.Min(), vs.Max());
//            }
//            return aabb;
//        }
//    }
//    public Colliders(int capacity)
//    {
//        colliders = new ICollider[capacity];
//    }
//    public Colliders(ICollider[] colliders)
//    {
//        this.colliders = colliders;
//    }

//    public bool Colliding(ICollider collider)
//    {
//        foreach (ICollider collider2 in colliders)
//        {
//            if (collider.Colliding(collider2))
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//}
//public class Vertices : List<Vector2>
//{
//    public Vertices() : base() { }
//    public Vertices(int capacity) : base(capacity) { }
//    public Vertices(IEnumerable<Vector2> vs) : base(vs) { }
//    public Vertices(params Vector2[] vs) : base(vs) { }
//    public CRectangle AABB
//    {
//        get
//        {
//            return new CRectangle(this.MinValue(), this.MaxValue());
//        }
//    }
//    public bool IsConvex()
//    {
//        if (Count < 3)
//        {
//            return false;
//        }

//        if (Count == 3)
//        {
//            return true;
//        }

//        for (int i = 0; i < Count; i++)
//        {
//            int next = i + 1 < Count ? i + 1 : 0;
//            Vector2 edge = this[next] - this[i];
//            for (int j = 0; j < Count; j++)
//            {
//                if (i == j || next == j)
//                {
//                    continue;
//                }

//                Vector2 line = this[j] - this[j];
//                if (line.Cross(edge) < 0)
//                {
//                    return false;
//                }
//            }
//        }
//        return true;
//    }
//}

