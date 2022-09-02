using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra;
using Everglow.Sources.Commons.Function.Numerics;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Physics
{
    internal class Mass
    {
        /// <summary>
        /// 质量
        /// </summary>
        public float mass;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// 速度
        /// </summary>
        public Vector2 velocity;
        /// <summary>
        /// 收到的力
        /// </summary>
        public Vector2 force;
        /// <summary>
        /// 是否是静态的（不受力的影响
        /// </summary>
        public bool isStatic;


        internal Vector<float> X;
        internal Vector<float> G;

        public Mass(float mass, Vector2 position, bool isStatic)
        {
            this.mass = mass;
            this.velocity = Vector2.Zero;
            this.force = Vector2.Zero;
            this.position = position;
            this.isStatic = isStatic;
        }

        public void Update(float deltaTime)
        {
            if (isStatic)
            {
                return;
            }
            var oldPos = position;
            position = X.ToVector2();
            var offset = position - oldPos;
            velocity = offset / deltaTime;
        }

        private Vector<float> G_1(float dt)
        {
            Vector<float> x_hat = (position + dt * velocity).ToMathNetVector();
            return Matrix<float>.Build.DenseIdentity(2) * mass / (dt * dt) * (X - x_hat);
        }

        public void FEM_Prepare(float dt)
        {
            X = (position + velocity * dt).ToMathNetVector();
        }

        public void FEM_UpdateG(float dt)
        {
            G = G_1(dt);
        }
    }
}
