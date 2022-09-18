using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
	public class MothWorldDoor : ModTile
	{
        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileLavaDeath[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile((int)base.Type);
			ModTranslation modTranslation = base.CreateMapEntryName(null);
			modTranslation.SetDefault("MothWorld");
			base.AddMapEntry(new Color(148, 0, 255), modTranslation);
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
			if((player.Center - new Vector2(i, j)).Length() < 64)
            {
                if (player.itemAnimation == player.itemAnimationMax)
                {
                    if (SubWorldModule.SubworldSystem.IsActive<MothWorld>())
                    {
                        SubWorldModule.SubworldSystem.Exit();
                    }
                    else
                    {
                        if (!SubWorldModule.SubworldSystem.Enter<MothWorld>())
                        {
                            Main.NewText("Fail!");
                        }
                    }
                }
            }
            base.NearbyEffects(i, j, closer);
        }
    }
}
