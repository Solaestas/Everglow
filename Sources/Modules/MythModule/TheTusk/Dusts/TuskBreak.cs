namespace Everglow.Sources.Modules.MythModule.TheTusk.Dusts
{
    public class TuskBreak : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, Main.rand.Next(8) * 28, 24, 28);
            dust.alpha = 0;
            dust.rotation = Main.rand.NextFloat((float)(MathHelper.TwoPi));
        }
        public override bool Update(Dust dust)
        {
            if (dust.fadeIn == 0)
            {
                dust.fadeIn = Main.rand.NextFloat(-0.019f, 0.019f);
            }
            dust.rotation += dust.fadeIn;
            dust.position += dust.velocity;
            dust.velocity.Y += 0.15f;
            if (dust.alpha > 245)
            {
                dust.active = false;
            }
            if (Collision.SolidCollision(dust.position, 28, 28))
            {
                if (dust.velocity.Length() >= 1)
                {
                    dust.velocity.Y *= Main.rand.NextFloat(-1f, -0.2f);
                    Vector2 v0 = dust.velocity.RotatedBy(Main.rand.NextFloat(-0.03f, 0.03f));
                    if (v0.Y < 0)
                    {
                        dust.velocity = v0;
                    }
                    dust.velocity *= 0.8f;
                    dust.position += dust.velocity * 2f;
                }
                else
                {
                    dust.velocity *= 0;
                }
            }
            if (dust.velocity.Length() < 0.03f)
            {
                dust.alpha += 2;
            }

            return false;
        }
    }
}
