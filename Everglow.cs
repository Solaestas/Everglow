using Everglow.Sources.Commons;
using ReLogic.Content.Sources;
using Terraria.ModLoader;

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

        public ModuleManager ModuleManager
        {
            get
            {
                return m_moduleManager;
            }
        }

        private static Everglow m_instance;

        private ModuleManager m_moduleManager;


        public Everglow()
        {
        }

        public override void Load()
        {
            m_instance = this;

            m_moduleManager = new ModuleManager();
            m_moduleManager.LoadAll();
        }

        public override void Unload()
        {
            m_moduleManager.UnloadAll();

            m_instance = null;
        }
    }
}