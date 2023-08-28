using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.SquamousShell;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using SubworldLibrary;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class SquamousShellSeal : ModTile
{
	public int ReSpawnTimer = 0;
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;

		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 10;
		TileObjectData.newTile.Width = 20;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(79, 76, 75));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
		base.NumDust(i, j, fail, ref num);
	}
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if(tile.TileFrameX == 0 && tile.TileFrameY == 0)
		{
			Color lightColor = Lighting.GetColor(i + 10, j + 5);
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;

			Texture2D deadS = ModAsset.DeadSquamousShell.Value;
			if (ReSpawnTimer <= 0)
			{
				spriteBatch.Draw(deadS, new Vector2(i, j) * 16 - Main.screenPosition + zero, null, lightColor, 0, deadS.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}
		}
		return base.PreDraw(i, j, spriteBatch);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		base.NearbyEffects(i, j, closer);
	}
	public override void MouseOver(int i, int j)
	{
		base.MouseOver(i, j);
	}
	public override bool RightClick(int i, int j)
	{
		if(NPC.CountNPCS(ModContent.NPCType<SquamousShell>()) == 0)
		{
			NPC.NewNPC(null, i * 16, j * 16, ModContent.NPCType<SquamousShell>());
			ReSpawnTimer = 360000;
		}
		return base.RightClick(i, j);
	}
}