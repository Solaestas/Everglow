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

        private static Everglow m_instance;
        public Everglow()
        {
        }

        public override void Load()
        {
            m_instance = this;
        }

        public override void Unload()
        {
            m_instance = null;
        }
    }
}