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
	public struct Layer : IUniqueID<string>
	{
		public Layer(string layerName, string texturePath)
		{
			UniqueID = layerName;
			Texture = ModContent.Request<Texture2D>(TexturePath = texturePath);
		}
		public readonly string UniqueID { get; }
		public string TexturePath;
		public Asset<Texture2D> Texture;
		public Vector3 Position;
		public int MaxFrameCount;
		public int FrameInterval;
		public bool HorizontalFrame;
		int currentFrame;
		int frameCounter;
		public bool PrePareDraw(out Texture2D texture, bool immediate = true)
		{
			switch (Texture.State)
			{
				case AssetState.Loaded:
					texture = Texture.Value;
					return true;
				case AssetState.Loading:
					texture = null;
					return false;
				case AssetState.NotLoaded:
					Texture = ModContent.Request<Texture2D>(TexturePath, immediate ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad);
					texture = Texture.Value;
					return immediate;
				default:
					texture = null;
					return false;
			}
		}
		public void Draw(SpriteBatch sprite, Vector3 CameraPos, bool waitLoad)
		{
			if (!PrePareDraw(out Texture2D texture, !waitLoad))
			{
				return;
			}

			int frameWidth = texture.Width;
			int frameHeight = texture.Height;
			if (HorizontalFrame)
			{
				frameWidth /= MaxFrameCount;
			}
			else
			{
				frameHeight /= MaxFrameCount;
			}

			float f = Position.Z / CameraPos.Z;

			int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPos.X * f);
			int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPos.X * f);
			int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPos.Y * f);
			int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPos.Y * f);

			Rectangle r1 = new((int)Position.X, (int)Position.Y, frameWidth, frameHeight);
			Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
			Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

			if (r3.Width == 0 || r3.Height == 0)
			{
				return;
			}

			float f2 = CameraPos.Z / (CameraPos.Z - Position.Z);
			Rectangle r4 = new((int)(CameraPos.X + (r3.X - CameraPos.X) * f2),
				(int)(CameraPos.Y + (r3.Y - CameraPos.Y) * f2),
				(int)(r3.Width * f2),
				(int)(r3.Height * f2));

			if (++frameCounter >= FrameInterval)
			{
				frameCounter = 0;
				if (++currentFrame >= MaxFrameCount)
				{
					currentFrame = 0;
				}
			}

			sprite.Draw(texture,
				new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
				new Rectangle((int)(r3.X - Position.X) + (HorizontalFrame ? currentFrame : 0) * frameWidth, (int)(r3.Y - Position.Y) + (HorizontalFrame ? 0 : currentFrame) * frameHeight, r3.Width, r3.Height),
				Color.White);
		}
	}
}