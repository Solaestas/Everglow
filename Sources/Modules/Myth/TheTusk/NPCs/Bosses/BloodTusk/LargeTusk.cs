using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class LargeTusk : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Tusk Spike"); //Large Tusk Spike
	}

	private Vector2 V = Vector2.Zero;
	private Vector2 VMax = Vector2.Zero;
	private Vector2 VBase = Vector2.Zero;
	private Vector2 VBaseType1 = Vector2.Zero;
	private Vector2 VBaseType2 = Vector2.Zero;
	private Vector2 VBaseType3 = Vector2.Zero;
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 0;
		NPC.width = 10;
		NPC.height = 40;
		NPC.defense = 0;
		NPC.lifeMax = 5;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.aiStyle = -1;
		NPC.alpha = 255;
		NPC.lavaImmune = true;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.dontTakeDamage = true;
		NPC.timeLeft = 4000;
	}

	private int wait = 90;
	private bool squ = false;
	private bool Down = true;
	private bool Spr = false;
	public int Style = 0;
	public override void OnSpawn(IEntitySource source)
	{
		Style = Main.rand.Next(6);
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if (VBaseType1 == Vector2.Zero)
		{
			VBaseType1 = new Vector2(0, Main.rand.NextFloat(-24, -18)).RotatedBy(Main.rand.NextFloat(-0.7f, 0.7f));
			VBaseType2 = new Vector2(0, Main.rand.NextFloat(-15, -7)).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
			VBaseType3 = new Vector2(0, Main.rand.NextFloat(-34, -25)).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
		}
		VMax = new Vector2(0, 84);
		NPC.TargetClosest(false);

		Player player = Main.player[NPC.target];
		if (NPC.collideX && Down)
			NPC.active = false;
		if (NPC.velocity.Length() <= 0.5f && !squ)//鼓包
		{
			if (wait >= 60 && wait < 90)
				VBase = VBase * 0.9f + VBaseType1 * 0.1f;
			if (wait >= 30 && wait < 60)
				VBase = VBase * 0.9f + VBaseType2 * 0.1f;
			if (wait >= 0 && wait < 30)
				VBase = VBase * 0.9f + VBaseType3 * 0.1f;
			if (wait < 2)
			{
				if (!Spr)
				{
					Spr = true;
					for (int f = 0; f < 6; f++)
					{
						Vector2 vd = new Vector2(0, Main.rand.NextFloat(-3f, -1f)).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f));
						Dust.NewDust(NPC.Bottom - new Vector2(4, -6), 0, 0, DustID.Blood, vd.X, vd.Y, 0, default, Main.rand.NextFloat(1f, 2f));
					}
				}
			}
		}
		if (NPC.velocity.Length() <= 0.5f && squ)
		{
			if (VBase.Y <= 0)
				VBase.Y *= 0.9f;
		}
		if (NPC.velocity.Length() <= 0.5f && NPC.alpha > 0 && !squ)
		{
			if (Main.tile[(int)(NPC.Bottom.X / 16d), (int)(NPC.Bottom.Y / 16d)].IsHalfBlock && Down)
			{
				Down = false;
				NPC.position.Y += 16;
			}
			startFight = true;
			V = VMax;
			NPC.alpha -= 25;
		}
		if (NPC.alpha <= 0)
		{
			NPC.alpha = 0;
			wait--;
			if (wait == 5)
				SoundEngine.PlaySound(SoundID.NPCDeath11.WithVolumeScale(.4f), NPC.Bottom);
		}
		if (wait <= 0 && !squ)
		{
			NPC.damage = 60;
			if (Main.expertMode)
				NPC.damage = 80;
			if (Main.masterMode)
				NPC.damage = 100;
			V *= 0.6f;
			if (V.Y <= 0.5f)
				squ = true;
		}
		if (squ)
		{
			V.Y += 0.9f;

			if (V.Y > 80)
			{
				NPC.alpha += 15;
				if (NPC.alpha > 240)
					NPC.active = false;
			}
			if (V.Y > 40)
				NPC.damage = 0;
		}
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Bleeding, 120);
	}
	private bool startFight = false;
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!startFight || NPC.velocity.Length() >= 0.5f)
			return false;
		var BackBase = new List<Vertex2D>();
		Color color = Lighting.GetColor((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16));
		float index = (84 - V.Y) / 84f;
		BackBase.Add(new Vertex2D(NPC.Bottom + new Vector2(-600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(0, 1, 0)));
		BackBase.Add(new Vertex2D(NPC.Bottom + new Vector2(0, 5) + VBase - Main.screenPosition, color, new Vector3(0.5f, 0, 0)));
		BackBase.Add(new Vertex2D(NPC.Bottom + new Vector2(600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(1, 1, 0)));
		Texture2D thang = ModAsset.LargeTuskBase.Value;
		Main.graphics.GraphicsDevice.Textures[0] = thang;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, BackBase.ToArray(), 0, BackBase.Count / 3);


		var bars = new List<Vertex2D>
		{
			new Vertex2D(NPC.Bottom + new Vector2(-12, 5) - Main.screenPosition, color, new Vector3(Style / 6f, index, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(12, V.Y - 79) - Main.screenPosition, color, new Vector3(Style / 6f + 1 / 6f, 0, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(12, 5) - Main.screenPosition, color, new Vector3(Style / 6f + 1 / 6f, index, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(-12, V.Y - 79) - Main.screenPosition, color, new Vector3(Style / 6f, 0, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(12, V.Y - 79) - Main.screenPosition, color, new Vector3(Style / 6f+ 1 / 6f, 0, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(-12, 5) - Main.screenPosition, color, new Vector3(Style / 6f, index, 0))
		};
		thang = ModAsset.LargeTusk.Value;
		Main.graphics.GraphicsDevice.Textures[0] = thang;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);


		var ForeBase = new List<Vertex2D>
		{
			new Vertex2D(NPC.Bottom + new Vector2(-600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(0, 1, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(0, 5) + VBase - Main.screenPosition, color, new Vector3(0.5f, 0, 0)),
			new Vertex2D(NPC.Bottom + new Vector2(600 / (VBase.Length() + 1f), 5) - Main.screenPosition, color, new Vector3(1, 1, 0))
		};

		thang = ModAsset.LargeTuskDrag.Value;
		Main.graphics.GraphicsDevice.Textures[0] = thang;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, ForeBase.ToArray(), 0, ForeBase.Count / 3);
		return false;
	}
}
