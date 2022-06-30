global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Network.PacketHandle;
using Everglow.Sources.Commons.Core.Profiler;
using Everglow.Sources.Commons.Core.Profiler.Fody;

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
        /// 获取 ProfilerManager 实例
        /// </summary>
        internal static ProfilerManager ProfilerManager
        {
            get
            {
                return Instance.m_profilerManager;
            }
        }

        /// <summary>
        /// 获取HookSystem实例
        /// </summary>
        internal static HookSystem HookSystem
        {
            get
            {
                return ModContent.GetInstance<HookSystem>();
            }
        }

        internal static MainThreadContext MainThreadContext
        {
            get
            {
                return Instance.m_mainThreadContext;
            }
        }

        private static Everglow m_instance;

        private ModuleManager m_moduleManager;
        private PacketResolver m_packetResolver;
        private ProfilerManager m_profilerManager;
        private MainThreadContext m_mainThreadContext;

        public Everglow()
        {
            m_instance = this;

            // 必须手动确定顺序
            m_profilerManager = new ProfilerManager();
            m_mainThreadContext = new MainThreadContext();
            m_moduleManager = new ModuleManager();
            m_packetResolver = new PacketResolver();
        }

        public override void Load()
        {
            m_mainThreadContext.Load();
            HookSystem.HookLoad();
            m_moduleManager.LoadAllModules();
        }


        public override void AddRecipes()
        {
            base.AddRecipes();
        }

        public override void Unload()
        {
            m_moduleManager.UnloadAllModules();
            HookSystem.HookUnload();
            m_mainThreadContext.Unload();

            m_profilerManager.Clear();

            m_packetResolver = null;
            m_moduleManager = null;
            m_instance = null;
            m_mainThreadContext = null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            m_packetResolver.Resolve(reader, whoAmI);
        }
    }
}