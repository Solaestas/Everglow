using Everglow.Sources.Commons.ModuleSystem;
using Everglow.Sources.Commons.Network.PacketHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZY.ZYPacket
{
    internal class PacketCache : IModule
    {
        public string Name => "PacketCache";
        public static Dictionary<Type, IZYPacket> packets;
        public void Load()
        {
            packets = new Dictionary<Type, IZYPacket>();
            foreach(var type in from t in Assembly.GetExecutingAssembly().GetTypes() where t.GetInterfaces().Contains(typeof(IZYPacket)) select t)
            {
                packets.Add(type, Activator.CreateInstance(type) as IZYPacket);
            }
        }

        public void Unload()
        {
            packets = null;
        }
    }

    internal static class PacketResolverExtension
    {
        public static void Send<T>(this PacketResolver resolver, int toClient = -1, int ignoreClient = -1)
        {
            resolver.Send(PacketCache.packets[typeof(T)], toClient, ignoreClient);
        }
    }
}
