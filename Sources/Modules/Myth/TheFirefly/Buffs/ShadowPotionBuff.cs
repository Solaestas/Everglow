using Everglow.Commons.FeatureFlags;
using Terraria.DataStructures;
using Terraria.ID;

namespace Everglow.Myth.TheFirefly.Buffs;

public class ShadowPotionBuff : ModBuff
{
	internal int LightTime = 0;
	Player player = Main.LocalPlayer;
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex)
	{
		Color playerLight = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16));
		if(Math.Max(Math.Max(playerLight.R, playerLight.G), playerLight.B) <= 190)
		{
			player.statDefense += 15;
			player.allDamage += 0.2f;
			LightTime++;
		}
		if (Math.Max(Math.Max(playerLight.R, playerLight.G), playerLight.B) >= 230)
		{
			player.lifeRegen -= (int)(player.statLifeMax2 * 0.01f);
			player.lifeRegenTime = 0;
			LightTime--;
		}
		if (LightTime > 4)
		{
			player.nightVision = true;
		}
		if (LightTime > 20) { LightTime = 20; }

	}
}
public class ShadowPotionPlayer : ModPlayer
{
	internal static int alpha = 0;
	public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
	{
		if (Player.HasBuff(ModContent.BuffType<ShadowPotionBuff>()))
		{
			if (alpha < 155)
				alpha++;
			else
				alpha--;

			Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Shadowflame, 0f, 0f, 155, default, 1.5f);
			dust.noGravity = true;
			dust.velocity *= 0.5f;
			dust.velocity.Y -= 1f;

			Color shadowSkinColor = new(80, 0, 120, alpha);

			drawInfo.colorBodySkin = shadowSkinColor;
			Player.skinColor = shadowSkinColor;
		}
		base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
	}
}