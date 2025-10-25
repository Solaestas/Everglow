using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils.Structs;
using Terraria.Audio;
using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil.YggdrasilTown.Liquids;

public class DarkSludgeLiquid : ModLiquid
{
	public override void SetStaticDefaults()
	{
		VisualViscosity = 200;
		LiquidFallLength = 20;
		DefaultOpacity = 1f;
		SlopeOpacity = 1f;
		WaterRippleMultiplier = 1f;
		SplashDustType = DustID.BorealWood;
		SplashSound = SoundID.SplashWeak;
		AddMapEntry(new Color(31, 26, 45));
	}

	public override int LiquidMerge(int i, int j, int otherLiquid)
	{
		return TileID.Dirt;
	}

	public override LightMaskMode LiquidLightMaskMode(int i, int j)
	{
		return LightMaskMode.None;
	}

	public override int ChooseWaterfallStyle(int i, int j)
	{
		return ModContent.GetInstance<DarkSludgeWaterfall>().Slot;
	}

	public override void RetroDrawEffects(int i, int j, SpriteBatch spriteBatch, ref RetroLiquidDrawInfo drawData, float liquidAmountModified, int liquidGFXQuality)
	{
		drawData.liquidAlphaMultiplier = 1f;
	}

	//Copies the normal mod splash code but makes the dust splash downwards rather than upwards
	#region upside down splash
	public override bool OnPlayerSplash(Player player, bool isEnter)
	{
		for (int j = 0; j < 20; j++)
		{
			int dust = Dust.NewDust(new Vector2(player.position.X - 6f, player.position.Y - (player.height / 2) + 8f), player.width + 12, 24, SplashDustType);
			Main.dust[dust].velocity.Y -= -2f;
			Main.dust[dust].velocity.X *= 2.5f;
			Main.dust[dust].scale = 1.3f;
			Main.dust[dust].alpha = 100;
			Main.dust[dust].noGravity = true;
		}
		SoundEngine.PlaySound(SplashSound, player.position);
		return false;
	}

	public override bool OnNPCSplash(NPC npc, bool isEnter)
	{
		for (int j = 0; j < 10; j++)
		{
			int dust = Dust.NewDust(new Vector2(npc.position.X - 6f, npc.position.Y - (npc.height / 2) + 8f), npc.width + 12, 24, SplashDustType);
			Main.dust[dust].velocity.Y -= -2f;
			Main.dust[dust].velocity.X *= 2.5f;
			Main.dust[dust].scale = 1.3f;
			Main.dust[dust].alpha = 100;
			Main.dust[dust].noGravity = true;
		}
		if (npc.aiStyle != NPCAIStyleID.Slime &&
				npc.type != NPCID.BlueSlime && npc.type != NPCID.MotherSlime && npc.type != NPCID.IceSlime && npc.type != NPCID.LavaSlime &&
				npc.type != NPCID.Mouse &&
				npc.aiStyle != NPCAIStyleID.GiantTortoise &&
				!npc.noGravity)
		{
			SoundEngine.PlaySound(SplashSound, npc.position);
		}
		return false;
	}

	public override bool OnProjectileSplash(Projectile proj, bool isEnter)
	{
		for (int j = 0; j < 10; j++)
		{
			int dust = Dust.NewDust(new Vector2(proj.position.X - 6f, proj.position.Y - (proj.height / 2) + 8f), proj.width + 12, 24, SplashDustType);
			Main.dust[dust].velocity.Y -= -2f;
			Main.dust[dust].velocity.X *= 2.5f;
			Main.dust[dust].scale = 1.3f;
			Main.dust[dust].alpha = 100;
			Main.dust[dust].noGravity = true;
		}
		SoundEngine.PlaySound(SplashSound, proj.position);
		return false;
	}

	public override bool OnItemSplash(Item item, bool isEnter)
	{
		for (int j = 0; j < 5; j++)
		{
			int dust = Dust.NewDust(new Vector2(item.position.X - 6f, item.position.Y - (item.height / 2) + 8f), item.width + 12, 24, SplashDustType);
			Main.dust[dust].velocity.Y -= -2f;
			Main.dust[dust].velocity.X *= 2.5f;
			Main.dust[dust].scale = 1.3f;
			Main.dust[dust].alpha = 100;
			Main.dust[dust].noGravity = true;
		}
		SoundEngine.PlaySound(SplashSound, item.position);
		return false;
	}
	#endregion
}