using Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.VFXs;
using Everglow.Sources.Commons.Core.VFX;

namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.Tiles
{
    public class BloodLightCrystal : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            MinPick = 375;
            ItemDrop = ModContent.ItemType<Items.BloodLightCrystal>();
            AddMapEntry(new Color(107, 34, 21, 205));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Color c0 = Lighting.GetColor(i, j);

            if(c0.R > 160 && c0.G + c0.B < 80)
            {
                BrokenCrystal bc = new BrokenCrystal
                {
                    timeLeft = 70,
                    size = Main.rand.NextFloat(0.65f, 1f),
                    velocity = new Vector2(Main.rand.NextFloat(2.5f, 7.5f), 0).RotatedByRandom(6.283),
                    Active = true,
                    Visible = true,
                    position = new Vector2(i * 16 + 8, j * 16 + 8)
                };
                if (Main.rand.NextBool(4))
                {
                    Dust d = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<Dusts.BloodSpark>());
                    d.velocity = new Vector2(Main.rand.NextFloat(1.5f, 4f), 0).RotatedByRandom(6.283);

                    d.color.R = (byte)Main.rand.Next(15,255);
                    d.alpha= Main.rand.Next(10);
                    d.noGravity = true;

                    
                    VFXManager.Add(bc);
                }
                WorldGen.KillTile(i, j,false,false,true);
            }
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

        }
    }
}
