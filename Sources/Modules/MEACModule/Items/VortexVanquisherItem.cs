using Everglow.Sources.Modules.MEACModule.Projectiles;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MEACModule.Items
{
    public class VortexVanquisherItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.width = 1;
            Item.height = 1;
            Item.useAnimation = 5;
            Item.useTime = 5;
            Item.shootSpeed = 5f;
            Item.knockBack = 2.5f;
            Item.damage = 30;
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.sellPrice(gold: 1);
        }

        private int CoolTimeForE = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            Texture2D RArr = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/Post").Value;
            if (!Main.gamePaused)
            {
                if (CoolTimeForE > 0)
                {
                    CoolTimeForE--;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(26.73f) * scale + new Vector2(4, 4), null, new Color(0, 0, 0, 255), 0f, RArr.Size() / 2f, scale * 1.91f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (CoolTimeForE / 60f).ToString("#.#"), drawPos + new Vector2(22.91f) * scale, Color.White, 0f, Vector2.Zero, scale * 1.91f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolTimeForE = 0;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(26.73f) * scale + new Vector2(4, 4), null, new Color(155, 155, 155, 50), 0f, RArr.Size() / 2f, scale * 1.91f, SpriteEffects.None, 0f);
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (base.CanUseItem(player))
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    if (player.altFunctionUse != 2)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<VortexVanquisher>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                        proj.scale *= Item.scale;
                    }
                    else
                    {
                        if(CoolTimeForE > 0)
                        {
                            return false;
                        }
                        CoolTimeForE = 720;
                        bool HasProj = false;
                        foreach (Projectile proj in Main.projectile)
                        {
                            if(proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<NonTrueMeleeProj.GoldShield>() && proj.active)
                            {
                                proj.timeLeft = 1200;
                                proj.ai[1] = 150;//盾量
                                HasProj = true;
                            }
                        }
                        if(!HasProj)
                        {
                            Projectile proj2 = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<NonTrueMeleeProj.GoldShield>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                            proj2.ai[1] = 150;//盾量
                        }
                        Vector2 CheckPoint = Main.MouseWorld;
                        for (int y = 0;y < 60;y++)
                        {
                            if (Collision.SolidCollision(CheckPoint, 1, 1))
                            {
                                break;
                            }
                            else
                            {
                                CheckPoint += new Vector2(0, 10) * player.gravDir;
                            }
                        }
                        if (!Collision.SolidCollision(CheckPoint, 1, 1))
                        {
                            return false;
                        }


                        Vector2 TotalVector = Vector2.Zero;
                        int TCount = 0;
                        for (int a = 0; a < 12; a++)
                        {
                            Vector2 v0 = new Vector2(10, 0).RotatedBy(a / 6d * Math.PI);
                            if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
                            {
                                TotalVector -= v0;
                                TCount++;
                            }
                            else
                            {
                                TotalVector += v0;
                            }
                        }
                        for (int a = 0; a < 24; a++)
                        {
                            Vector2 v0 = new Vector2(20, 0).RotatedBy(a / 12d * Math.PI);
                            if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
                            {
                                TotalVector -= v0 * 0.5f;
                                TCount++;
                            }
                            else
                            {
                                TotalVector += v0 * 0.5f;
                            }
                        }
                        if(TotalVector == Vector2.Zero || TCount > 30)
                        {
                            return false;
                        }

                        int f = Projectile.NewProjectile(player.GetSource_ItemUse(Item), CheckPoint, Vector2.Zero, ModContent.ProjectileType<NonTrueMeleeProj.StonePost>(), Item.damage, 0, player.whoAmI, 1);
                        float Angle = (float)Math.Atan2(TotalVector.Y, TotalVector.X);
                        Main.projectile[f].rotation = (float)(Angle - Math.PI * 1.5);
                    }
                }
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {

        }
    }
}
