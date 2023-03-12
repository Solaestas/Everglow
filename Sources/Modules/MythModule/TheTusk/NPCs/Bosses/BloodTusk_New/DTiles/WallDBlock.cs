using Terraria.Localization;
using Terraria.ID;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Everglow.Sources.Modules.ZYModule.TileModule;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk_New.DTiles
{
    internal class WallDBlock : DBlock
    {
        public NPC npc;
        public int dir = 1;
        public int id;
        public override Color MapColor => new Color(125, 0, 0);
        public override void Draw()
        {
            //base.Draw();
        }
        public override void AI()
        {
            if (npc != null)
            {
                if (id == 0)
                {
                    Position = new Vector2(npc.Center.X - 125, npc.Center.Y- npc.ai[0]);
                    size = new Vector2(50, npc.ai[0]);

                    if (dir == -1)
                    {
                        Position = new Vector2(npc.Center.X + 125 - size.X, npc.Center.Y - npc.ai[0]);
                    }
                }
                else if(id==1)
                {
                    Position = new Vector2(npc.Center.X - 90, npc.Center.Y- npc.ai[0] * 0.3f);
                    size = new Vector2(70, npc.ai[0] * 0.3f);

                    if (dir == -1)
                    {
                        Position = new Vector2(npc.Center.X + 90 - size.X, npc.Center.Y - npc.ai[0] * 0.3f);
                    }
                }
                if (!Main.npc[npc.whoAmI].active)
                {
                    Kill();
                }
                Position+=new Vector2(0,30);
            }
        }
    }
}
