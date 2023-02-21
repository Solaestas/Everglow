global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
using Everglow.Core.Network.PacketHandle;
using Everglow.Core;
using Everglow.Core.ModuleSystem;
using Everglow.Core.ObjectPool;

namespace Everglow
{
	public class Everglow : Mod
    {
        /// <summary>
        /// Get the instance of Everglow
        /// </summary>
        public static Everglow Instance
        {
            get
            {
                return m_instance;
            }
        }

        /// <summary>
        /// 获取 ModuleManager 实例
        /// </summary>
        public static ModuleManager ModuleManager
        {
            get
            {
                return Instance.m_moduleManager;
            }
        }

        /// <summary>
        /// 获取 PacketResolver 实例
        /// </summary>
        public static PacketResolver PacketResolver
        {
            get
            {
                return Instance.m_packetResolver;
            }
        }

        /// <summary>
        /// 获取HookSystem实例
        /// </summary>
        public static HookSystem HookSystem
        {
            get
            {
                return ModContent.GetInstance<HookSystem>();
            }
        }

        public static RenderTargetPool RenderTargetPool
        {
            get
            {
                return Instance.m_renderTargetPool;
            }
        }

        private static Everglow m_instance;
        public static event Action OnPostSetupContent;

        private ModuleManager m_moduleManager;
        private PacketResolver m_packetResolver;
        private RenderTargetPool m_renderTargetPool;
        public Everglow()
        {
            m_instance = this;

            m_moduleManager = new ModuleManager();

            if (Main.netMode != NetmodeID.Server)
            {
                m_renderTargetPool = new RenderTargetPool();
            }

            m_packetResolver = new PacketResolver();
        }

        public override void Load()
        {
            HookSystem.HookLoad();
            m_moduleManager.LoadAllModules();
        }

        public override void PostSetupContent()
        {
			OnPostSetupContent?.Invoke();
		}

        public override void Unload()
        {
            m_moduleManager.UnloadAllModules();
            HookSystem.HookUnload();


            m_packetResolver = null;
            m_moduleManager = null;
            m_instance = null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            m_packetResolver.Resolve(reader, whoAmI);
        }
    }
}