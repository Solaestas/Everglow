using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core.Collide
{
    public class Vertices : List<Vector2>
    {
        public Vertices() { }

        public Vertices(int capacity) : base(capacity) { }

        public Vertices(IEnumerable<Vector2> vertices) : base(vertices) { }
    }
}
