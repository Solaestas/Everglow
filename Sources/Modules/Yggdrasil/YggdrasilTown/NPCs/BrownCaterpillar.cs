using Everglow.Commons.Coroutines;
using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.GreenCore.Items.Weapons;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using Terraria;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class BrownCaterpillar : ModNPC
{
	private CoroutineManager _caterpillarCoroutine = new CoroutineManager();
	public struct Segment
	{
		public Vector2 Normal;
		public int Index;
		public int Style;
		public bool Flip;
		public Vector2 SelfPosition;
		public Segment() { }
	}

	public List<Segment> Segments = new List<Segment>();
	public override void SetDefaults()
	{
		NPCID.Sets.DontDoHardmodeScaling[Type] = true;//必须要设置一个这个否则专家大师模式属性翻倍三倍

		NPC.width = 400;
		NPC.height = 400;
		NPC.noTileCollide = true;
		NPC.lifeMax = 16000000;

		NPC.damage = 22;
		NPC.defense = 8;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.2f;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.noGravity = true;
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return 0f;
	}
	public override void OnSpawn(IEntitySource source)
	{
		Segments = new List<Segment>();
		for(int i = 0;i < 10;i++)
		{
			Segment segment = new Segment();
			segment.Style = 1;
			if(i == 0)
			{
				segment.Style = 0;
			}
			if (i == 9)
			{
				segment.Style = 2;
			}
			segment.SelfPosition = new Vector2((i - 4) * 30, 0);
			segment.Normal = new Vector2(0, -1);
			segment.Flip = false;
			Segments.Add(segment);
		}
		NPC.velocity *= 0;
		_caterpillarCoroutine.StartCoroutine(new Coroutine(Falling()));
	}
	public bool Flying = true;
	public override void AI()
	{
		_caterpillarCoroutine.Update();
		_caterpillarCoroutine.Update();
		_caterpillarCoroutine.Update();
		NPC.velocity *= 0;
		AdjustPosition();
	}
	private IEnumerator<ICoroutineInstruction> Falling()
	{
		int addTime = 30;
		while (addTime > 0)
		{
			if (Flying)
			{
				addTime = 30;
				NPC.velocity.Y += 0.4f;
				NPC.velocity *= 0.96f;
			}
			bool hasAddTime = false;
			for (int i = 0; i < Segments.Count; i++)
			{
				Segment segment = Segments[i];
				Vector2 checkNormal = GetNormalOfTiles(segment.SelfPosition + NPC.Center);
				//如果附近有物块,停止下落
				if (checkNormal != Vector2.zeroVector)
				{
					Flying = false;
					NPC.velocity *= 0f;
					segment.Normal = Vector2.Lerp(segment.Normal, checkNormal, 0.02f);
				}
				else//悬空则继续下落
				{
					segment.Normal = Vector2.Lerp(new Vector2(0, -1), checkNormal, 0.2f);
					segment.SelfPosition += new Vector2(0, 5);

					float distance0 = 40;
					float distance1 = 40;
					float force = 0;
					while(distance0 > 30 || distance1 > 30)
					{
						force++;
						if(force > 5)
						{
							if(Main.rand.NextBool(4) && !hasAddTime)
							{
								hasAddTime = true;
								addTime++;
							}
							break;
						}
						if (i > 0)//除头外,与前一节距离过远则拉住
						{
							Vector2 v = Segments[i - 1].SelfPosition - segment.SelfPosition;
							distance0 = v.Length();
							if (distance0 > 30)
							{
								segment.SelfPosition = Segments[i - 1].SelfPosition - Vector2.Normalize(v) * (28 + force);
							}
						}
						if (i < Segments.Count - 1)//除尾外,与后一节距离过远也拉住
						{
							Vector2 v = Segments[i + 1].SelfPosition - segment.SelfPosition;
							distance1 = v.Length();
							if (distance1 > 30)
							{
								segment.SelfPosition = Segments[i + 1].SelfPosition - Vector2.Normalize(v) * (28 + force);
							}
						}
					}

					//调整角度
					if (i > 0 && i < Segments.Count - 1)
					{
						Vector2 direction = Segments[i + 1].SelfPosition - Segments[i - 1].SelfPosition;

						direction = direction.RotatedBy(-MathHelper.PiOver2);
						segment.Normal = Vector2.Normalize(direction);
					}
					if(i == 0)
					{
						segment.Normal = Vector2.Lerp(segment.Normal, Segments[i + 1].Normal, 0.3f);
					}
					if (i == Segments.Count - 1)
					{
						segment.Normal = Vector2.Lerp(segment.Normal, Segments[i - 1].Normal, 0.3f);
					}
				}
				Segments[i] = segment;
			}

			addTime--;
			AdjustPosition();
			yield return new SkipThisFrame();
		}

		_caterpillarCoroutine.StartCoroutine(new Coroutine(Crawling()));
	}
	private IEnumerator<ICoroutineInstruction> Crawling()
	{
		float tValue = 0;
		Vector2 toHead = Vector2.Normalize(Segments[Segments.Count - 1].SelfPosition - Segments[0].SelfPosition);
		for (int t = 0; t < 60; t++)
		{
			Segment tail = Segments[Segments.Count - 1];//尾巴
			tail.SelfPosition = Vector2.Lerp(tail.SelfPosition, Segments[0].SelfPosition + toHead * 100, 0.03f);
			float height = CheckOverHeight(tail.SelfPosition + NPC.Center, Vector2.Normalize(toHead).RotatedBy(MathHelper.PiOver2));
			if (height > 2 && height < 100)
			{
				tail.SelfPosition += 2 * toHead.RotatedBy(MathHelper.PiOver2);
			}
			tValue = (float)Utils.Lerp(tValue, 60, 0.03f);
			Segments[Segments.Count - 1] = tail;
			for (int i = 1; i < Segments.Count - 1; i++)
			{
				float lerp = 1 - i / (float)Segments.Count;
				Segment segment = Segments[i];
				segment.SelfPosition = Vector2.Lerp(tail.SelfPosition, Segments[0].SelfPosition, MathF.Pow(lerp, 1.05f));
				float x0 = MathF.Max(0.5f - Math.Abs(lerp - 0.5f), 0);

				Vector2 round = new Vector2(0, tValue * 0.7f).RotatedBy(-lerp * MathHelper.TwoPi) + new Vector2(0, -tValue);
				segment.SelfPosition += round.RotatedBy(toHead.ToRotation()) * x0 * 1.5f;

				Vector2 direction = Segments[i + 1].SelfPosition - Segments[i - 1].SelfPosition;

				direction = direction.RotatedBy(-MathHelper.PiOver2);
				segment.Normal = Vector2.Normalize(direction);
				Segments[i] = segment;
			}
			yield return new SkipThisFrame();
		}
		AdjustPosition();
		_caterpillarCoroutine.StartCoroutine(new Coroutine(CrawlingII()));
	}
	private IEnumerator<ICoroutineInstruction> CrawlingII()
	{
		float tValue = 0;
		Vector2 toTail = GetNormalOfTiles(Segments[0].SelfPosition + NPC.Center).RotatedBy(-MathHelper.PiOver4);
		if(toTail == Vector2.zeroVector)
		{
			toTail = Vector2.Normalize(Segments[0].SelfPosition - Segments[Segments.Count - 1].SelfPosition);
		}
		for (int t = 0; t < 60; t++)
		{
			Segment head = Segments[0];//头
			head.SelfPosition += toTail * 2f;
			toTail = toTail.RotatedBy(-0.029f * MathF.Sin(t / 60f * MathF.PI));
			head.Normal = toTail.RotatedBy(MathHelper.PiOver2);
			tValue = (float)Utils.Lerp(tValue, 60, 0.03f);
			Segments[0] = head;

			for (int i = 1; i < Segments.Count; i++)
			{
				Segment segment = Segments[i];
				if (GetNormalOfTiles(segment.SelfPosition + NPC.Center) == Vector2.zeroVector)
				{
					segment.SelfPosition += toTail.RotatedBy(-MathHelper.PiOver2) * tValue / 30f;
				}
				Vector2 v = Segments[i - 1].SelfPosition - segment.SelfPosition;
				if (v.Length() > 30)
				{
					segment.SelfPosition = Segments[i - 1].SelfPosition - Vector2.Normalize(v) * 28;
				}
				Vector2 direction = segment.SelfPosition - Segments[i - 1].SelfPosition;

				direction = direction.RotatedBy(-MathHelper.PiOver2);
				segment.Normal = Vector2.Normalize(direction);
				Segments[i] = segment;
			}
			if (t > 10 && GetNormalOfTiles(Segments[0].SelfPosition + NPC.Center) != Vector2.zeroVector)
			{
				break;
			}
			yield return new SkipThisFrame();
		}
		while(GetNormalOfTiles(Segments[0].SelfPosition + NPC.Center) == Vector2.zeroVector)
		{
			Segment head = Segments[0];//头
			toTail += toTail.RotatedBy(-MathHelper.PiOver2) * 0.02f;
			toTail = Vector2.Normalize(toTail);
			head.SelfPosition += toTail * 2;
			
			head.Normal = Vector2.Lerp(head.Normal, Segments[1].Normal, 0.2f);
			tValue = (float)Utils.Lerp(tValue, 120, 0.03f);
			Segments[0] = head;
			for (int i = 1; i < Segments.Count; i++)
			{
				Segment segment = Segments[i];
				if(GetNormalOfTiles(segment.SelfPosition + NPC.Center) == Vector2.zeroVector)
				{
					segment.SelfPosition += toTail.RotatedBy(-MathHelper.PiOver2) * 2;
				}
				Vector2 v = Segments[i - 1].SelfPosition - segment.SelfPosition;
				if (v.Length() > 30)
				{
					segment.SelfPosition = Segments[i - 1].SelfPosition - Vector2.Normalize(v) * 28;
				}
				Vector2 direction = segment.SelfPosition - Segments[i - 1].SelfPosition;

				direction = direction.RotatedBy(-MathHelper.PiOver2);
				segment.Normal = Vector2.Normalize(direction);
				Segments[i] = segment;
			}
			yield return new SkipThisFrame();
		}
		AdjustPosition();
		_caterpillarCoroutine.StartCoroutine(new Coroutine(Crawling()));
	}
	public void AdjustPosition()
	{
		Vector2 v4P = Segments[4].SelfPosition;
		NPC.Center += v4P;
		for (int i = 0; i < Segments.Count; i++)
		{
			Segment segment = Segments[i];
			segment.SelfPosition -= v4P;
			Segments[i] = segment;
		}
	}
	public override void OnKill()
	{
	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{

	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		for (int i = 0; i < Segments.Count; i++)
		{
			Vector2 drawPos = Segments[i].SelfPosition + NPC.Center - Main.screenPosition;
			int width = texture.Width / 3;
			Rectangle drawRectangle = new Rectangle(Segments[i].Style * width, 0, width, texture.Height);
			spriteBatch.Draw(texture, drawPos, drawRectangle, drawColor, Segments[i].Normal.ToRotation() + MathHelper.PiOver2, new Vector2(width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
		}
		for (int i = 0; i < 1; i++)
		{
			if(Segments.Count > 0)
			{
				Vector2 drawPos = Segments[i].SelfPosition + NPC.Center - Main.screenPosition;
				int width = texture.Width / 3;
				Rectangle drawRectangle = new Rectangle(Segments[i].Style * width, 0, width, texture.Height);
				spriteBatch.Draw(texture, drawPos, drawRectangle, drawColor, Segments[i].Normal.ToRotation() + MathHelper.PiOver2, new Vector2(width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
			}
		}
		return false;
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot)
	{
		return base.CanHitPlayer(target, ref cooldownSlot);
	}
	public override bool? CanBeHitByProjectile(Projectile projectile)
	{
		if (projectile != null && projectile.active)
		{
			for (int i = 0; i < Segments.Count; i++)
		    {
				Segment segment = Segments[i];
				int x = (int)(NPC.Center.X + segment.SelfPosition.X - 20);
				int y = (int)(NPC.Center.Y + segment.SelfPosition.Y - 20);
				Rectangle rectangle = new Rectangle(x, y, 40, 40);

				if (projectile.Colliding(projectile.Hitbox, rectangle) && NPC.immune[projectile.owner] == 0 && projectile.friendly)
				{
					return true;
				}
			}
		}
		return false;
	}
	public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
	{
		base.ModifyHitByProjectile(projectile, ref modifiers);
	}
	public override bool? CanBeHitByItem(Player player, Item item)
	{
		for (int i = 0; i < Segments.Count; i++)
		{
			Segment segment = Segments[i];
			int x = (int)(NPC.Center.X + segment.SelfPosition.X - 20);
			int y = (int)(NPC.Center.Y + segment.SelfPosition.Y - 20);
			Rectangle rectangle = new Rectangle(x, y, 40, 40);
			Rectangle itemBox = item.Hitbox;
			itemBox.X = (int)(player.MountedCenter.X);
			itemBox.Y = (int)(player.MountedCenter.Y);
			if (Rectangle.Intersect(itemBox, rectangle) != Rectangle.emptyRectangle)
			{
				return true;
			}
		}
		return false;
	}
	public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
	{
		return base.ModifyCollisionData(victimHitbox, ref immunityCooldownSlot, ref damageMultiplier, ref npcHitbox);
	}
	public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
	{
		Vector2 v0 = NPC.Center;
		NPC.width = 10;
		NPC.height = 10;
		NPC.Center = v0;
		base.ModifyIncomingHit(ref modifiers);
	}
	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		Vector2 v0 = NPC.Center;
		NPC.width = 500;
		NPC.height = 500;
		NPC.Center = v0;
		base.OnHitByProjectile(projectile, hit, damageDone);
	}
	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
	{
		Vector2 v0 = NPC.Center;
		NPC.width = 500;
		NPC.height = 500;
		NPC.Center = v0;
		base.OnHitByItem(player, item, hit, damageDone);
	}
	public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
	{
		if(Segments.Count > 5)
		{
			position = Segments[4].SelfPosition + NPC.Center;
		}
		return true;
	}


	public Vector2 GetNormalOfTiles(Vector2 postion)
	{
		Vector2 normal = Vector2.Zero;
		for (int i = 0; i < 16; i++)
		{
			Vector2 check = new Vector2(0, 16).RotatedBy(i / 16f * MathHelper.TwoPi);
			Vector2 checkWorld = postion + check;
			if (TileCollisionUtils.PlatformCollision(checkWorld))
			{
				normal -= check;
			}
			else
			{
				normal += check;
			}
		}
		if (normal.Length() < 0.1f)
		{
			return Vector2.zeroVector;
		}
		return Vector2.Normalize(normal);
	}
	public float CheckOverHeight(Vector2 postion, Vector2 normalDirection)
	{
		for (int i = 0; i < 1000; i++)
		{
			Vector2 check = normalDirection * i;
			Vector2 checkWorld = postion + check;
			if (TileCollisionUtils.PlatformCollision(checkWorld))
			{
				return i;
			}
		}
		return 1000;
	}
}
