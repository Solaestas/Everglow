using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class StoneScaleWood : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreTile>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineStone>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreSmallUp>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreSmall>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreLargeUp>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreLarge>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreMiddle>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<DarkForestSoil>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<DarkForestGrass>()] = true;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileShine2[Type] = false;

		DustType = ModContent.DustType<StoneDragonScaleWoodDust>();
		MinPick = 150;
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(44, 40, 37));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		//Tile tile = Main.tile[i, j];
		//if(!tile.HasTile)
		//{
		//	return false;
		//}
		//spriteBatch.End();
		//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

		//Vector2 worldPos = new Vector2(i, j);
		//Texture2D noiseTex = ModAsset.DragonSacleNoise.Value;
		//Effect noise = ModAsset.DragonSacle.Value;
		//noise.Parameters["tex0"].SetValue(noiseTex);
		//noise.Parameters["worldPos"].SetValue(worldPos);
		//noise.Parameters["textureWidth"].SetValue(512);
		//noise.CurrentTechnique.Passes["newTexutre"].Apply();
		return true;
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		//spriteBatch.End();
		//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
	}
}
