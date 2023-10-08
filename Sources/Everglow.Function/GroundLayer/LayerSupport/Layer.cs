using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.GroundLayer.Basics;
using ReLogic.Content;
using Terraria.Initializers;
using Terraria.ModLoader;

namespace Everglow.Commons.GroundLayer.LayerSupport
{
	public struct Layer : IUniqueID<int>
	{
		static int nextID = int.MinValue;
		public Layer(string texturePath)
		{
			id = nextID++;
			Texture = ModContent.Request<Texture2D>(TexturePath = texturePath);
		}
		int id;
		public readonly int UniqueID => id;
		public string TexturePath;
		public Asset<Texture2D> Texture;
		public Vector3 Position;
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
	}
}