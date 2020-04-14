using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace Server
{
    [System.Serializable]
    public class WebServer : MonoSingleton<WebServer>
    {
        [HideInInspector]
        public bool m_isLobbyData = false;
        public bool m_isRoomData = false;
        public userlist m_userData = null;
        public room m_roomData = null;
        public jackpotdata jackpot = null;
        //public double[] jackpot;
        public jackpotdata m_jackpotData = null;
        public delegate void CallBack();
        public CallBack m_dCallBack;

        private string m_lobbyUrl = "http://211.252.84.138/inplay/json/userlist.php";
        private string m_roomUrl = "http://211.252.84.138/inplay/json/roomdata.php";
        private string m_checkRoomUrl = "http://211.252.84.138/inplay/json/roomindex.php";
        private string m_jackpotUrl = "http://211.252.84.138/inplay/json/jackpotdata.php";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            GetLobby();
        }
        public room RoomData() // 룸 json 프로퍼티
        {
            if (!m_isRoomData)
                return null;
            m_isRoomData = false;
            return m_roomData;
        }

        public void GetLobby() // 로비 정보 가져오기 Web통신
        {
            StartCoroutine(WaitForRequest(m_lobbyUrl));
        }
        public void GetRoomData() // 방 정보 가져오기 Web통신
        {
            m_roomData = null;
            StartCoroutine(WaitForRequestRoom());
            StartCoroutine(WaitForRequestJackpot());
        }
        public void GetRoomData(short roomIndex, RoomManager.StartGame dele) // 방 정보 가져오기 인자 방 정보, 종료 이벤트
        {
            StartCoroutine(WaitForRequestRoom(roomIndex, dele));
        }

        private IEnumerator WaitForRequest(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                Debug.Log(www.text);
                Debug.Log("로비 정보 확인");
                ProcessPlayer(www.text); // 로비 정보 json에 오류가 없으면
            }
            else
            {
                Debug.Log("WWW Error");
            }
        }
        private IEnumerator WaitForRequestRoom() // 방 정보 받는 코루틴
        {
            WWWForm form = new WWWForm();
            form.AddField("gamecode", GameManager.instance.GameCode);
            UnityWebRequest www = UnityWebRequest.Post(m_roomUrl, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                m_isRoomData = true;
                m_roomData = JsonUtility.FromJson<room>(www.downloadHandler.text);
                m_dCallBack();
            }
        }
        private IEnumerator WaitForRequestJackpot() // 방 정보 받는 코루틴
        {
            WWWForm form = new WWWForm();
            form.AddField("gamecode", GameManager.instance.GameCode);
            UnityWebRequest www = UnityWebRequest.Post(m_jackpotUrl, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                jackpot = JsonUtility.FromJson<jackpotdata>(www.downloadHandler.text);
            }
        }
        public IEnumerator WaitForRequestRoom(short roomIndex, RoomManager.StartGame dele) // 게임방에 입장 가능한지 확인
        {
            WWWForm form = new WWWForm();
            form.AddField("gamecode", GameManager.instance.GameCode);
            form.AddField("roomindex", roomIndex);
            UnityWebRequest www = UnityWebRequest.Post(m_checkRoomUrl, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                checkroom checker = JsonUtility.FromJson<checkroom>(www.downloadHandler.text);
                //Debug.Log(checker.roomstate);
                dele(checker.roomstate);
            }
        }

        private void ProcessPlayer(string jsonString)
        {
            m_userData = JsonUtility.FromJson<userlist>(jsonString);
            SceneManager.LoadScene("Lobby");
            if (GameManager.instance.m_isGame) // 게임 중이라면 ServerManager를 파괴하고 GameManager의 게임 상태를 false로 변경
            {
                Destroy(ServerManager.instance.gameObject);
                GameManager.instance.m_isGame = false;
            }
        }
    }
    [System.Serializable]
    public class userlist // 로비 데이터
    {
        public string userid;
        public userdata[] userdata; // 유저 데이터 리스트
        public servermessage[] servermessage; // 공지사항
        public string storeurl; // 스토어 url
        public string ip;
    }
    [System.Serializable]
    public class servermessage
    {
        public string title;
        public string url;
    }
    [System.Serializable]
    public class userdata // 유저 데이터
    {
        public string gamecode; // 게임 코드
        public string id; // 유저 id
        public string totalmoney; // 유저 보유 금액
        public string freespinmoney;
        public string jackpotmoney;
    }
    [System.Serializable]
    public class room
    {
        public roomdata[] roomdata;
    }
    [System.Serializable]
    public class roomdata // 방 정보
    {
        public short state; // 게임 상태
        public short freestate;
        public double totalmoney; // 총합 머니
        public double lastmoney; // 마지막 유저 보유금
        public int[] data;
    }
    [System.Serializable]
    public class checkroom
    {
        public short roomstate;
    }

    [System.Serializable]
    public class jackpotdata
    {
        public double[] jackpot;
    }


}
