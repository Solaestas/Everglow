using Everglow.Common.Enums;
using Everglow.Common.Interfaces;

namespace Everglow.Common.VFX;

/// <summary>
/// 一个非抽象的Visual子类必须具有一个无参构造函数
/// </summary>
public abstract class Visual : IVisual
{
	public abstract CodeLayer DrawLayer { get; }

	public virtual bool Active { get; set; } = true;

	public virtual bool Visible { get; set; } = true;

	public int Type => VFXManager.Instance.GetVisualType(this);

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
		VFXManager.Instance.Register(this);
	}

	public virtual void Unload()
	{
	}

	public virtual void Update()
	{
	}
}