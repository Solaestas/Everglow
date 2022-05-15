using Everglow.Sources.Commons.Network.PacketHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Network.Packets
{
    public class TestPacket : IPacket
    {
        public TestPacket()
        {

        }
        public void Receive(BinaryReader reader, int whoAmI)
        {
            throw new NotImplementedException();
        }

        public void Send(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
