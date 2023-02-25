using ReLogic.Content;

namespace Everglow.ZYModule.Commons.Function.Base;

internal abstract class BaseItem : ModItem
{
	public Asset<Texture2D> Asset => ModContent.Request<Texture2D>(Texture);
	protected override bool CloneNewInstances => true;
	public override bool IsCloneable => true;
}
