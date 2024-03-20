using ReLogic.Content;
using Terraria.IO;

namespace Everglow.Commons.TmlHooks.SwitchWorldItems;

public abstract class SwitchWorldItemBase
{
	public abstract Asset<Texture2D> Icon { get; }

	public abstract int OrderIndex
	{
		get;
	}

	public virtual int Compare(SwitchWorldItemBase x)
	{
		return OrderIndex.CompareTo(x.OrderIndex);
	}

	public abstract void Enter(WorldFileData data);
}