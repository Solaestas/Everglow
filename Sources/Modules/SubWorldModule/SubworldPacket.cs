using Everglow.Sources.Commons.Core.Network.PacketHandle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.SubWorldModule
{
    internal class SubworldPacket : IPacket
    {
        int packettype;
        ushort message;
        public SubworldPacket(int type, ushort message,int toclient = -1, int ignore = -1)
        {
            packettype = type;
            this.message = message;
            Everglow.PacketResolver.Send(this, toclient, ignore);
        }
        public void Receive(BinaryReader reader, int whoAmI)
        {
            SubworldLibrary.HandlePacket(reader, whoAmI);
        }
        public void Send(BinaryWriter writer)
        {
            writer.Write(packettype);
            writer.Write(message);
        }
    }
}
