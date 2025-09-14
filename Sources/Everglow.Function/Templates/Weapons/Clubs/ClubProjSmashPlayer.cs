namespace Everglow.Commons.Templates.Weapons.Clubs;

public class ClubProjSmashPlayer : ModPlayer
{
	public override void PostUpdateMiscEffects()
	{
		if (ClubProjSmash.OwnSmashClubPlayers.Contains(Player.whoAmI))
		{
			Player.maxFallSpeed += 10000f;
		}
	}
}