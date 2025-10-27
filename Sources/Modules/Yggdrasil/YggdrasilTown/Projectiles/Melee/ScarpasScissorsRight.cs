namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;

public class ScarpasScissorsRight : ScarpasScissorsCutProj
{
	protected override bool IsLeftBlade => false;

	public override string Texture => ModAsset.ScarpasScissorsRight_Mod;

	protected override Vector2 TextureOrigin => new Vector2(9.5f, 25.5f);
}