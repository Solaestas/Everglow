using Terraria.DataStructures;

namespace Everglow.Commons.MEAC;

public class TestPlayerDrawer : ModPlayer
{
	public bool HideLeg { get; set; } = false;

	public float HeadRotation { get; set; } = 0;

	public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
	{
		Player.headRotation = HeadRotation;
		if (HideLeg)
		{
			if (Player.gravDir == 1)
			{
				Player.bodyPosition = new Vector2(0, 2);// 偷偷调个参,防止腿太短的玩家裂开
			}

			drawInfo.hidesBottomSkin = true;
		}
	}
}