using System.IO.Pipes;
using System.Net;
using System.Threading.Tasks;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Everglow.Sources.Modules.SubWorldModule
{
    internal class SubserverSocket : ISocket
    {
        private readonly int index;

        public SubserverSocket(int index)
        {
            this.index = index;
        }

        void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state) { }

        void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
        {
            byte[] packet = new byte[size + 1];
            packet[0] = (byte)index;
            Buffer.BlockCopy(data, offset, packet, 1, size);
            Task.Factory.StartNew(SendToMainServerCallBack, packet);

            //string str = "W" + packet[3] + "(" + size + ") ";
            //for (int i = 0; i < size + 1; i++)
            //{
            //	str += packet[i] + " ";
            //}
            //ModContent.GetInstance<SubworldLibrary>().Logger.Info(str);
        }

        void ISocket.Close() { }

        void ISocket.Connect(RemoteAddress address) { }

        RemoteAddress ISocket.GetRemoteAddress() => new TcpAddress(new IPAddress(new byte[4]), 0); // TODO: 想办法以最简洁的方式获取实际地址，以备不时之需

        bool ISocket.IsConnected() => true;

        bool ISocket.IsDataAvailable() => true;

        void ISocket.SendQueuedPackets() { }

        bool ISocket.StartListening(SocketConnectionAccepted callback) => true;

        void ISocket.StopListening() { }

        private static void SendToMainServerCallBack(object data)
        {
            using NamedPipeClientStream pipe = new NamedPipeClientStream(".", SubworldSystem.current.FullName, PipeDirection.Out);
            pipe.Connect();
            pipe.Write((byte[])data);
        }
    }
}