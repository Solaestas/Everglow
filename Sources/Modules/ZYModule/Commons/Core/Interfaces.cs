using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core
{
    public interface IUpdateable
    {
        void Update();
    }

    public interface IDrawable
    {
        void Draw();
    }

    public interface IActive
    {
        bool Active { get; set; }
        void Kill()
        {
            Active = false;
        }
    }
}
