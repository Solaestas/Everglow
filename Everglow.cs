global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Mono.Cecil.Cil;
global using MonoMod.Cil;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using Terraria;
global using Terraria.DataStructures;
global using Terraria.GameContent;
global using Terraria.ID;
global using Terraria.ModLoader;
global using ReLogic.Content;

using Everglow.Sources.Commons.ModuleSystem;

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
        /// »ñÈ¡ ModuleManager ÊµÀý
        /// </summary>
        public static ModuleManager ModuleManager
        {
            get { return Instance.m_moduleManager; }
        }

        private static Everglow m_instance;

        private ModuleManager m_moduleManager;

        public Everglow()
        {
        }

        public override void Load()
        {
            m_instance = this;
            Main.graphics.
            m_moduleManager = new ModuleManager();
        }

        public override void Unload()
        {
            m_moduleManager.UnloadAllModules();
            m_instance = null;
        }
    }
}