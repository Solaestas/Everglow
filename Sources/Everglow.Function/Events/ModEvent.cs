using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Events
{
	public abstract class ModEvent : ModType
	{
		public sealed override void Register()
		{
			EventManager.Register(this);
		}

		public float SortRank = 1;
		public bool Active { get; internal set; }

		public virtual string DefName => FullName;
		public virtual bool IsBackground => false;

		public virtual void Update()
		{

		}
		public virtual bool CanActivate(params object[] args)
		{
			return !Active;
		}
		public virtual bool CanDeactivate(params object[] args)
		{
			return Active;
		}
		public virtual void OnActivate(params object[] args)
		{

		}
		public virtual void OnDeactivate(params object[] args)
		{

		}
		public virtual void Draw(SpriteBatch sprite)
		{

		}
		public virtual void SaveData(TagCompound tag)
		{

		}
		public virtual void LoadData(string defName, TagCompound tag)
		{

		}
		public virtual ModEvent Clone()
		{
			return (ModEvent)MemberwiseClone();
		}
	}
}