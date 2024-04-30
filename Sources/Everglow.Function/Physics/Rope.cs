using MathNet.Numerics.LinearAlgebra;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics;

public delegate Vector2 RenderingTransformFunction(Vector2 pos);

public class Rope
{
	public struct _Mass
	{
		public bool IsStatic;
		public float Mass;
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector2 Force;

		public _Mass()
		{
			this.Mass = 1f;
			this.Velocity = Vector2.Zero;
			this.Force = Vector2.Zero;
			this.Position = Vector2.Zero;
			this.IsStatic = false;
		}
	}
	private static Vector2 RotateBy(Vector2 vector, float radians)
	{
		float cos = MathF.Cos(radians);
		float sin = MathF.Sin(radians);
		return new Vector2(
			vector.X * cos - vector.Y * sin,
			vector.X * sin + vector.Y * cos
		);
	}
	public struct _Spring
	{
		internal float Elasticity;
		internal float RestLength;
		internal int A;
		internal int B;

		public _Spring()
		{
			this.Elasticity = 1f;
			this.RestLength = 0f;
			this.A = 0;
			this.B = 0;
		}
	}
	private _Mass[] m_masses;
	private _Spring[] m_springs;
	private Vector2[] m_dummyPos;
	private Vector2[] m_gradiants;
	private RenderingTransformFunction m_renderingTransform;

	private float m_damping;
	private float m_elasticity;
	private bool m_hasCollision;
	public _Mass[] GetMassList
	{
		get
		{
			return m_masses;
		}
	}
	public _Spring[] GetSpringList
	{
		get
		{
			return m_springs;
		}
	}
	public RenderingTransformFunction RenderingTransform
	{
		get
		{
			return m_renderingTransform;
		}
	}
	private float CrossProduct2D(Vector2 a, Vector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}
	public Rope Clone(Vector2 deltaPosition)
	{
		Rope clone = new Rope();
		clone.m_elasticity = m_elasticity;
		clone.m_damping = m_damping;
		clone.m_renderingTransform = m_renderingTransform;
		clone.m_masses = new _Mass[m_masses.Length];
		clone.m_springs = new _Spring[m_springs.Length];
		clone.m_gradiants = new Vector2[m_gradiants.Length];
		clone.m_dummyPos = new Vector2[m_dummyPos.Length];
		m_masses.CopyTo(clone.m_masses, 0);
		m_springs.CopyTo(clone.m_springs, 0);
		m_gradiants.CopyTo(clone.m_gradiants, 0);
		m_dummyPos.CopyTo(clone.m_dummyPos, 0);
		for (int i = 0; i < clone.m_masses.Length; i++)
		{
			clone.m_masses[i].Position += deltaPosition;
		}
		return clone;
	}
	private Rope()
	{
		m_damping = 0.99f;
		m_hasCollision = false;
	}
	private Vector2 G_prime(int A, int B, float elasticity, float restLength)
	{
		var offset = m_dummyPos[A] - m_dummyPos[B];
		var length = offset.Length();
		var unit = offset.NormalizeSafe();

		return -elasticity * (length - restLength) * unit;
	}
	private Matrix<float> G_Hessian(float dt, int A, int B, float elasticity, float restLength)
	{
		ref _Mass mA = ref m_masses[A];
		ref _Mass mB = ref m_masses[B];

		var offset = m_dummyPos[A] - m_dummyPos[B];
		var length = (float)offset.Length();
		var length2 = Vector2.Dot(offset, offset);

		var span = offset.ToMathNetVector().OuterProduct(offset.ToMathNetVector());
		var term1 = span * elasticity / length2;
		var term2 = (Matrix<float>.Build.DenseIdentity(2) - span / length2) * elasticity * (1 - restLength / length);
		return Matrix<float>.Build.DenseIdentity(2) * mA.Mass / (dt * dt) + term1 + term2;
	}
	private Vector2 CheckCollision(int i, Vector2 oldPos, Vector2 newPos)
	{
		ref _Mass m = ref m_masses[i];
		Vector3 sdf = SDFUtils.CalculateTileSDF(m.Position);
		float EPS = 1e-2f;
		if (sdf.X < EPS)
		{
			newPos -= 1f * (EPS - sdf.X) * new Vector2(sdf.Y, sdf.Z);
		}
		return newPos;
	}
	public void ApplyForce()
	{
		float gravity = 9;
		for (int i = 0; i < m_masses.Length; i++)
		{
			ref _Mass m = ref m_masses[i];
			m.Force += new Vector2(2 * (MathF.Sin((float)Main.timeForVisualEffects / 72f + m.Position.X / 13f + m.Position.Y / 4f) + 0.9f), 0)
				* Main.windSpeedCurrent
				+ new Vector2(0, gravity * m.Mass);
		}
	}
	public void ApplyForceSpecial(int index,Vector2 force)
	{
		ref _Mass m = ref m_masses[index];
		m.Force += force;
	}
	public void ClearForce()
	{
		//for (int i = 0; i < m_masses.Length; i++)
		//{
		//    ref _Mass m = ref m_masses[i];

		//}
	}
	public void Update(float deltaTime)
	{
		for (int i = 0; i < m_masses.Length; i++)
		{
			ref _Mass m = ref m_masses[i];
			if (float.IsNaN(m.Position.X) && float.IsNaN(m.Position.Y))
			{
				Terraria.Main.NewText("!!");
			}

			m.Velocity *= (float)Math.Pow(m_damping, deltaTime);
			m_dummyPos[i] = (m.Position + m.Velocity * deltaTime);

			if (m.IsStatic)
			{
				m_dummyPos[i] = m.Position;
			}
		}

		for (int k = 0; k < 16; k++)
		{
			for (int i = 0; i < m_masses.Length; i++)
			{
				ref _Mass m = ref m_masses[i];
				Vector2 x_hat = m.Position + deltaTime * m.Velocity;
				m_gradiants[i] = m.Mass / (deltaTime * deltaTime) * (m_dummyPos[i] - x_hat);
				m_gradiants[i] -= m.Force;
			}

			for (int i = 0; i < m_springs.Length; i++)
			{
				ref _Spring spr = ref m_springs[i];
				Vector2 v = G_prime(spr.A, spr.B, spr.Elasticity, spr.RestLength);
				m_gradiants[spr.A] -= v;
				m_gradiants[spr.B] -= -v;

				//var He = G_Hessian(deltaTime, spr.A, spr.B, spr.Elasticity, spr.RestLength);
				//m_springH[spr.A, spr.A] = He;
				//m_springH[spr.B, spr.B] = He;
				//m_springH[spr.A, spr.B] = -He;
				//m_springH[spr.B, spr.A] = -He;
			}

			for (int i = 0; i < m_masses.Length; i++)
			{
				ref _Mass m = ref m_masses[i];
				m.Force = Vector2.Zero;
				if (m.IsStatic)
				{
					continue;
				}
				float alpha = 1f / (m.Mass / (deltaTime * deltaTime) + 4 * m_elasticity);
				var dx = alpha * m_gradiants[i];
				m_dummyPos[i] -= dx;

			}
		}

		if (m_hasCollision)
		{
			float tTest;
			bool res = CollisionUtils.CCD_SegmentPoint(new Vector2(0, 2), new Vector2(1, -2), new Vector2(0, 0), new Vector2(0, -1), new Vector2(0.5f, 0.5f), Vector2.Zero, out tTest);
			for (int i = 0; i < m_springs.Length; i++)
			{
				ref _Spring spr = ref m_springs[i];
				ref _Mass A = ref m_masses[spr.A];
				ref _Mass B = ref m_masses[spr.B];

				Vector2 Av = m_dummyPos[spr.A] - A.Position;
				Vector2 Bv = m_dummyPos[spr.B] - B.Position;

				Vector2 center = (m_dummyPos[spr.A] + m_dummyPos[spr.B]) / 2f;
				int tileX = (int)(center.X / 16);
				int tileY = (int)(center.Y / 16);

				Vector2 baseOffset = new Vector2(tileX * 16 - 16, tileY * 16 - 16);

				float minTimeToCollision = 1f;
				Vector2 targetPos = Vector2.Zero;
				Vector2 normal = Vector2.Zero;
				for (int a = -3; a <= 3; a++)
				{
					for (int b = -3; b <= 3; b++)
					{
						if (tileX + b < 0 || tileX + b >= Terraria.Main.maxTilesX || tileY + a < 0 || tileY + a >= Terraria.Main.maxTilesY)
						{
							continue;
						}
						var tile = Terraria.Main.tile[tileX + b, tileY + a];
						ICollider2D collider = SDFUtils.ExtractColliderFromTile(tile, tileX + b, tileY + a, false);

						if (collider != null)
						{
							IPolygonalCollider2D collider2d = collider as IPolygonalCollider2D;
							foreach (var point in collider2d.GetPolygon().Points)
							{
								float t;
								if (CollisionUtils.CCD_SegmentPoint(A.Position - baseOffset, Av, B.Position - baseOffset, Bv, point - baseOffset, Vector2.Zero, out t))
								{
									if (t < 1 && t < minTimeToCollision)
									{
										minTimeToCollision = t;

										var proj = A.Position + Vector2.Dot(B.Position - A.Position, point - A.Position) * Vector2.Normalize(B.Position - A.Position);
										normal = Vector2.Normalize(point - proj);
										targetPos = point;
									}
								}
							}
						}
					}
				}

				if (minTimeToCollision < 1)
				{
					//Terraria.Main.NewText(minTimeToCollision);
					minTimeToCollision *= 0.9f;

					Vector2 contVA = Av * (1 - minTimeToCollision);
					Vector2 contVB = Bv * (1 - minTimeToCollision);

					Vector2 dummyA = A.Position + Av * minTimeToCollision;
					Vector2 dummyB = B.Position + Bv * minTimeToCollision;

					var torqueA = CrossProduct2D(dummyA - targetPos, contVA);
					var torqueB = CrossProduct2D(dummyB - targetPos, contVB);

					var MoI = (dummyA - targetPos).LengthSquared() * A.Mass + (dummyB - targetPos).LengthSquared() * B.Mass;

					var rot = -(torqueA + torqueB) / MoI * 10;
					var rr = RotateBy(dummyA - targetPos, rot);
					m_dummyPos[spr.A] = targetPos + RotateBy(dummyA - targetPos, rot);
					m_dummyPos[spr.B] = targetPos + RotateBy(dummyB - targetPos, -rot);
				}
			}
		}

		for (int i = 0; i < m_masses.Length; i++)
		{
			ref _Mass m = ref m_masses[i];
			if (m.IsStatic)
			{
				continue;
			}
			Vector2 x_hat = m.Position + deltaTime * m.Velocity;

			if (m_hasCollision)
			{
				m.Position = CheckCollision(i, m.Position, m_dummyPos[i]);
			}
			else
			{
				m.Position = m_dummyPos[i];
			}

			m.Velocity += (m.Position - x_hat) / deltaTime;
		}
	}
	private void InitRopes(int count, float elasticity, RenderingTransformFunction renderingTransform, bool hasCollision)
	{
		m_masses = new _Mass[count];
		m_dummyPos = new Vector2[count];
		m_gradiants = new Vector2[count];
		m_springs = new _Spring[count - 1];
		m_damping = 0.99f;

		m_elasticity = elasticity;
		m_renderingTransform = renderingTransform;
		m_hasCollision = hasCollision;
	}
	/// <summary>
	/// 自动生成一串由位置决定的绳子链
	/// </summary>
	/// <param name="positions"></param>
	public Rope(List<Vector2> positions, float elasticity, float mass, RenderingTransformFunction renderingTransform, bool hasCollision = false)
	{
		InitRopes(positions.Count, elasticity, renderingTransform, hasCollision);

		for (int i = 0; i < positions.Count; i++)
		{
			m_masses[i] = new _Mass
			{
				Mass = mass,
				Position = positions[i],
				IsStatic = (i == positions.Count - 1)
			};
		}

		for (int i = 0; i < positions.Count - 1; i++)
		{
			m_springs[i] = new _Spring
			{
				Elasticity = elasticity,
				RestLength = (positions[i] - positions[i + 1]).Length(),
				A = i,
				B = i + 1
			};
		}
	}
	/// <summary>
	/// 自动生成一串由两端位置决定的绳子链,且两端固定
	/// </summary>
	/// <param name="positions"></param>
	public Rope(Vector2 start, Vector2 end, int count, float elasticity, float mass, RenderingTransformFunction renderingTransform, bool hasCollision = false, int knotDistance = 0, float knotMass = 1)
	{
		List<Vector2> positions = new List<Vector2>();
		for (int t = 0; t <= count; t++)
		{
			positions.Add(Vector2.Lerp(start, end, t / (float)count));
		}
		InitRopes(positions.Count, elasticity, renderingTransform, hasCollision);

		for (int i = 0; i < positions.Count; i++)
		{
			float specialMass = mass;
			if(knotDistance > 0)
			{
				if(i % knotDistance == knotDistance / 2)
				{
					specialMass = knotMass;
				}
			}
			m_masses[i] = new _Mass
			{
				Mass = specialMass,
				Position = positions[i],
				IsStatic = (i == positions.Count - 1) || (i == 0)
			};
		}

		for (int i = 0; i < positions.Count - 1; i++)
		{
			m_springs[i] = new _Spring
			{
				Elasticity = elasticity,
				RestLength = (positions[i] - positions[i + 1]).Length(),
				A = i,
				B = i + 1
			};
		}
	}
	/// <summary>
	/// 自动生成一串给定长度的绳子系统，质量和大小随机
	/// </summary>
	/// <param name="position"></param>
	/// <param name="elasticity"></param>
	/// <param name="scale"></param>
	/// <param name="count"></param>
	public Rope(Vector2 position, float elasticity, float scale, int count, RenderingTransformFunction renderingTransform, bool hasCollision = false)
	{
		InitRopes(count, elasticity, renderingTransform, hasCollision);
		Random rand = Random.Shared;

		m_masses[0] = new _Mass
		{
			Mass = scale * ((float)rand.NextDouble() * 0.68f + 1),
			Position = position,
			IsStatic = true
		};

		m_masses[^1] = new _Mass
		{
			Mass = scale * ((float)rand.NextDouble() * 0.68f + 1) * 1.3f,
			Position = position + new Vector2(0, 6 * count - 6),
			IsStatic = false
		};
		for (int i = 1; i < count - 1; i++)
		{
			m_masses[i] = new _Mass
			{
				Mass = scale * ((float)rand.NextDouble() * 0.68f + 1),
				Position = position + new Vector2(0, 6 * i),
				IsStatic = false
			};
		}

		for (int i = 0; i < count - 1; i++)
		{
			m_springs[i] = new _Spring
			{
				Elasticity = elasticity,
				RestLength = 20f,
				A = i,
				B = i + 1
			};
		}
	}
}
