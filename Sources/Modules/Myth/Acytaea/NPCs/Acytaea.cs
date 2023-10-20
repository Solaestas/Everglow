using Everglow.Commons.Coroutines;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Skeleton2D;
using Everglow.Myth.Acytaea.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.Localization;

namespace Everglow.Myth.Acytaea.NPCs;

[AutoloadHead]
[Pipeline(typeof(NPPipeline), typeof(AcytaeaPipeline))]
public class Acytaea : VisualNPC
{
	private bool canDespawn = false;
	public override string HeadTexture => NPC.boss ?
		"Everglow/Myth/Acytaea/NPCs/Acytaea_Head_Boss" :
		"Everglow/Myth/Acytaea/NPCs/Acytaea_Head";

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 25;
		NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
		NPCID.Sets.AttackFrameCount[NPC.type] = 4;
		NPCID.Sets.DangerDetectRange[NPC.type] = 400;
		NPCID.Sets.AttackType[NPC.type] = 2;
		NPCID.Sets.AttackTime[NPC.type] = 60;
		NPCID.Sets.AttackAverageChance[NPC.type] = 15;
		NPCID.Sets.ActsLikeTownNPC[Type] = true;
	}

	public override void SetDefaults()
	{
		NPC.townNPC = true;
		NPC.friendly = true;
		NPC.width = 34;
		NPC.height = 48;
		NPC.aiStyle = 7;
		NPC.damage = 100;
		NPC.defense = 100;
		NPC.lifeMax = 250;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.knockBackResist = 0.5f;
		NPC.boss = false;
		NPC.friendly = true;
		AnimationType = 22;
		NPCID.Sets.TrailingMode[NPC.type] = 0;
		NPCID.Sets.TrailCacheLength[NPC.type] = 8;
		NPC.knockBackResist = 0;
		Music = Common.MythContent.QuickMusic("AcytaeaFighting");
	}

	public override bool CheckActive()
	{
		return canDespawn;
	}
	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref DownedBossSystem.downedAcytaea, -1);
		if (Main.netMode == NetmodeID.Server)
			NetMessage.SendData(MessageID.WorldData);
	}
	public override void AI()
	{
		NPCHappiness NH = NPC.Happiness;
		NH.SetBiomeAffection<ForestBiome>((AffectionLevel)50);
		NH.SetBiomeAffection<SnowBiome>((AffectionLevel)70);
		NH.SetBiomeAffection<CrimsonBiome>((AffectionLevel)90);
		NH.SetBiomeAffection<CorruptionBiome>((AffectionLevel)90);
		NH.SetBiomeAffection<UndergroundBiome>((AffectionLevel)(-20));
		NH.SetBiomeAffection<DesertBiome>((AffectionLevel)20);
		NH.SetBiomeAffection<DungeonBiome>((AffectionLevel)(-50));
		NH.SetBiomeAffection<OceanBiome>((AffectionLevel)50);
		NH.SetBiomeAffection<JungleBiome>((AffectionLevel)30);
	}
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		//TODO hjson
		string tx1 = "我很喜欢这种鸟语花香的绿色树林";
		string tx2 = "去抓蝴蝶吗?";
		if (Language.ActiveCulture.Name == "en-US")
		{
			tx1 = "I love the forest with birds singing and flowers blooming";
			tx2 = "Catching butterflies?";
		}
		else if (Language.ActiveCulture.Name == "ru-RU")
		{
			tx1 = "Я люблю лес где птички поют и цветочки благоухают";
			tx2 = "Может половить бабочек?";
		}
		else
		{
			tx1 = "I love the forest with birds singing and flowers blooming";
			tx2 = "Catching butterflies?";
		}
		// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.

				new FlavorTextBestiaryInfoElement(tx1),
			new FlavorTextBestiaryInfoElement(tx2),
			new FlavorTextBestiaryInfoElement("Mods.Everglow.Bestiary.Acytaea")
		});
	}
	public override bool PreKill()
	{
		return base.PreKill();
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return 0;
	}

	public override bool CanChat()
	{
		return !NPC.boss;
	}

	public override string GetChat()
	{
		//TODO Hjson 重写
		IList<string> list = new List<string>();
		if (Language.ActiveCulture.Name == "zh-Hans")
		{
			if (Main.dayTime)
			{
				list.Add("你在干嘛啊");
				if (NPC.CountNPCS(22) != 0)
					list.Add("对了,要好好感谢向导");
				if (NPC.CountNPCS(54) != 0)
					list.Add("其实服装师以前成就厉害的,暗影魔法大师呢");
				list.Add("没事不要烦我");
				list.Add("干嘛!");
				list.Add("额，好吧，我承认我的脾气有点暴躁");
				list.Add("我觉得你是不是喜欢我");
				list.Add("你烦不烦啊!");
				list.Add("不就是砍了你一刀么,没什么大不了");
				list.Add("嘻嘻嘻");
			}
			else
			{
				list.Add("晚上就应该出去浪");
				list.Add("你在干嘛啊");
				if (NPC.CountNPCS(22) != 0)
					list.Add("对了,要好好感谢向导");
				if (NPC.CountNPCS(54) != 0)
					list.Add("其实服装师以前成就厉害的,暗影魔法大师呢");
				list.Add("没事不要烦我");
				list.Add("干嘛!");
				list.Add("额,好吧,我承认我的脾气有点暴躁");
				list.Add("我觉得你是不是喜欢我");
				list.Add("你烦不烦啊!");
				list.Add("不就是砍了你一刀么,没什么大不了");
				list.Add("嘻嘻嘻");
			}
		}
		else if (Language.ActiveCulture.Name == "en-US")
		{
			if (Main.dayTime)
			{
				list.Add("What are you doing?");
				if (NPC.CountNPCS(22) != 0)
					list.Add("Btw, you should be thankful to the Guide");
				if (NPC.CountNPCS(54) != 0)
					list.Add("The Clothier was actually a Senior dark Mage");
				list.Add("Don't bother me if you only tell nonsense");
				list.Add("What's up?");
				list.Add("Err, well, I am a little grumpy");
				list.Add("You love me, don't you?");
				list.Add("How boring you are!");
				list.Add("I just cut you open, not a big deal");
				list.Add("*Chunckle");
			}
			else
			{
				list.Add("YOLO, go on adventures at night");
				list.Add("What are you doing?");
				if (NPC.CountNPCS(22) != 0)
					list.Add("Btw, you should be thankful to the Guide");
				if (NPC.CountNPCS(54) != 0)
					list.Add("The Clothier was actually a Senior dark Mage");
				list.Add("Don't bother me if you only tell nonsense");
				list.Add("What's up?");
				list.Add("Err, well, I am a little grumpy");
				list.Add("♥ Wanna eat me? ♥");
				list.Add("How boring you are!");
				list.Add("I just cut you open, not a big deal");
				list.Add("*Chunckle");
			}
		}
		else if (Language.ActiveCulture.Name == "ru-RU")
		{
			if (Main.dayTime)
			{
				list.Add("Что ты делаешь?");
				if (NPC.CountNPCS(22) != 0)
					list.Add("Кстати, ты должен быть благодарен Гиду");
				if (NPC.CountNPCS(54) != 0)
					list.Add("Портной на самом деле был Старшим Тёмным Магом");
				list.Add("Не беспокойся меня если ты говоришь только глупости");
				list.Add("Как дела?");
				list.Add("Эээ, да, я немного сварливая");
				list.Add("Ты любишь меня, не так ли?");
				list.Add("Какой ты скучный!");
				list.Add("Я просто разрезала тебя, ничего страшного");
				list.Add("*Смеётся");
			}
			else
			{
				list.Add("Ты живёшь один раз, иди путешествовать ночью");
				list.Add("Что ты делаешь?");
				if (NPC.CountNPCS(22) != 0)
					list.Add("Кстати, ты должен быть благодарен Гиду");
				if (NPC.CountNPCS(54) != 0)
					list.Add("Портной на самом деле был Старшим Тёмным Магом");
				list.Add("Не беспокойся меня если ты говоришь только глупости");
				list.Add("Как дела?");
				list.Add("Эээ, да, я немного сварливая");
				list.Add("♥ Хочешь съесть меня? ♥");
				list.Add("Какой ты скучный!");
				list.Add("Я просто разрезала тебя, ничего страшного");
				list.Add("*Смеётся");
			}
		}
		else
		{
			if (Main.dayTime)
			{
				list.Add("What are you doing?");
				if (NPC.CountNPCS(22) != 0)
					list.Add("Btw, you should be thankful to the Guide");
				if (NPC.CountNPCS(54) != 0)
					list.Add("The Clothier was actually a Senior dark Mage");
				list.Add("Don't bother me if you only tell nonsense");
				list.Add("What's up?");
				list.Add("Err, well, I am a little grumpy");
				list.Add("You love me, don't you?");
				list.Add("How boring you are!");
				list.Add("I just cut you open, not a big deal");
				list.Add("*Chunckle");
			}
			else
			{
				list.Add("YOLO, go on adventures at night");
				list.Add("What are you doing?");
				if (NPC.CountNPCS(22) != 0)
					list.Add("Btw, you should be thankful to the Guide");
				if (NPC.CountNPCS(54) != 0)
					list.Add("The Clothier was actually a Senior dark Mage");
				list.Add("Don't bother me if you only tell nonsense");
				list.Add("What's up?");
				list.Add("Err, well, I am a little grumpy");
				list.Add("♥ Wanna eat me? ♥");
				list.Add("How boring you are!");
				list.Add("I just cut you open, not a big deal");
				list.Add("*Chunckle");
			}
		}
		return list[Main.rand.Next(list.Count)];
	}

	public override void SetChatButtons(ref string button, ref string button2)
	{
		//TODO Hjson
		if (Language.ActiveCulture.Name == "zh-Hans")
		{
			button = Language.GetTextValue("挑战");
			button2 = Language.GetTextValue("帮助");
		}
		else
		{
			button = Language.GetTextValue("Challenge");
			button2 = Language.GetTextValue("Help");
		}
	}

	public override void OnChatButtonClicked(bool firstButton, ref string shopName)
	{
		if (firstButton)
		{
			NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Acytaea_Boss>());
			NPC.active = false;
		}
		else
		{

		}
	}

	public override void TownNPCAttackStrength(ref int damage, ref float knockback)
	{
		damage = 30;
		knockback = 2f;
	}

	public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
	{
		cooldown = 60;
		randExtraCooldown = 60;
	}

	public override void TownNPCAttackMagic(ref float auraLightMultiplier)
	{
		for (int d = 0; d < Main.projectile.Length; d++)
		{
			if (Main.projectile[d].type == ModContent.ProjectileType<AcytaeaSword_projectile_TownNPC>())
				return;
		}
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaSword_projectile_TownNPC>(), 50, 6, Main.myPlayer, NPC.whoAmI);
	}

	public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
	{
		projType = ModContent.ProjectileType<AcytaeaSword_projectile_TownNPC>();
		attackDelay = 60;
	}

	public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
	{
		multiplier = 2f;
	}

	public override void Draw()
	{
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		return true;
	}
}