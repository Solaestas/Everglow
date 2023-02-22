using Terraria.UI;

namespace Everglow.Common.UI;

internal class UISystem : ModSystem
{
	public static EverglowUISystem EverglowUISystem
	{
		get => Instance.system;
	}

	public static UISystem Instance
	{
		get => instance;
	}

	private EverglowUISystem system;
	private static UISystem instance;
	private Point screenSize;

	public UISystem()
	{
		system = new EverglowUISystem();
		instance = this;
	}

	public override void Load()
	{
		base.Load();
		if (Main.netMode != NetmodeID.Server)
		{
			system.Load();
		}
	}

	public override void UpdateUI(GameTime gameTime)
	{
		base.UpdateUI(gameTime);

		if (Main.netMode != NetmodeID.Server)
		{
			if (screenSize != Main.ScreenSize)
			{
				screenSize = Main.ScreenSize;
				system.Calculation();
			}
			system.Update(gameTime);
		}
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		base.ModifyInterfaceLayers(layers);
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		if (mouseTextIndex != -1)
		{
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"Everglow: Everglow UI System",
				delegate
				{
					if (Main.netMode != NetmodeID.Server)
					{
						system.Draw(Main.spriteBatch);
					}

					return true;
				},
				InterfaceScaleType.UI));
		}
	}
}
