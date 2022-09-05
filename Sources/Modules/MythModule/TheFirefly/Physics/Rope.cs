using Everglow.Sources.Commons.Function.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Physics
{
    internal class Rope
    {
        private struct _Mass
        {
            internal bool IsStatic;
            internal float Mass;
            internal Vector2 Position;
            internal Vector2 Velocity;
            internal Vector2 Force;

            public _Mass()
            {
                this.Mass = 1f;
                this.Velocity = Vector2.Zero;
                this.Force = Vector2.Zero;
                this.Position = Vector2.Zero;
                this.IsStatic = false;
            }
        }

        private struct _Spring
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
        private float[,] m_tmpM;
        private Matrix<float>[,] m_springH;

        private float m_damping;
        private float m_elasticity;
        private Rope()
        {
            m_damping = 0.99f;
        }

        private Vector2 G_prime(float dt, int A, int B, float elasticity, float restLength)
        {
            ref _Mass mA = ref m_masses[A];
            ref _Mass mB = ref m_masses[B];
            Vector2 x_hat = mA.Position + dt * mA.Velocity;

            var offset = m_dummyPos[A] - m_dummyPos[B];
            var length = offset.Length();
            var unit = offset / length;

            Vector2 force = -elasticity * (length - restLength) * unit;
            return mA.Mass / (dt * dt) * (m_dummyPos[A] - x_hat) - force;
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

        private void PrepareHessian()
        {
            
        }

        private void ApplyForce()
        {
            float gravity = 1;
            for (int i = 0; i < m_masses.Length; i++)
            {
                ref _Mass m = ref m_masses[i];
                m.Force += new Vector2(0.04f + 0.06f *
                    (float)(Math.Sin(Main.timeForVisualEffects / 72f + m.Position.X / 13d + m.Position.Y / 4d)), 0)
                    * (Main.windSpeedCurrent + 1f) * 2f
                    + new Vector2(0, gravity * m.Mass);
            }

        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < m_masses.Length; i++)
            {
                ref _Mass m = ref m_masses[i];

                m.Velocity *= 0.99f;
                m_dummyPos[i] = (m.Position + m.Velocity * deltaTime);
            }

            for (int k = 0; k < 16; k++)
            {
                for (int i = 0; i < m_masses.Length; i++)
                {
                    m_gradiants[i] += m_masses[i].Force;
                }

                for (int i = 0; i < m_springs.Length; i++)
                {
                    ref _Spring spr = ref m_springs[i];
                    Vector2 v = G_prime(deltaTime, spr.A, spr.B, spr.Elasticity, spr.RestLength);
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
                    if (m.IsStatic)
                    {
                        return;
                    }
                    float alpha = 1f / (m.Mass / (deltaTime * deltaTime) + 4 * m_elasticity);
                    var dx = alpha * m_gradiants[i];
                    m_dummyPos[i] -= dx;
                }
            }


            for (int i = 0; i < m_masses.Length; i++)
            {
                ref _Mass m = ref m_masses[i];
                if (m.IsStatic)
                {
                    var oldPos = m.Position;
                    m.Position = m_dummyPos[i];
                    m.Velocity = (m.Position - oldPos) / deltaTime;
                    m.Force = Vector2.Zero;
                    return;
                }
            }
        }

        /// <summary>
        /// 自动生成一串给定长度的绳子系统，质量和大小随机
        /// </summary>
        /// <param name="position"></param>
        /// <param name="elasticity"></param>
        /// <param name="scale"></param>
        /// <param name="count"></param>
        public Rope(Vector2 position, float elasticity, float scale, int count)
        {
            m_masses = new _Mass[count];
            m_dummyPos = new Vector2[count];
            m_gradiants = new Vector2[count];
            m_springs = new _Spring[count - 1];
            m_tmpM = new float[2, count];
            m_springH = new Matrix<float>[count, count];

            m_elasticity = elasticity;

            m_masses[0] = new _Mass
            {
                Mass = scale * Main.rand.NextFloat(1f, 1.68f),
                Position = position,
                IsStatic = true
            };

            m_masses[^1] = new _Mass
            {
                Mass = scale * Main.rand.NextFloat(1f, 1.68f) * 1.3f,
                Position = position + new Vector2(0, 6 * count - 6),
                IsStatic = false
            };
            for (int i = 1; i < count - 1; i++)
            {
                m_masses[i] = new _Mass
                {
                    Mass = scale * Main.rand.NextFloat(1f, 1.68f),
                    Position = position + new Vector2(0, 6 * i),
                    IsStatic = false
                };
            }

            for (int i = 1; i < count - 1; i++)
            {
                m_masses[i] = new _Mass
                {
                    Mass = scale * Main.rand.NextFloat(1f, 1.68f),
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
}
