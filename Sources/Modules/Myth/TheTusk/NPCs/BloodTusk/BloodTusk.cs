using Everglow.Commons.Coroutines;
using Everglow.Commons.DataStructures;
using Everglow.Myth.TheTusk.Items;
using Everglow.Myth.TheTusk.Items.Accessories;
using Everglow.Myth.TheTusk.Items.BossDrop;
using Everglow.Myth.TheTusk.Items.Weapons;
using Everglow.Myth.TheTusk.Projectiles;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using static Everglow.Myth.TheTusk.NPCs.BloodTusk.BloodTuskAtlas;

namespace Everglow.Myth.TheTusk.NPCs.BloodTusk;

[AutoloadBossHead]
[NoGameModeScale]
public class BloodTusk : ModNPC
{
	/// <summary>
	/// MapIcon Slot.
	/// </summary>
	public int SecondStageHeadSlot = -1;

	/// <summary>
	/// Draw offset added when being hit.
	/// </summary>
	public Vector2 BeatOffset = Vector2.Zero;

	/// <summary>
	/// 0~1,0 is confront (normal) while 1 is cowered.
	/// </summary>
	public float CowerValue;

	/// <summary>
	/// Drawing sequence.
	/// </summary>
	public List<DrawPiece> DrawPieceSequence = new List<DrawPiece>();

	/// <summary>
	/// A coroutine to run the state pattern.
	/// </summary>
	public CoroutineManager BloodTuskCoroutine = new CoroutineManager();

	public enum States
	{
		Sleep,
		Phase1,
		Phase2,
		Phase3,
		Death,
	}

	public override void Load()
	{
		string texture = BossHeadTexture + "_Void";
		SecondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1);
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		string tex = "It was just a wolf tooth, dropped to the Crimson when its owner was defeated by a hero, gradually corrupted by the power of Cthulhu and granted mentality.";
		if (Language.ActiveCulture.Name == "zh-Hans")
		{
			tex = "原本只是一颗狼牙,在它的主人被勇士讨伐时掉落至猩红之地,逐渐为克苏鲁的力量所沾染,有了自己的意识";
		}
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
		{
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
			new FlavorTextBestiaryInfoElement(tex),
		});
	}

	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 0;
		NPC.width = 40;
		NPC.height = 142;
		NPC.defense = 15;
		NPC.lifeMax = 7800;
		if (Main.expertMode)
		{
			NPC.lifeMax = 10200;
			NPC.defense = 20;
		}
		if (Main.masterMode)
		{
			NPC.lifeMax = 13400;
			NPC.defense = 25;
		}
		for (int i = 0; i < NPC.buffImmune.Length; i++)
		{
			NPC.buffImmune[i] = true;
		}
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 2, 0, 0);
		NPC.color = new Color(0, 0, 0, 0);
		NPC.alpha = 0;
		NPC.aiStyle = -1;
		NPC.boss = true;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = false;
		NPC.HitSound = SoundID.DD2_SkeletonHurt;
		NPC.DeathSound = SoundID.DD2_SkeletonDeath;
		NPC.dontTakeDamage = true;
		Music = Common.MythContent.QuickMusic("TuskBiome");
	}

	public override void OnSpawn(IEntitySource source)
	{
		int counts = 0;
		while (!Collision.SolidCollision(NPC.Bottom, 0, 0))
		{
			NPC.position += new Vector2(0, 10);
			counts++;
			if (counts > 100)
			{
				NPC.active = false;
				break;
			}
		}
		CowerValue = 1;
		BloodTuskCoroutine.StartCoroutine(new Coroutine(Rise()));
		DrawPieceSequence = new List<DrawPiece>
		{
			 Gum_Middle, SubTusk3, SubTusk5, Tusk0, SubTusk0, SubTusk1,
			 SubTusk6, SubTusk7, Gum_Bottom, Gum_Surface, SubTusk2, SubTusk4, Gum_Surface_Center,
		};
	}

	public override void BossHeadSlot(ref int index)
	{
		// if (firstIcon == 9999999)
		// {
		// firstIcon = index;
		// }
		// int slot = SecondStageHeadSlot;
		// if (NPC.alpha == 0 && slot != -1)
		// {
		// index = slot;
		// }
		// if (NPC.alpha > 0)
		// {
		// index = firstIcon;
		// }
	}

	public override void AI()
	{
		NPC.TargetClosest();
		BloodTuskCoroutine.Update();
	}

	public IEnumerator<ICoroutineInstruction> Rise()
	{
		Vector2 vector = NPC.Bottom;
		for (int i = 0; i <= 60; i++)
		{
			CowerValue *= 0.9f;
			if (i == 60)
			{
				CowerValue = 0;
			}
			NPC.height = (int)MathHelper.Lerp(142, 10, CowerValue);
			NPC.Bottom = vector;
			if (NPC.alpha > 25)
			{
				NPC.alpha -= 25;
			}
			else
			{
				NPC.alpha = 0;
			}
			yield return new SkipThisFrame();
		}
		NPC.height = 142;
		NPC.Bottom = vector;
		BloodTuskCoroutine.StartCoroutine(new Coroutine(Cower()));
	}

	public IEnumerator<ICoroutineInstruction> Cower()
	{
		Vector2 vector = NPC.Bottom;
		for (int i = 0; i <= 60; i++)
		{
			CowerValue = MathHelper.Lerp(CowerValue, 1f, 0.1f);
			if (i == 60)
			{
				CowerValue = 1;
			}
			NPC.height = (int)MathHelper.Lerp(142, 10, CowerValue);
			NPC.Bottom = vector;
			if (i > 13)
			{
				NPC.alpha += 10;
			}
			yield return new SkipThisFrame();
		}
		NPC.height = 10;
		NPC.alpha = 255;
		NPC.Bottom = vector;
		Relocation();
		SwitchActionRise();
	}

	/// <summary>
	/// Simultaneously generate spikes on the entire ground.
	/// </summary>
	/// <param name="times"></param>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> SimultaneouslyGroundSpike(int times)
	{
		for (int i = 0; i < times; i++)
		{
			Vector2 leftSpikePos = NPC.Bottom + new Vector2(-40 - i * 30, 0);
			Vector2 rightSpikePos = NPC.Bottom + new Vector2(40 - i * 30, 0);
			for (int j = 0; j < 25; j++)
			{
				Vector2 leftNormal = -TileCollisionUtils.GetTopographicGradient(leftSpikePos, 8);
				for (int k = 0; k < 15; k++)
				{
					if (!Collision.SolidCollision(leftSpikePos, 0, 0))
					{
						leftSpikePos += leftNormal * 20;
					}
					else if (Collision.SolidCollision(leftSpikePos - leftNormal * 20, 0, 0))
					{
						leftSpikePos -= leftNormal * 20;
					}
					else
					{
						break;
					}
				}
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), leftSpikePos, Vector2.zeroVector, ModContent.ProjectileType<Tusk_ground_little>(), 30, 1, NPC.target);
				leftSpikePos += TileCollisionUtils.GetTopographicGradient(leftSpikePos, 8).RotatedBy(-MathHelper.PiOver2) * 100;

				Vector2 rightNormal = -TileCollisionUtils.GetTopographicGradient(rightSpikePos, 8);
				for (int k = 0; k < 15; k++)
				{
					if (!Collision.SolidCollision(rightSpikePos, 0, 0))
					{
						rightSpikePos += rightNormal * 20;
					}
					else if (Collision.SolidCollision(rightSpikePos - rightNormal * 20, 0, 0))
					{
						rightSpikePos -= rightNormal * 20;
					}
					else
					{
						break;
					}
				}
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), rightSpikePos, Vector2.zeroVector, ModContent.ProjectileType<Tusk_ground_little>(), 30, 1, NPC.target);
				rightSpikePos += TileCollisionUtils.GetTopographicGradient(rightSpikePos, 8).RotatedBy(MathHelper.PiOver2) * 100;
			}
			yield return new WaitForFrames(50);
		}
		BloodTuskCoroutine.StartCoroutine(new Coroutine(Rise()));
	}

	/// <summary>
	/// A slow wave of spike.
	/// </summary>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> SlowWaveGroundSpike()
	{
		Vector2 leftSpikePos = NPC.Bottom + new Vector2(-40, 0);
		Vector2 rightSpikePos = NPC.Bottom + new Vector2(40, 0);
		for (int i = 0; i < 60; i++)
		{
			Vector2 leftNormal = -TileCollisionUtils.GetTopographicGradient(leftSpikePos, 8);
			for (int k = 0; k < 15; k++)
			{
				if (!Collision.SolidCollision(leftSpikePos, 0, 0))
				{
					leftSpikePos += leftNormal * 10;
				}
				else if (Collision.SolidCollision(leftSpikePos - leftNormal * 10, 0, 0))
				{
					leftSpikePos -= leftNormal * 10;
				}
				else
				{
					break;
				}
			}
			Projectile p0 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), leftSpikePos, Vector2.zeroVector, ModContent.ProjectileType<Tusk_ground_little>(), 30, 1, NPC.target);
			p0.timeLeft = Main.rand.Next(101, 110);
			leftSpikePos += TileCollisionUtils.GetTopographicGradient(leftSpikePos, 8).RotatedBy(-MathHelper.PiOver2) * 30;

			Vector2 rightNormal = -TileCollisionUtils.GetTopographicGradient(rightSpikePos, 8);
			for (int k = 0; k < 15; k++)
			{
				if (!Collision.SolidCollision(rightSpikePos, 0, 0))
				{
					rightSpikePos += rightNormal * 10;
				}
				else if (Collision.SolidCollision(rightSpikePos - rightNormal * 10, 0, 0))
				{
					rightSpikePos -= rightNormal * 10;
				}
				else
				{
					break;
				}
			}
			Projectile p1 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), rightSpikePos, Vector2.zeroVector, ModContent.ProjectileType<Tusk_ground_little>(), 30, 1, NPC.target);
			p1.timeLeft = Main.rand.Next(101, 110);
			rightSpikePos += TileCollisionUtils.GetTopographicGradient(rightSpikePos, 8).RotatedBy(MathHelper.PiOver2) * 30;
			yield return new WaitForFrames(4);
		}
		BloodTuskCoroutine.StartCoroutine(new Coroutine(Rise()));
	}

	public void SwitchActionRise()
	{
		switch (Main.rand.Next(4))
		{
			case 0:
				BloodTuskCoroutine.StartCoroutine(new Coroutine(Rise()));
				break;
			case 1:
				BloodTuskCoroutine.StartCoroutine(new Coroutine(SimultaneouslyGroundSpike(1)));
				break;
			case 2:
				BloodTuskCoroutine.StartCoroutine(new Coroutine(SimultaneouslyGroundSpike(3)));
				break;
			case 3:
				BloodTuskCoroutine.StartCoroutine(new Coroutine(SlowWaveGroundSpike()));
				break;
		}
	}

	/// <summary>
	/// When invisible, teleport to a random point.
	/// </summary>
	public void Relocation()
	{
		Player player = Main.player[NPC.target];
		for (int i = 0; i < 100; i++)
		{
			Vector2 randomPoint = (NPC.Bottom + player.Bottom) * 0.5f + new Vector2(Main.rand.NextFloat(-800, 800), -300);
			for (int j = 0; j < 60; j++)
			{
				if (Collision.SolidCollision(randomPoint, 0, 0))
				{
					NPC.Bottom = randomPoint;
					return;
				}
				randomPoint.Y += 10;
			}
		}
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Bleeding, 120);
	}

	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
	{
		if (NPC.Center - player.Center != Vector2.Zero)
		{
			BeatOffset += Vector2.Normalize(NPC.Center - player.Center) * Math.Clamp(item.knockBack, 0, 20f);
		}
	}

	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		if (projectile.velocity != Vector2.Zero)
		{
			BeatOffset += Vector2.Normalize(projectile.velocity) * Math.Clamp(projectile.knockBack, 0, 20f);
		}
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		if (NPC.life <= 0)
		{
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		Texture2D tuskAtlas = ModAsset.BloodTusk_Atlas.Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null);

		Effect effect = Commons.ModAsset.Shader2D.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();

		List<Vertex2D> bars = new List<Vertex2D>();
		DrawPieceSequence.ForEach(drawPiece => drawPiece.Draw(NPC, bars));
		Main.graphics.graphicsDevice.Textures[0] = tuskAtlas;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);

		spriteBatch.End();
		spriteBatch.Begin(sBS);
		return false;
	}

	public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
	{
		return base.DrawHealthBar(hbPosition, ref scale, ref position);
	}

	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref DownedBossSystem.downedTusk, -1);
		if (Main.netMode == NetmodeID.Server)
		{
			NetMessage.SendData(MessageID.WorldData);
		}
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TuskMirror>()));
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodyTuskTrophy>(), 10, 1));

		npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TuskTreasureBag>()));
		npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<TuskRelic>()));

		var rule = new LeadingConditionRule(new Conditions.NotExpert());
		rule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<ToothStaff>(), ModContent.ItemType<ToothBow>(), ModContent.ItemType<ToothSpear>(), ModContent.ItemType<TuskLace>(), ModContent.ItemType<ToothMagicBall>(), ModContent.ItemType<BloodyBoneYoyo>(), ModContent.ItemType<SpineGun>(), ModContent.ItemType<ToothKnife>()));

		npcLoot.Add(rule);
		base.ModifyNPCLoot(npcLoot);
	}
}