namespace Everglow.Sources.Modules.MythModule.TheFirefly.Walls
{
    public class LiveFluorescentLeafWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<Dusts.FluorescentTreeDust>();
            ItemDrop = ModContent.ItemType<Items.DarkCocoonWall>();
            HitSound = SoundID.Dig;
            AddMapEntry(new Color(8, 22, 48));
        }
    }
}