using System.Reflection;
using Everglow.Commons.Mechanics;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Cooldowns;
using Everglow.Yggdrasil.KelpCurtain.Items.Armors.DevilHeart;
using Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;
using Everglow.Yggdrasil.KelpCurtain.Items.PermanentBoosters;
using Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Ruin;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;
using Everglow.Yggdrasil.Netcode;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Cooldowns;
using Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Auburn;
using Everglow.Yggdrasil.YggdrasilTown.Items.PermanentBoosters;
using SubworldLibrary;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using static Everglow.Yggdrasil.KelpCurtain.Items.Armors.Ruin.RuinMask;
using static Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.IstafelsSunfireGrasp;

namespace Everglow.Yggdrasil.Common;

public class YggdrasilPlayer : ModPlayer
{
	#region Permanent Boosters

	/// <summary>
	/// <see cref="AntiHeavenSicknessPill"/>
	/// </summary>
	public int ConsumedAntiHeavenSicknessPill { get; set; }

	/// <summary>
	/// <see cref="JadeFruit"/>"/>
	/// </summary>
	public int ConsumedJadeGlazeFruit { get; set; }

	/// <summary>
	/// <see cref="LampBorerHoney"/>
	/// </summary>
	public int ConsumedLampBorerHoney { get; set; }

	/// <summary>
	/// <see cref="SquamousCore"/>
	/// </summary>
	public int ConsumedSquamousCore { get; set; }

	public bool UseAntiHeavenSicknessPill()
	{
		if (ConsumedAntiHeavenSicknessPill < AntiHeavenSicknessPill.AntiHeavenSicknessPillMax)
		{
			ConsumedAntiHeavenSicknessPill++;
			return true;
		}
		return false;
	}

	public bool UseJadeGlazeFruit()
	{
		if (ConsumedJadeGlazeFruit < JadeFruit.MaxJadeFruits)
		{
			ConsumedJadeGlazeFruit++;
			return true;
		}
		return false;
	}

	public bool UseSquamousCore()
	{
		if (ConsumedSquamousCore < SquamousCore.SquamousCoreMax)
		{
			ConsumedSquamousCore++;
			return true;
		}
		return false;
	}

	public bool UseLampBorerHoney()
	{
		if (ConsumedLampBorerHoney < LampBorerHoney.MaxUse)
		{
			ConsumedLampBorerHoney++;
			return true;
		}
		return false;
	}

	#endregion

	#region Armors

	public bool lightSeekerRangedSet = false;
	public bool devilHeartSet = false;
	public bool molluscsSet = false;
	public bool auburnSet = false;
	public bool ruinSet = false;

	#endregion

	#region Accessories

	public bool istafelsSunfireGrasp = false;

	#endregion

	public override bool CloneNewInstances => true;

	public override void Load()
	{
		MonoModHooks.Add(typeof(PlayerLoader).GetMethod(nameof(PlayerLoader.ResetMaxStatsToVanilla), BindingFlags.Public | BindingFlags.Static), Hook_ResetMaxStatsToVanilla);
	}

	public static void Hook_ResetMaxStatsToVanilla(Action<Player> orig, Player player)
	{
		int lifeFix1 = 0, lifeFix2 = 0, lifeFix3 = 0, lifeFix4 = 0, manaFix1 = 0;
		if (player.TryGetModPlayer(out YggdrasilPlayer modPlayer))
		{
			lifeFix1 = AntiHeavenSicknessPill.LifeBonusTable.Take(modPlayer.ConsumedAntiHeavenSicknessPill).Sum(p => p.Value);
			lifeFix2 = modPlayer.ConsumedJadeGlazeFruit * JadeFruit.LifePerJadeFruit;
			lifeFix3 = modPlayer.ConsumedSquamousCore * SquamousCore.SquamousCoreLife;
			lifeFix4 = modPlayer.ConsumedLampBorerHoney * LampBorerHoney.LifePerHoney;
		}

		if (SubworldSystem.IsActive<YggdrasilWorld>())
		{
			player.statLifeMax = 100 + player.ConsumedLifeCrystals * 4 + player.ConsumedLifeFruit * 1 + lifeFix1 + lifeFix2 + lifeFix3 + lifeFix4;
			player.statManaMax = 20 + player.ConsumedManaCrystals * 4 + manaFix1 * 2;
		}
		else
		{
			orig(player);
			player.statLifeMax += lifeFix1 + lifeFix2 + lifeFix3 + lifeFix4;
			player.statManaMax += (int)(manaFix1 * 0.4f);
		}
	}

	public override void ResetEffects()
	{
		lightSeekerRangedSet = false;
		devilHeartSet = false;
		molluscsSet = false;
		auburnSet = false;
		ruinSet = false;

		istafelsSunfireGrasp = false;
	}

	public override void ProcessTriggers(TriggersSet triggersSet)
	{
		if (EverglowKeyBinds.ArmorSetBonusHotKey.JustPressed)
		{
			if (devilHeartSet && !Player.HasCooldown<DevilHeartSetCooldown>())
			{
				Player.AddBuff(ModContent.BuffType<DevilHeartSetBuff>(), DevilHeartLightBreastPlate.BuffDuration);
				Player.AddCooldown(DevilHeartSetCooldown.ID, DevilHeartLightBreastPlate.CooldownDuration);
			}

			if (auburnSet && !Player.HasCooldown<AuburnSelfReinforcingCooldown>())
			{
				Player.AddBuff(ModContent.BuffType<AuburnSelfReinforcing>(), AuburnHoodie.BuffDuration);
				Player.AddCooldown(AuburnSelfReinforcingCooldown.ID, AuburnHoodie.BuffCooldown);
			}

			if (ruinSet
				&& Player.HeldItem.type == ModContent.ItemType<WoodlandWraithStaff>()
				&& !Player.HasCooldown<RuinSetCooldown>())
			{
				Player.AddBuff(ModContent.BuffType<RuinSetBuff>(), RuinSetPlayer.BuffDuration);
				Player.AddCooldown(RuinSetCooldown.ID, RuinSetPlayer.CooldownDuration);
				Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Player.velocity, ModContent.ProjectileType<WoodlandWraithStaff_SetAnimation>(), 0, 0, Player.whoAmI);
			}

			if (molluscsSet && !Player.HasCooldown<MolluscsSetCooldown>())
			{
				Player.AddBuff(ModContent.BuffType<MolluscsSetBuff>(), ShellMolluscsBreastPlate.BuffDuration);
				Player.AddCooldown(MolluscsSetCooldown.ID, ShellMolluscsBreastPlate.CooldownDuration);
			}
		}

		if (EverglowKeyBinds.AccessorySkillKey.JustPressed)
		{
			if (istafelsSunfireGrasp && !Player.HasCooldown<IstafelsSunfireGraspSkillCooldown>())
			{
				Player.GetModPlayer<IstafelsSunfireGraspPlayer>().SkillEnable = true;
			}
		}
	}

	public override bool CanUseItem(Item item)
	{
		if (item.type == ModContent.ItemType<AntiHeavenSicknessPill>())
		{
			return ConsumedAntiHeavenSicknessPill < AntiHeavenSicknessPill.AntiHeavenSicknessPillMax;
		}
		else if (item.type == ModContent.ItemType<JadeFruit>())
		{
			return ConsumedJadeGlazeFruit < JadeFruit.MaxJadeFruits;
		}
		else if (item.type == ModContent.ItemType<LampBorerHoney>())
		{
			return ConsumedLampBorerHoney < LampBorerHoney.MaxUse;
		}
		else if (item.type == ModContent.ItemType<SquamousCore>())
		{
			return ConsumedSquamousCore < SquamousCore.SquamousCoreMax;
		}

		return base.CanUseItem(item);
	}

	public override void SaveData(TagCompound tag)
	{
		tag[nameof(ConsumedAntiHeavenSicknessPill)] = ConsumedAntiHeavenSicknessPill;
		tag[nameof(ConsumedJadeGlazeFruit)] = ConsumedJadeGlazeFruit;
		tag[nameof(ConsumedSquamousCore)] = ConsumedSquamousCore;
		tag[nameof(ConsumedLampBorerHoney)] = ConsumedLampBorerHoney;
	}

	public override void LoadData(TagCompound tag)
	{
		if (tag.TryGet(nameof(ConsumedAntiHeavenSicknessPill), out int life1))
		{
			ConsumedAntiHeavenSicknessPill = life1;
		}
		if (tag.TryGet(nameof(ConsumedJadeGlazeFruit), out int life2))
		{
			ConsumedJadeGlazeFruit = life2;
		}
		if (tag.TryGet(nameof(ConsumedSquamousCore), out int mana1))
		{
			ConsumedSquamousCore = mana1;
		}
		if (tag.TryGet(nameof(ConsumedLampBorerHoney), out int honey))
		{
			ConsumedLampBorerHoney = honey;
		}
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModIns.PacketResolver.Send(
			new PermanentBoostPacket()
			{
				consumedAntiHeavenSicknessPill = ConsumedAntiHeavenSicknessPill,
				consumedJadeGlazeFruit = ConsumedJadeGlazeFruit,
				consumedSquamousCore = ConsumedSquamousCore,
				consumedLampBorerHoney = ConsumedLampBorerHoney,
			}, toWho, fromWho);
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		var clone = (YggdrasilPlayer)targetCopy;
		clone.ConsumedLampBorerHoney = ConsumedLampBorerHoney;
		clone.ConsumedAntiHeavenSicknessPill = ConsumedAntiHeavenSicknessPill;
		clone.ConsumedJadeGlazeFruit = ConsumedJadeGlazeFruit;
		clone.ConsumedSquamousCore = ConsumedSquamousCore;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		var clone = (YggdrasilPlayer)clientPlayer;
		if (ConsumedLampBorerHoney != clone.ConsumedLampBorerHoney)
		{
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}