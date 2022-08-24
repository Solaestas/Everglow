using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures
{
    [DebuggerDisplay("position = ({position.X}, {position.Y}) radius = {radius}")]
    public struct Circle
    {
        public Vector2 position;
        public float radius;

        public Circle(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public Vector2 GetEdge(float angle) => position + new Vector2(radius, 0).RotatedBy(angle);
        public Vector2 GetEdge(Rotation rot) => position + radius * rot.XAxis;
        public override string ToString()
        {
            return $"({position.X}, {position.Y}) : {radius}";
        }
    }
}
