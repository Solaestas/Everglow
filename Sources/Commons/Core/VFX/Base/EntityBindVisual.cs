using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.VFX.Base;

/// <summary>
/// 与一种实体绑定的Visual，生命周期由该实体决定
/// </summary>
internal abstract class EntityBindVisual : Visual
{
    public Entity entity;
    /// <summary>
    /// Visual内部的active，可用来提前终止该Visual的生命周期
    /// </summary>
    public bool active = true;
    public override bool Active 
    { 
        get => active && entity.active;
        set => active = value;
    }
}


internal abstract class ProjBindVisual : EntityBindVisual
{
    public Projectile Projectile => entity as Projectile;
    public override bool Active
    { 
        get => active && entity.active && Main.projectile[entity.whoAmI] == entity; 
        set => active = value;
    }
    protected ProjBindVisual() { }
    protected ProjBindVisual(Projectile proj)
    {
        if(proj is null)
        {
            return;
        }

        entity = proj; 
    }

}