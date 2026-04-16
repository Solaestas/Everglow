using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class WaterDeliveryHole : ModTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Width = 5;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			18,
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.addTile(Type);
		DustType = ModContent.DustType<WaterDeliveryHoleDust>();
		AddMapEntry(new Color(78, 162, 255));
	}

	public void AddScene(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX == 36 & tile.TileFrameY == 0)
		{
			var vfx = new WaterDeliveryHole_VFX
			{
				Active = true,
				Visible = true,
				Position = new Vector2(i, j).ToWorldCoordinates(8, 8),
				OriginTilePos = new Point(i, j),
				OriginTileType = Type,
				Direction = -1,
			};
			Ins.VFXManager.Add(vfx);
			var foreground = new WaterDeliveryHole_foreground
			{
				Active = true,
				Visible = true,
				Position = new Vector2(i, j).ToWorldCoordinates(8, 20),
				OriginTilePos = new Point(i, j),
				OriginTileType = Type,
				Direction = -1,
			};
			Ins.VFXManager.Add(foreground);
		}
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
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
		Player player = Main.LocalPlayer;
		WaterDeliveryHole_TeleportPlayer modPlayer = player.GetModPlayer<WaterDeliveryHole_TeleportPlayer>();
		if (!modPlayer.Active)
		{
			if (closer && player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
			{
				Tile tile = Main.tile[i, j];
				float closestDistance = 1000;
				Vector2 destination = Vector2.Zero;
				Vector2 currentPos = new Vector2(i * 16 + 8 - tile.TileFrameX / 18 * 16 + 40, j * 16 + 8 - tile.TileFrameY / 18 * 16 + 18);
				for (int x = -40; x <= 40; x++)
				{
					for (int y = -40; y <= 40; y++)
					{
						var checkTile = TileUtils.SafeGetTile(i + x, j + y);
						if (checkTile != null && checkTile.HasTile && checkTile.TileType == Type)
						{
							Vector2 dest = new Vector2((i + x) * 16 + 8 - checkTile.TileFrameX / 18 * 16 + 40, (j + y) * 16 + 8 - checkTile.TileFrameY / 18 * 16 + 18);
							float distance = Vector2.Distance(player.Center, dest);
							if(distance < closestDistance && (dest - currentPos).Length() > 32)
							{
								closestDistance = distance;
								destination = dest;
								player.velocity.Y += -5;
							}
						}
					}
				}
				if(destination != Vector2.zeroVector)
				{
					Teleport(player, destination);
				}
			}
		}
		base.NearbyEffects(i, j, closer);
	}

	public void Teleport(Player player, Vector2 destination)
	{
		WaterDeliveryHole_TeleportPlayer modPlayer = player.GetModPlayer<WaterDeliveryHole_TeleportPlayer>();
		modPlayer.Active = true;
		modPlayer.Timer = modPlayer.MaxTeleportTime;
		modPlayer.OldPos = Main.screenPosition;

		player.Center = destination;
	}
}