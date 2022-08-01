using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;

namespace Everglow.Sources.Modules.ZYModule.Items
{
    public class SightOfTile : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.value = 0;
            Item.rare = 0;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<SightOfTileProj>();
            Item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(ModContent.GetInstance<SightOfTileUI>().InTile)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.rand.NextFloat(0, 200f), Main.rand.NextFloat(0, 200f));
                return false;
            }
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SightOfTileProjRead>(), damage, knockback, player.whoAmI, Main.rand.NextFloat(0, 200f), Main.rand.NextFloat(0, 200f));
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            return !ModContent.GetInstance<SightOfTileUI>().EnableMapIOUI;
        }
        public override void HoldItem(Player player)
        {
            if(Main.mouseRight && Main.mouseRightRelease)
            {
                if(ModContent.GetInstance<SightOfTileUI>().EnableMapIOUI)
                {
                    ModContent.GetInstance<SightOfTileUI>().CloseUI();
                }
                else
                {
                    ModContent.GetInstance<SightOfTileUI>().OpenUI();
                }
            }
            base.HoldItem(player);
        }
    }
    class SightOfTileUI : ModSystem
    {
        public class UICircle
        {
            public float Size;
            public Vector2 AddCenter;
            public Texture2D texcoord;
            public Texture2D contain;

            public UICircle(float size, Vector2 addCenter, Texture2D texcoord, Texture2D contain)
            {
                Size = 0f;
                AddCenter = Vector2.Zero;
                this.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value;
                this.contain = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/InTile").Value;
            }
        }
        UICircle circle0 = new UICircle(0f, Vector2.Zero, ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value, ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/InTile").Value);
        UICircle circle1 = new UICircle(0f, Vector2.Zero, ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value, ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/OutTile").Value);
        public Vector2 DrawCenter;
        int AnimationTimer = 0;
        public bool EnableMapIOUI = false;
        public bool InTile = true;
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if(EnableMapIOUI)
            {
                if (AnimationTimer < 30)
                {
                    AnimationTimer+=3;
                }
                else
                {
                    AnimationTimer = 30;
                }
            }
            else
            {
                if(AnimationTimer > 0)
                {
                    AnimationTimer-=3;
                }
                else
                {
                    AnimationTimer = 0;
                }
            }
            if (AnimationTimer > 0)
            {
                circle0.contain = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/InTile").Value;
                circle1.contain = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/OutTile").Value;
                UpdateAndDraw(circle0, 0);
                UpdateAndDraw(circle1, 1);
            }
            base.PostDrawInterface(spriteBatch);
        }
        public void UpdateAndDraw(UICircle c0, int Count)
        {
            c0.AddCenter = new Vector2(0, AnimationTimer * 1.0f).RotatedBy(AnimationTimer / 60d * Math.PI + Count * Math.PI);
            if(AnimationTimer < 30)
            {
                c0.Size = AnimationTimer / 30f;
            }
            CheckMouseOver(c0);
            DrawUICircle(c0);
            CheckMouseCLick(c0, Count);
        }
        public void CheckMouseCLick(UICircle c0, int Count)
        {
            if ((Main.MouseScreen - DrawCenter - c0.AddCenter).Length() < 20)
            {
                if(Main.mouseLeft && Main.mouseLeftRelease)
                {
                    if(Count == 0)
                    {
                        InTile = true;
                    }
                    if (Count == 1)
                    {
                        InTile = false;
                    }
                    CloseUI();
                }
            }
        }
        public void CheckMouseOver(UICircle c0)
        {
            if ((Main.MouseScreen - DrawCenter - c0.AddCenter).Length() < 20 && c0.texcoord != ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_1").Value)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                c0.Size = 1.2f;
                c0.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_1").Value;
            }
            if ((Main.MouseScreen - DrawCenter - c0.AddCenter).Length() >= 20 && c0.texcoord != ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                c0.Size = 1f;
                c0.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value;
            }
        }
        public void DrawUICircle(UICircle c0)
        {
            Main.spriteBatch.Draw(c0.texcoord, DrawCenter + c0.AddCenter, null, Color.White, 0, c0.texcoord.Size() / 2f, c0.Size, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(c0.contain, DrawCenter + c0.AddCenter, null, Color.White, 0, c0.contain.Size() / 2f, c0.Size, SpriteEffects.None, 0);
        }
        public void OpenUI()
        {
            DrawCenter = Main.MouseScreen / Main.UIScale;
            circle0.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value;
            circle1.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value;
            EnableMapIOUI = true;
        }
        public void CloseUI()
        {
            circle0.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value;
            circle1.texcoord = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Wires_0").Value;
            EnableMapIOUI = false;
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if(Main.LocalPlayer.HeldItem.type != ModContent.ItemType<SightOfTile>())
            {
                CloseUI();
            }
            base.UpdateUI(gameTime);
        }
    }
}
