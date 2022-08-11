using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Pylon
{
	internal class Pylon_A_TileEntity:TEModdedPylon
    {
		internal bool IsDestoryed = true;
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(IsDestoryed);
        }
        public override void NetReceive(BinaryReader reader)
        {
			IsDestoryed = reader.ReadBoolean();
        }
    }
    internal class Pylon_A:ModPylon
    {
		public const int CrystalHorizontalFrameCount = 2;
		public const int CrystalVerticalFrameCount = 8;
		public const int CrystalFrameHeight = 64;
		public Asset<Texture2D> crystalTexture;
		public Asset<Texture2D> mapIcon;
		public override void Load()
		{
			crystalTexture = ModContent.Request<Texture2D>(Texture + "_Crystal");
			mapIcon = ModContent.Request<Texture2D>(Texture + "_MapIcon");
		}
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TEModdedPylon moddedPylon = ModContent.GetInstance<Pylon_A_TileEntity>();
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.PreventsSandfall[Type] = true;
			AddToArray(ref TileID.Sets.CountsAsPylon);
			//ModTranslation pylonName = CreateMapEntryName();
			//AddMapEntry(Color.Transparent, pylonName);
		}

		public override int? IsPylonForSale(int npcType, Player player, bool isNPCHappyEnough)
		{
			return null;
		}
		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.cursorItemIconEnabled = true;
			Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Pylon_A_Item>();
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<Pylon_A_TileEntity>().Kill(i, j);
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 2, 3, ModContent.ItemType<Pylon_A_Item>());
		}
		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
		{
			if(ModContent.GetInstance<Pylon_A_TileEntity>().IsDestoryed)
            {
				return false;
            }
			return Vector2.Distance(Main.LocalPlayer.Center, ModContent.GetInstance<Pylon_A_TileEntity>().Position.ToVector2() + new Vector2(24, 32)) <= 80;
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (ModContent.GetInstance<Pylon_A_TileEntity>().IsDestoryed)
			{
				return;
			}
			DefaultDrawPylonCrystal(spriteBatch, i, j, crystalTexture, Color.White, CrystalFrameHeight, CrystalHorizontalFrameCount, CrystalVerticalFrameCount);
		}

		public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
		{
			if (ModContent.GetInstance<Pylon_A_TileEntity>().IsDestoryed ||
				Vector2.Distance(pylonInfo.PositionInTiles.ToVector2() * 16 + new Vector2(32, 40), Main.LocalPlayer.Center) > 80)
			{
				return;
			}
			bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
			DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.MythMod.ItemName.Pylon_A_Item", ref mouseOverText);
		}
	}
	internal class Pylon_A_Item:ModItem
    {
        public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Pylon_A>());
		}
        public override bool? UseItem(Player player)
        {
			if (player.itemAnimationMax == player.itemAnimation && Item.favorited)
			{
				ModContent.GetInstance<Pylon_A_TileEntity>().IsDestoryed = false;
				Main.NewText("晶塔已修复");
			}
            return base.UseItem(player);
        }
    }
}
