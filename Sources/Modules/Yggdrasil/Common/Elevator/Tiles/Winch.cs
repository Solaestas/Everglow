using Everglow.Sources.Modules.ZYModule.TileModule;
using Everglow.Yggdrasil.Common;
using Terraria.Localization;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles
{
	public class Winch : ModTile
	{
		public override void PostSetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;

			AddMapEntry(new Color(112, 75, 75));

			DustType = DustID.Iron;

		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			blockDamaged = false;
			return false;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PlaceInWorld(int i, int j, Item item)
		{
			Tile thisTile = Main.tile[i, j];
			thisTile.TileFrameX = 0;
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Tile thisTile = Main.tile[i, j];
			//只有帧位于18的整数倍时才会
			if (thisTile.TileFrameX % 18 == 0)
			{
				bool HasLift = false;
				for (int x = -2; x < 4; x++)
				{
					for (int y = 1; y < 16; y++)
					{
						if (Main.tile[i + x, j + y].HasTile)
							return;
					}
				}
				foreach (var Dtile in TileSystem.GetTiles<YggdrasilElevator>())
				{
					Vector2 Dc = Dtile.Center;
					float Dy = Math.Abs(Dc.Y / 16f - j);
					//电梯至少要在绞盘下10格
					if (Dc.X / 16f - i == 0 && Dy > 10)
					{
						HasLift = true;
						//确保这个电梯的所有绞盘是自己,如果不是就手动生成电梯
						for (int y = 0; y < Dc.Y / 16f - 20; y++)
						{
							int CoordY = (int)(Dc.Y / 16f) - y;
							Tile tile = Main.tile[i, CoordY];
							if (CoordY < j + 5)
								break;
							if (tile.TileType == Type)
								HasLift = false;
						}
					}
				}
				if (!HasLift)
				{
					TileSystem.AddTile(new YggdrasilElevator() { Position = new Vector2(i, j + 15) * 16 - new Vector2(48, 8) });
					thisTile.TileFrameX = 1;
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;

			Texture2D t = YggdrasilContent.QuickTexture("Common/Elevator/Tiles/LiftWinch");
			Color c0 = Lighting.GetColor(i, j);

			spriteBatch.Draw(t, new Vector2(i * 16, j * 16) - Main.screenPosition + new Vector2(8, 6)/* + new Vector2((int)Vdrag.X, (int)Vdrag.Y)*/ + zero, null, c0, 0, t.Size() / 2f, 1, SpriteEffects.None, 0);
			base.PostDraw(i, j, spriteBatch);
		}
	}
}
