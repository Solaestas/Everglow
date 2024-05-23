using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class CrimsonTuskControlable : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("");
			}
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 50;
		if (Main.expertMode)
			NPC.damage = 80;
		if (Main.masterMode)
			NPC.damage = 120;
		for (int i = 0; i < NPC.buffImmune.Length; i++)
		{
			NPC.buffImmune[i] = true;
		}
		NPC.width = 30;
		NPC.height = 30;
		NPC.defense = 0;
		NPC.lifeMax = 5;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.aiStyle = -1;
		NPC.alpha = 0;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.dontTakeDamage = true;
	}
	private bool start = true;
	private Vector2[] vpos = new Vector2[400];
	public NPC Owner;
	public override void OnSpawn(IEntitySource source)
	{
		if (Owner == null)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc != null && npc.active)
				{
					if (npc.type == ModContent.NPCType<BloodTusk>())
					{
						if ((npc.Center - NPC.Center).Length() < 20000)
						{
							Owner = npc;
							break;
						}
					}
				}
			}
			if (Owner == null)
			{
				NPC.active = false;
				return;
			}
		}
	}
	public override void AI()
	{
		if (Owner == null)
		{
			NPC.active = false;
			return;
		}
		BloodTusk bloodTusk = Owner.ModNPC as BloodTusk;
		bloodTusk.FlyingTentacleTusks[(int)NPC.ai[1]] = NPC;
		NPC.TargetClosest(true);

		if (start)
		{
			NPC.velocity = new Vector2(0, -7f).RotatedBy(NPC.ai[0] * NPC.ai[2] / 4d);
			start = false;
		}
		NPC.rotation = (float)(Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + Math.PI / 2d);

		length = (NPC.Center - bloodTusk.LookingCenter).Length();
		if (length > 700)
		{
			NPC.velocity *= 0.9f;
			NPC.velocity -= (NPC.Center - bloodTusk.LookingCenter) / length;
		}

	}
	private float length = 100;
	private bool CanD = false;
	private int killing = 0;
	private bool killed = false;
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (length < 15)
			CanD = false;
		else
		{
			CanD = true;
		}
		return false;
	}
	public override bool PreAI()
	{
		if (killed)
		{
			killing--;
			if (Collision.SolidCollision(NPC.Center - Vector2.One * 5f, 10, 10))
				NPC.velocity *= 0.9f;
			else
			{
				NPC.velocity.Y += 0.25f;
				NPC.velocity *= 0.99f;
			}
			if (NPC.velocity != Vector2.Zero)
				NPC.rotation = (float)(Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + Math.PI / 2d);
			if (killing <= 0)
				NPC.active = false;
			if (killing is < 178 and > 0)
			{
				for (int i = 1; i < 400; ++i)
				{
					if (vkilpos[i] == Vector2.Zero)
						break;
					float Gravi = 0.2f;
					var AddiForce = new Vector2(0, Gravi);
					Vector2 AddiForce1 = Vector2.Zero;
					if (i > 1)
						AddiForce1 = vkilpos[i - 1] - vkilpos[i];
					else
					{
					}
					AddiForce1 *= 0.1f;
					float KS = 0.1f;
					/*if (i < 40)
                        {
                            KS = 0.1f + (40 - i) / 240f;
                        }*/
					float k1 = (AddiForce1.Length() - 0.1f) * KS;
					if (k1 >= 0)
						AddiForce1 = Vector2.Normalize(AddiForce1) * k1;
					else
					{
						AddiForce1 = Vector2.Zero;
					}

					Vector2 AddiForce2 = Vector2.Zero;
					if (i < 399)
					{
						if (vkilpos[i + 1] != Vector2.Zero)
						{
							AddiForce2 = vkilpos[i + 1] - vkilpos[i];
							AddiForce2 *= 0.1f;
							float k2 = (AddiForce2.Length() - 0.1f) * KS;
							if (k2 >= 0)
								AddiForce2 = Vector2.Normalize(AddiForce2) * k2;
							else
							{
								AddiForce2 = Vector2.Zero;
							}

						}
						else
						{
							AddiForce2 = Vector2.Zero;

						}
					}

					AddiForce += (AddiForce1 + AddiForce2) * 0.2f;
					Dvkil[i] += AddiForce;
					float f = (float)Math.Exp(Dvkil[i].Length() / 400f) - 1;
					Dvkil[i] *= 1 - f;
					if (Collision.SolidCollision(vkilpos[i] + Dvkil[i] - new Vector2(5, 15), 10, 10))
						Dvkil[i].Y *= -0.6f;
					else
					{
						Delvkilpos[i] *= 0.93f;
						vkilpos[i] += Dvkil[i] + Delvkilpos[i];
					}
				}
			}
			return false;
		}
		return true;
	}
	private Vector2[] vkilpos = new Vector2[400];
	private Vector2[] Prevkilpos = new Vector2[400];
	private Vector2[] Delvkilpos = new Vector2[400];
	private Vector2[] Dvkil = new Vector2[400];
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (Owner == null)
		{
			NPC.active = false;
			return;
		}
		BloodTusk bloodTusk = Owner.ModNPC as BloodTusk;
		if (length < 15)
			return;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		var bars2 = new List<Vertex2D>();

		for (int i = 1; i < 400; ++i)
		{
			if (killing is < 178 and > 0)
			{
				if (vkilpos[i] == Vector2.Zero)
					break;
				if (killing == 9)
				{
					int k = Dust.NewDust(vkilpos[i] - new Vector2(0, 8), 0, 0, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 3f));
					Main.dust[k].noGravity = true;

				}
			}
			if (i == 1)
			{
				vpos[0] = NPC.Center;
				vpos[1] = vpos[0] - NPC.velocity;
			}
			else
			{
				vpos[i] = vpos[i - 1] + (vpos[i - 1] - vpos[i - 2]) * 0.95f + (bloodTusk.LookingCenter - vpos[i - 1]) / (bloodTusk.LookingCenter - vpos[i - 1]).Length() * 0.2f;
			}
			if (killing == 179)
				Prevkilpos[i] = vpos[i];
			if (killing == 178)
			{
				vkilpos[i] = vpos[i];
				Delvkilpos[i] = vkilpos[i] - Prevkilpos[i];
			}
			int width = 9;
			var normalDir = vpos[i - 1] - vpos[i];
			if (killing is < 178 and > 0)
				normalDir = vkilpos[i - 1] - vkilpos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
			Color colori = Lighting.GetColor((int)(vpos[i].X / 16d), (int)(vpos[i].Y / 16d));
			if (killing is < 178 and > 0)
				colori = Lighting.GetColor((int)(vkilpos[i].X / 16d), (int)(vkilpos[i].Y / 16d));
			var factor = Math.Abs(i / 30f % 2 - 1);

			var w2 = MathHelper.Lerp(1f, 0.0f, 0);

			if (killing is < 178 and > 0)
			{
				bars2.Add(new Vertex2D(vkilpos[i] + normalDir * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 1, w2)));
				bars2.Add(new Vertex2D(vkilpos[i] + normalDir * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 0, w2)));
			}
			else
			{
				bars2.Add(new Vertex2D(vpos[i] + normalDir * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 1, w2)));
				bars2.Add(new Vertex2D(vpos[i] + normalDir * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 0, w2)));
			}
			if (killing is < 178 and > 0)
			{
				if ((vkilpos[i] - bloodTusk.LookingCenter).Length() < 6)
					break;
			}
			else
			{
				if ((vpos[i] - bloodTusk.LookingCenter).Length() < 6)
					break;
			}
		}
		if (NPC.ai[3] == 30)
		{
			killed = true;
			killing = 180;
			NPC.ai[3] = 60;
		}
		var triangleList2 = new List<Vertex2D>();
		if (bars2.Count > 2)
		{
			triangleList2.Add(bars2[0]);
			var vertex = new Vertex2D((bars2[0].position + bars2[1].position) * 0.5f + Vector2.Normalize(NPC.velocity), Color.White, new Vector3(0, 0.5f, 0));
			triangleList2.Add(bars2[1]);
			triangleList2.Add(vertex);
			for (int i = 0; i < bars2.Count - 2; i += 2)
			{
				triangleList2.Add(bars2[i]);
				triangleList2.Add(bars2[i + 2]);
				triangleList2.Add(bars2[i + 1]);

				triangleList2.Add(bars2[i + 1]);
				triangleList2.Add(bars2[i + 2]);
				triangleList2.Add(bars2[i + 3]);
			}
			Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/Tusk/BloodRope").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		if (CanD)
		{
			Color colorx = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
			colorx = NPC.GetAlpha(colorx);
			Texture2D texture = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/CrimsonTuskControlable").Value;
			Vector2 DarwV = NPC.Center;
			if (killing is < 178 and > 0)
				DarwV = vkilpos[1];
			Main.spriteBatch.Draw(texture, DarwV - Main.screenPosition - new Vector2(0, 10) + new Vector2(0, -10).RotatedBy(NPC.rotation), null, colorx, NPC.rotation, new Vector2(7f, 31f), NPC.scale, SpriteEffects.None, 0);
		}
	}
}
