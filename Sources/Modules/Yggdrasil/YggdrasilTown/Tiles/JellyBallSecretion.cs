using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class JellyBallSecretion : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;

		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = false;
		Main.tileShine2[Type] = false;

		DustType = ModContent.DustType<JellyBallGel>();
		HitSound = SoundID.NPCHit1;

		AddMapEntry(new Color(25, 39, 160));
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return base.PreDraw(i, j, spriteBatch);
	}
}