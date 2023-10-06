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
		if (!NPC.boss)
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
		else
		{
			BeABoss();
		}
	}
	#region Boss
	private int wingFrame = 0;
	private int wingFrameCounter = 0;
	private CoroutineManager _acytaeaCoroutine = new CoroutineManager();
	public Skeleton2D acytaeaSkeleton;
	public void StartToBeABoss()
	{
		NPC.TargetClosest(true);
		_acytaeaCoroutine.StartCoroutine(new Coroutine(StartFighting()));
		StartSkeletons();
	}
	public void StartSkeletons()
	{
		Texture2D backWing = ModAsset.AcytaeaBackWing.Value;
		Texture2D frontWing = ModAsset.AcytaeaFrontWing.Value;
		Texture2D backArm = ModAsset.AcytaeaBackArm.Value;
		Texture2D body = ModAsset.AcytaeaBody.Value;
		Texture2D legs = ModAsset.AcytaeaLeg.Value;
		Texture2D frontArm = ModAsset.AcytaeaFrontArm.Value;
		Texture2D head = ModAsset.AcytaeaHead.Value;

		Bone2D bodyBone = new Bone2D();
		bodyBone.Length = 0;
		bodyBone.Rotation = NPC.rotation;
		bodyBone.WorldSpacePosition = NPC.Center;
		bodyBone.Name = "bodyBone";
		bodyBone.Scale = Vector2.One;
		Bone2D backWingBone = CreateLimbBone(0, 0, "backWingBone", Vector2.zeroVector);
		bodyBone.AddChild(backWingBone);
		Bone2D frontWingBone = CreateLimbBone(0, 0, "frontWingBone", Vector2.zeroVector);
		bodyBone.AddChild(frontWingBone);
		Bone2D backArmBone = CreateLimbBone(1, 0, "backArmBone", Vector2.zeroVector);
		bodyBone.AddChild(backArmBone);
		Bone2D legsBone = CreateLimbBone(10, 0, "legsBone", Vector2.zeroVector);
		legsBone.AddChild(bodyBone);
		Bone2D frontArmsBone = CreateLimbBone(1, 0, "frontArmsBone", Vector2.zeroVector);
		bodyBone.AddChild(frontArmsBone);
		Bone2D headBone = CreateLimbBone(1, 0, "headBone", Vector2.zeroVector);
		bodyBone.AddChild(headBone);
		List<Bone2D> bones = new List<Bone2D>
		{
			bodyBone,
			backWingBone,
			frontWingBone,
			backArmBone,
			legsBone,
			frontArmsBone,
			headBone,
		};
		acytaeaSkeleton = new Skeleton2D(bones);

		acytaeaSkeleton.Slots.Add(CreateSlot("bodySlot", bodyBone, body));
		acytaeaSkeleton.Slots.Add(CreateSlot("backWingSlot", backWingBone, backWing));
		acytaeaSkeleton.Slots.Add(CreateSlot("frontWingSlot", frontWingBone, frontWing));
		acytaeaSkeleton.Slots.Add(CreateSlot("backArmSlot", backArmBone, backArm));
		acytaeaSkeleton.Slots.Add(CreateSlot("legsSlot", legsBone, legs));
		acytaeaSkeleton.Slots.Add(CreateSlot("frontArmSlot", frontArmsBone, frontArm));
		acytaeaSkeleton.Slots.Add(CreateSlot("headSlot", headBone, head));
	}
	public Slot CreateSlot(string name, Bone2D bone, Texture2D attachmentTexture, float attachmentRot = 0)
	{
		Slot slot = new Slot();
		slot.Name = name;
		slot.Bone = bone;
		RegionAttachment attachment = new RegionAttachment();
		attachment.Texture = attachmentTexture;
		attachment.Rotation = attachmentRot;
		attachment.Size = Vector2.One;
		slot.Attachment = attachment;
		return slot;
	}
	/// <summary>
	/// 创建一个肢体骨骼
	/// </summary>
	public Bone2D CreateLimbBone(float length, float rotation, string name, Vector2 position)
	{
		Bone2D bone = new Bone2D();
		bone.Length = length;
		bone.Rotation = rotation;
		bone.WorldSpacePosition = NPC.Center;
		bone.Name = name;
		bone.Position = position;
		bone.Scale = Vector2.One;
		return bone;
	}
	public void BeABoss()
	{
		Player player = Main.player[NPC.target];
		UpdateWings();
		_acytaeaCoroutine.Update();
		if (!player.active || player.dead)
		{
			NPC.active = false;
			NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, NPC.type);
		}
		else
		{
			if ((player.Center - NPC.Center).Length() > 15000)
				NPC.active = false;
		}
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			NPC.direction *= -1;
			NPC.spriteDirection = NPC.direction;
		}
	}
	public void UpdateWings()
	{
		wingFrameCounter++;
		if (wingFrameCounter > 6)
		{
			wingFrameCounter = 0;
			wingFrame++;
			if (wingFrame > 3)
			{
				wingFrame = 0;
			}
		}
	}
	private IEnumerator<ICoroutineInstruction> StartFighting()
	{
		Player player = Main.player[NPC.target];
		NPC.direction = 1;
		NPC.noGravity = true;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 60; t++)
		{
			Vector2 targetPos = player.Center + new Vector2(-200 * NPC.direction, 0);
			NPC.position = targetPos * 0.05f + NPC.position * 0.95f;
			yield return new SkipThisFrame();
		}
		_acytaeaCoroutine.StartCoroutine(new Coroutine(SlashPlayer()));
	}
	private IEnumerator<ICoroutineInstruction> SlashPlayer()
	{
		Player player = Main.player[NPC.target];
		NPC.direction = 1;
		if (player.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 600; t++)
		{
			Vector2 targetPos = player.Center + new Vector2(200 * NPC.direction, 0);
			NPC.Center = targetPos * 0.05f + NPC.Center * 0.95f;
			yield return new SkipThisFrame();
		}
	}
	public void DrawSelfBoss(SpriteBatch spriteBatch, Color drawColor)
	{
		Vector2 drawPos = NPC.Center - Main.screenPosition;
		Vector2 commonOrigin = NPC.Hitbox.Size() / 2f;
		SpriteEffects drawEffect = SpriteEffects.None;
		Vector2 wingorigin = commonOrigin + new Vector2(10, 0);
		if (NPC.spriteDirection == -1)
		{
			drawEffect = SpriteEffects.FlipHorizontally;
			wingorigin = commonOrigin + new Vector2(26, 0);
		}
		Texture2D backWing = ModAsset.AcytaeaBackWing.Value;
		Texture2D frontWing = ModAsset.AcytaeaFrontWing.Value;
		Texture2D backArm = ModAsset.AcytaeaBackArm.Value;
		Texture2D body = ModAsset.AcytaeaBody.Value;
		Texture2D legs = ModAsset.AcytaeaLeg.Value;
		Texture2D frontArm = ModAsset.AcytaeaFrontArm.Value;
		Texture2D head = ModAsset.AcytaeaHead.Value;
		Texture2D[] backWingFrames = new Texture2D[] { ModAsset.AcytaeaBackWing_0.Value, ModAsset.AcytaeaBackWing_1.Value, ModAsset.AcytaeaBackWing_2.Value, ModAsset.AcytaeaBackWing_3.Value };
		Texture2D[] frontWingFrames = new Texture2D[] { ModAsset.AcytaeaFrontWing_0.Value, ModAsset.AcytaeaFrontWing_1.Value, ModAsset.AcytaeaFrontWing_2.Value, ModAsset.AcytaeaFrontWing_3.Value };
		if (acytaeaSkeleton != null)
		{
			foreach (Slot slot in acytaeaSkeleton.Slots)
			{
				if (slot.Name == "bodySlot")
				{
					slot.Bone.Rotation = MathF.Sin((float)(Main.timeForVisualEffects * 0.03f));
				}
				slot.Bone.WorldSpacePosition = NPC.Center;
				if (slot.Name == "backWingSlot")
				{
					slot.Attachment.Texture = backWingFrames[Math.Clamp(wingFrame, 0, 3)];
				}
				if (slot.Name == "frontWingSlot")
				{
					slot.Attachment.Texture = frontWingFrames[Math.Clamp(wingFrame, 0, 3)];
				}
				if (slot.Name == "frontArmSlot")
				{
					slot.Bone.Rotation += 0.02f;
				}
			}
			acytaeaSkeleton.DrawDebugView(spriteBatch);
		}
		else
		{
			spriteBatch.Draw(backWing, drawPos, new Rectangle(0, 56 * wingFrame, 86, 56), drawColor, NPC.rotation, wingorigin, 1f, drawEffect, 0);
			spriteBatch.Draw(frontWing, drawPos, new Rectangle(0, 56 * wingFrame, 86, 56), drawColor, NPC.rotation, wingorigin, 1f, drawEffect, 0);
			spriteBatch.Draw(backArm, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
			spriteBatch.Draw(body, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
			spriteBatch.Draw(legs, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
			spriteBatch.Draw(frontArm, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
			spriteBatch.Draw(head, drawPos, null, drawColor, NPC.rotation, commonOrigin, 1f, drawEffect, 0);
		}
	}
	#endregion
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

	#region TownNPC

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
			NPC.friendly = false;
			NPC.aiStyle = -1;

			NPC.lifeMax = 165000;
			NPC.life = 165000;
			if (Main.expertMode)
			{
				NPC.lifeMax = 275000;
				NPC.life = 275000;
			}
			if (Main.masterMode)
			{
				NPC.lifeMax = 385000;
				NPC.life = 385000;
			}
			NPC.boss = true;
			NPC.localAI[0] = 0;
			NPC.aiStyle = -1;
			NPC.width = 40;
			NPC.height = 56;
			StartToBeABoss();
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

	#endregion TownNPC

	public override void Draw()
	{
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		DrawSelfBoss(spriteBatch, drawColor);
		return false;
	}
}