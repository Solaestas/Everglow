﻿using Terraria.Localization;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class DarkMud : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileMerge[Type][(ushort)ModContent.TileType<Tiles.StoneScaleWood>()] = false;
            Main.tileBlendAll[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = false;

            Main.ugBackTransition = 1000;
            DustType = DustID.BorealWood;
            MinPick = 150;
            HitSound = SoundID.Dig;
            
            ItemDrop = ModContent.ItemType<Items.StoneDragonScaleWood>();

            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(31, 26, 45), modTranslation);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
        }
        public override void RandomUpdate(int i, int j)
        {
            int CountNoneA = 0;
            int CountA = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -4; y < 0; y++)
                {
                    if (Main.tile[i + x, j + y].HasTile)
                    {
                        CountNoneA++;
                    }
                }
            }
            for (int x = -1; x < 2; x++)
            {
                if (Main.tile[i + x, j].HasTile)
                {
                    CountA++;
                }
            }
            if (!Main.tile[i - 5, j - 1].HasTile && !Main.tile[i + 5, j - 1].HasTile && !Main.tile[i - 2, j - 1].HasTile && !Main.tile[i + 2, j - 1].HasTile && !Main.tile[i + 3, j - 1].HasTile && !Main.tile[i - 3, j - 1].HasTile && !Main.tile[i - 4, j - 1].HasTile && !Main.tile[i + 4, j - 1].HasTile)
            {
                if (Main.rand.NextBool(8))
                {
                    if (CountNoneA == 0)
                    {
                        if (CountA == 3)
                        {
                            //int Dy = Main.rand.Next(5);
                            //WorldGen.Place3x4(i, j - 1, (ushort)ModContent.TileType<OceanMod.Tiles.Tree1.CyanVineOre>(), 0);
                            //CombatText.NewText(new Rectangle(i * 16, j * 16, 16, 16), Color.Cyan, Dy);
                        }
                    }
                }
            }
        }
    }
}
