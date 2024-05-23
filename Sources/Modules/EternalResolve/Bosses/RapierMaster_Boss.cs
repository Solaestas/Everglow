using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.EternalResolve.Bosses.Projectiles;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace Everglow.EternalResolve.Bosses;
public class RapierMaster_Boss : ModNPC
{
	//public override string Texture => "Terraria/Images/NPC_0";
	public override void SetDefaults()
	{
		NPC.boss = true;
		NPC.friendly = false;
		NPC.width = 24;
		NPC.height = 50;
		NPC.lifeMax = 2000;
		NPC.defense = 30;
		NPC.aiStyle = -1;
        NPC.npcSlots = 80;
        NPC.knockBackResist = 0f;
        NPC.value = Item.buyPrice(0, 1, 0, 0);
        NPC.lavaImmune = true;
        NPC.noGravity = false;
        NPC.noTileCollide = false;
		NPC.buffImmune[BuffID.Confused] = true;
		NPC.damage = 50;
        aIStates = new();
		aiScore = new int[100];
    }
	public List<AIState> aIStates;
	public Player player => Main.player[NPC.target];

	public int t
	{
		set => NPC.ai[1] = value;
		get => (int)NPC.ai[1];
	}
    
	Vector2 tPos;

	#region #治疗相关内容
	int healCD = 1800;
	private void HealAI()
	{
		if (healCD > 0)
			healCD--;
		if(NPC.lifeMax-NPC.life > 150)
		{
			if (healCD == 0)
			{
				NPC.life += 200;
				CombatText.NewText(NPC.Hitbox, CombatText.HealLife, 150);
				healCD = 1800;
				SoundEngine.PlaySound(SoundID.Item3,NPC.Center);
			}
		}
	}
    #endregion

    #region #格挡相关内容
    int parry = 0;
    int noAI = 0;
    int parryTextCD = 90;
	public void ParryEffect()
	{
		if(parryTextCD==0)
		{
			CombatText.NewText(NPC.Hitbox,new Color(0.1f,1f,1f),"格挡！");
			parryTextCD = 90;
            //TODO : Localization Needed
        }
        noAI = 10;
        for (int g = 0; g < 20; g++)
        {
            Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new FireSpark_MetalStabDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = NPC.Center,
                maxTime = Main.rand.Next(1, 25),
                scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(10f, 27.0f)),
                rotation = Main.rand.NextFloat(6.283f),
                ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) }
            };
            Ins.VFXManager.Add(spark);
        }

    }
    public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
	{
        modifiers.ModifyHitInfo += (ref NPC.HitInfo i) =>
        {
            if (parry > 0)//如果能挡
            {
				if (projectile.penetrate != 1)
					parry -= 2;
				else
					parry -= 1;
				projectile.penetrate--;
				i.Damage = 1;
				i.Crit = false;
				SoundEngine.PlaySound(SoundID.NPCHit4,NPC.Center);
				ParryEffect();
			    
            }
        };
    }
	public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
	{
        modifiers.ModifyHitInfo += (ref NPC.HitInfo i) =>
        {
            if (parry > 0)
            {
				
                parry -= 2;
                i.Damage = 1;
				i.Crit = false;
                SoundEngine.PlaySound(SoundID.NPCHit4, NPC.Center);
				ParryEffect();
            }
            
        };
    }
    #endregion

    /// <summary>
    /// 当一个ai未被使用时会增加此ai的score，每次选择ai时会根据score调整概率
    /// </summary>
    int[] aiScore;
    public void SwitchAIP1()
    {
        t = 0;
        NPC.ai[2] = 0;
        NPC.ai[3] = 0;
        parry = 0;
        if (NPC.ai[0] == 0)
        {
            for (int i = 1; i < 3; i++)
            {
                if (Main.rand.Next(5) < aiScore[i])
                {
                    NPC.ai[0] = i;
                    goto SetScore;
                }
            }

            NPC.ai[0] = Main.rand.Next(1,4);
			//NPC.ai[0] = 2;

        }
        else
        {
			//ai2:走路时间
			switch (NPC.ai[0])
			{
				case 1:
                    NPC.ai[2] = Main.rand.Next(20, 60);
                    break;
                case 2:
                    NPC.ai[2] = Main.rand.Next(40, 100);
                    break;
                case -3:
                    NPC.ai[2] = Main.rand.Next(30, 80);
                    break;
                default:
                    NPC.ai[2] = Main.rand.Next(40, 100);
                    break;
			}
            NPC.ai[0] = 0;
        }
		SetScore:
		if (NPC.ai[0] != 0)
		{
			for (int i = 1; i <= 3; i++)
			{
				aiScore[i]++;
			}
			aiScore[(int)NPC.ai[0]] = 0;
		}
    }
    public override void AI()
    {
		HealAI();
		if (parryTextCD > 0)
			parryTextCD--;
		else
			parryTextCD = 0;
		if (noAI == 0)
		{
			#region
			NPC.maxFallSpeed = 2333f;
			NPC.TargetClosest();
			UpdateAIState();
			bool dashing = aIStates.Any(i => i is DashAI);
			if (NPC.velocity.Y == 0 && !dashing)
				NPC.velocity.X *= 0.95f;
			Point p = NPC.Center.ToTileCoordinates();
			Tile tile = Main.tile[p.X, p.Y + 1];
			if (tile.active() && Main.tileSolid[tile.type])
			{
				NPC.position.Y -= 16f;
			}
			#endregion
			if (NPC.ai[0] == 0)//走路
			{
				NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, (NPC.Center.X < player.Center.X ? 1 : -1) * Main.rand.NextFloat(2f, 5f), 0.05f);
				UpdateDirection();
				if (t == 0)
				{
					parry = (int)(NPC.ai[2] / 10);
				}
				if (++t > NPC.ai[2])
				{
					SwitchAIP1();
				}
			}
			if (NPC.ai[0] == 1)//跳两下
			{
				NPC.ai[3] = Terraria.Utils.AngleLerp(NPC.ai[3], NPC.DirectionTo(player.Center).ToRotation(), 0.02f);

				if (++t < 50)
				{
					NPC.velocity *= 0.9f;
				}

				if (t == 25)
				{
					NPC.ai[3] = NPC.DirectionTo(player.Center).ToRotation();
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ModContent.ProjectileType<CrutchRapier_Hostile>(), NPC.damage / 8, 0f, Main.myPlayer, NPC.whoAmI);
					}
				}
				if (t == 30 || t == 50)
				{
					UpdateDirection();
					NPC.ai[3] = NPC.DirectionTo(player.Center).ToRotation();
					Vector2 vel = NPC.DirectionTo(player.Center);
					vel.Y -= 0.2f;
					vel.Y *= 1.2f;
					if (t == 30)
					{
						Dash(vel * 18, 20);
					}
					if (t == 50)
					{
						Dash(vel * 15, 20);
					}
				}
				if (t == 80 && Main.rand.NextBool(2))
				{
					t = 0;
					NPC.ai[0] = -3;

				}
				if (t > 100)
					SwitchAIP1();
			}
			if (NPC.ai[0] == 2)//后撤，长冲
			{

				Vector2 vel = NPC.DirectionTo(player.Center);
				vel.Y = 0;
				vel.Normalize();
				NPC.ai[2] = vel.X;
				if (t++ == 0)
				{
					UpdateDirection();
					float y = NPC.Center.Y - player.Center.Y > 120 ? -Main.rand.Next(8, 15) : 0;
					Dash(-vel * 6 + new Vector2(0, y), 20);
				}
				if (t == 25)
				{
					Dash(vel * 15, 15);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<CrutchRapier_Stab_Hostile>(), NPC.damage / 6, 0f, Main.myPlayer, NPC.whoAmI);
					}
				}
				if (t > 25 && t < 40)
				{
					for (int i = 0; i < 3; i++)
					{
						NPC.velocity = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height);
						NPC.position += NPC.velocity;
					}
				}
				if (t > 50)
					SwitchAIP1();
			}
			if (NPC.ai[0] == -3)//戳一下
			{
				UpdateDirection();
				if (t++ == 0)
				{
					Vector2 vel = NPC.DirectionTo(player.Center) * 8;
					vel.Y -= 1;
					Dash(vel, 40);
				}
				if (t == 30)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center - player.velocity * 5), ModContent.ProjectileType<CrutchRapier_Stab_Hostile>(), NPC.damage / 6, 0f, Main.myPlayer, NPC.whoAmI, 1f);
					}
				}
				if (t > 40)
					SwitchAIP1();
			}
			if (NPC.ai[0]==3)//标记
			{
				
                if (++t < 20)
                {
                    NPC.velocity *= 0.9f;
                }
				if(t==20)
				{
                    Dash(new Vector2(0,-10),60);
                    Vector2 vec = new Vector2(player.velocity.X * 40, -Main.rand.NextFloat(0f, 120f));
					tPos = player.Center + vec;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center+vec, Vector2.zeroVector, ModContent.ProjectileType<Rapier_Slash>(), NPC.damage / 6, 0f, Main.myPlayer, NPC.whoAmI, 1f);
                    }
                }
				if(t==80)
				{
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,( tPos-NPC.Center)*0.002f, ModContent.ProjectileType<CrutchRapier_Stab_Hostile>(), NPC.damage / 6, 0f, Main.myPlayer, NPC.whoAmI, 1f);
                    }
                    NPC.noGravity = true;
                }
				if(t>120)
				{
					NPC.noGravity = false;
                    SwitchAIP1();
                }
            }
			if (NPC.ai[0] == 1111)//下砸,没做好
			{
				if (t++ == 0)
				{
					Vector2 tPos = new Vector2(player.Center.X + player.velocity.X * 30, NPC.Center.Y - 400);
					Jump(NPC.DirectionTo(tPos) * Vector2.Distance(NPC.Center, tPos) * 0.06f, 30);

				}
				if (t < 30)
					NPC.velocity.Y += 0.2f;

				if (t > 30 && t < 60)
				{
					NPC.velocity.X *= 0.9f;
					if (NPC.velocity.Y != 0 && NPC.velocity.Y < 30)
						NPC.velocity.Y += 2;
				}
				if (t > 60)
					SwitchAIP1();
			}
		}
		else
		{
			NPC.position -= NPC.velocity;
			noAI--;
			if (noAI < 0)
				noAI = 0;
		}
    }

	public void WalkToPlayer(float speed,float n=0.5f)
	{
		NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, (NPC.Center.X < player.Center.X ? 1 : -1) * speed, n);
    }
	public void UpdateDirection()
	{
		NPC.direction = NPC.spriteDirection = NPC.Center.X < player.Center.X ? 1 : -1;
	}
    public void UpdateAIState()
	{
        for (int i = 0; i < aIStates.Count; i++)
        {
            AIState aIState = aIStates[i];
            aIState.Update(NPC);
            aIState.timer++;
			if (aIState.timer > aIState.maxTime)
			{
				aIState.OnRemove(NPC);
				aIStates.Remove(aIState);
			}
        }
    }
	public void AddAIState(AIState a)
    {
		aIStates.Add(a);
		a.OnActive(NPC);
	}
	public void Dash(Vector2 vel,int maxTime)
	{
		DashAI dash = new DashAI() 
		{
			vel=vel,
			maxTime=maxTime 
		};
		AddAIState(dash);
	}
	public void Jump(Vector2 vel, int maxTime)
    {
		JumpAI ai = new JumpAI()
		{
			vel = vel,
			maxTime = maxTime
		};
        AddAIState(ai);
    }
	public class DashAI : AIState
	{
		public Vector2 vel = Vector2.UnitX;
		public override void Update(NPC npc)
		{
			if(timer<maxTime*0.4f)
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(npc.Center-new Vector2(15)+new Vector2(0,10), 30, 30, DustID.Smoke, 0, 0, 0, default, 1.6f);
				d.noGravity = true;
			}
			if (timer < maxTime * 0.3f) 
			{
                npc.noGravity = true;
                npc.velocity = vel;
			}
			else
			{
				npc.noGravity = false;
				npc.velocity *= 0.95f;
                
            }
		}
	}
	public class JumpAI : AIState
	{
		bool doublejump = false;
		public Vector2 vel=Vector2.UnitX;
		public override void Update(NPC npc)
		{
			if (timer == 0)
				npc.velocity.Y = vel.Y;
			if(timer<maxTime*0.6f)
			{
				npc.velocity.X = MathHelper.Lerp(npc.velocity.X,vel.X,0.05f);
			}
		}
	}
	public class AIState
	{
		public int timer = 0;
		public int maxTime = 0;
		public virtual void OnActive(NPC npc)
		{

		}
		public virtual void Update(NPC npc)
		{

		}
		public virtual void OnRemove(NPC npc)
		{

		}
	}
    public override bool CanHitPlayer(Player target, ref int cooldownSlot)
	{
		return false;
	}
	public override bool CanHitNPC(NPC target)
	{
		return false;
	}
	public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
	{
		NPC.lifeMax = (int)(2000 * balance * bossAdjustment);
		NPC.damage = 50;
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
        Texture2D tex = ModContent.Request<Texture2D>("Everglow/EternalResolve/Bosses/RapierMaster_Boss").Value;
        Main.spriteBatch.Draw(tex, NPC.Center-Main.screenPosition, null, drawColor, NPC.rotation, tex.Size() / 2, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        if (noAI > 0)
		{
			int type = ModContent.ItemType<CrutchBayonet>();
			Texture2D itemTexture = TextureAssets.Item[type].Value;
			Main.spriteBatch.Draw(itemTexture, NPC.Center+new Vector2(20,0)*NPC.spriteDirection - Main.screenPosition, null, drawColor, 0, itemTexture.Size() / 2f, 1, NPC.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
        return false;
	}
}
