namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Dusts;

public class CosmicCrack : ModDust
{
    public override void OnSpawn(Dust dust)
    {
        dust.alpha = 255;
        dust.noLight = true;
        dust.noGravity = true;
        base.OnSpawn(dust);
    }
    public override bool Update(Dust dust)
    {
        dust.position += dust.velocity;
        dust.scale -= 0.03f;
        dust.velocity *= 0.99f;
        if (dust.scale <= 0)
            dust.active = false;

        return false;
    }
    public static void DrawAll(SpriteBatch sb)
    {
        for (int g = 0; g < Main.dust.Length; g++)
        {
            Dust d = Main.dust[g];
            if (d.type == ModContent.DustType<CosmicCrack>() && d.active)
            {
                //Texture2D tex = ModContent.Request<Texture2D>("MythMod/Dusts/CosmicFlame").Value;
                //sb.Draw(tex,d.position-Main.screenPosition,null,Color.White,0,tex.Size()/2,d.scale,SpriteEffects.None,0);
                Texture2D tex2 = ModContent.Request<Texture2D>("MythMod/Dusts/CosmicCrack").Value;
                sb.Draw(tex2, d.position - Main.screenPosition, null, Color.White, g * 2, tex2.Size() / 2, d.scale, SpriteEffects.None, 0);
            }
        }
    }
}
