using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics.PBEngine.Constrains
{
	public abstract class Constrain
    {
        public abstract void Apply(float deltaTime);
        public abstract void ApplyForce(float deltaTime);

        public abstract List<(Vector2, Color)> GetDrawMesh();
    }
}
