using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class MossCover : ModBuff
{
	public Color origColor = Color.White;

	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.statDefense += 5;
		player.GetModPlayer<MossCoverPlayer>().HasMossCover = true;
		base.Update(player, ref buffIndex);
	}
}

public class MossCoverPlayer : ModPlayer
{
	public Color BuffColor = new Color(0.4f, 0.8f, 0.3f, 1f);

	public bool HasMossCover = false;

	public override void ResetEffects()
	{
		HasMossCover = false;
	}

	public override void PostUpdateBuffs()
	{
		if (HasMossCover)
		{
		}
		base.PostUpdateBuffs();
	}

	public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
	{
		if (HasMossCover)
		{
			r = BuffColor.R / 255f;
			g = BuffColor.G / 255f;
			b = BuffColor.B / 255f;
		}
		base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
	}
}