using Everglow.Commons;
using Everglow.Commons.Interfaces;
using Everglow.Commons.Modules;
using Everglow.Commons.Network.PacketHandle;
using Everglow.Commons.ObjectPool;
using Everglow.Commons.VFX;
using log4net;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Everglow;

[NoJIT]
public class Everglow : Mod
{
	public static event Action OnPostSetupContent;

	private PacketResolver m_packetResolver;

	public override void Load()
	{
		ModIns.Mod = this;

		Ins.Set<ILog>(Logger);
		Ins.Set<GraphicsDevice>(Main.instance.GraphicsDevice);
		Ins.Set<IVisualQualityController>(new VisualQualityController());
		Ins.Set<ModuleManager>(new ModuleManager());
		foreach (var config in Ins.ModuleManager.CreateInstances<ModConfig>())
		{
			AddConfig(config.Name, config);
		}
		foreach (var ins in Ins.ModuleManager.CreateInstances<ILoadable>())
		{
			AddContent(ins);
		}
		Ins.Set<IMainThreadContext>(new MainThreadContext());
		Ins.Set<RenderTargetPool>(Main.netMode != NetmodeID.Server ? new RenderTargetPool() : null);
		Ins.Set<VFXBatch>(Main.netMode != NetmodeID.Server ? new VFXBatch() : null);
		Ins.Set<IVFXManager>(Main.netMode != NetmodeID.Server ? new VFXManager() : new FakeManager());

		m_packetResolver = new PacketResolver(this);
	}

	public override void PostSetupContent()
	{
		OnPostSetupContent?.Invoke();
	}

	public override void Unload()
	{
		m_packetResolver = null;
		ModIns.Mod = null;

		Ins.Clear();
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		m_packetResolver.Resolve(reader, whoAmI);
	}
}