using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Tiles
{
    public class BloodifyPlatform : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[(int)base.Type] = true;
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileSolidTop[(int)base.Type] = true;
            Main.tileSolid[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            Main.tileTable[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            TileID.Sets.Platforms[(int)base.Type] = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 27;
            TileObjectData.newTile.StyleWrapLimit = 27;
            TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile((int)base.Type);
            base.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(168, 11, 0), modTranslation);
            modTranslation.SetDefault("Bloodify Platform");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "血化平台");
            HitSound = SoundID.Grass;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, DustID.Blood, 0f, 0f, 1, Color.White, 1f);
            return false;
        }
        public override void PostSetDefaults()
        {
            Main.tileNoSunLight[(int)base.Type] = false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.rand.Next(30) == 0)
            {
                Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, DustID.Blood, 0f, 0f, 1, Color.White, 1f);
            }
            if (Main.rand.Next(300) == 0)
            {
                WorldGen.KillTile(i, j);
            }
            return base.PreDraw(i, j, spriteBatch);
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}
