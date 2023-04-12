using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Geometries;
internal abstract class Geometry
{
	public Transform Transform { get; protected set; }
	public virtual bool IsConvex => false;
	public virtual bool IsPlane => false;
	public virtual bool IsClosed => false;
	public virtual bool HaveInternalGap => false;
}
