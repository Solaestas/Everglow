using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Function.NetUtils
{
    public interface INetUpdate<T>
    {
        public void LocalUpdate(T input);
        public void NetUpdate(T input);
        public void Forcast();
    }
}
