using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            velocity += force / mass * deltaTime;
            position += velocity * deltaTime;
            force = Vector2.Zero;
        }
    }
}
