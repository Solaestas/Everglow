using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons
{
    public interface IModule
    {
        public string Name { get; }
        public string Description { get; }
        public void Load();
        public void Unload();
    }
}
