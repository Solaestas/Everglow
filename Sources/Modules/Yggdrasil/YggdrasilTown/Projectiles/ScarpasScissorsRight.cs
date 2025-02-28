namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ScarpasScissorsRight : ScarpasScissorsCutProj
{
	protected override bool IsLeftBlade => false;

	public override string Texture => ModAsset.ScarpasScissorsRight_Mod;

	protected override Vector2 TextureOrigin => new Vector2(10.5f, 32.5f);
}