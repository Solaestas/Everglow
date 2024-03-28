using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Shapes
{
	public class TriangleWeb_Delaunay
	{
		private static TriangleWeb_Delaunay instance;
		public static TriangleWeb_Delaunay Instance
		{
			get
			{
				instance ??= new();
				return instance;
			}
		}

		private int highest = -1;
		private IList<Vector2> verts;
		private readonly List<int> indices;
		private readonly List<TriangleNode> triangles;
		public TriangleWeb_Delaunay()
		{
			triangles = new List<TriangleNode>();
			indices = new List<int>();
		}
		public DelaunayTriangulation CalculateTriangulation(IList<Vector2> verts)
		{
			DelaunayTriangulation result = null;
			CalculateTriangulation(verts, ref result);
			return result;
		}
		public void CalculateTriangulation(IList<Vector2> verts, ref DelaunayTriangulation result)
		{
			if (verts == null)
			{
				throw new ArgumentNullException("points");
			}
			if (verts.Count < 3)
			{
				throw new ArgumentException("You need at least 3 points for a triangulation");
			}
			triangles.Clear();
			this.verts = verts;
			highest = 0;
			for (int i = 0; i < verts.Count; i++)
			{
				if (Higher(highest, i))
				{
					highest = i;
				}
			}
			triangles.Add(new TriangleNode(-2, -1, highest));
			RunBowyerWatson();
			GenerateResult(ref result);
			this.verts = null;
		}
		public void CalculateTriangulation(IList<Vector2> verts, out List<Triangle> triangles)
		{
			triangles = CalculateTriangulation(verts).TransToTriangles();
		}
		public void CalculateTriangulation(IList<Vector2> verts, ref DelaunayTriangulation result, out List<Triangle> triangles)
		{
			CalculateTriangulation(verts, ref result);
			triangles = result.TransToTriangles();
		}

		private bool Higher(int pi0, int pi1)
		{
			if (pi0 == -2)
			{
				return false;
			}
			else if (pi0 == -1)
			{
				return true;
			}
			else if (pi1 == -2)
			{
				return true;
			}
			else if (pi1 == -1)
			{
				return false;
			}
			else
			{
				var p0 = verts[pi0];
				var p1 = verts[pi1];
				return p0.Y < p1.Y || p0.Y <= p1.Y && p0.X < p1.X;
			}
		}

		private void RunBowyerWatson()
		{
			for (int i = 0; i < verts.Count; i++)
			{
				var pi = i;
				if (pi == highest)
				{
					continue;
				}

				var ti = FindTriangleNode(pi);
				var t = triangles[ti];
				var p0 = t.P0;
				var p1 = t.P1;
				var p2 = t.P2;
				var nti0 = triangles.Count;
				var nti1 = nti0 + 1;
				var nti2 = nti0 + 2;
				var nt0 = new TriangleNode(pi, p0, p1);
				var nt1 = new TriangleNode(pi, p1, p2);
				var nt2 = new TriangleNode(pi, p2, p0);
				nt0.A0 = t.A2;
				nt1.A0 = t.A0;
				nt2.A0 = t.A1;
				nt0.A1 = nti1;
				nt1.A1 = nti2;
				nt2.A1 = nti0;
				nt0.A2 = nti2;
				nt1.A2 = nti0;
				nt2.A2 = nti1;
				t.C0 = nti0;
				t.C1 = nti1;
				t.C2 = nti2;
				triangles[ti] = t;
				triangles.Add(nt0);
				triangles.Add(nt1);
				triangles.Add(nt2);
				if (nt0.A0 != -1)
				{
					LegalizeEdge(nti0, nt0.A0, pi, p0, p1);
				}

				if (nt1.A0 != -1)
				{
					LegalizeEdge(nti1, nt1.A0, pi, p1, p2);
				}

				if (nt2.A0 != -1)
				{
					LegalizeEdge(nti2, nt2.A0, pi, p2, p0);
				}
			}
		}

		private void GenerateResult(ref DelaunayTriangulation result)
		{
			result ??= new DelaunayTriangulation();
			result.Clear();
			for (int i = 0; i < verts.Count; i++)
			{
				result.Vertices.Add(verts[i]);
			}
			for (int i = 1; i < triangles.Count; i++)
			{
				var t = triangles[i];
				if (t.IsLeaf && t.IsInner)
				{
					result.Triangles.Add(t.P0);
					result.Triangles.Add(t.P1);
					result.Triangles.Add(t.P2);
				}
			}
		}

		private void ShuffleIndices()
		{
			indices.Clear();
			indices.Capacity = verts.Count;
			for (int i = 0; i < verts.Count; i++)
			{
				indices.Add(i);
			}
			Debug.Assert(indices.Count == verts.Count);
			for (int i = 0; i < verts.Count - 1; i++)
			{
				var j = Main.rand.Next(i, verts.Count);
				var tmp = indices[i];
				indices[i] = indices[j];
				indices[j] = tmp;
			}
		}

		private int LeafWithEdge(int ti, int e0, int e1)
		{
			Debug.Assert(triangles[ti].HasEdge(e0, e1));
			while (!triangles[ti].IsLeaf)
			{
				var t = triangles[ti];
				if (t.C0 != -1 && triangles[t.C0].HasEdge(e0, e1))
				{
					ti = t.C0;
				}
				else if (t.C1 != -1 && triangles[t.C1].HasEdge(e0, e1))
				{
					ti = t.C1;
				}
				else if (t.C2 != -1 && triangles[t.C2].HasEdge(e0, e1))
				{
					ti = t.C2;
				}
				else
				{
					Debug.Assert(false);
					throw new Exception("This should never happen");
				}
			}
			return ti;
		}

		private bool LegalEdge(int k, int l, int i, int j)
		{
			Debug.Assert(k != highest && k >= 0);
			var lMagic = l < 0;
			var iMagic = i < 0;
			var jMagic = j < 0;
			Debug.Assert(!(iMagic && jMagic));
			if (lMagic)
			{
				return true;
			}
			else if (iMagic)
			{
				Debug.Assert(!jMagic);
				var p = verts[l];
				var l0 = verts[k];
				var l1 = verts[j];
				return Geom.ToTheLeft(p, l0, l1);
			}
			else if (jMagic)
			{
				Debug.Assert(!iMagic);
				var p = verts[l];
				var l0 = verts[k];
				var l1 = verts[i];
				return !Geom.ToTheLeft(p, l0, l1);
			}
			else
			{
				Debug.Assert(k >= 0 && l >= 0 && i >= 0 && j >= 0);
				var p = verts[l];
				var c0 = verts[k];
				var c1 = verts[i];
				var c2 = verts[j];
				Debug.Assert(Geom.ToTheLeft(c2, c0, c1));
				Debug.Assert(Geom.ToTheLeft(c2, c1, p));
				return !Geom.InsideCircumcircle(p, c0, c1, c2);
			}
		}

		private void LegalizeEdge(int ti0, int ti1, int pi, int li0, int li1)
		{
			ti1 = LeafWithEdge(ti1, li0, li1);
			var t0 = triangles[ti0];
			var t1 = triangles[ti1];
			var qi = t1.OtherPoint(li0, li1);
			Debug.Assert(t0.HasEdge(li0, li1));
			Debug.Assert(t1.HasEdge(li0, li1));
			Debug.Assert(t0.IsLeaf);
			Debug.Assert(t1.IsLeaf);
			Debug.Assert(t0.P0 == pi || t0.P1 == pi || t0.P2 == pi);
			Debug.Assert(t1.P0 == qi || t1.P1 == qi || t1.P2 == qi);
			if (!LegalEdge(pi, qi, li0, li1))
			{
				var ti2 = triangles.Count;
				var ti3 = ti2 + 1;
				var t2 = new TriangleNode(pi, li0, qi);
				var t3 = new TriangleNode(pi, qi, li1);
				t2.A0 = t1.Opposite(li1);
				t2.A1 = ti3;
				t2.A2 = t0.Opposite(li1);
				t3.A0 = t1.Opposite(li0);
				t3.A1 = t0.Opposite(li0);
				t3.A2 = ti2;
				triangles.Add(t2);
				triangles.Add(t3);
				var nt0 = triangles[ti0];
				var nt1 = triangles[ti1];
				nt0.C0 = ti2;
				nt0.C1 = ti3;
				nt1.C0 = ti2;
				nt1.C1 = ti3;
				triangles[ti0] = nt0;
				triangles[ti1] = nt1;
				if (t2.A0 != -1)
				{
					LegalizeEdge(ti2, t2.A0, pi, li0, qi);
				}

				if (t3.A0 != -1)
				{
					LegalizeEdge(ti3, t3.A0, pi, qi, li1);
				}
			}
		}

		private int FindTriangleNode(int pi)
		{
			var curr = 0;
			while (!triangles[curr].IsLeaf)
			{
				var t = triangles[curr];
				curr = t.C0 >= 0 && PointInTriangle(pi, t.C0) ? t.C0 : t.C1 >= 0 && PointInTriangle(pi, t.C1) ? t.C1 : t.C2;
			}
			return curr;
		}

		private bool PointInTriangle(int pi, int ti)
		{
			var t = triangles[ti];
			return ToTheLeft(pi, t.P0, t.P1)
				&& ToTheLeft(pi, t.P1, t.P2)
				&& ToTheLeft(pi, t.P2, t.P0);
		}

		private bool ToTheLeft(int pi, int li0, int li1)
		{
			if (li0 == -2)
			{
				return Higher(li1, pi);
			}
			else if (li0 == -1)
			{
				return Higher(pi, li1);
			}
			else if (li1 == -2)
			{
				return Higher(pi, li0);
			}
			else if (li1 == -1)
			{
				return Higher(li0, pi);
			}
			else
			{
				Debug.Assert(li0 >= 0);
				Debug.Assert(li1 >= 0);
				return Geom.ToTheLeft(verts[pi], verts[li0], verts[li1]);
			}
		}

		private struct TriangleNode
		{
			public int P0;
			public int P1;
			public int P2;
			public int C0;
			public int C1;
			public int C2;
			public int A0;
			public int A1;
			public int A2;
			public bool IsLeaf
			{
				get
				{
					return C0 < 0 && C1 < 0 && C2 < 0;
				}
			}
			public bool IsInner
			{
				get
				{
					return P0 >= 0 && P1 >= 0 && P2 >= 0;
				}
			}
			public TriangleNode(int P0, int P1, int P2)
			{
				this.P0 = P0;
				this.P1 = P1;
				this.P2 = P2;
				C0 = -1;
				C1 = -1;
				C2 = -1;
				A0 = -1;
				A1 = -1;
				A2 = -1;
			}
			public bool HasEdge(int e0, int e1)
			{
				if (e0 == P0)
				{
					return e1 == P1 || e1 == P2;
				}
				else if (e0 == P1)
				{
					return e1 == P0 || e1 == P2;
				}
				else if (e0 == P2)
				{
					return e1 == P0 || e1 == P1;
				}
				return false;
			}
			public int OtherPoint(int p0, int p1)
			{
				if (p0 == P0)
				{
					return p1 == P1 ? P2 : p1 == P2 ? P1 : throw new ArgumentException("p0 and p1 not on triangle");
				}
				return p0 == P1
					? p1 == P0 ? P2 : p1 == P2 ? P0 : throw new ArgumentException("p0 and p1 not on triangle")
					: p0 == P2
					? p1 == P0 ? P1 : p1 == P1 ? P0 : throw new ArgumentException("p0 and p1 not on triangle")
					: throw new ArgumentException("p0 and p1 not on triangle");
			}
			public int Opposite(int p)
			{
				return p == P0 ? A0 : p == P1 ? A1 : p == P2 ? A2 : throw new ArgumentException("p not in triangle");
			}
			public override string ToString()
			{
				return IsLeaf
					? string.Format("TriangleNode({0}, {1}, {2})", P0, P1, P2)
					: string.Format("TriangleNode({0}, {1}, {2}, {3}, {4}, {5})", P0, P1, P2, C0, C1, C2);
			}
		}
		public class Geom
		{
			public static bool AreCoincident(Vector2 a, Vector2 b)
			{
				return (a - b).Length() < 0.000001f;
			}
			public static bool ToTheLeft(Vector2 p, Vector2 l0, Vector2 l1)
			{
				return (l1.X - l0.X) * (p.Y - l0.Y) - (l1.Y - l0.Y) * (p.X - l0.X) >= 0;
			}
			public static bool ToTheRight(Vector2 p, Vector2 l0, Vector2 l1)
			{
				return !ToTheLeft(p, l0, l1);
			}
			public static bool PointInTriangle(Vector2 p, Vector2 c0, Vector2 c1, Vector2 c2)
			{
				return ToTheLeft(p, c0, c1)
					&& ToTheLeft(p, c1, c2)
					&& ToTheLeft(p, c2, c0);
			}
			public static bool InsideCircumcircle(Vector2 p, Vector2 c0, Vector2 c1, Vector2 c2)
			{
				var ax = c0.X - p.X;
				var ay = c0.Y - p.Y;
				var bx = c1.X - p.X;
				var by = c1.Y - p.Y;
				var cx = c2.X - p.X;
				var cy = c2.Y - p.Y;
				return
						(ax * ax + ay * ay) * (bx * cy - cx * by) -
						(bx * bx + by * by) * (ax * cy - cx * ay) +
						(cx * cx + cy * cy) * (ax * by - bx * ay)
				 > 0.000001f;
			}
			public static Vector2 RotateRightAngle(Vector2 v)
			{
				var x = v.X;
				v.X = -v.Y;
				v.Y = x;
				return v;
			}
			public static bool LineLineIntersection(Vector2 p0, Vector2 v3, Vector2 p1, Vector2 v1, out float m0, out float m1)
			{
				var det = v3.X * v1.Y - v3.Y * v1.X;
				if (Math.Abs(det) < 0.001f)
				{
					m0 = float.NaN;
					m1 = float.NaN;
					return false;
				}
				else
				{
					m0 = ((p0.Y - p1.Y) * v1.X - (p0.X - p1.X) * v1.Y) / det;
					m1 = Math.Abs(v1.X) >= 0.001f ? (p0.X + m0 * v3.X - p1.X) / v1.X : (p0.Y + m0 * v3.Y - p1.Y) / v1.Y;
					return true;
				}
			}
			public static Vector2 LineLineIntersection(Vector2 p0, Vector2 v3, Vector2 p1, Vector2 v1)
			{
				return LineLineIntersection(p0, v3, p1, v1, out float m0, out float m1) ? p0 + m0 * v3 : new Vector2(float.NaN, float.NaN);
			}
			public static Vector2 CircumcircleCenter(Vector2 c0, Vector2 c1, Vector2 c2)
			{
				var mp0 = 0.5f * (c0 + c1);
				var mp1 = 0.5f * (c1 + c2);
				var v3 = RotateRightAngle(c0 - c1);
				var v1 = RotateRightAngle(c1 - c2);
				LineLineIntersection(mp0, v3, mp1, v1, out float m0, out float m1);
				return mp0 + m0 * v3;
			}
			public static Vector2 TriangleCentroid(Vector2 c0, Vector2 c1, Vector2 c2)
			{
				var val = 1.0f / 3.0f * (c0 + c1 + c2);
				return val;
			}
			public static float Area(IList<Vector2> polygon)
			{
				var area = 0.0f;
				var count = polygon.Count;
				for (int i = 0; i < count; i++)
				{
					var j = i == count - 1 ? 0 : i + 1;
					var p0 = polygon[i];
					var p1 = polygon[j];
					area += p0.X * p1.Y - p1.Y * p1.X;
				}
				return 0.5f * area;
			}
		}
		public class DelaunayTriangulation
		{
			public readonly List<Vector2> Vertices;
			public readonly List<int> Triangles;
			public int Count => Triangles.Count / 3;
			internal DelaunayTriangulation()
			{
				Vertices = new List<Vector2>();
				Triangles = new List<int>();
			}
			internal void Clear()
			{
				Vertices.Clear();
				Triangles.Clear();
			}
			public bool Verify()
			{
				try
				{
					for (int i = 0; i < Triangles.Count; i += 3)
					{
						var c0 = Vertices[Triangles[i]];
						var c1 = Vertices[Triangles[i + 1]];
						var c2 = Vertices[Triangles[i + 2]];
						for (int j = 0; j < Vertices.Count; j++)
						{
							var p = Vertices[j];
							if (Geom.InsideCircumcircle(p, c0, c1, c2))
							{
								return false;
							}
						}
					}
					return true;
				}
				catch
				{
					return false;
				}
			}
			public List<Triangle> TransToTriangles()
			{
				List<Triangle> result = new();
				for (int i = 0; i < Triangles.Count; i += 3)
				{
					var c0 = Vertices[Triangles[i]];
					var c1 = Vertices[Triangles[i + 1]];
					var c2 = Vertices[Triangles[i + 2]];
					result.Add(new(c0, c1, c2));
				}
				return result;
			}
		}
	}
}
