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
            mass1 = m1;
            mass2 = m2;
            this.damping = damping;
        }

        //private Vector2 Force(Mass A, Mass B, float elasticity, float restLength)
        //{
        //    var offset = (A.X - B.X);
        //    var length = (float)offset.Length();
        //    var unit = offset / length;

        //    return -elasticity * (length - restLength) * unit + A.force;
        //}

        private Vector<float> G_prime(Vector<float> x, float dt, Mass A, Mass B, float elasticity, float restLength)
        {
            var pos = B.position.ToMathNetVector();
            var vel = B.velocity.ToMathNetVector();

            Vector<float> fixedPos = A.position.ToMathNetVector();
            var offset = (x - fixedPos);
            var length = (float)offset.L2Norm();

            var unit = offset / length;
            Vector<float> force = -elasticity * (length - restLength) * unit + Vector<float>.Build.DenseOfArray(new float[] { 0, 9.8f });
            Vector<float> term2 = Matrix<float>.Build.DenseIdentity(2) * B.mass / (dt * dt) * (x - pos - dt * vel);
            return term2 - force;
        }

        private Matrix<float> G_Hessian(Vector<float> x, float dt, Mass A, Mass B, float elasticity, float restLength)
        {
            Vector<float> fixedPos = A.position.ToMathNetVector();
            var offset = (x - fixedPos);
            var length = (float)offset.L2Norm();
            var length2 = offset.DotProduct(offset);

            var span = offset.OuterProduct(offset);
            var term1 = span * elasticity / length2;
            var term2 = (Matrix<float>.Build.DenseIdentity(2) - span / length2) * elasticity * (1 - restLength / length);
            return Matrix<float>.Build.DenseIdentity(2) * B.mass / (dt * dt) + term1 + term2;
        }

        private Vector<float> NewtonsMethod(float dt, Mass A, Mass B, float elasticity, float restLength)
        {
            Vector<float> x = B.position.ToMathNetVector();
            for (int i = 0; i < 10; i++)
            {
                var gPrime = G_prime(x, dt, A, B, elasticity, restLength);
                var H = G_Hessian(x, dt, A, B, elasticity, restLength);
                var deltaX = H.Solve(-gPrime);
                x += deltaX;
                if (deltaX.DotProduct(deltaX) < 1e-3)
                {
                    break;
                }
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

        public void FEM_CalculateG(float deltaTime)
        {
            //mass1.G -= Force(mass1, mass2, elasticity, restLength);
            //mass2.G -= Force(mass2, mass1, elasticity, restLength);
            //mass1.K = elasticity;
            //mass2.K = elasticity;
        }
    }
}