using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.VFX.Interfaces;

namespace Everglow.Sources.Commons.Core.VFX.Base;

public abstract class Visual : IVisual
{
    public abstract CallOpportunity DrawLayer { get; }
    public virtual bool Active { get; set; } = true;
    public virtual bool Visible { get; set; } = true;
    public int Type => Everglow.ModuleManager.GetModule<VFXManager>().GetVisualType(this);

    public virtual string Name => GetType().Name;

    public abstract void Draw();

    public virtual void Kill()
    {
        Active = false;
    }

    public virtual void Load()
    {
        Everglow.ModuleManager.GetModule<VFXManager>().Register(this);
    }

    public virtual void Unload()
    {

    }

    public virtual void Update()
    {

    }
}
