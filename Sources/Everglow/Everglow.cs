using System.Reflection;
using Everglow.Commons;
using Everglow.Commons.Hooks;
using Everglow.Commons.Interfaces;
using Everglow.Commons.Modules;
using Everglow.Commons.Network.PacketHandle;
using Everglow.Commons.ObjectPool;
using Everglow.Commons.TileHelper;
using Everglow.Commons.VFX;
using log4net;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Everglow;

public class Everglow : Mod
{
	private PacketResolver m_packetResolver;

	public override void Load()
	{
		ModIns.Mod = this;

		AddServices();
		AddContents();
		ShakeTreeTweak.Load();
		m_packetResolver = new PacketResolver(this);
	}

	private void AddServices()
	{
		Ins.Begin();
		Ins.Add<ILog>(Logger);
		Ins.Add<GraphicsDevice>(Main.instance.GraphicsDevice);
		Ins.Add<IVisualQualityController, VisualQualityController>();
		Ins.Add<ModuleManager>();
		Ins.Add<IHookManager, HookManager>();
		Ins.Add<IMainThreadContext, MainThreadContext>();
		if (Main.netMode != NetmodeID.Server)
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
		m_packetResolver = null;
		ModIns.Mod = null;
		Ins.Clear();
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		m_packetResolver.Resolve(reader, whoAmI);
	}
}