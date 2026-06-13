using Everglow.Yggdrasil.YggdrasilTown.VFXs.RandomNPC;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools.Developer;

public class VFXPersonItem : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override bool CanUseItem(Player player)
	{
		VFXPerson person = new VFXPerson()
		{
			Active = true,
			Visible = true,
			Position = Main.MouseWorld,
			Velocity = Vector2.Zero,
			Timer = 0,
			MaxTime = 3000000,
			SkinColor = new Color(209, 160, 156),
		};
		Ins.VFXManager.Add(person);
		return false;
	}
}