namespace Everglow.Sources.Modules.MythModule.Common
{
    public class NewBossBar : ModSystem//Boss血条修改
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            for (int f = 0; f < 200; f++)
            {
                if (Main.npc[f].type == ModContent.NPCType<TheTusk.NPCs.Bosses.BloodTusk.BloodTusk>() && Main.npc[f].active && !Main.npc[f].friendly)
                {
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskBar").Value, new Vector2(Main.screenWidth / 2f - 2, Main.screenHeight - 55), null, Color.White, 0f, new Vector2(21, 22), 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
