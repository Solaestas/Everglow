using Everglow.SpellAndSkull.GlobalItems;
using Terraria;

namespace Everglow.SpellAndSkull.Items;

public abstract class SpellTomeItem : ModItem
{
	public List<int>DecorativeProjectileTypes = new List<int>();
	public override void SetDefaults()
	{
		if (!MagicBooksReplace.MagicBookType.Contains(Item.type))
		{
			MagicBooksReplace.MagicBookType.Add(Item.type);
		}
		base.SetDefaults();
	}
}