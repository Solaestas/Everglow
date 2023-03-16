namespace Everglow.Food;

internal class FoodSystem : IModule
{
	string IModule.Name => "食物系统";
	void IModule.Load()
	{
		Terraria.On_Player.UpdateStarvingState += Player_UpdateStarvingState;
	}
	void IModule.Unload()
	{
		Terraria.On_Player.UpdateStarvingState -= Player_UpdateStarvingState;
	}
	private void Player_UpdateStarvingState(Terraria.On_Player.orig_UpdateStarvingState orig, Player self, bool withEmote)
	{
		orig(self, false);
	}
}
