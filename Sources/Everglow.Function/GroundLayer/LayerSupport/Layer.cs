using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.GroundLayer.Basics;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Initializers;
using Terraria.ModLoader;

namespace Everglow.Commons.GroundLayer.LayerSupport
{
	public class Layer(string layerName, string texturePath, Vector3 position, Point size, Point frameSize, bool horizontal = true, int frameMaxCount = 1, int frameInterval = int.MaxValue) : IUniqueID<string>
	{
		public string UniqueID { get; } = layerName;
		public Vector3 Position = position;
		Asset<Texture2D> textureAsset = ModContent.Request<Texture2D>(texturePath);
		int frameIndex;
		int frameTimer;
		int disposeTimer;
		bool PrepareDraw(out Texture2D texture,bool immediate)
		{
			switch (textureAsset.State)
			{
				default:
				case AssetState.NotLoaded:
					{
						textureAsset = ModContent.Request<Texture2D>(texturePath, immediate ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad);
						texture = null;
						return false;
					}
				case AssetState.Loaded:
					{
						texture = textureAsset.Value;
						return true;
					}
				case AssetState.Loading:
					{
						if(immediate)
						{
							while (!textureAsset.IsLoaded)
								;
							texture=textureAsset.Value;
							return true;
						}
						texture = null; 
						return false;
					}
			}
		}
		public void DoDraw(SpriteBatch sprite, Vector3 CameraPos, bool waitLoad,Color color)
		{
			float f = Position.Z / CameraPos.Z;

			int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPos.X * f);
			int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPos.X * f);
			int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPos.Y * f);
			int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPos.Y * f);

			Rectangle r1 = new((int)Position.X, (int)Position.Y, size.X, size.Y);
			Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
			Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

			if (r3.Width == 0 || r3.Height == 0)
			{
				return;
			}

			if(!PrepareDraw(out var texture,!waitLoad))
			{
				if (textureAsset.IsLoaded)
				{
					if (++disposeTimer > 1200)
					{
						disposeTimer = 0;
						textureAsset.Dispose();
					}
				}
				return;
			}

			if (++frameTimer >= frameInterval)
			{
				frameTimer = 0;
				if (++frameIndex >= frameMaxCount)
				{
					frameIndex = 0;
				}
			}

			float f2 = CameraPos.Z / (CameraPos.Z - Position.Z);
			Rectangle r4 = new((int)(CameraPos.X + (r3.X - CameraPos.X) * f2 - Main.screenPosition.X),
				(int)(CameraPos.Y + (r3.Y - CameraPos.Y) * f2 - Main.screenPosition.Y),
				(int)(r3.Width * f2),
				(int)(r3.Height * f2));

			Rectangle r5 = new Rectangle((int)(r3.X - Position.X) + frameIndex * (horizontal ? 1 : 0) * frameSize.X,
				(int)(r3.Y - Position.Y) + frameIndex * (horizontal ? 0 : 1) * frameSize.Y,
				(int)(r3.Width * (frameSize.X / (float)size.X)),
				(int)(r3.Height * (frameSize.Y / (float)size.Y)));



			Draw(sprite, texture, r4, r5, color);
		}
		public virtual void Draw(SpriteBatch sprite,Texture2D texture,Rectangle screenTargetArea, Rectangle drawClippingArea, Color color)
		{
			sprite.Draw(texture,
				screenTargetArea,
				drawClippingArea,
				Color.White);
			sprite.Draw(texture,
				new Vector2(screenTargetArea.X, screenTargetArea.Y),
				drawClippingArea,
				color,
				0,
				Vector2.Zero,
				new Vector2(drawClippingArea.Width / (float)screenTargetArea.Width, drawClippingArea.Height / (float)screenTargetArea.Height),
				SpriteEffects.None,
				0);
		}
	}
}