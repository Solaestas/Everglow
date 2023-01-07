using Everglow.Sources.Modules.MythModule.Common;
using IL.Terraria.Graphics.Shaders;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class Hepuyuan : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Prinomial Jade Winged Spear");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "和璞鸢");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Нефритовый коршун");
			Tooltip.SetDefault("Exclusive weapon\nLeft click to dash, right click to smash\n'You should't be getting it'\nHomaging to Genshin Impact");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "专属武器\n左键突刺,右键打桩\n你不应该得到它\n致敬<原神>");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "Эксклюзивное оружие \nщелкните левой кнопкой мыши, чтобы сделать дэш, щелчок правой кнопкой мыши, чтобы разрушать\n\"Вы не должны его получить\"\nдань уважения Genshin Impact");*/
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            // Common Properties
            Item.rare = 11; // Assign this item a rarity level of Pink
            Item.value = Item.sellPrice(silver: 50); // The number and type of coins item can be sold for to an NPC

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.useAnimation = 18; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useTime = 18; // The length of the item's use time in ticks (60 ticks == 1 second.)
            Item.UseSound = SoundID.Item71; // The sound that this item plays when used.
            Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()		

            // Weapon Properties
            Item.damage = 674;
            Item.crit = 22;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

            // Projectile Properties
            Item.shootSpeed = 17f; // The speed of the projectile measured in pixels per frame.
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.Hepuyuan.Hepuyuan>(); //The projectile that is fired from this weapon
        }
        public override bool AltFunctionUse(Player player)
        {
            return CanDown;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 18;
                Item.useAnimation = 18;
            }
            else
            {
                Item.useTime = 18;
                Item.useAnimation = 18;
            }
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        bool CanDown;
        public override void HoldItemFrame(Player player)
        {
            for (int h = 0; h < 20; h++)
            {
                if (Collision.SolidCollision(player.Center + new Vector2(0, h * 16), 1, 1))
                {
                    CanDown = false;
                    return;
                }
            }
            CanDown = true;
            base.HoldItemFrame(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            coll = 90;
            CoolRarr = 480 - (int)(MythContentPlayer.StackDamageAdd / 0.05f * 90);
            MythContentPlayer.Dashcool = 480 - (int)(MythContentPlayer.StackDamageAdd / 0.05f * 90);

            if (player.altFunctionUse == 2)
            {
                Vector2 VelcD = new Vector2(0, 1);
                Projectile.NewProjectile(source, position, VelcD, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.Hepuyuan.HepuyuanDown>(), damage * 5, knockback, player.whoAmI, 0f, 0f);
                player.velocity += VelcD * 4;
                HepuyuanOwner.MouseCooling = 40;
                return false;
            }
            float[] Threaten = new float[200];//威胁优先级判定
            for (int d = 0; d < 200; d++)
            {
                if (Main.npc[d].active && !Main.npc[d].friendly && !Main.npc[d].dontTakeDamage && Collision.CanHit(player, Main.npc[d]))//活着,敌对,能被打
                {
                    Threaten[d] += 1;
                    if ((Main.npc[d].Center - player.Center).Length() < 500)//距离
                    {
                        Threaten[d] += (500 - (Main.npc[d].Center - player.Center).Length()) * 12;
                    }
                    if ((Main.npc[d].Center - player.Center).Length() < 800)//距离
                    {
                        Threaten[d] += Main.npc[d].life + Main.npc[d].lifeMax * 0.2f;//血量和血量上限
                        Threaten[d] += Main.npc[d].damage * 12f;//伤害
                        if (Main.npc[d].boss)
                        {
                            Threaten[d] += 1000;//Boss需要额外增加威胁度
                        }
                        if (Main.npc[d].CanBeChasedBy(null, false))
                        {
                            Threaten[d] += 10;//能被追踪
                        }
                        if (Main.npc[d].velocity.Length() > 3)//速度
                        {
                            Threaten[d] += Main.npc[d].velocity.Length() * 110;//速度威胁
                            Vector2 VplayerToNPC = Vector2.Normalize(Main.npc[d].Center - player.Center) * 40;
                            float EscapeT = Vector2.Dot(VplayerToNPC, Main.npc[d].velocity) / Main.npc[d].life * 300;//逃跑系数
                            if (EscapeT > 0)
                            {
                                Threaten[d] += EscapeT;
                            }
                            float CrashT = Vector2.Dot(VplayerToNPC, -Main.npc[d].velocity) * Main.npc[d].damage / 100f;//撞击系数
                            if (CrashT > 0)
                            {
                                Threaten[d] += CrashT;
                            }
                        }
                    }
                    Vector2 playerToNPC = Vector2.Normalize(Main.npc[d].Center - player.Center);
                    Vector2 playerToMouseWorld = Vector2.Normalize(Main.MouseWorld - player.Center);
                    float CosineTheta = Math.Clamp(Vector2.Dot(playerToNPC, playerToMouseWorld), 0, 1);//用于计算鼠标方向权重
                    if (Main.npc[d].type == NPCID.TargetDummy)
                    {
                        Threaten[d] = 1;
                    }
                    float k0 = HepuyuanOwner.MouseCooling / 20f;
                    Threaten[d] = Threaten[d] * CosineTheta * (1 - k0) + Threaten[d] * k0;
                }
            }
            float MaxT = 0;//最高威胁值
            float TotalT = 0;//总威胁值
            int MaxD = -1;//产生最高威胁值的怪
            for (int d = 0; d < 200; d++)
            {
                if (Threaten[d] > MaxT)
                {
                    MaxT = Threaten[d];
                    MaxD = d;
                }
                TotalT += Threaten[d];
            }
            /*Vector2 TotalAimPos = Vector2.Zero;
			for(int d = 0; d < 200; d++)
            {
				if (Threaten[d] > 0)
				{
					TotalAimPos += (Main.npc[d].Center + Main.npc[d].velocity * 2) * Threaten[d];//总威胁坐标
				}
			}
			if(TotalT > 0)
            {
				TotalAimPos /= TotalT;//加权平均威胁中心
			}*/
            Vector2 NewVelocity = velocity;
            if (MaxT > 0 && HepuyuanOwner.MouseCooling > 3)
            {

                NewVelocity = Vector2.Normalize((Main.npc[MaxD].Center + Main.npc[MaxD].velocity * 2) - player.Center) * velocity.Length();
            }
            Projectile.NewProjectile(source, position, NewVelocity, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.Hepuyuan.Hepuyuan>(), damage * 2, knockback, player.whoAmI, 0f, 0f);

            player.velocity += NewVelocity * 4;
            HepuyuanOwner.MouseCooling = 30;
            return false;
        }
        int CoolRarr = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;
            Texture2D RArr = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Melee/Hepuyuan/RightGreenSpice").Value;
            if (!Main.gamePaused)
            {
                if (!CanDown)
                {
                    spriteBatch.Draw(RArr, drawPos + new Vector2(42) * scale, null, new Color(0, 0, 0, 255), 0f, new Vector2(8), scale * 3, SpriteEffects.None, 0f);
                    //Main.spriteBatch.DrawString(FontAssets.MouseText.Value, ((int)(CoolRarr / 60f)).ToString(), drawPos + new Vector2(12), Color.Red, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(RArr, drawPos + new Vector2(42) * scale, null, new Color(255, 255, 255, 0), 0f, new Vector2(8), scale * 3, SpriteEffects.None, 0f);
                }
            }
        }
        public static int coll = 0;
        public override void HoldItem(Player player)
        {
            if (Main.mouseRight)
            {
                coll = 18;
            }
            if (coll > 0)
            {
                player.maxFallSpeed = 10000f;
                coll--;
            }
            else
            {
                coll = 0;
            }
        }
        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(Item.UseSound, player.Center);
            }

            return null;
        }
    }
    public class HepuyuanOwner : ModPlayer
    {
        public static int MouseCooling = 0;
        public override void PostUpdate()
        {
            if (MouseCooling > 0)
            {
                MouseCooling--;
            }
            else
            {
                MouseCooling = 0;
            }
            base.PostUpdate();
        }
        public override void UpdateEquips()
        {
            if (Player.HeldItem.type == ModContent.ItemType<Hepuyuan>())
            {
                Player.jumpSpeedBoost += 12.75f;
				//Player.noFallDmg = true;
			}
        }
    }
}
