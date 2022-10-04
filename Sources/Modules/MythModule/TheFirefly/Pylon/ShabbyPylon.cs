namespace Everglow.Sources.Modules.MythModule.TheFirefly.Pylon;

internal class ShabbyPylonTileEntity : TEModdedPylon
{
}
internal class ShabbyPylon : BaseModPylon<ShabbyPylonTileEntity>
{
	public override int DropItemType => ModContent.ItemType<ShabbyPylonItem>();
	public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
	{
		return Vector2.Distance(Main.LocalPlayer.Center, ModContent.GetInstance<ShabbyPylonTileEntity>().Position.ToVector2() + new Vector2(24, 32)) <= 80;
	}
	public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
	{
		float offset;
		Color shadowColor;
		bool animation;
		if (PylonSystem.Instance.shabbyPylonEnable)
		{
			AscensionTimer = MathUtils.Approach(AscensionTimer, MaxAscensionTime, 1);
			float factor = (AscensionTimer / MaxAscensionTime);
			offset = DefaultVerticalOffset * factor;
			shadowColor = Color.White * 0.1f * factor;
			animation = true;
		}
		else
		{
			offset = 0;
			shadowColor = Color.Transparent;
			animation = false;
		}

		DrawModPylon(spriteBatch, i, j, crystalTexture, crystalHighlightTexture, new Vector2(0, offset), shadowColor, Color.Gray, 4, CrystalVerticalFrameCount, animation);
	}

	public float AscensionTimer = 0;
	public const float MaxAscensionTime = 300;
	public int AnimationTimer = 0;
	public const float MaxTime = 300f;
	public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
	{
		if (!PylonSystem.Instance.shabbyPylonEnable)
		{
			return;
		}

		if (Main.mapFullscreen && PylonSystem.Instance.firstEnableAnimation)
		{
			var firefly = TileEntity.ByPosition.FirstOrDefault(pair => pair.Value is FireflyPylonTileEntity);
			var target = firefly.Key.ToWorldCoordinates() - Main.ScreenSize.ToVector2() / 2;
			AnimationTimer++;

			//TODO 这里直接改screenPosition没用，需要一个改屏幕位置和阻止玩家操作的轮子
			Main.screenPosition = (AnimationTimer / MaxTime).Lerp(Main.screenPosition, target);
			if (AnimationTimer >= MaxTime)
			{
				PylonSystem.Instance.firstEnableAnimation = false;
			}
			return;
		}

		bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
		DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.Everglow.ItemName.ShabbyPylonItem", ref mouseOverText);
	}
}
internal class ShabbyPylonItem : ModItem
{
	public override bool? UseItem(Player player)
	{
		if (Item.favorited && !PylonSystem.Instance.shabbyPylonEnable)
		{
			PylonSystem.Instance.shabbyPylonEnable = true;
			PylonSystem.Instance.firstEnableAnimation = true;

			Main.NewText("Repaired");
		}
		return null;
	}
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<ShabbyPylon>());
	}
}
