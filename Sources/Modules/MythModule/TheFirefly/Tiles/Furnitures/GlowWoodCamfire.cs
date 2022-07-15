using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Everglow.Sources.Commons.Core.Utils;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
	public class GlowWoodCamfire : ModTile
	{
		private Asset<Texture2D> flameTexture;

		public override void SetStaticDefaults() {
			// Properties
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.IsValidSpawnPoint[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

			DustType = ModContent.DustType<BlueGlow>();
			AdjTiles = new int[] { TileID.Campfire};

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2); // this style already takes care of direction for us
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			//TileObjectData.newTile.DrawYOffset = 3;
			TileObjectData.addTile(Type);

			// Etc
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("GlowWood Camfire");
			AddMapEntry(new Color(0, 14, 175), name);

			AnimationFrameHeight = 36;

		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 6)
			{
				frame = (frame + 1) % 9;
				frameCounter = 0;
			}
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Player player = Main.LocalPlayer;
			if (player != null && !player.dead && player.active)
			{
				player.AddBuff(BuffID.Campfire, 20, true, false);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			if (tile.TileFrameX < 54)
			{
				r = 0.1f;
				g = 0.9f;
				b = 1f;
			}
			else
			{
				r = 0f;
				g = 0f;
				b = 0f;
			}

		}
		public override void HitWire(int i, int j)
		{
			int tileX = 3;
			int tileY = 2;
			Tile tile = Main.tile[i, j];
			int x = i - tile.TileFrameX / 18 % tileX;
			int y = j - tile.TileFrameY / 18 % tileY;
			for (int m = x; m < x + tileX; m++)
			{
				for (int n = y; n < y + tileY; n++)
				{
					if (!tile.HasTile)
					{
						continue;
					}
					if (tile.TileType == Type)
					{
						tile = Main.tile[m, n];
						if (tile.TileFrameX < 18 * tileX)
						{
							tile = Main.tile[m, n];
							tile.TileFrameX += (short)(18 * tileX);
						}
						else
						{
							tile = Main.tile[m, n];
							tile.TileFrameX -= (short)(18 * tileX);
						}
					}
				}
			}
			if (!Wiring.running)
			{
				return;
			}
			for (int k = 0; k < tileX; k++)
			{
				for (int l = 0; l < tileY; l++)
				{
					Wiring.SkipWire(x + k, y + l);
				}
			}
		}
		public override bool RightClick(int i, int j)
        {
			int tileX = 3;
			int tileY = 2;
			Tile tile = Main.tile[i, j];
			int x = i - tile.TileFrameX / 18 % tileX;
			int y = j - tile.TileFrameY / 18 % tileY;
			for (int m = x; m < x + tileX; m++)
			{
				for (int n = y; n < y + tileY; n++)
				{
					if (!tile.HasTile)
					{
						continue;
					}
					if (tile.TileType == Type)
					{
						tile = Main.tile[m, n];
						if (tile.TileFrameX < 18 * tileX)
						{
							tile = Main.tile[m, n];
							tile.TileFrameX += (short)(18 * tileX);
						}
						else
						{
							tile = Main.tile[m, n];
							tile.TileFrameX -= (short)(18 * tileX);
						}
					}
				}
			}
			return true;
		}
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
		}
		 
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Furnitures.GlowWoodCamfire>();
		}
		public override void KillMultiTile(int x, int y, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Furnitures.GlowWoodCamfire>());
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodCampfireGlow");
			spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.8f, 0.8f, 0.8f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

			base.PostDraw(i, j, spriteBatch);
		}
	}
}
