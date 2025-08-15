using Everglow.Commons.Utilities;
using Everglow.SpellAndSkull.GlobalItems;

namespace Everglow.SpellAndSkull.Items;

public abstract class SpellTomeItem : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MagicWeapons;

	public List<int> DecorativeProjectileTypes = new List<int>();

	public override void SetDefaults()
	{
		if (!MagicBooksReplace.MagicBookType.Contains(Item.type))
		{
			MagicBooksReplace.MagicBookType.Add(Item.type);
		}
		base.SetDefaults();
	}
}