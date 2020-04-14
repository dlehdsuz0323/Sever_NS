using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using UnityEngine;

namespace Server
{
    public class LocalClient
    {
        public string LocalIp;
        //private const string LocalIp = "192.168.11.11";
        public const int PortNumber = 20002;
        private const int BufferSize = 4096;
        public bool m_isConnected = false;
        private NetworkStream _stream;
        public NetworkStream Stream
        {
            get { return _stream; }
        }
        private readonly BinaryFormatter _formatter;
        private TcpClient _client;
        private byte[] _buffer;
        public string UserName { get; set; }
        //public event Connected OnConnected;
        //public event Disconnected OnDisconnected;
        public event ConnectError OnConnectError;
        //public event ReceiveObject OnReceiveObject;
        /// <summary>
        /// 클라이언트용 생성자
        /// </summary>
        public LocalClient() : this(null)
        {
        }
        /// <summary>
        /// 서버용 생성자
        /// </summary>
        /// <param name="client">이미 생성된 TcpClient</param>

        public LocalClient(TcpClient client)
        {     
            _client = client;
            _formatter = new BinaryFormatter();
            _formatter.Binder = new PacketBinder();
            _buffer = new byte[BufferSize];
        }

        public bool Start()
        {         
            if (_client != null)
            {
                _stream = _client.GetStream();
                //서버 -> 클라이언트
                BeginRead();
                return false;
            }
            else
            {
                //클라이언트 -> 서버
                StartClient();
                return true;
            }
        }
        private void StartClient()
        {
            //클라이언트 -> 서버
            
            _client = new TcpClient();
            try // 연결요청
            {
                Connect();
            }
            catch (SocketException ex)
            {
                //if (OnConnectError != null)
                //    OnConnectError(ex);
                Debug.Log(ex);
            }
        }
        public void Connect()
        {
            SimpleDebug.ShowDebugMessage("연결 중", "LocalClient");
            m_isConnected = true;
            _client.BeginConnect(LocalIp, PortNumber, EndConnect, null);
        }
        private void EndConnect(IAsyncResult result) // 연결응답
        {
            try
            {        
                SimpleDebug.ShowDebugMessage("연결 응답 종료", "LocalClient");
                m_isConnected = false;
                _client.EndConnect(result);
                _stream = _client.GetStream();
                ServerManager.instance.Connected();
                BeginRead();
            }
            catch (SocketException ex)
            {
                if (OnConnectError != null)
                    OnConnectError(ex);
            }
        }
        private void BeginRead() // 데이터 받을 준비
        {               
            _stream.BeginRead(_buffer, 0, BufferSize, new AsyncCallback(ReadObject), null);
        }
        public void Close()
        {
            _stream.Close();    
            _client.Close();
        }
        private void ReadObject(IAsyncResult result) // 받은 데이터 처리
        {         
            int readSize = 0;
            try
            {
                lock (_stream)
                {
                    readSize = _stream.EndRead(result);
                }
                if (readSize < 1)
                    throw new Exception("Disconnect");
                ServerManager.instance.ReceiveObject(this, _buffer); // 서버매니저로 데이터 옮기고 호출대기
                lock (_stream)
                {
                    BeginRead();
                }
            }
            catch (Exception ex)
            {
                ServerManager.instance.ReConnect();
                Debug.Log(ex);
            }       
        }
        public void SendPacket(byte[] packet) // 데이터 전송
        {
            //byte[] data = null;
            try
            {
                if (packet == null || packet.Length < 1)
                    return;
                int index = 0;
                _stream.Write(packet, 0, Converter.GetShort(packet, ref index));
                _stream.Flush();
            }
            catch
            {
                ServerManager.instance.ReConnect();
                Debug.Log("send error");
            }//try
        }
    }
    public delegate void Connected(LocalClient client);
    public delegate void Disconnected(LocalClient client, Exception ex);
    public delegate void ConnectError(SocketException ex);
    public delegate void ReceiveObject(LocalClient client, byte[] haader);
    sealed class PacketBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type returntype = null;
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            string sharedAssemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            typeName = typeName.Replace(sharedAssemblyName, assemblyName);
            returntype = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
            return returntype;
        }
    }
}
