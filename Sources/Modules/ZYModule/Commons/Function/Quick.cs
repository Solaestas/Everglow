using System.Diagnostics.CodeAnalysis;
using Everglow.Common.CustomTile;
using Everglow.Common.CustomTile.Collide;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.ZYModule.Commons.Function;


internal enum TextureType
{
	Noise,
	WhitePixel,
	Circle,
	WhiteGreenBar
}
internal enum EffectType
{
	Default,
	Test
}
internal static class Quick
{
	public static Dictionary<TextureType, Asset<Texture2D>> textures = new();
	public static Dictionary<EffectType, Asset<Effect>> effects = new();
	public static GraphicsDevice GD => Main.instance.GraphicsDevice;
	public static SpriteBatch SB => Main.spriteBatch;
	public static float AirSpeed => 0.001f;
	public const string ModulePath = "Everglow/Sources/Modules/ZYModule/";
	public const string ResourcePath = ModulePath + "Commons/Resource/";
	public static Texture2D GetValue(this TextureType type, bool async = true)
	{
		string path = ResourcePath + "Images/" + type.ToString().Replace('_', '/');
		if (textures.TryGetValue(type, out var texture))
		{
			if (!async && !texture.IsLoaded)
				texture = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
			return texture.Value;
		}
		else
		{
			textures[type] = ModContent.Request<Texture2D>(path, async ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);
			return textures[type].Value;
		}
	}
	public static Effect GetValue(this EffectType type, bool async = false)
	{
		string path = ResourcePath + "Effects/" + type.ToString().Replace('_', '/');
		if (effects.TryGetValue(type, out var effect))
		{
			if (!async && !effect.IsLoaded)
				effect = ModContent.Request<Effect>(path, AssetRequestMode.ImmediateLoad);
			return effect.Value;
		}
		else
		{
			effects[type] = ModContent.Request<Effect>(path, async ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);
			return effects[type].Value;
		}
	}
	public static Asset<Texture2D> RequestTexture(string path, bool async = true) =>
		ModContent.Request<Texture2D>(ModulePath + path,
		async ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);
	public static Asset<Effect> RequestEffect(string path, bool async = false) =>
		ModContent.Request<Effect>(ModulePath + path,
		async ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);

	public static void Log(object obj)
	{
		Everglow.Instance.Logger.Info(obj);
		Main.NewText(obj, Color.Green);
		Console.WriteLine(obj);
	}
	[DoesNotReturn]
	public static void Throw(Exception ex)
	{
		Everglow.Instance.Logger.Error($"{ex.Source} : {ex.Message}");
		Console.WriteLine(ex);
		throw ex;
	}

	public static Vector2 ToMouse(this Player player)
	{
		return player.GetModPlayer<PlayerManager>().MouseWorld.Current - player.Center;
	}





	/// <summary>
	/// 造成一次无法被闪避的伤害
	/// </summary>
	/// <param name="player">受到伤害的玩家</param>
	/// <param name="from">伤害的直接来源，可能是NPC或者Projectile</param>
	/// <param name="damage">基础伤害</param>
	/// <param name="pvp">是否为PVP造成的伤害</param>
	public static void ApplyDamage(this Player player, Entity from, int damage, bool pvp)
	{
		int direction = player.Center.X > from.Center.X ? -1 : 1;
		bool crit = false;
		int realDamage;

		damage = Main.DamageVar(damage, -player.luck);

		int banner;
		if (from is NPC npc)
		{
			banner = Item.NPCtoBanner(npc.BannerID());
			if (banner > 0 && player.HasNPCBannerBuff(banner))
			{
				var bannerEffect = ItemID.Sets.BannerStrength[Item.BannerToItem(banner)];
				damage = !Main.expertMode ?
					(int)(damage * bannerEffect.NormalDamageReceived) :
					(int)(damage * bannerEffect.ExpertDamageReceived);
			}

			if (Main.myPlayer == player.whoAmI && !npc.dontTakeDamage)
			{
				float thorns = player.turtleThorns ? 2 : player.thorns;
				if (thorns > 0)
				{
					int thornsDamage = (int)(damage * thorns);
					player.ApplyDamageToNPC(npc, Math.Min(thornsDamage, 1000), 10, -direction, false);
				}
				if (player.cactusThorns)
					player.ApplyDamageToNPC(npc, Main.masterMode ? 45 : Main.expertMode ? 30 : 15, 10, -direction, false);
			}

			if (player.resistCold && npc.coldDamage)
				damage = (int)(damage * 0.7f);

			NPCLoader.ModifyHitPlayer(npc, player, ref damage, ref crit);
			PlayerLoader.ModifyHitByNPC(player, npc, ref damage, ref crit);
			realDamage = (int)player.Hurt(PlayerDeathReason.ByNPC(npc.whoAmI), damage, direction, false, false, crit);
			if (realDamage > 0 && !player.dead)
				player.StatusFromNPC(npc);
			NPCLoader.OnHitPlayer(npc, player, damage, crit);
			PlayerLoader.OnHitByNPC(player, npc, damage, crit);
		}
		else if (from is Projectile proj)
		{
			banner = proj.bannerIdToRespondTo;
			if (banner > 0 && player.HasNPCBannerBuff(banner))
			{
				var bannerEffect = ItemID.Sets.BannerStrength[Item.BannerToItem(banner)];
				damage = !Main.expertMode ?
					(int)(damage * bannerEffect.NormalDamageReceived) :
					(int)(damage * bannerEffect.ExpertDamageReceived);
			}

			if (player.resistCold && proj.coldDamage)
				damage = (int)(damage * 0.7f);

			float multiple = Main.GameModeInfo.EnemyDamageMultiplier;
			if (Main.GameModeInfo.IsJourneyMode)
			{
				CreativePowers.DifficultySliderPower power = CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>();
				if (power.GetIsUnlocked())
					multiple = power.StrengthMultiplierToGiveNPCs;
			}
			damage = (int)(damage * multiple);
			ProjectileLoader.ModifyHitPlayer(proj, player, ref damage, ref crit);
			PlayerLoader.ModifyHitByProjectile(player, proj, ref damage, ref crit);
			realDamage = (int)player.Hurt(PlayerDeathReason.ByProjectile(pvp ? proj.owner : -1, proj.whoAmI), damage, direction, pvp, false, crit);
			if (realDamage > 0)
				proj.StatusPlayer(player.whoAmI);
			ProjectileLoader.OnHitPlayer(proj, player, realDamage, crit);
			PlayerLoader.OnHitByProjectile(player, proj, realDamage, crit);
		}

	}
}
