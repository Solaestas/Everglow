using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.Tiles
{
    public class BloodLightCrystalEntity : ModTileEntity
    {
        public const float DISSOLVE_TIME = 1.5f; //溶解时长（秒）

        private float dissolveProgress; //溶解进度; 0为未开始，1为完成
        public override void Update()
        {
            //Main.NewText("Exists");
        }
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            Main.NewText("[" + x + "," + y + "]:" + tile.HasTile + "," + tile.TileType + "<>" + ModContent.TileType<BloodLightCrystal>());
            return tile.HasTile && tile.TileType == ModContent.TileType<BloodLightCrystal>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            dissolveProgress = 0f;
            Main.NewText("Placed");
            return Place(i, j);
        }
    }
}
