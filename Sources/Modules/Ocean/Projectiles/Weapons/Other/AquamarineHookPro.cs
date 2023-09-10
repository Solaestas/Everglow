using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other
{
    public class AquamarineHookPro : ModProjectile
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 22;
			base.Projectile.height = 22;
			base.Projectile.aiStyle = 7;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = 100;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft *= 10;
			Main.projHook[base.Projectile.type] = true;
		}
		public override bool PreAI()
		{
			if (Main.player[base.Projectile.owner].dead || Main.player[base.Projectile.owner].stoned || Main.player[base.Projectile.owner].webbed || Main.player[base.Projectile.owner].frozen)
			{
				base.Projectile.Kill();
				return false;
			}
			Vector2 mountedCenter = Main.player[base.Projectile.owner].MountedCenter;
			Vector2 vector = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
			float num = mountedCenter.X - vector.X;
			float num2 = mountedCenter.Y - vector.Y;
			float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			base.Projectile.rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
			if (base.Projectile.ai[0] == 0f)
			{
				if (num3 > 900f)
				{
					base.Projectile.ai[0] = 1f;
				}
				Vector2 value = base.Projectile.Center - new Vector2(5f);
				Vector2 value2 = base.Projectile.Center + new Vector2(5f);
				Point point = Utils.ToTileCoordinates(value - new Vector2(16f));
				Point point2 = Utils.ToTileCoordinates(value2 + new Vector2(32f));
				int num4 = point.X;
				int num5 = point2.X;
				int num6 = point.Y;
				int num7 = point2.Y;
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.maxTilesX)
				{
					num5 = Main.maxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.maxTilesY)
				{
					num7 = Main.maxTilesY;
				}
				for (int i = num4; i < num5; i++)
				{
					int j = num6;
					while (j < num7)
					{
						if (Main.tile[i, j] == null)
						{
							Main.tile[i, j] = new Tile();
						}
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						if (value.X + 10f > vector2.X && value.X < vector2.X + 16f && value.Y + 10f > vector2.Y && value.Y < vector2.Y + 16f && Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[i, j].TileType] || Main.tile[i, j].TileType == 314) && (base.Projectile.type != 403 || Main.tile[i, j].TileType == 314))
						{
							if (Main.player[base.Projectile.owner].grapCount < 3)
							{
								Main.player[base.Projectile.owner].grappling[Main.player[base.Projectile.owner].grapCount] = base.Projectile.whoAmI;
								Main.player[base.Projectile.owner].grapCount++;
							}
							if (Main.myPlayer == base.Projectile.owner)
							{
								int num8 = 0;
								int num9 = -1;
								int num10 = 92500;
								int num11 = 1;
								for (int k = 0; k < 1000; k++)
								{
									if (Main.projectile[k].active && Main.projectile[k].owner == base.Projectile.owner && Main.projectile[k].type == base.Projectile.type)
									{
										if (Main.projectile[k].timeLeft < num10)
										{
											num9 = k;
											num10 = Main.projectile[k].timeLeft;
										}
										num8++;
									}
								}
								if (num8 > num11)
								{
									Main.projectile[num9].Kill();
								}
							}
							WorldGen.KillTile(i, j, true, true, false);
							SoundEngine.PlaySound(SoundID.Dig, new Vector2(i * 16, j * 16));
							base.Projectile.velocity.X = 0f;
							base.Projectile.velocity.Y = 0f;
							base.Projectile.ai[0] = 2f;
							base.Projectile.position.X = (float)(i * 16 + 8 - base.Projectile.width / 2);
							base.Projectile.position.Y = (float)(j * 16 + 8 - base.Projectile.height / 2);
							base.Projectile.damage = 0;
							base.Projectile.netUpdate = true;
							if (Main.myPlayer == base.Projectile.owner)
							{
								NetMessage.SendData(13, -1, -1, null, base.Projectile.owner, 0f, 0f, 0f, 0, 0, 0);
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
					if (base.Projectile.ai[0] == 2f)
					{
						return false;
					}
				}
				return false;
			}
			if (base.Projectile.ai[0] == 1f)
			{
				float num12 = 20f;
				if (num3 < 24f)
				{
					base.Projectile.Kill();
				}
				num3 = num12 / num3;
				num *= num3;
				num2 *= num3;
				base.Projectile.velocity.X = num;
				base.Projectile.velocity.Y = num2;
				return false;
			}
			if (base.Projectile.ai[0] == 2f)
			{
				int num13 = (int)(base.Projectile.position.X / 16f) - 1;
				int num14 = (int)((base.Projectile.position.X + (float)base.Projectile.width) / 16f) + 2;
				int num15 = (int)(base.Projectile.position.Y / 16f) - 1;
				int num16 = (int)((base.Projectile.position.Y + (float)base.Projectile.height) / 16f) + 2;
				if (num13 < 0)
				{
					num13 = 0;
				}
				if (num14 > Main.maxTilesX)
				{
					num14 = Main.maxTilesX;
				}
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num16 > Main.maxTilesY)
				{
					num16 = Main.maxTilesY;
				}
				bool flag = true;
				for (int l = num13; l < num14; l++)
				{
					for (int m = num15; m < num16; m++)
					{
						if (Main.tile[l, m] == null)
						{
							Main.tile[l, m] = new Tile();
						}
						Vector2 vector3;
						vector3.X = (float)(l * 16);
						vector3.Y = (float)(m * 16);
						if (base.Projectile.position.X + (float)(base.Projectile.width / 2) > vector3.X && base.Projectile.position.X + (float)(base.Projectile.width / 2) < vector3.X + 16f && base.Projectile.position.Y + (float)(base.Projectile.height / 2) > vector3.Y && base.Projectile.position.Y + (float)(base.Projectile.height / 2) < vector3.Y + 16f && Main.tile[l, m].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[l, m].TileType] || Main.tile[l, m].TileType == 314 || Main.tile[l, m].TileType == 5))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					base.Projectile.ai[0] = 1f;
					return false;
				}
				if (Main.player[base.Projectile.owner].grapCount < 10)
				{
					Main.player[base.Projectile.owner].grappling[Main.player[base.Projectile.owner].grapCount] = base.Projectile.whoAmI;
					Main.player[base.Projectile.owner].grapCount++;
					return false;
				}
			}
			return false;
		}
        public override void PostDraw(Color lightColor)
		{
            Texture2D texture = ModAsset.AquamarineHookPro_Chain.Value;
			Vector2 vector = base.Projectile.Center;
			Vector2 mountedCenter = Main.player[base.Projectile.owner].MountedCenter;
			Rectangle? sourceRectangle = null;
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num = (float)texture.Height;
			Vector2 vector2 = mountedCenter - vector;
			float rotation = (float)Math.Atan2((double)vector2.Y, (double)vector2.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(vector.X) && float.IsNaN(vector.Y))
			{
				flag = false;
			}
			if (float.IsNaN(vector2.X) && float.IsNaN(vector2.Y))
			{
				flag = false;
			}
			while (flag)
			{
				if ((double)vector2.Length() < (double)num + 1.0)
				{
					flag = false;
				}
				else
				{
					Vector2 value = vector2;
					value.Normalize();
					vector += value * num;
					vector2 = mountedCenter - vector;
					Color color = Lighting.GetColor((int)vector.X / 16, (int)((double)vector.Y / 16.0));
					color = base.Projectile.GetAlpha(color);
					Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
				}
			}
		}
    }
}
