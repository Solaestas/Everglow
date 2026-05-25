using Everglow.Commons.Utilities;
using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Example.BgSlides;

namespace Everglow.Example.Items;

public class ExampleUndergroundBackgroundTool : ModItem
{
	public override void SetDefaults() => base.SetDefaults();

	public override void HoldItem(Player player)
	{
		BackgroundSystem bgSystem = ModContent.GetInstance<BackgroundSystem>();

		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			List<Vector2> star_5 = new List<Vector2>();
			for (int k = 0; k < 10; k++)
			{
				float length = 1500;
				if (k % 2 == 1)
				{
					length *= 0.5f;
				}
				star_5.Add(new Vector2(0, -length).RotatedBy(k / 10f * MathHelper.TwoPi) + Main.MouseWorld);
			}

			ExampleUnderGroundBg exSlide = new ExampleUnderGroundBg();
			Point tilePos = Main.MouseWorld.ToTileCoordinates();
			for (int x = -100; x <= 100; x += 3)
			{
				for (int y = -100; y <= 100; y += 3)
				{
					Vector2 pos = (tilePos + new Point(x, y)).ToWorldCoordinates();
					if (MathUtils.IsPointInPolygon(star_5, pos))
					{
						exSlide.BgTiles.Add(tilePos + new Point(x, y));
					}
				}
			}
			bgSystem.AddBgSlide(exSlide);
		}
	}
}