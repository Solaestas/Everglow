using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class FemaleLampLeaves : ModTile, ISceneTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<LampWood_Dust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(71, 71, 145));
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return false;
	}
	public void AddScene(int i, int j)
	{
		for (int x = 0; x < 3; x++)
		{
			if (!Main.rand.NextBool(4))
			{
				FemaleLampLeaves_leaf leaf = new FemaleLampLeaves_leaf { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = ModContent.TileType<FemaleLampLeaves>() };
				leaf.rotation = Main.rand.NextFloat(6.283f);
				leaf.startRotation = leaf.rotation;
				leaf.scale = Main.rand.NextFloat(0.85f, 1.15f);
				Ins.VFXManager.Add(leaf);
			}
			else
			{
				FemaleLampLeaves_leaf_fore leaf = new FemaleLampLeaves_leaf_fore { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = ModContent.TileType<FemaleLampLeaves>() };
				leaf.rotation = Main.rand.NextFloat(6.283f);
				leaf.startRotation = leaf.rotation;
				leaf.scale = Main.rand.NextFloat(0.75f, 1.25f);
				Ins.VFXManager.Add(leaf);
			}
		}
	}
}
