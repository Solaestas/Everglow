//using System.IO.Pipes;
//using System.Net;
//using System.Threading.Tasks;
//using Terraria.Net;
//using Terraria.Net.Sockets;

//namespace Everglow.Sources.Modules.SubWorldModule
//{
//    public class SubserverSocket : ISocket
//    {
//        public SubserverSocket(int index)
//        {
//            this.index = index;
//        }
//        void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state = null)
//        {
//        }
//        void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state = null)
//        {
//            byte[] array = new byte[size + 1];
//            array[0] = (byte)index;
//            Buffer.BlockCopy(data, offset, array, 1, size);
//            Task.Factory.StartNew(new Action<object>(SendToMainServerCallBack), array);
//        }
//        void ISocket.Close()
//        {
//        }
//        void ISocket.Connect(RemoteAddress address)
//        {
//        }
//        RemoteAddress ISocket.GetRemoteAddress()
//        {
//            return new TcpAddress(new IPAddress(new byte[4]), 0);
//        }
//        bool ISocket.IsConnected()
//        {
//            return true;
//        }
//        bool ISocket.IsDataAvailable()
//        {
//            return true;
//        }
//        void ISocket.SendQueuedPackets()
//        {
//        }
//        bool ISocket.StartListening(SocketConnectionAccepted callback)
//        {
//            return true;
//        }
//        void ISocket.StopListening()
//        {
//        }
//        private static void SendToMainServerCallBack(object data)
//        {
//            using (NamedPipeClientStream namedPipeClientStream = new(".", SubworldSystem.current.FullName, PipeDirection.Out))
//            {
//                namedPipeClientStream.Connect();
//                namedPipeClientStream.Write((byte[])data);
//            }
//        }
//        private readonly int index;
//    }
//}