using Everglow.Myth.MiscItems.Projectiles.Weapon.Melee.Hepuyuan;

namespace Everglow.Myth.TheTusk.Projectiles;

public class TuskSpicePro : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 100;
		Projectile.height = 100;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 80;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
		Projectile.extraUpdates = 1;
	}
	public override void AI()
	{
		if (Projectile.timeLeft <= 78)
			Projectile.friendly = false;
	}
	public static int CyanStrike = 0;
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
	{
		CyanStrike = 1;
		Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
		base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
	}

	public override void Load()
	{
		On_CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
		On_Main.DoDraw_Tiles_NonSolid += DrawTusk;
	}
	public override void Unload()
	{
		//On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool -= CombatText_NewText_Rectangle_Color_string_bool_bool;
		//On.Terraria.Main.DoDraw_Tiles_NonSolid -= DrawTusk;
	}
	public bool[,] hasHit = new bool[200, 1000];
	private void DrawTusk(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
	{
		for (int p = 0; p < Main.projectile.Length; p++)
		{
			if (Main.projectile[p].type == ModContent.ProjectileType<TuskSpicePro>() && Main.projectile[p].active)
			{
				if (Main.projectile[p].timeLeft >= 79)
				{
					for (int j = 0; j < 200; j++)
					{
						hasHit[j, p] = false;
					}
				}
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				var Vx = new List<VertexBase.CustomVertexInfo>();
				Vector2 Vbase = Main.projectile[p].Center - Main.screenPosition;
				var v0 = new Vector2(0, -1);
				var v0T = new Vector2(1, 0);

				float length = Main.projectile[p].ai[0];
				float origfade = Math.Clamp((80 - Main.projectile[p].timeLeft) / 24f, 0, 1f);
				if (Main.projectile[p].timeLeft < 20)
					origfade = Main.projectile[p].timeLeft / 20f;
				v0 = v0.RotatedBy(Main.projectile[p].rotation);
				Vbase -= v0 * 16f;
				v0 = v0 * length * origfade;
				v0T = v0T.RotatedBy(Main.projectile[p].rotation);
				float wid = 12f * Main.projectile[p].ai[1];
				v0T = v0T * wid;

				Color cr = Lighting.GetColor((int)(Main.projectile[p].Center.X / 16), (int)(Main.projectile[p].Center.Y / 16));
				float fadeK = 1 - origfade;

				Vx.Add(new VertexBase.CustomVertexInfo(Vbase + v0 * 2 + v0T, cr, new Vector3(1, 0, 0)));
				Vx.Add(new VertexBase.CustomVertexInfo(Vbase + v0T, cr, new Vector3(1, origfade, 0)));
				Vx.Add(new VertexBase.CustomVertexInfo(Vbase - v0T, cr, new Vector3(0, origfade, 0)));

				Vx.Add(new VertexBase.CustomVertexInfo(Vbase + v0 * 2 + v0T, cr, new Vector3(1, 0, 0)));
				Vx.Add(new VertexBase.CustomVertexInfo(Vbase + v0 * 2 - v0T, cr, new Vector3(0, 0, 0)));
				Vx.Add(new VertexBase.CustomVertexInfo(Vbase - v0T, cr, new Vector3(0, origfade, 0)));

				Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/Tuskplus" + (Main.projectile[p].whoAmI % 6).ToString()).Value;

				if (Main.projectile[p].timeLeft % 6 == 0 && Main.projectile[p].timeLeft < 75)
				{
					for (int j = 0; j < 200; j++)
					{
						if (!hasHit[j, p])
						{
							for (int X0 = 0; X0 < 6; X0++)
							{
								Rectangle hbx = Main.npc[j].Hitbox;
								if (hbx.Intersects(new Rectangle((int)(Main.projectile[p].Center + v0 * 0.4f * X0).X - 20, (int)(Main.projectile[p].Center + v0 * 0.4f * X0).Y - 20, 40, 40)))
								{
									if (!Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
									{
										int Dam = Projectile.damage;
										bool Crit = Main.rand.Next(100) < Main.projectile[p].ai[0];
										Main.npc[j].StrikeNPC(Dam, 0, Math.Sign(Main.projectile[p].velocity.X), Main.rand.Next(100) < Main.projectile[p].ai[0]);
										Player player2 = Main.player[Main.projectile[p].owner];
										if (Crit)
											player2.addDPS((Dam - Main.npc[j].defense) * 2);
										else
										{
											player2.addDPS((Dam - Main.npc[j].defense) * 2);
										}
										hasHit[j, p] = true;
									}
								}
							}
						}
					}
				}
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
		orig(self);
	}
	private int CombatText_NewText_Rectangle_Color_string_bool_bool(On_CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
	{
		if (CyanStrike > 0)
		{
			color = new Color(0, 255, 174);
			CyanStrike--;
		}
		return orig(location, color, text, dramatic, dot);
	}
}