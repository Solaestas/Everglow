using ReLogic.Content;

namespace Everglow.ZY.Commons.Function.Base;

internal abstract class BaseItem : ModItem
{
	public Asset<Texture2D> Asset => ModContent.Request<Texture2D>(Texture);

	public override bool CloneNewInstances => true;

	public override bool IsCloneable => true;
}
