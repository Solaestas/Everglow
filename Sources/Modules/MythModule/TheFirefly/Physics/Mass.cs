using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Physics
{
    internal class Mass
    {
        public float mass;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 force;
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
