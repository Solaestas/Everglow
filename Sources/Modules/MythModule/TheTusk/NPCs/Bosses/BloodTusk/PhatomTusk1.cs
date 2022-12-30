using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk
{
    public class PhatomTusk1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        private Vector2[] V = new Vector2[10];
        private Vector2[] VMax = new Vector2[10];
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
        }
        private int wait = -1000000;
        private bool squ = false;
        private bool Down = true;
        private int Dir = 0;
        public override void AI()
        {

            NPC.localAI[0]++;
            V[3].Y += 1;
            if (wait == -1000000)
            {

                wait = Main.rand.Next(5);
                NPC.rotation = Main.rand.NextFloat(-0.3f, 0.3f);
                Dir = Main.rand.Next(100);
            }
            if (!NPC.collideX && !NPC.collideY)
            {
                if (NPC.velocity.Length() < 1)
                {
                    V[5].Y += 1;
                }
            }
            VMax[0] = new Vector2(0, 34).RotatedBy(NPC.rotation);
            if (NPC.collideX && Down)
            {
                NPC.active = false;
            }
            if (Collision.SolidCollision(NPC.position - Vector2.One * 5f + new Vector2(5, -10), 10, 10))
            {
                NPC.active = false;
            }
            if ((NPC.collideY || V[5].Y > 4) && NPC.alpha > 0 && !squ)
            {
                if (V[4] == Vector2.Zero)
                {
                    V[4] = NPC.Bottom;
                }
                // 弹幕
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.MagicHit>(), (int)NPC.ai[0], 5, Main.myPlayer);
                if (Main.tile[(int)(NPC.Bottom.X / 16d), (int)(NPC.Bottom.Y / 16d)].IsHalfBlock && Down)
                {
                    Down = false;
                    NPC.position.Y += 16;
                    V[4].Y += 16;
                }
                if (Main.tile[(int)(NPC.Bottom.X / 16d), (int)(NPC.Bottom.Y / 16d) + 1].IsHalfBlock && Down)
                {
                    Down = false;
                    NPC.position.Y += 16;
                    V[4].Y += 16;
                }
                startFight = true;
                if (NPC.alpha == 255)
                {
                    RamInt = Main.rand.Next(9);
                }
                V[0] = VMax[0];
                NPC.alpha -= 25;

            }
            if (NPC.alpha <= 0)
            {
                NPC.alpha = 0;
                wait--;
            }
            if (wait <= 0 && !squ)
            {
                NPC.damage = 0;
                V[0] *= 0.8f;
                if (V[0].Y <= 0.5f)
                {
                    squ = true;
                }
            }
            if (squ)
            {
                V[0] += new Vector2(0, 2.9f).RotatedBy(NPC.rotation);
                if (V[0].Y > 40)
                {
                    if (V[5].X > 0)
                    {
                        V[5].X -= 0.05f;
                    }
                    NPC.alpha += 15;
                    if (NPC.alpha > 240)
                    {
                        NPC.active = false;
                    }
                }
            }
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
        }
        private bool startFight = false;
        private int RamInt = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!startFight)
            {
                return false;
            }
            Color color = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
            color = NPC.GetAlpha(color) * ((255 - NPC.alpha) / 255f);
            Color color2 = NPC.GetAlpha(color);
            Texture2D tb = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/TuskplusBottom").Value;
            float Sc = (255 - NPC.alpha) / 200f;
            Main.spriteBatch.Draw(tb, V[4] - Main.screenPosition + new Vector2(4, 4) - new Vector2(0, 12) + new Vector2(0, 16).RotatedBy(NPC.rotation), null, color2, NPC.rotation, new Vector2(tb.Width / 2f, tb.Height / 2f), Sc, SpriteEffects.None, 0f);
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/Tusk" + RamInt.ToString()).Value;
            if (Dir > 50)
            {
                Main.spriteBatch.Draw(t, NPC.position - Main.screenPosition + new Vector2(10, 24) + V[0], new Rectangle(0, 0, t.Width, t.Height - (int)V[0].Y), color, NPC.rotation, new Vector2(t.Width / 2f, t.Height / 2f), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(t, NPC.position - Main.screenPosition + new Vector2(10, 24) + V[0], new Rectangle(0, 0, t.Width, t.Height - (int)V[0].Y), color, NPC.rotation, new Vector2(t.Width / 2f, t.Height / 2f), 1f, SpriteEffects.FlipHorizontally, 0f);
            }

            return false;
        }
    }
}
