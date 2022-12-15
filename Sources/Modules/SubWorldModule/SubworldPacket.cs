//using Everglow.Sources.Commons.Core.Network.PacketHandle;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Everglow.Sources.Modules.SubWorldModule
//{
//    internal class SubworldPacket : IPacket
//    {
//        MemoryStream stream;
//        BinaryWriter writer;
//        public SubworldPacket()
//        {
//            stream = new();
//            writer = new(stream);
//        }
//        public void Receive(BinaryReader reader, int whoAmI)
//        {
//            SubworldLibrary.HandlePacket(reader, whoAmI);
//        }
//        public void Send(BinaryWriter writer)
//        {
//            writer.Write(stream.ToArray());
//        }
//        public SubworldPacket Write(short value) { writer.Write(value); return this; }
//        public SubworldPacket Write(ushort value) { writer.Write(value); return this; }
//        public SubworldPacket Write(int value) { writer.Write(value); return this; }
//        public SubworldPacket Write(uint value) { writer.Write(value); return this; }
//        public SubworldPacket Write(float value) { writer.Write(value); return this; }
//        public SubworldPacket Write(double value) { writer.Write(value); return this; }
//        public SubworldPacket Write(long value) { writer.Write(value); return this; }
//        public SubworldPacket Write(ulong value) { writer.Write(value); return this; }
//        public SubworldPacket Write(string value) { writer.Write(value); return this; }
//        public SubworldPacket Write(char value) { writer.Write(value); return this; }
//        public SubworldPacket Write(char[] value) { writer.Write(value); return this; }
//        public SubworldPacket Write(byte value) { writer.Write(value); return this; }
//        public SubworldPacket Write(byte[] value) { writer.Write(value); return this; }
//        public void Send(int toclient = -1, int ignore = -1) => Everglow.PacketResolver.Send(this, toclient, ignore);
//    }
//}
