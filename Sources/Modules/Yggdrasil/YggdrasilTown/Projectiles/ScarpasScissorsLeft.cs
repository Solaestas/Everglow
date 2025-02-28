namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ScarpasScissorsLeft : ScarpasScissorsCutProj
{
	protected override bool IsLeftBlade => true;

	public override string Texture => ModAsset.ScarpasScissorsLeft_Mod;

	protected override Vector2 TextureOrigin => new Vector2(22.5f, 32.5f);
}