namespace Everglow.Sources.Commons.Core.VFX.Visuals;

/// <summary>
/// 一个非抽象的Visual子类必须具有一个无参构造函数
/// </summary>
public abstract class Visual : IVisual
{
    public abstract CallOpportunity DrawLayer
    {
        get;
    }

    public virtual bool Active { get; set; } = true;
    public virtual bool Visible { get; set; } = true;
    public int Type => Everglow.ModuleManager.GetModule<VFXManager>().GetVisualType(this);
    public virtual string Name => GetType().Name;
    public Visual() => OnSpawn();

    public abstract void Draw();

    public virtual void Kill()
    {
        Active = false;
    }

    public virtual void OnSpawn()
    {
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