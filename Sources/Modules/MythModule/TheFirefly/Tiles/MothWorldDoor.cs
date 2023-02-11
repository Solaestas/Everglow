using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Terraria.ObjectData;
using SubworldLibrary;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class MothWorldDoor : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 7;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("MothWorld");
            base.AddMapEntry(new Color(148, 0, 255), modTranslation);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/MothWorldDoorGlow");

            spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

            base.PostDraw(i, j, spriteBatch);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if ((player.Center - new Vector2(i * 16, j * 16)).Length() < 12)
            {
				//if (SubWorldModule.SubworldSystem.IsActive<MothWorld>())
				//{
				//    SubWorldModule.SubworldSystem.Exit();
				//}
				//else
				//{
				//    if (!SubWorldModule.SubworldSystem.Enter<MothWorld>())
				//    {
				//        Main.NewText("Fail!");
				//    }
				//}
				if (!SubworldSystem.IsActive<MothWorld>())
				{
					SubworldSystem.Enter<MothWorld>();
				}
				else if (SubworldSystem.IsActive<MothWorld>())
				{
					SubworldSystem.Exit();
				}
				else
                {
                    if (!SubworldSystem.Enter<MothWorld>())
                    {
                        Main.NewText("Fail!");
                    }
                }
            }
            base.NearbyEffects(i, j, closer);
        }
    }
}