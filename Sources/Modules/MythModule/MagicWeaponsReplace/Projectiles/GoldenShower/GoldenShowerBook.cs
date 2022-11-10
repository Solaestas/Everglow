using Terraria.DataStructures;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MEACModule;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.GoldenShower
{
    internal class GoldenShowerBook : MagicBookProjectile, IWarpProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.Ichor;
            ItemType = ItemID.GoldenShower;
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];
            ConstantUsingTime++;

            if (player.itemTime <= 0 || player.HeldItem.type != ItemID.GoldenShower)
            {
                if (Timer < 0)
                {
                    int Rain = Math.Min(ConstantUsingTime / 6, 120);
                    for (int d = 0; d < Rain; d++)
                    {
                        Vector2 velocity = new Vector2(0, Main.rand.NextFloat(-16f, -12f)).RotatedBy(Main.rand.NextFloat(-(Rain / 120f), (Rain / 120f)));
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 + new Vector2(0, -10), velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                    }
                    ConstantUsingTime = 0;
                    Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
                    Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
                    for (int i = 0; i < 45; ++i)
                    {
                        double rot = 0;
                        rot += Projectile.rotation;
                        Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 2.5f;
                        Dust d0 = Dust.NewDustDirect(BasePos - Y0, 0, 0, DustType);
                        d0.noGravity = true;
                        d0.velocity *= 6f;
                        Dust d1 = Dust.NewDustDirect(BasePos + Y0, 0, 0, DustType);
                        d1.noGravity = true;
                        d1.velocity *= 6f;
                    }
                    SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                    int HitType = ModContent.ProjectileType<GoldenShowerBomb>();
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One, HitType, Projectile.damage, Projectile.knockBack * 6, Projectile.owner, Rain / 4f, Projectile.rotation + Main.rand.NextFloat(6.283f));
                    Projectile.Kill();
                }
            }
            if (player.itemTime == 2 && player.HeldItem.type == ItemType)
            {
                if(Main.mouseRight)
                {
                    player.statMana -= 4;
                    for (int d = 0; d < 3; d++)
                    {
                        Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * player.HeldItem.shootSpeed * 1.3f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 + new Vector2(0, -10), velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                    }
                }
                else
                {
                    Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed * 1.3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 + new Vector2(0, -10), velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            for (int d = 0; d < 16; d++)
            {
                Vector2 velocity = new Vector2(0, Main.rand.NextFloat(-16f, -12f)).RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f));
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 + new Vector2(0, -20), velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
            }
        }
        internal int ConstantUsingTime = 0;

        public override void SpecialDraw()
        {
            if(Timer < 24 && ConstantUsingTime > 150)
            {
                float tTimer = Timer - 6;
                float Rain = Math.Min(ConstantUsingTime / 6, 120) / 120f;
                float Fade = (24 - tTimer) / 24f;
                if(Fade < 0)
                {
                    Fade = 0;
                }
                Rain *= Fade;
                DrawTexCircle(tTimer * 24 * Rain / Fade, 184 * Fade, new Color(Rain, Rain, Rain, Rain), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/DarklineWave"), 0);
                DrawTexCircle(tTimer * 24 * Rain / Fade, 184 * Fade, new Color(Rain, Rain * 0.9f, 0, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightlineWave"), 0);
            }

            if (Timer < 12 && ConstantUsingTime > 150)
            {
                float Rain = Math.Min(ConstantUsingTime / 6, 120) / 120f;
                float Fade = (12 - Timer) / 12f;
                Rain *= Fade;
                DrawTexCircle(Timer * 40 * Rain / Fade, 184 * Fade, new Color(Rain, Rain, Rain, Rain), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/DarklineWave"), 0);
                DrawTexCircle(Timer * 40 * Rain / Fade, 184 * Fade, new Color(Rain, Rain * 0.9f, 0, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightlineWave"), 0);
            }
        }
        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3((h * 24 / radious) % 1, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3((h * 24 / radious) % 1, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }

        public void DrawWarp()
        {
            if (Timer < 24 && ConstantUsingTime > 150)
            {
                float tTimer = Timer - 6;
                float Rain = Math.Min(ConstantUsingTime / 6, 120) / 120f;
                float Fade = (24 - tTimer) / 24f;
                if (Fade < 0)
                {
                    Fade = 0;
                }
                Rain *= Fade;
                DrawTexCircle(tTimer * 24 * Rain / Fade, 184 * Fade, new Color(Rain, Rain * 0.9f, 0, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightlineWave"), 0);
            }

            if (Timer < 22 && ConstantUsingTime > 150)
            {
                float Rain = Math.Min(ConstantUsingTime / 6, 120) / 120f;
                float Fade = (22 - Timer) / 22f;
                Rain *= Fade;
                DrawTexCircle(Timer * 40 * Rain / Fade, 184 * Fade, new Color(Rain, Rain * 0.9f, 0, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightlineWave"), 0);
            }
        }
    }
}