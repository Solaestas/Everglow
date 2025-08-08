namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;

public class ScarpasScissorsLeft : ScarpasScissorsCutProj
{
	protected override bool IsLeftBlade => true;

	public override string Texture => ModAsset.ScarpasScissorsLeft_Mod;

	protected override Vector2 TextureOrigin => new Vector2(17.5f, 23.5f);
}