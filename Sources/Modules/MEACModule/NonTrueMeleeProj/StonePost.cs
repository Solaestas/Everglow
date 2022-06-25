namespace Everglow.Sources.Modules.MEACModule.NonTrueMeleeProj
{
    public class StonePost : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1800;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D BackG = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePostBackGround").Value;
            Texture2D Front = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/StonePost").Value;
            Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Front, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);
            int LeftTime = 1800 - Projectile.timeLeft;
            float fx = 0;
            if (LeftTime < 10)
            {
                fx = (float)(-Math.Cos(LeftTime / 10d * Math.PI) + 1) / 2f;
            }
            else if(LeftTime < 40)
            {
                fx = (float)(-Math.Cos((LeftTime + 20) / 30d * Math.PI) + 1) / 2f;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect efS = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/StoneColor", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            efS.Parameters["Str"].SetValue(fx);
            efS.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/img_color", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            efS.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(BackG, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, new Vector2(BackG.Width / 2f, BackG.Height), 1, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}