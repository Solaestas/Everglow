using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics.MassSpringSystem;
public interface IMassSpringMesh
{
	public List<_Mass> GetMasses();

	public List<ElasticConstrain> GetElasticConstrains();
}
