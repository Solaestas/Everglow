using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class MiningPowerPickaxe : ModItem
{
	public const int Pick = 59;
	public const int PickTileMax = 8;
	public const int SearchTileMax = 50;
	public const int SearchTileRange = 24 * 16; // 24 Blocks Range
	public const int ChargeMax = 400;
	public const int ChargeCost = 5;

	private static Point16 MouseTileTargetCoord => new Point16(Player.tileTargetX, Player.tileTargetY);

	private static Tile MouseTileTarget => Framing.GetTileSafely(MouseTileTargetCoord);

	private int Charge { get; set; } = 0;

	private float ChargeProgress => Charge / (float)ChargeMax;

	private string ChargeProgressText => $"{Charge}/{ChargeMax}";

	private List<Point16> TargetTiles { get; set; } = [];

	public override void SetDefaults()
	{
		Item.width = 56;
		Item.height = 50;

		Item.pick = Pick;
		Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		Item.tileBoost = 1;

		Item.DamageType = DamageClass.Melee;
		Item.damage = 28;
		Item.knockBack = 2f;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 14; // The 'useAnimation' of pickaxe must less than useTime, to avoid duplicate call of UseItem() returning true.
		Item.useTime = 25;
		Item.autoReuse = true;
		Item.useTurn = true;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 3);
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		tooltips.Add(new TooltipLine(Mod, "Charge", Language.GetTextValue($"Charge: {ChargeProgressText}"))
		{
			OverrideColor = Color.LimeGreen,
		});
	}

	public override void HoldItem(Player player)
	{
		// When mouse right is held.
		if (MouseUtils.MouseRight.IsHeld)
		{
			// Charge the pickaxe.
			if (++Charge > ChargeMax)
			{
				Charge = ChargeMax;
			}
		}

		// When mouse left is held, reset target tiles.
		if (MouseUtils.MouseLeft.IsUp)
		{
			TargetTiles.Clear();
		}
	}

	public override bool? UseItem(Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			if (Charge > ChargeCost && !Main.SmartCursorIsUsed)
			{
				// If the mouse tile target is valid, fill the target tiles list.
				if (TargetTiles.Count == 0
					&& player.IsTargetTileInItemRange(Item)
					&& CheckTile(MouseTileTarget)
					&& CheckPickPower(player, MouseTileTarget, MouseTileTargetCoord))
				{
					// Get all linked tiles (Order by distance to player).
					TargetTiles = GetLinkedTiles(MouseTileTargetCoord, MouseTileTarget, SearchTileMax, player);
				}

				// Pick the nearest [PickTileMax] tiles from target tiles.
				if (TargetTiles.Count != 0)
				{
					// Decrease charge
					Charge = Math.Max(Charge - ChargeCost, 0);

					foreach (var tile in TargetTiles.Take(PickTileMax))
					{
						player.PickTile(tile.X, tile.Y, Pick); // TODO: Replace test pick power with actual pick power.
					}

					// Remove picked tiles from target tiles.
					TargetTiles.RemoveAll(p => !Main.tile[p].HasTile);
				}
			}
		}

		return true;
	}

	public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
	{
		// Draw charge text under player.
		if (Main.LocalPlayer.HeldItem.ModItem == this)
		{
			DrawChargeText(spriteBatch);

			// Draw signal over tile to represent they're selected.
			if (TargetTiles.Count > 0)
			{
				foreach (var tile in TargetTiles)
				{
					var drawPos = tile.ToWorldCoordinates() - Main.screenPosition;
					var texture = Commons.ModAsset.Point.Value;
					spriteBatch.Draw(texture, drawPos, null, new Color(1f, 1f, 1f, 0f), 0f, texture.Size() * 0.5f, 0.08f, SpriteEffects.None, 0f);
				}
			}
		}
	}

	private void DrawChargeText(SpriteBatch spriteBatch)
	{
		var stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, ChargeProgressText, Vector2.One);
		var drawPos = Main.LocalPlayer.Bottom - Main.screenPosition;
		var textColor = new Color(
			MathHelper.Lerp(1f, Color.LimeGreen.R / 255f * (0.7f + ChargeProgress * 0.3f), ChargeProgress),
			Color.LimeGreen.G / 255f * (0.9f + ChargeProgress * 0.2f),
			Color.LimeGreen.B / 255f)
			* (0.9f + 0.1f * MathF.Sin((float)Main.timeForVisualEffects * 0.04f));
		spriteBatch.DrawString(FontAssets.MouseText.Value, ChargeProgressText, drawPos, textColor, 0, new Vector2(stringSize.X * 0.5f, -stringSize.Y * 0.5f), 1f, SpriteEffects.None, 0);
	}

	private static List<Point16> GetLinkedTiles(Point16 clickedTilePos, Tile clickedTile, int tileNumMax, Player player)
	{
		var clickedTileType = clickedTile.type;
		if (!CheckTile(clickedTile))
		{
			return [];
		}

		// Search linked tiles
		var result = new List<Point16>();
		Queue<Point16> tileQueue = new();
		tileQueue.Enqueue(clickedTilePos);
		while (tileQueue.Count != 0)
		{
			var currentTile = tileQueue.Dequeue();
			result.Add(currentTile);

			if (result.Count > tileNumMax)
			{
				break;
			}

			// Search nearby tiles
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					var nearbyTilePos = new Point16(currentTile.X + i, currentTile.Y + j);
					var nearbyTile = Framing.GetTileSafely(nearbyTilePos); // var nearbyTile = Main.tile[nearbyTilePos];
					if (!CheckTilePos(nearbyTilePos) || !CheckTile(nearbyTile)
						|| nearbyTile.type != clickedTileType
						|| tileQueue.Any(t => t == nearbyTilePos)
						|| result.Any(t => t == nearbyTilePos)
						|| Vector2.Distance(nearbyTilePos.ToWorldCoordinates(), player.Center) > SearchTileRange)
					{
						continue;
					}

					tileQueue.Enqueue(nearbyTilePos);
				}
			}
		}

		return result
			.Distinct()
			.OrderBy(p => Vector2.Distance(p.ToWorldCoordinates(), player.Center))
			.ToList();
	}

	private static bool CheckTilePos(Point16 pos) =>
		pos.X < Main.maxTilesX && pos.X >= 0 && pos.Y < Main.maxTilesY && pos.Y >= 0;

	private static bool CheckTile(Tile tile) =>
		tile != null && tile.HasTile && TileID.Sets.Ore[tile.type];

	private static bool CheckPickPower(Player player, Tile tile, Point16 clickPos) =>
		player.GetPickaxeDamage(clickPos.X, clickPos.Y, Pick, 0, tile) > 0;
}