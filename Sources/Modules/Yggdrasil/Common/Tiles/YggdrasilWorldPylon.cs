using Everglow.Commons.Templates.Pylon;
using Everglow.Yggdrasil.Common.Dusts;
using Everglow.Yggdrasil.Common.Projectiles;

namespace Everglow.Yggdrasil.Common.Tiles;

public class YggdrasilWorldPylon : EverglowPylonBase<YggdrasilWorldPylonTileEntity>
{
	public override int DropItemType => ModContent.ItemType<YggdrasilWorldPylon_Item>();

	public override void PostSetDefaults() => AddMapEntry(new Color(255, 255, 227));

	public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Lighting.AddLight(new Point(i + 1, j).ToWorldCoordinates(), new Vector3(1.3f, 1.3f, 1.3f));
		DrawModPylon(spriteBatch, i, j, CrystalTexture, CrystalHighlightTexture, new Vector2(0, DefaultVerticalOffset), Color.White * 0.1f, Color.White, CrystalVerticalFrameCount, true, ModContent.DustType<YggdrasilWorldPylonDust>());
	}

	public override bool RightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;
		int projectileType = ModContent.ProjectileType<TeleportToYggdrasil>();
		if (player.ownedProjectileCounts[projectileType] <= 0)
		{
			player.AddBuff(BuffID.Shimmer, 30);
			Projectile.NewProjectileDirect(WorldGen.GetProjectileSource_PlayerOrWires(i, j, false, player), Main.MouseWorld, Vector2.zeroVector, projectileType, 0, 0, player.whoAmI);
		}
		return false;
	}
}