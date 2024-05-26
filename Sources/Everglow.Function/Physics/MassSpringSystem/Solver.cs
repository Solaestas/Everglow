using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics.MassSpringSystem;
public abstract class Solver
{
	public abstract void Step(MassSpringSystem system, float deltaTime);
}
