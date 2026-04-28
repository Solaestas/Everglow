using Everglow.Commons.TileHelper;
using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.KelpCurtain.Items.Placeables;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.WaterDeliveryHoles;

public class WaterDeliveryHole_TopLeft : ShapeDataTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		TotalHeight = 4;
		TotalWidth = 4;
		Main.tileSolid[Type] = false;
		Main.tileBlendAll[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileWaterDeath[Type] = false;
		CustomItemType = ModContent.ItemType<WaterDeliveryHole_Item>();
		base.SetStaticDefaults();

		DustType = ModContent.DustType<WaterDeliveryHoleDust>();
		AddMapEntry(new Color(78, 162, 255));
	}

	public void AddScene(int i, int j)
	{
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		Tile tile = Main.tile[i, j];
		Texture2D tex = ModAsset.WaterDeliveryHole.Value;
		Vector2 pos = new Vector2(i * 16, j * 16) - Main.screenPosition + zero;
		spriteBatch.Draw(tex, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY + 36, 16, 16), new Color(0f, 0f, 0.7f, 0));
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (Main.dedServ)
		{
			base.NearbyEffects(i, j, closer);
			return;
		}
		Lighting.AddLight(new Point(i, j).ToWorldCoordinates(), new Vector3(0.1f, 0.2f, 0.5f));
		var teleportSystem = ModContent.GetInstance<WaterDeliveryHole_TeleportPlayer>();
		teleportSystem.GetDestinationAndTeleport(i, j);
		base.NearbyEffects(i, j, closer);
	}
}