using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.Common
{

	public class MythContentNPCLoot : GlobalNPC
	{
		//ModifyNPCLoot uses a unique system called the ItemDropDatabase, which has many different rules for many different drop use cases.
		//Here we go through all of them, and how they can be used.
		//There are tons of other examples in vanilla! In a decompiled vanilla build, GameContent/ItemDropRules/ItemDropDatabase adds item drops to every single vanilla NPC, which can be a good resource.
		// TODO: Finish Weapon Ports first
		//public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		//{
		//    Player player = Main.LocalPlayer;
		//    if (npc.type == NPCID.Nutcracker || npc.type == 349)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Accessories.WalnutClip>(), 180/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.Accessories.WalnutClip>(), 270/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Accessories.WalnutClip>(), 360/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 48)
		//    {
		//        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FeatherMagic>(), 70));
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<FeatherMagic>(), 60/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<FeatherMagic>(), 90/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<FeatherMagic>(), 120/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 125 || npc.type == 126)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<BloodLightCyanFlame>(), 16/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<BloodLightCyanFlame>(), 50/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<BloodLightCyanFlame>(), 200/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 50)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<JellySlingshot>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<JellySlingshot>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<JellySlingshot>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 262)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<ThornBomb>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ThornBomb>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ThornBomb>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 245)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<TrapYoyo>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<TrapYoyo>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<TrapYoyo>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 127)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<MachineFit>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<MachineFit>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<MachineFit>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 127)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<MachineSkeGun>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<MachineSkeGun>(), 125/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<MachineSkeGun>(), 500/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 113)
		//    {
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<BloodGoldBlade>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<BloodGoldBlade>(), 125/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<BloodGoldBlade>(), 500/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));

		//    }
		//    if (npc.type == 134)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<LaserWhip>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<LaserWhip>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<LaserWhip>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 266)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<ChaosCurrent>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ChaosCurrent>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ChaosCurrent>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 636)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<RainbowLight>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<RainbowLight>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<RainbowLight>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == ModContent.NPCType<NPCs.BloodTusk.BloodTusk>())
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<ToothSpear>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ToothSpear>(), 125/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ToothSpear>(), 500/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == ModContent.NPCType<NPCs.BloodTusk.BloodTusk>())
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<ToothBow>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ToothBow>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ToothBow>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 657)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<GelPowerGun>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<GelPowerGun>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<GelPowerGun>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 39)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<AshBone>(), 60/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<AshBone>(), 90/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<AshBone>(), 120/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 398)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<SilenceMirror>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SilenceMirror>(), 125/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<SilenceMirror>(), 500/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 370)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<BlueRain>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<BlueRain>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<BlueRain>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 140)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<AmbiguousNight>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<AmbiguousNight>(), 150/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<AmbiguousNight>(), 200/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == NPCID.WyvernHead)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<ThunderFlower>(), 50/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ThunderFlower>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ThunderFlower>(), 140/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 82)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<ComingGhost>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ComingGhost>(), 150/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ComingGhost>(), 200/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 370)
		//    {
		//        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeTooth>(), 3));
		//    }
		//    if (npc.type == 628)
		//    {
		//        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WindMoveSeed>(), 1, 3, 5));
		//    }
		//    if (npc.type == 1)
		//    {
		//        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlueGel>(), 800));
		//    }
		//    if (npc.type == -3)
		//    {
		//        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RedGel>(), 800));
		//    }
		//    if (npc.type == -1)
		//    {
		//        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GreenGel>(), 800));
		//    }
		//    if (!npc.friendly && npc.lifeMax > 100)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new EclipseMasterPostPlant(), ModContent.ItemType<GoldRoundYoyo>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new EclipseExpertPostPlant(), ModContent.ItemType<GoldRoundYoyo>(), 60/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new EclipseNormalPostPlant(), ModContent.ItemType<GoldRoundYoyo>(), 80/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (!npc.friendly && npc.lifeMax > 100)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CorruptionMasterHardmode(), ModContent.ItemType<CurseClub>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CorruptionExpertHardmode(), ModContent.ItemType<CurseClub>(), 150/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CorruptionNormalHardmode(), ModContent.ItemType<CurseClub>(), 200/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (!npc.friendly && npc.lifeMax > 100)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CorruptionMasterHardmode(), ModContent.ItemType<CorruptEye>(), 160/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CorruptionExpertHardmode(), ModContent.ItemType<CorruptEye>(), 275/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CorruptionNormalHardmode(), ModContent.ItemType<CorruptEye>(), 450/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (!npc.friendly && npc.lifeMax > 100)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CrimsonExpertHardmode(), ModContent.ItemType<GoldLiquidPupil>(), 160/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CrimsonExpertHardmode(), ModContent.ItemType<GoldLiquidPupil>(), 275/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CrimsonNormalHardmode(), ModContent.ItemType<GoldLiquidPupil>(), 450/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (!npc.friendly && npc.lifeMax > 100)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CrimsonMasterHardmode(), ModContent.ItemType<BloodLiquidClub>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CrimsonExpertHardmode(), ModContent.ItemType<BloodLiquidClub>(), 150/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new CrimsonNormalHardmode(), ModContent.ItemType<BloodLiquidClub>(), 200/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (!npc.friendly && npc.lifeMax > 100)
		//    {
		//        /*大师*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new HallowMasterHardmode(), ModContent.ItemType<CrystalClub>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*专家*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new HallowExpertHardmode(), ModContent.ItemType<CrystalClub>(), 150/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        /*普通*/
		//        npcLoot.Add(ItemDropRule.ByCondition(new HallowNormalHardmode(), ModContent.ItemType<CrystalClub>(), 200/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//    if (npc.type == 344)
		//    {
		//        npcLoot.Add(ItemDropRule.ByCondition(new InFrostMoonFinal(), ModContent.ItemType<FrozenStormPine>(), 50/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//        npcLoot.Add(ItemDropRule.ByCondition(new InFrostMoonFinal(), ModContent.ItemType<XmasWhip>(), 50/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//    }
		//}
	}
	class CrimsonExpertHardmode : IItemDropRuleCondition
	{
		//TODO:下列掉落条件需要翻译
		bool CanD => Main.expertMode && !Main.masterMode && Main.hardMode && Main.LocalPlayer.ZoneCrimson;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Crimson";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的猩红之地";
			}
			return desc;
		}
	}
	class CrimsonMasterHardmode : IItemDropRuleCondition
	{
		bool CanD => Main.masterMode && Main.hardMode && Main.LocalPlayer.ZoneCrimson;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the  Crimson";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的猩红之地";
			}
			return desc;
		}
	}
	class CrimsonNormalHardmode : IItemDropRuleCondition
	{
		bool CanD => !Main.expertMode && Main.hardMode && Main.LocalPlayer.ZoneCrimson;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the  Crimson";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的猩红之地";
			}
			return desc;
		}
	}
	class CorruptionExpertHardmode : IItemDropRuleCondition
	{
		bool CanD => Main.expertMode && !Main.masterMode && Main.hardMode && Main.LocalPlayer.ZoneCorrupt;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Corrupt";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的腐化之地";
			}
			return desc;
		}
	}
	class CorruptionMasterHardmode : IItemDropRuleCondition
	{
		bool CanD => Main.masterMode && Main.hardMode && Main.LocalPlayer.ZoneCorrupt;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Corrupt";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的腐化之地";
			}
			return desc;
		}
	}
	class CorruptionNormalHardmode : IItemDropRuleCondition
	{
		bool CanD => !Main.expertMode && Main.hardMode && Main.LocalPlayer.ZoneCorrupt;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Corrupt";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的腐化之地";
			}
			return desc;
		}
	}
	class HallowExpertHardmode : IItemDropRuleCondition
	{
		bool CanD => Main.expertMode && !Main.masterMode && Main.hardMode && Main.LocalPlayer.ZoneHallow;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Hallow";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的神圣之地";
			}
			return desc;
		}
	}
	class HallowMasterHardmode : IItemDropRuleCondition
	{
		bool CanD => Main.masterMode && Main.hardMode && Main.LocalPlayer.ZoneHallow;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Hallow";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的神圣之地";
			}
			return desc;
		}
	}
	class HallowNormalHardmode : IItemDropRuleCondition
	{
		bool CanD => !Main.expertMode && Main.hardMode && Main.LocalPlayer.ZoneHallow;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only hardmode and in the Hallow";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在困难模式的神圣之地";
			}
			return desc;
		}
	}
	class EclipseExpertPostPlant : IItemDropRuleCondition
	{
		bool CanD => Main.expertMode && !Main.masterMode && Main.eclipse && NPC.downedPlantBoss;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only defeated Plantera and in the Eclipse";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在世纪之花后日食";
			}
			return desc;
		}
	}
	class EclipseMasterPostPlant : IItemDropRuleCondition
	{
		bool CanD => Main.masterMode && Main.eclipse && NPC.downedPlantBoss;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only defeated Plantera and in the Eclipse";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在世纪之花后日食";
			}
			return desc;
		}
	}
	class EclipseNormalPostPlant : IItemDropRuleCondition
	{
		bool CanD => !Main.expertMode && Main.eclipse && NPC.downedPlantBoss;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only defeated Plantera and in the Eclipse";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在世纪之花后日食";
			}
			return desc;
		}
	}
	class InFrostMoonFinal : IItemDropRuleCondition
	{
		bool CanD => Main.snowMoon && Main.invasionProgressWave >= 20;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only Frost Moon during Chrismas";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在圣诞节期间霜月";
			}
			return desc;
		}
	}
	class InPumpkMoonFinal : IItemDropRuleCondition
	{
		bool CanD => Main.pumpkinMoon && Main.invasionProgressWave >= 20;
		public bool CanDrop(DropAttemptInfo info)
		{
			return CanD;
		}
		public bool CanShowItemDropInUI()
		{
			return CanD;
		}
		public string GetConditionDescription()
		{
			string desc = "Only Pumpkin Moon during Chrismas";
			if (Language.ActiveCulture.Name == "zh-Hans")
			{
				desc = "仅在圣诞节期间霜月";
			}
			return desc;
		}
	}
}
