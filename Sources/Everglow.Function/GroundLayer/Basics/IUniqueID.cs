using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.GroundLayer.Basics
{
	public interface IUniqueID<T> where T : IComparable<T>
	{
		public T UniqueID { get; }
	}
}