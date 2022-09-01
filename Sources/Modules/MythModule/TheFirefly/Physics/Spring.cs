using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Physics
{
    internal class Spring
    {
        /// <summary>
        /// 弹簧的弹性常数
        /// </summary>
        public float elasticity;

        /// <summary>
        /// 弹簧在不受外力的情况下的长度
        /// </summary>
        public float restLength;

        /// <summary>
        /// 弹簧受到的阻力 d(v)
        /// </summary>
        public float damping;

        /// <summary>
        /// 弹簧端点上的两个质点
        /// </summary>
        public Mass mass1;
        public Mass mass2;

        public Spring(float elasticity, float restLength, float damping, Mass m1, Mass m2)
        {
            this.elasticity = elasticity;
            this.restLength = restLength;
            this.mass1 = m1;
            this.mass2 = m2;
            this.damping = damping;
        }

        private Matrix outerProduct(Vector4 v1, Vector4 v2)
        {
            Matrix matrix;
            matrix.M11 = v1.X * v2.X;
            matrix.M12 = v1.X * v2.Y;
            matrix.M13 = v1.X * v2.Z;
            matrix.M14 = v1.X * v2.W;

            matrix.M21 = v1.Y * v2.X;
            matrix.M22 = v1.Y * v2.Y;
            matrix.M23 = v1.Y * v2.Z;
            matrix.M24 = v1.Y * v2.W;

            matrix.M31 = v1.Z * v2.X;
            matrix.M32 = v1.Z * v2.Y;
            matrix.M33 = v1.Z * v2.Z;
            matrix.M34 = v1.Z * v2.W;

            matrix.M41 = v1.W * v2.X;
            matrix.M42 = v1.W * v2.Y;
            matrix.M43 = v1.W * v2.Z;
            matrix.M44 = v1.W * v2.W;
            return matrix;
        }

        private Vector3 V4ToV3(Vector4 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        private Vector3 G_prime(Vector3 x, float dt, Vector3 fixedPoint, Mass mass, float elasticity, float restLength)
        {
            var pos = new Vector3(mass.position, 0f);
            var vel = new Vector3(mass.velocity, 0f);

            var length = (x - fixedPoint).Length();
            var unit = (x - fixedPoint) / length;
            Vector3 force = -elasticity * (length - restLength) * unit;
            Vector4 term2 = Vector4.Transform(new Vector4(x - pos - dt * vel, 0), Matrix.Identity * mass.mass / (dt * dt) ); 
            return V4ToV3(term2) - force;
        }

        private Matrix G_Hessian(Vector3 x, float dt, Vector3 fixedPoint, Mass mass, float elasticity, float restLength)
        {
            var length = (x - fixedPoint).Length();
            var length2 = (x - fixedPoint).LengthSquared();

            var span = outerProduct(new Vector4(x, 0), new Vector4(x, 0));
            var term1 = span * elasticity / length2;
            var term2 = (Matrix.Identity - span / length2) * elasticity * (1 - restLength / length);
            return Matrix.Identity * mass.mass / (dt * dt) + term1 + term2;
        }

        private static Vector4 SolveAxB(in Matrix A, Vector4 b)
        {
            Matrix AInv = Matrix.Invert(A);
            return Vector4.Transform(b, AInv);
        }

        private Vector3 NewtonsMethod(float dt, Vector3 fixedPoint, Mass mass, float elasticity, float restLength)
        {
            Vector3 x = new Vector3(mass.position, 0);
            for (int i = 0; i < 10; i++)
            {
                var gp = new Vector4(G_prime(x, dt, fixedPoint, mass, elasticity, restLength), 0);
                var deltaX = V4ToV3(SolveAxB(G_Hessian(x, dt, fixedPoint, mass, elasticity, restLength), -gp));
                if (deltaX.LengthSquared() < 1e-3)
                {
                    break;
                }
                x += deltaX;
            }
            return x;
        }

        private void ForceSingleDirection(Mass m1, Mass m2, float deltaTime)
        {
            // E(x) = 1/2 * k * (|x1 - x2| - L)^2
            // f(x) = -k * (|x1-x2| - L) * (x1-x2) / |x1-x2|

            // var g = x - x0 - dtv0 - dt^2 M^-1 f(x)

            //Vector3 xx = NewtonsMethod(deltaTime, new Vector3(Main.LocalPlayer.position, 0), m1, elasticity, restLength);
            //var oldPos = m1.position;
            //m1.position = new Vector2(xx.X, xx.Y);
            //m1.velocity = (m1.position - oldPos) / deltaTime;
            //return;

            // 求解阻尼简谐运动的微分方程解析解
            if (4 * elasticity - damping * damping <= 0)
            {
                return;
            }
            float gamma = 0.5f * MathF.Sqrt(4 * elasticity - damping * damping);
            Vector2 unit = Vector2.Normalize(m1.position - m2.position);
            Vector2 dir = m1.position - m2.position - unit * restLength;
            Vector2 c = dir * (damping / (2 * gamma)) + m1.velocity * (1.0f / gamma);
            Vector2 target = dir * MathF.Cos(gamma * deltaTime) + c * MathF.Sin(gamma * deltaTime);
            target *= MathF.Exp(-0.5f * deltaTime * damping);
            Vector2 acc = (target - dir) * (1.0f / deltaTime / deltaTime) - m1.velocity * (1.0f / deltaTime);

            m1.force += acc;

            //float dis = (m2.position - m1.position).Length();
            //Vector2 n = Vector2.Normalize(m2.position - m1.position);
            //Vector2 acc = n * (dis - restLength) * (elasticity);
            //m1.force += acc * m1.mass;
        }

        public void ApplyForce(float deltaTime)
        {
            ForceSingleDirection(mass1, mass2, deltaTime);
            // ForceSingleDirection(mass2, mass1, deltaTime);
        }
    }
}
