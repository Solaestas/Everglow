using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class MothYoyoProjectile : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 100f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 260f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 23f;
		}
		private readonly Vector3[] cubeVec = new Vector3[]
		{
			new Vector3(1,1,1),
			new Vector3(1,1,-1),
			new Vector3(1,-1,-1),
			new Vector3(1,-1,1),
			new Vector3(-1,1,1),
			new Vector3(-1,1,-1),
			new Vector3(-1,-1,-1),
			new Vector3(-1,-1,1)
		};
		public override void SetDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 5f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16f;
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
		}
		public override void PostAI()
		{
            if (Projectile.localAI[0]==1&&Main.myPlayer==Projectile.owner)
            {
				
				for(int i=0;i<cubeVec.Length;i++)
                {
					Projectile proj= Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),Projectile.Center,Vector2.Zero,ModContent.ProjectileType<MothYoyoSub>(),Projectile.damage/2,0,Projectile.owner,Projectile.whoAmI);
					(proj.ModProjectile as MothYoyoSub).targetPos = cubeVec[i] * 30;
					proj.CritChance = Projectile.CritChance;
					proj.netUpdate2 = true;
                }
            }
		}
        public override bool PreDraw(ref Color lightColor)
		{
            #region 抄抄原版的线（
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
				Vector2 vector = mountedCenter;
				vector.Y += Main.player[Projectile.owner].gfxOffY;
				float num4 = Projectile.Center.X - vector.X;
				float num5 = Projectile.Center.Y - vector.Y;
				Math.Sqrt((double)(num4 * num4 + num5 * num5));
				float rotation = (float)Math.Atan2((double)num5, (double)num4) - 1.57f;
				if (!Projectile.counterweight)
				{
					int num6 = -1;
					if (Projectile.position.X + (float)(Projectile.width / 2) < Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2))
					{
						num6 = 1;
					}
					num6 *= -1;
					Main.player[Projectile.owner].itemRotation = (float)Math.Atan2((double)(num5 * (float)num6), (double)(num4 * (float)num6));
				}
				bool flag = true;
				if (num4 == 0f && num5 == 0f)
				{
					flag = false;
				}
				else
				{
					float num7 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
					num7 = 12f / num7;
					num4 *= num7;
					num5 *= num7;
					vector.X -= num4 * 0.1f;
					vector.Y -= num5 * 0.1f;
					num4 = Projectile.position.X + (float)Projectile.width * 0.5f - vector.X;
					num5 = Projectile.position.Y + (float)Projectile.height * 0.5f - vector.Y;
				}
				while (flag)
				{
					float num8 = 12f;
					float num9 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
					float num10 = num9;
					if (float.IsNaN(num9) || float.IsNaN(num10))
					{
						flag = false;
					}
					else
					{
						if (num9 < 20f)
						{
							num8 = num9 - 8f;
							flag = false;
						}
						num9 = 12f / num9;
						num4 *= num9;
						num5 *= num9;
						vector.X += num4;
						vector.Y += num5;
						num4 = Projectile.position.X + (float)Projectile.width * 0.5f - vector.X;
						num5 = Projectile.position.Y + (float)Projectile.height * 0.1f - vector.Y;
						if (num10 > 12f)
						{
							float num11 = 0.3f;
							float num12 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
							if (num12 > 16f)
							{
								num12 = 16f;
							}
							num12 = 1f - num12 / 16f;
							num11 *= num12;
							num12 = num10 / 80f;
							if (num12 > 1f)
							{
								num12 = 1f;
							}
							num11 *= num12;
							if (num11 < 0f)
							{
								num11 = 0f;
							}
							num11 *= num12;
							num11 *= 0.5f;
							if (num5 > 0f)
							{
								num5 *= 1f + num11;
								num4 *= 1f - num11;
							}
							else
							{
								num12 = Math.Abs(Projectile.velocity.X) / 3f;
								if (num12 > 1f)
								{
									num12 = 1f;
								}
								num12 -= 0.5f;
								num11 *= num12;
								if (num11 > 0f)
								{
									num11 *= 2f;
								}
								num5 *= 1f + num11;
								num4 *= 1f - num11;
							}
						}
						rotation = (float)Math.Atan2((double)num5, (double)num4) - 1.57f;
						Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.White;
						color.A = (byte)((float)color.A * 0.4f);
						color = new Color(0, 150, 255, 0)*0.5f;//改个色
					//Main.NewText(color);
						//color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
						//color = new Microsoft.Xna.Framework.Color((int)((byte)((float)color.R * num13)), (int)((byte)((float)color.G * num13)), (int)((byte)((float)color.B * num13)), (int)((byte)((float)color.A * num13)));
						Main.EntitySpriteDraw(TextureAssets.FishingLine.Value, new Vector2(vector.X - Main.screenPosition.X + (float)TextureAssets.FishingLine.Width() * 0.5f, vector.Y - Main.screenPosition.Y + (float)TextureAssets.FishingLine.Height() * 0.5f) - new Vector2(6f, 0f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.FishingLine.Width(), (int)num8)), color, rotation, new Vector2((float)TextureAssets.FishingLine.Width() * 0.5f, 0f), 1f, SpriteEffects.None, 0);
					}
				}
			#endregion
			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
			Main.EntitySpriteDraw(tex, Projectile.Center-Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, 0, 0);
			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2, Projectile.scale, 0, 0);
			return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
			return new Color(255,255,255,0);
        }
    }
}
