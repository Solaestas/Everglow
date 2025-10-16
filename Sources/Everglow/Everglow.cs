using System.Reflection;
using Everglow.Commons;
using Everglow.Commons.Hooks;
using Everglow.Commons.Interfaces;
using Everglow.Commons.Modules;
using Everglow.Commons.Netcode;
using Everglow.Commons.ObjectPool;
using Everglow.Commons.TileHelper;
using Everglow.Commons.VFX;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Everglow;

public class Everglow : Mod
{
	public override void Load()
	{
		ModIns.Mod = this;

		AddServices();
		AddContents();
		ShakeTreeTweak.Load();
		ModIns.PacketResolver = new PacketResolver(this);
	}

	private void AddServices()
	{
		Ins.Begin();
		Ins.Add(Logger);
		if(!Main.dedServ)
		{
			Ins.Add(Main.instance.GraphicsDevice);
			Ins.Add(Main.spriteBatch);
		}
		Ins.Add<IVisualQualityController, VisualQualityController>();
		Ins.Add<ModuleManager>();
		Ins.Add<IHookManager, HookManager>();
		Ins.Add<IMainThreadContext, MainThreadContext>();
		if (!Main.dedServ)
		{
			Ins.Add<RenderTargetPool>();
			Ins.Add<IVFXManager, VFXManager>();
			Ins.Add<VFXBatch>();
		}
		else
		{
			Ins.Add<IVFXManager, FakeManager>();
		}
		Ins.End();
	}

	private void AddContents()
	{
		foreach (var config in Ins.ModuleManager.CreateInstances<ModConfig>())
		{
			var name = config.GetType().Name;
			config.Mod = this;
			if (config.Mode == ConfigScope.ServerSide && (Side == ModSide.Client || Side == ModSide.NoSync)) // Client and NoSync mods can't have ServerSide ModConfigs. Server can, but won't be synced.
			{
				throw new Exception($"The ModConfig {config.Name} can't be loaded because the config is ServerSide but this Mods ModSide isn't Both or Server");
			}

			if (config.Mode == ConfigScope.ClientSide && Side == ModSide.Server) // Doesn't make sense.
			{
				throw new Exception($"The ModConfig {config.Name} can't be loaded because the config is ClientSide but this Mods ModSide is Server");
			}

			if (config.Autoload(ref name))
			{
				AddConfig(name, config);
			}
		}
		foreach (var ins in Ins.ModuleManager.CreateInstances<ILoadable>(type =>
		{
			if (type.IsSubclassOf(typeof(ModGore)))
			{
				return false;
			}
			var attr = type.GetCustomAttributes<AutoloadAttribute>().FirstOrDefault();
			return attr == null || attr.NeedsAutoloading;
		}))
		{
			AddContent(ins);
		}
	}

	public override void PostSetupContent()
	{
		ModIns.PostSetupContent();
	}

	public override void Unload()
	{
		ModIns.Unload();
		ModIns.PacketResolver = null;
		ModIns.Mod = null;
		Ins.Clear();
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI) => ModIns.PacketResolver.Resolve(reader, whoAmI);
}