using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.CustomTiles;
public interface ICollider
{
	public Vector2 Position { get; set; }
	public Vector2 Velocity { get; set; }

}
