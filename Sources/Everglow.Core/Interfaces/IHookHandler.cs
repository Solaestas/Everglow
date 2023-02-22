using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Common.Interfaces;

public interface IHookHandler
{
	public string Name { get; }
	public bool Enable { get; set; }	
}
