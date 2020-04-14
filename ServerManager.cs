using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.SceneManagement;

namespace Server
{
    public enum E_CODE : short
    {
        TEST = 1001,
        REQ_REEL = 2001,
        REEL_DATA = 2002,
        SPIN = 3001,
        SPIN_DATA = 3002,
        SELECT_FREE_SPIN = 3011,
        RECV_FREE_SPIN = 3012,
        JOIN_GAME = 4001,
        RECV_JOIN_GAME = 4002,
        PING = 4201,
        RECV_PING = 4202,
        EXIT_GAME = 4011,
        ROOM_DATA = 4101,
        RECV_ROOM_DATA = 4102,
        ACCUONT = 5001,
        RECV_ACCOUNT = 5002,
        TEST_ACCOUNT = 5011,
        RECV_TEST_ACCOUNT = 5012,
    }
    public class ServerManager : MonoSingleton<ServerManager>
    {
        private const string UserName = "Demo";
        public LocalClient _client;
        public bool m_isConnectServer = false;
        private bool m_isSend = false;
        public bool m_isSetting = false;

        private bool m_isQuit = false;
        [HideInInspector]
        public StringBuilder m_strBulider = new StringBuilder();
        public short m_lastGameCode;
        private Coroutine m_pingCorutine;
        // 데이터
        private bool m_bConnectServer = false; // 서버 접속 상태
        // 계정
        Packet.OnRecvTestAccount m_testAccountData = null; // 테스트 계정 패킷
        Packet.OnRecvAccount m_accountData = null; // 로그인 패킷
        Packet.OnRecvJoinGame m_joinGameData = null; // 게임 정보 패킷
        // 로비
        Packet.OnRecvRoomData m_roomData = null; // 방 정보 패킷
        // 게임
        Packet.OnRecvReelData m_reelData = null; // 릴 데이터 패킷
        Packet.OnRecvSpinData m_spinData = null; // 스핀 데이터 패킷
        Packet.OnRecvFreeSpin m_freeSpinData = null; // 프리스핀 데이터 패킷
        // 전체합산, 프리게임 따로 확률
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _client = new LocalClient();
            _client.LocalIp = WebServer.instance.m_userData.ip;
            _client.Start();
            StartCoroutine(SecondUpdate());
        }
        public void Init()
        {
            gameObject.name = "ServerManager";
        }
        public void Close()
        {
            if (!m_isConnectServer)
                return;
            StopAllCoroutines();
            SendPacket(new Packet.OnExitGame(m_lastGameCode).Make());
            _client.Close();
        }
        public void Connected() // 서버에 접속 완료
        {
            m_isConnectServer = true;
            m_bConnectServer = true;
        }
        public void DisConnect()
        {
            Close();
        }
        private void ConnectError(Exception ex) // 접속 에러
        {
            OptionManager.instance.m_disConnectText.SetActive(true);
            Debug.Log("접속 에러\n" + ex.ToString());
        }
        public void ReceiveObject(LocalClient client, byte[] header)
        {
            int index = 2;
            E_CODE code = (E_CODE)Converter.GetShort(header, ref index);
            short subCode = Converter.GetShort(header, ref index);
            short gameCode = Converter.GetShort(header, ref index);
            short errorCode = Converter.GetShort(header, ref index); // 에러코드
            if(errorCode == 1 || errorCode == 2)
                Debug.LogError("Type " + (short)code + " ErrorCode : " + errorCode);
            else
                Debug.Log("Type " + (short)code + " Not Error");

            switch (code) // 유니티 단일 스레드 코루틴으로 패킷 처리
            {
                case E_CODE.REEL_DATA:
                    m_reelData = new Packet.OnRecvReelData(header);            
                    break;
                case E_CODE.SPIN_DATA:                 
                    m_spinData = new Packet.OnRecvSpinData(header);
                    break;
                case E_CODE.RECV_JOIN_GAME:
                    m_joinGameData = new Packet.OnRecvJoinGame(header);
                    break;
                case E_CODE.RECV_FREE_SPIN:
                    m_freeSpinData = new Packet.OnRecvFreeSpin(header);
                    break;
                case E_CODE.RECV_ROOM_DATA:
                    m_roomData = new Packet.OnRecvRoomData(header);
                    break;
                case E_CODE.RECV_ACCOUNT:
                    m_accountData = new Packet.OnRecvAccount(header);
                    break;
                case E_CODE.RECV_TEST_ACCOUNT:
                    m_testAccountData = new Packet.OnRecvTestAccount(header);
                    break;
                default:
                    break;
            }
        }
        // 0 정상 1 돈 부족 2 규칙위반
        public void SendPacket(LocalClient client, byte[] header) // 패킷 전송 클라이언트 정보
        {
            client.SendPacket(header);
        }
        public void SendPacket(byte[] header) // 패킷 전송
        {
            if (!m_isConnectServer && !m_isQuit)
            {
                Debug.Log("장기 미 접속 서버 끊기");
                GameManager.instance.OutGame();
                Destroy(gameObject);
                WebServer.instance.GetLobby();
            }
            else
            {
                _client.SendPacket(header);
            }
        }
        public bool CheckConnect(byte[] header)
        {
            if (!m_isConnectServer) // 연결 안됬을때 패킷을 보냈을경우
            {               
                if (!_client.m_isConnected) // 서버 연결 중이 아닐때
                {
                    StartCoroutine(ReConnectServer());
                     // 재시작
                     // 로그인 재요청
                }
                else
                {
                    //StartCoroutine(ConnectWait(header));
                }
                return true;
            }

            return false;
        }
        IEnumerator Ping()
        {
            Debug.Log("Start Ping");
            yield return new WaitForSecondsRealtime(60);
            StartCoroutine(Ping());
            if (m_isSend)
            {
                m_isSend = false;
                yield break;
            }
            Debug.Log("SendPing");
            SendPacket(new Packet.OnPing(GameManager.instance.GameCode).Make());
        }

        public void ReConnect()
        {
            Debug.Log("서버 끊김");
            m_isQuit = true;
        }
        IEnumerator ReConnectServer() // 서버 재접속
        {
            Debug.Log("서버 재접속");
            yield return new WaitForSeconds(2.0f);
            GameManager.instance.OutGame();
            Destroy(gameObject);
            WebServer.instance.GetLobby();
        }

        IEnumerator SecondUpdate() // 서버 패킷 처리
        {
            while(true)
            {
                //if (recvData && m_pingCorutine != null)
                //    StopCoroutine(m_pingCorutine);
                if (m_bConnectServer)
                {
                    GameManager.instance.m_isGame = true;
                    m_lastGameCode = GameManager.instance.GameCode;
                    if (!GameManager.instance.m_isDebug)
                        SendPacket(new Packet.OnAccount(GameManager.instance.GameCode, GameManager.instance.m_userId).Make());
                    else
                        SendPacket(new Packet.OnTestAccount(GameManager.instance.GameCode).Make());
                    m_bConnectServer = false;
                }
                if (m_testAccountData != null) // 5011
                {
                    Debug.Log("테스트 계정 받음");
                    GameManager.instance.m_userId = m_testAccountData.o_testeruserid;
                    SendPacket(new Packet.OnAccount(GameManager.instance.GameCode, GameManager.instance.m_userId).Make());
                    m_testAccountData = null;
                }
                if (m_accountData != null) // 5001
                {
                    Debug.Log("로그인 데이터 받음");
                    SendPacket(new Packet.OnJoinGame(GameManager.instance.GameCode).Make()); // 두번째 인자 방 번호
                    GameManager.instance.m_totalMoney = m_accountData.o_totalcoin;
                    m_accountData = null;
                }
                if (m_joinGameData != null) // 4001
                {
                    Debug.Log("게임 방 접속");
                    Debug.Log(GameManager.instance.GameCode + "---" + GameManager.instance.m_roomIndex);
                    SendPacket(new Packet.OnRoomData(GameManager.instance.GameCode, GameManager.instance.m_roomIndex).Make());
                    StartCoroutine(Ping());
                    m_joinGameData = null;
                }
                if (m_roomData != null) // 4111
                {
                    Debug.Log("방 정보 확인");
                    SendPacket(new Packet.OnStart(GameManager.instance.GameCode, 1).Make());
                    m_roomData = null;
                }
                if (m_reelData != null)
                {
                    Debug.Log("릴 데이터 받음");
                    GameManager.instance.SetReelData(m_reelData);
                    m_reelData = null;         
                }
                if(m_spinData != null)
                {
                    Debug.Log("스핀 데이터 받음");
                    //m_pingCorutine = StartCoroutine(Ping());
                    ReelManager.instance.StopReel(m_spinData);
                    GameManager.instance.SetUserDataToSpin(m_spinData);
                    UIManager.instance.ShowSpinToCredit();
                    m_spinData = null;
                }
                if(m_freeSpinData != null)
                {
                    Debug.Log("프리스핀 데이터 받음");
                    FreeSpinUI.instance.SetMultiple(m_freeSpinData);
                    m_freeSpinData = null;
                }
                if (m_isQuit)
                {
                    StartCoroutine(ReConnectServer());
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        private void OnApplicationQuit()
        {
            Close();
        }
    }

}
