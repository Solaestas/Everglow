namespace Everglow.Core.PlayerUtils;


internal class PlayerManager : ModPlayer
{
	public VirtualKey ControlLeft { get; private set; } = new VirtualKey();
	public VirtualKey ControlRight { get; private set; } = new VirtualKey();
	public VirtualKey ControlUp { get; private set; } = new VirtualKey();
	public VirtualKey ControlDown { get; private set; } = new VirtualKey();
	public VirtualKey ControlJump { get; private set; } = new VirtualKey();
	public VirtualKey ControlUseItem { get; private set; } = new VirtualKey();
	public VirtualKey ControlUseTile { get; private set; } = new VirtualKey();
	public VirtualKey MouseLeft { get; private set; } = new VirtualKey();
	public VirtualKey MouseRight { get; private set; } = new VirtualKey();
	public MouseTrail MouseWorld { get; private set; } = new MouseTrail();
	public override void PostUpdate()
	{
		ControlLeft.LocalUpdate(Player.controlLeft);
		ControlRight.LocalUpdate(Player.controlRight);
		ControlUp.LocalUpdate(Player.controlUp);
		ControlDown.LocalUpdate(Player.controlDown);
		ControlJump.LocalUpdate(Player.controlJump);
		ControlUseItem.LocalUpdate(Player.controlUseItem);
		if (Main.myPlayer == Player.whoAmI)
		{
			MouseLeft.LocalUpdate(Main.mouseLeft);
			MouseRight.LocalUpdate(Main.mouseRight);
			ControlUseTile.LocalUpdate(Player.controlUseTile);
			MouseWorld.LocalUpdate(Main.MouseWorld);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				Everglow.PacketResolver.Send(new InputPacketToServer());
			}
		}
		else
		{
			MouseLeft.Forcast();
			MouseRight.Forcast();
			ControlUseTile.Forcast();
			MouseWorld.Forcast();
		}
	}


}
