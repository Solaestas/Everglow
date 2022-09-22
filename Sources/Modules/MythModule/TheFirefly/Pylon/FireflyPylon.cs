using ReLogic.Content;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Pylon
{
	internal class FireflyPylon_TileEntity:TEModdedPylon
    {
		
    }
    internal class FireflyPylon:ModPylon
    {
		public const int CrystalHorizontalFrameCount = 2;
		public const int CrystalVerticalFrameCount = 8;
		public const int CrystalFrameHeight = 64;
		public Asset<Texture2D> crystalTexture;
		public Asset<Texture2D> crystalHighlightTexture;
		public Asset<Texture2D> mapIcon;
		public override void Load()
		{
			crystalTexture = ModContent.Request<Texture2D>(Texture + "_Crystal");
			crystalHighlightTexture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Pylon/CommonPylon_CrystalHighlight");
			mapIcon = ModContent.Request<Texture2D>(Texture + "_MapIcon");
		}
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.StyleHorizontal = true;
			TEModdedPylon moddedPylon = ModContent.GetInstance<FireflyPylon_TileEntity>();
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.PreventsSandfall[Type] = true;
			AddToArray(ref TileID.Sets.CountsAsPylon);
			ModTranslation pylonName = CreateMapEntryName();
			AddMapEntry(new Color(68, 68, 106), pylonName);
		}

		public override int? IsPylonForSale(int npcType, Player player, bool isNPCHappyEnough)
		{
			return null;
		}
		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<FireflyPylon_Item>();
			Main.LocalPlayer.cursorItemIconEnabled = true;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<FireflyPylon_TileEntity>().Kill(i, j);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 2, 3, ModContent.ItemType<FireflyPylon_Item>());
		}
		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
		{
			return Vector2.Distance(Main.LocalPlayer.Center, ModContent.GetInstance<FireflyPylon_TileEntity>().Position.ToVector2() + new Vector2(24, 32)) <= 80;
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawModPylon(i, j, spriteBatch, new Vector2(0, -12f), new Color(5, 0, 55, 30), Color.White, 4, ModContent.DustType<Dusts.FireflyPylonDust>());
			//DefaultDrawPylonCrystal(spriteBatch, i, j, crystalTexture, crystalHighlightTexture, new Vector2(0, -12f), new Color(5, 0, 55, 30), new Color(0,0,155,20), 4, CrystalVerticalFrameCount);
		}

		public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
		{

			bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
			DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.MythMod.ItemName.FireflyPylon_Item", ref mouseOverText);
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D tex = MythContent.QuickTexture("TheFirefly/Pylon/FireflyPylonGlow");

			spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

			base.PostDraw(i, j, spriteBatch);
		}
		private void DrawModPylon(int i, int j, SpriteBatch spriteBatch, Vector2 crystalOffset, Color pylonShadowColor, Color dustColor, int dustChanceDenominator, int dustType = 43)
        {
			Vector2 value = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen)
			{
				value = Vector2.Zero;
			}

			Point p = new Point(i, j);
			Tile tile = Main.tile[p.X, p.Y];
			if (tile == null || !tile.HasTile)
			{
				return;
			}

			TileObjectData tileData = TileObjectData.GetTileData(tile);
			int frameY = Main.tileFrameCounter[597] / CrystalVerticalFrameCount;
			Rectangle rectangle = crystalTexture.Frame(1, CrystalVerticalFrameCount, 0, frameY);
			Rectangle value2 = crystalHighlightTexture.Frame(1, CrystalVerticalFrameCount, 0, frameY);
			rectangle.Height--;
			value2.Height--;
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 vector = new Vector2((float)tileData.CoordinateFullWidth / 2f, (float)tileData.CoordinateFullHeight / 2f);
			Vector2 vector2 = p.ToWorldCoordinates(vector.X - 2f, vector.Y) + crystalOffset;
			float num = (float)Math.Sin((double)Main.GlobalTimeWrappedHourly * (Math.PI * 2.0) / 5.0);
			Vector2 value3 = vector2 + value + new Vector2(0f, num * 4f);
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)) && Main.rand.NextBool(dustChanceDenominator))
			{
				Rectangle r = Utils.CenteredRectangle(vector2, rectangle.Size());
				int num2 = Dust.NewDust(r.TopLeft(), r.Width, r.Height, dustType, 0f, 0f, 254, dustColor, 0.5f);
				Main.dust[num2].velocity *= 0.1f;
				Main.dust[num2].velocity.Y -= 0.2f;
			}

			Color color = Lighting.GetColor(p.X, p.Y);
			color = Color.Lerp(color, Color.White, 0.8f);
			spriteBatch.Draw(crystalTexture.Value, value3 - Main.screenPosition, rectangle, color * 0.7f, 0f, origin, 1f, SpriteEffects.None, 0f);
			float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * (MathF.PI * 2f) / 1f) * 0.2f + 0.8f;
			Color color2 = pylonShadowColor * scale;
			for (float num3 = 0f; num3 < 1f; num3 += 355f / (678f * MathF.PI))
			{
				spriteBatch.Draw(crystalTexture.Value, value3 - Main.screenPosition + (MathF.PI * 2f * num3).ToRotationVector2() * (6f + num * 2f), rectangle, color2, 0f, origin, 1f, SpriteEffects.None, 0f);
			}

			int num4 = 0;
			if (Main.InSmartCursorHighlightArea(p.X, p.Y, out bool actuallySelected))
			{
				num4 = 1;
				if (actuallySelected)
				{
					num4 = 2;
				}
			}

			if (num4 != 0)
			{
				int num5 = (color.R + color.G + color.B) / 3;
				if (num5 > 10)
				{
					Color selectionGlowColor = Colors.GetSelectionGlowColor(num4 == 2, num5);
					spriteBatch.Draw(crystalHighlightTexture.Value, value3 - Main.screenPosition, value2, selectionGlowColor, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
			}
		}
	}
	internal class FireflyPylon_Item:ModItem
    {
        public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<FireflyPylon>());
		}
        public override bool? UseItem(Player player)
        {
			if (player.itemAnimationMax == player.itemAnimation && Item.favorited)
			{
				//ModContent.GetInstance<FireflyPylon_TileEntity>().IsDestoryed = false;
				//Main.NewText("晶塔已修复");
			}
            return base.UseItem(player);
        }
    }
}
