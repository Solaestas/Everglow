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


        internal Vector2 X;
        internal Vector2 G;
        internal float K;

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
            position = X;
            var offset = position - (oldPos + deltaTime * velocity);
            velocity += offset / deltaTime;
            force = Vector2.Zero;
        }

        private Vector2 G_1(float dt)
        {
            Vector2 x_hat = (position + dt * velocity);
            return mass / (dt * dt) * (X - x_hat);
        }

        public void FEM_Prepare(float dt)
        {
            velocity *= 0.99f;
            X = (position + velocity * dt);
        }

        public void FEM_UpdateG(float dt)
        {
            G = G_1(dt);
        }
    }
}
