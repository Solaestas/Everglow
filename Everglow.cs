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
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Network.PacketHandle;

namespace Everglow
{
    public class Everglow : Mod
    {
        /// <summary>
        /// Get the instance of Everglow
        /// </summary>
        public static Everglow Instance
        {
            get { return m_instance; }
        }

        /// <summary>
        /// 获取 ModuleManager 实例
        /// </summary>
        public static ModuleManager ModuleManager
        {
            get { return Instance.m_moduleManager; }
        }

        /// <summary>
        /// 获取 PacketResolver 实例
        /// </summary>
        public static PacketResolver PacketResolver
        {
            get { return Instance.m_packetResolver; }
        }

        private static Everglow m_instance;

        private ModuleManager m_moduleManager = new ModuleManager();
        private PacketResolver m_packetResolver = new PacketResolver();

        public Everglow()
        {
        }

        public override void Load()
        {
            m_instance = this;
            m_moduleManager.LoadAllModules();
        }

        public override void Unload()
        {
            m_moduleManager.UnloadAllModules();

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