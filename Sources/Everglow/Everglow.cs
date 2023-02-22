global using System;
global using System.IO;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
using Everglow.Common;
using Everglow.Common.Enums;
using Everglow.Common.Hooks;
using Everglow.Common.Interfaces;
using Everglow.Common.ModuleSystem;
using Everglow.Common.Network.PacketHandle;
using Everglow.Common.ObjectPool;
using Everglow.Common.VFX;
using Humanizer;

namespace Everglow;

public class Everglow : Mod
{
	public static event Action OnPostSetupContent;

	private PacketResolver m_packetResolver;

	public override void Load()
	{
		Ins.SetInstance(
			Logger,
			Main.instance.GraphicsDevice,
			new VisualQualityController(),
			ModContent.GetInstance<HookManager>(),
			new ModuleManager(),
			new MainThreadContext(),
			Main.netMode != NetmodeID.Server ? new RenderTargetPool() : null,
			Main.netMode != NetmodeID.Server ? new VFXBatch() : null,
			Main.netMode != NetmodeID.Server ? new VFXManager() : new FakeManager());
		ModIns.SetInstance(this);
		m_packetResolver = new PacketResolver(this);
		Ins.ModuleManager.LoadAllModules();
	}

	public override void PostSetupContent()
	{
		OnPostSetupContent?.Invoke();
	}

	public override void Unload()
	{
		Ins.ModuleManager.UnloadAllModules();
		Ins.DisposeAll();
		ModIns.DisposeAll();
		m_packetResolver = null;
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		m_packetResolver.Resolve(reader, whoAmI);
	}
}