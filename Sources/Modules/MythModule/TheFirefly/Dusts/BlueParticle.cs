﻿namespace Everglow.Sources.Modules.MythModule.TheFirefly.Dusts
{
    public class BlueParticle : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale *= 1f;
            dust.alpha = 0;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += 0.1f;
            dust.scale *= 0.95f;
            dust.velocity *= 0.95f;
            Lighting.AddLight(dust.position, 0, 0, dust.scale * 0.75f);
            if (dust.scale < 0.15f)
            {
                dust.active = false;
            }
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            if (dust.scale > 0.6f)
            {
                return new Color?(new Color(0f, 0.6f, 0.9f, 0f));
            }
            else
            {
                return new Color?(new Color(0, 0.6f, 0.9f, (0.6f - dust.scale) / 0.6f));
            }
        }
    }
}
