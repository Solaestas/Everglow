using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
    public class GlowWoodDresserType2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileContainer[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicDresser[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = ModContent.DustType<BlueGlow>();
            AdjTiles = new int[] { TileID.Dressers };
            DresserDrop = ModContent.ItemType<Items.Furnitures.GlowWoodDresserType2>();

            // Names
            ContainerName.SetDefault("GlowWood Dresser");
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("GlowWood Dresser");
            AddMapEntry(new Color(0, 14, 175), name);

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY == 0)
            {
                Main.CancelClothesWindow(true);
                Main.mouseRightRelease = false;
                int left = (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameX / 18);
                left %= 3;
                left = Player.tileTargetX - left;
                int top = Player.tileTargetY - (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY / 18);
                if (player.sign > -1)
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    player.sign = -1;
                    Main.editSign = false;
                    Main.npcChatText = string.Empty;
                }
                if (Main.editChest)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    Main.editChest = false;
                    Main.npcChatText = string.Empty;
                }
                if (player.editedChestName)
                {
                    NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                    player.editedChestName = false;
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    if (left == player.chestX && top == player.chestY && player.chest != -1)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
                        Main.stackSplit = 600;
                    }
                }
                else
                {
                    player.piggyBankProjTracker.Clear();
                    int num213 = Chest.FindChest(left, top);
                    if (num213 != -1)
                    {
                        Main.stackSplit = 600;
                        if (num213 == player.chest)
                        {
                            player.chest = -1;
                            Recipe.FindRecipes();
                            SoundEngine.PlaySound(SoundID.MenuClose);
                        }
                        else if (num213 != player.chest && player.chest == -1)
                        {
                            player.chest = num213;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            SoundEngine.PlaySound(SoundID.MenuOpen);
                            player.chestX = left;
                            player.chestY = top;
                        }
                        else
                        {
                            player.chest = num213;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            player.chestX = left;
                            player.chestY = top;
                        }
                        Recipe.FindRecipes();
                        return true;
                    }
                }
                return false;
            }
            Main.playerInventory = false;
            player.chest = -1;
            Recipe.FindRecipes(false);
            Main.interactedDresserTopLeftX = Player.tileTargetX;
            Main.interactedDresserTopLeftY = Player.tileTargetY;
            Main.OpenClothesWindow();
            return true;
        }

        public override void MouseOverFar(int i, int j)
        {
            string chestName = this.ContainerName.GetDefault();
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            int left = Player.tileTargetX;
            int top = Player.tileTargetY;
            left -= (int)(tile.TileFrameX % 54 / 18);
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }
            int chestIndex = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chestIndex < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
            }
            else
            {
                string defaultName = TileLoader.ContainerName(tile.TileType); // This gets the ContainerName text for the currently selected language
                if (player.cursorItemIconText == defaultName)
                {
                    player.cursorItemIconText = Main.chest[chestIndex].name;
                }
                else
                {
                    player.cursorItemIconText = chestName;
                }
                if (player.cursorItemIconText == chestName)
                {
                    player.cursorItemIconID = ModContent.ItemType<Items.Furnitures.GlowWoodDresserType2>();
                    player.cursorItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }

        public override void MouseOver(int i, int j)
        {
            string chestName = this.ContainerName.GetDefault();
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            int left = Player.tileTargetX;
            int top = Player.tileTargetY;
            left -= (int)(tile.TileFrameX % 54 / 18);
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chest < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                if (Main.chest[chest].name != "")
                {
                    player.cursorItemIconText = Main.chest[chest].name;
                }
                else
                {
                    player.cursorItemIconText = chestName;
                }
                if (player.cursorItemIconText == chestName)
                {
                    player.cursorItemIconID = ModContent.ItemType<Items.Furnitures.GlowWoodDresserType2>();
                    player.cursorItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY > 0)
            {
                player.cursorItemIconID = ItemID.FamiliarShirt;
                player.cursorItemIconText = "  ";
            }

        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 16, 32, DresserDrop);
            Chest.DestroyChest(x, y);
        }
    }
}