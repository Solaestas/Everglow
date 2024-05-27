using Everglow.Commons;
using Everglow.Sources.Menu.Entities;
using Everglow.Commons.Vertex;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Star = Everglow.Sources.Menu.Entities.Star;
using Terraria.ModLoader;

namespace Everglow.Sources.Menu
{
	public sealed class MenuManualMusicRegistration : ILoadable
	{
		public void Load(Mod mod)
		{
			//MusicLoader.AddMusic(mod, "Commons/Menu/Music/BaseMusic");
			MusicLoader.AddMusic(mod, "Commons/Menu/Music/MenuMusic");
		}
		public void Unload()
		{
		}
	}
	internal class EverglowModMenu : ModMenu
	{
		public override int Music => MusicLoader.GetMusicSlot(ModIns.Mod, "Commons/Menu/Music/MenuMusic");

		public List<Star> stars = new();
        public override void Load()
        {

        }
        public override void Unload()
        {
            stars=null;
        }
        public void Update()
        {
            for (int i=0;i<stars.Count;i++)
            {
                Star star = stars[i];
                star.Update();
                star.timeLeft--;
                if(star.timeLeft<=0)
                {
                    stars.Remove(star);
                }
            }

            if(Main.rand.NextBool(25))//生成流星
            {
                TraillingStar star = new TraillingStar();

                byte c = (byte)(50 + Main.rand.Next(100));
                star.color.R -= c;
                star.color.G -= c;
                star.maxTime = 100;
                star.timeLeft = star.maxTime;
                star.scale *= Main.rand.NextFloat(2.5f);
                star.position = new Vector2(Main.rand.Next(Main.screenWidth+350),-100) * Main.UIScale;
                star.velocity = new Vector2(-12-Main.rand.Next(20),8+Main.rand.Next(25))*0.8f;
                stars.Add(star);
            }
            //生成旋转星星
            //if (Main.rand.NextBool())
            {
                Star star = new Star();

                byte c = (byte)(50+ Main.rand.Next(120));
                star.color.R -= c;
                star.color.G -= c;
                star.scale *= Main.rand.NextFloat(2);
                star.maxTime = 200 + Main.rand.Next(200);
                star.timeLeft = star.maxTime;

                star.position = new Vector2(Main.rand.Next(Main.screenWidth), Main.rand.Next(Main.screenHeight))*Main.UIScale;

                stars.Add(star);
            }
        }
        public void DrawStars()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.Additive,SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            
            foreach(Star star in stars)
            {
                star.Draw();
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,SamplerState.AnisotropicClamp,DepthStencilState.Default,RasterizerState.CullNone,null,Main.UIScaleMatrix);
        }
        float t = 0;
        public Vector2 scale => new Vector2(Main.UIScale * Main.screenWidth / 1920f, Main.UIScale * Main.screenHeight / 1080f);
        public void DrawBackGround()//天、星月
        {
            float rotSpeed = 0.0007f;
            SpriteBatch spriteBatch = Main.spriteBatch;
            t++;
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

            Texture2D tex = Terraria.GameContent.TextureAssets.MagicPixel.Value;
            tex = ModAsset.bg.Value;
            spriteBatch.Draw(tex, scale.X*new Vector2(960, 820), null, Color.White, t * rotSpeed, tex.Size() / 2, scale.X, 0, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            Update();

            DrawStars();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

            Texture2D moon = ModAsset.Moon.Value;
            //Vector2 moonVec = new Vector2(872, 628) - new Vector2(1250, 1250);
            Vector2 moonVec = new Vector2(680, 920) - new Vector2(1250, 1250);
            spriteBatch.Draw(moon, scale.X*(new Vector2(960, 820) + moonVec.RotatedBy(t * rotSpeed)), null, Color.White, t * rotSpeed, moon.Size() / 2, 1.05f*scale.X, 0, 0);

            Main.spriteBatch.End();
        }
        RenderTarget2D bg;
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Main.spriteBatch.End();

            var renderTargets = Ins.RenderTargetPool.GetRenderTarget2DArray(1);
            bg = renderTargets.Resource[0];

            Main.graphics.GraphicsDevice.SetRenderTarget(bg);//在rt上绘制背景，方便水反
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            
            DrawBackGround();

            Main.graphics.GraphicsDevice.SetRenderTarget(null);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            Main.spriteBatch.Draw(bg,new Rectangle(0,0,Main.screenWidth,Main.screenHeight),Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);


            //绘制水以及反射的天空
            Texture2D water = ModAsset.water.Value;
            spriteBatch.Draw(water, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White*0.95f);
            Rectangle rec = new Rectangle(864,729,592,168);
            rec.X = (int)(rec.X * Main.UIScale * Main.screenWidth /1920f);
            rec.Y = (int)(rec.Y * Main.UIScale * Main.screenHeight /1080f);
            Vector2 size = new Vector2(bg.Width, bg.Height);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
			Vertex2D[] vertex = new Vertex2D[] {
            new Vertex2D(new Vector2(rec.X,rec.Y),Color.White*0.7f,new Vector3(rec.X/size.X,rec.Y/size.Y,0)),
            new Vertex2D(new Vector2(rec.X,rec.Y+rec.Height),Color.White*0.2f,new Vector3(rec.X/size.X,(rec.Y-rec.Height)/size.Y,0)),
            new Vertex2D(new Vector2(rec.X+rec.Width,rec.Y),Color.White*0.7f,new Vector3((rec.X+rec.Width)/size.X,rec.Y/size.Y,0)),
            new Vertex2D(new Vector2(rec.X+rec.Width,rec.Y+rec.Height),Color.White*0.2f,new Vector3((rec.X+rec.Width)/size.X,(rec.Y-rec.Height)/size.Y,0))
            };

            Main.graphics.GraphicsDevice.Textures[0] = bg;

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip,vertex,0,2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            //绘制前置两层
            Texture2D front2 = ModAsset.front2.Value;
            spriteBatch.Draw(front2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            Texture2D front1 = ModAsset.front1.Value;
            spriteBatch.Draw(front1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            //TODO 绘制标题字
            // Texture2D front0 = ModAsset.上层.Value;
            // spriteBatch.Draw(front0, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);


            bg = null;
            renderTargets.Release();
            return false;
        }
    }
}
