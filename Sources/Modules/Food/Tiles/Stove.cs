using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities;
using Everglow.Food.Items.Cookers;
using Everglow.Food.UI;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Everglow.Food.Tiles;

public class Stove : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		DustType = DustID.LifeCrystal;

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 2;
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Platform | AnchorType.Table | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18,
		};
		TileObjectData.newTile.Origin = new Point16(0, 2);

		// MyTileEntity refers to the tile entity mentioned in the previous section
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<StoveEntity>().Hook_AfterPlacement, -1, 0, true);

		// This is required so the hook is actually called.
		TileObjectData.newTile.UsesCustomCanPlace = true;

		TileObjectData.addTile(Type);

		AddMapEntry(new Color(127, 2, 0));
	}

	public override bool RightClick(int i, int j)
	{
		var tile = Main.tile[i, j];
		StoveEntity stoveEneity;
		Point hitPoint = new Point(i - (tile.TileFrameX % 36) / 18, j - tile.TileFrameY / 18);
		TryGetStoveEntityAs(hitPoint.X, hitPoint.Y, out stoveEneity);
		if (stoveEneity != null)
		{
			switch (stoveEneity.PotState)
			{
				case 0:
					{
						Item item = Main.LocalPlayer.HeldItem;
						if (item.type == ModContent.ItemType<Casserole_Item>())
						{
							item.stack--;
							if (item.stack <= 0)
							{
								item.active = false;
							}
							stoveEneity.PotState = 1;
							return false;
						}
						if (item.type == ModContent.ItemType<SteamBox_Item>())
						{
							item.stack--;
							if (item.stack <= 0)
							{
								item.active = false;
							}
							stoveEneity.PotState = 2;
							return false;
						}
						break;
					}
				case 1:
					{
						CasseroleUI casseroleUI = new CasseroleUI(hitPoint);
						bool checkSame = false;
						foreach (var casserole in StoveUIManager.PotUIs)
						{
							if (casserole.AnchorTilePos == hitPoint)
							{
								checkSame = true;
								casserole.Open = !casserole.Open;
								break;
							}
						}
						if (!checkSame)
						{
							StoveUIManager.PotUIs.Add(casseroleUI);
						}
						return false;
					}
				case 2:
					{
						Item item = Main.LocalPlayer.HeldItem;
						if (item.type == ModContent.ItemType<SteamBox_Item>())
						{
							item.stack--;
							if (item.stack <= 0)
							{
								item.active = false;
							}
							stoveEneity.PotState = 3;
							return false;
						}
						else
						{
							SteamBoxUI steamboxUI = new SteamBoxUI(hitPoint);
							bool checkSame = false;
							foreach (var casserole in StoveUIManager.PotUIs)
							{
								if (casserole.AnchorTilePos == hitPoint)
								{
									checkSame = true;
									casserole.Open = !casserole.Open;
									break;
								}
							}
							if (!checkSame)
							{
								StoveUIManager.PotUIs.Add(steamboxUI);
							}
						}
						break;
					}
				case 3:
					{
						SteamBoxUI2 steamboxUI2 = new SteamBoxUI2(hitPoint);
						bool checkSame = false;
						foreach (var casserole in StoveUIManager.PotUIs)
						{
							if (casserole.AnchorTilePos == hitPoint)
							{
								checkSame = true;
								casserole.Open = !casserole.Open;
								break;
							}
						}
						if (!checkSame)
						{
							StoveUIManager.PotUIs.Add(steamboxUI2);
						}

						break;
					}
				default:
					{
						break;
					}
			}

		}
		FurnitureUtils.LightHitwire(i, j, Type, 2, 3);
		return base.RightClick(i, j);
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 2, 1);
	}

	public override void MouseOver(int i, int j)
	{

	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		int frameX = tile.TileFrameX;
		int frameY = tile.TileFrameY;
		if (frameX % 36 == 0 && frameY == 0)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		if (frameX < 36 || frameY < 36)
		{
			return;
		}
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}

		ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i); // Don't remove any casts.
		var color = new Color(1f, 1f, 1f, 0.6f) * 0.5f;
		int width = 16;
		int height = 16;

		Texture2D flameTexture = ModAsset.Stove.Value;
		for (int k = 0; k < 7; k++)
		{
			float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.25f;
			float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.25f;

			spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + yy + k * 0.2f - 6) + zero, new Rectangle(frameX, frameY + 18, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
		}
		Lighting.AddLight(new Point(i, j).ToWorldCoordinates(), new Vector3(0.8f, 0.6f, 0.8f));
		if (!Main.gamePaused)
		{
			if (Main.rand.NextBool(8))
			{
				Dust flame = Dust.NewDustDirect(new Vector2(i, j) * 16, 16, 8, DustID.Torch);
				flame.velocity.Y -= Main.rand.NextFloat(2, 3);
			}
		}
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		StoveEntity stoveEneity;
		TryGetStoveEntityAs(pos.X, pos.Y, out stoveEneity);
		if (stoveEneity != null)
		{
			Texture2D pot = ModAsset.StoveAtlas.Value;
			Vector2 offset = new Vector2(8, 13);
			switch (stoveEneity.PotState)
			{
				case 1:
					{
						Rectangle frame = new Rectangle(2, 2, 56, 26);
						spriteBatch.Draw(pot, pos.ToWorldCoordinates() - Main.screenPosition + offset, frame, Lighting.GetColor(pos), 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);
						break;
					}
				case 2:
					{
						Rectangle frame = new Rectangle(6, 51, 48, 14);
						spriteBatch.Draw(pot, pos.ToWorldCoordinates() - Main.screenPosition + offset, frame, Lighting.GetColor(pos), 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);
						break;
					}
				case 3:
					{
						Rectangle frame = new Rectangle(6, 51, 48, 28);
						spriteBatch.Draw(pot, pos.ToWorldCoordinates() - Main.screenPosition + offset, frame, Lighting.GetColor(pos), 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);
						break;
					}
				default:
					{
						break;
					}
			}
			if (Main.tile[pos].TileFrameX < 36)
			{
				return;
			}
			Rectangle flame = new Rectangle(2, 36, 24, 10);
			offset = new Vector2(8, 25);
			spriteBatch.Draw(pot, pos.ToWorldCoordinates() - Main.screenPosition + offset, flame, new Color(0.5f, 0.5f, 0.5f, 0), 0, flame.Size() * 0.5f, 1, SpriteEffects.None, 0);
		}
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX % 36 == 0 && tile.TileFrameY == 0)
		{
			StoveEntity stoveEneity;
			TryGetStoveEntityAs(i, j, out stoveEneity);
			if (stoveEneity == null)
			{
				TileEntity.PlaceEntityNet(i, j, ModContent.TileEntityType<StoveEntity>());
				TryGetStoveEntityAs(i, j, out stoveEneity);
			}
			base.NearbyEffects(i, j, closer);
		}
	}

	public static bool TryGetStoveEntityAs<T>(int i, int j, out T entity)
	where T : TileEntity
	{
		Point16 origin = new Point16(i, j);
		if (TileEntity.ByPosition.TryGetValue(origin, out TileEntity existing) && existing is T existingAsT)
		{
			entity = existingAsT;
			return true;
		}

		entity = null;
		return false;
	}
}

public class StoveEntity : ModTileEntity
{
	public int PotState;
	public int[] Ingredients;
	public PotUI MyPotUI;

	public enum PotStateID
	{
		None,
		Casserole,
		Stockpot,
		DeepFryer,
		Steamer,
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		var tile = Main.tile[x, y];
		if (tile.HasTile && tile.TileType == ModContent.TileType<Stove>())
		{
			MyPotUI = GetPotUI(x, y);
			if (MyPotUI != null)
			{
				Ingredients = MyPotUI.Ingredients;
			}
			return true;
		}
		return false;
	}

	public override void OnNetPlace()
	{
		if (Main.netMode == NetmodeID.Server)
		{
			NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
		}
	}

	public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
	{
		if (Main.netMode == NetmodeID.MultiplayerClient)
		{
			int width = 1;
			int height = 1;
			NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);
			NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
			return -1;
		}
		int placedEntity = Place(i, j);
		return placedEntity;
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add("StovePotState", PotState);
		tag.Add("StovePotIngredients", Ingredients);
		base.SaveData(tag);
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		tag.TryGet<int>("StovePotState", out PotState);
		tag.TryGet<int[]>("StovePotIngredients", out Ingredients);
	}

	public PotUI GetPotUI(int x, int y)
	{
		foreach (var potUI in StoveUIManager.PotUIs)
		{
			if (potUI.AnchorTilePos == new Point(x, y))
			{
				return potUI;
			}
		}
		return null;
	}
}