
namespace Server
{
    namespace Packet
    {
        interface IHeader
        {
            byte[] Make();
        }
        interface IPacket
        {

        }
        public class OnMessgae : IPacket
        {
            public short io_length; // 길이( 2바이트 )
            public Server.E_CODE io_code; // 코드( 2바이트 )
            public short io_subcode; //  서버코드( 2바이트 )
            public short io_gamecode;
            public string io_messgae;
            public OnMessgae(short gamecode, string message)
            {
                io_code = E_CODE.SPIN;
                io_subcode = 0;
                io_gamecode = gamecode;
                io_messgae = message;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                io_length += Converter.SetString(newByte, ref index, io_messgae);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }
        public class OnStart : IPacket // 
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public Server.E_CODE io_code;    // 제어 코드; 2001 게임 릴 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short i_reqreel;    // 요구 릴; 1, 3, 5 ~ : 일반 게임 릴\n2, 4, 6 ~ : 프리 게임 릴
            public OnStart(short gamecode, short reqreel)
            {
                io_code = E_CODE.REQ_REEL;
                io_subcode = 0;
                io_gamecode = gamecode;
                i_reqreel = reqreel;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                io_length += Converter.SetShort(newByte, ref index, i_reqreel);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }
        public class OnRecvReelData : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public Server.E_CODE io_code;    // 제어 코드; 2002 게임 릴 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)

            public byte[] o_reel1 = new byte[250];    // 릴 1 배열; 릴 배열
            public byte[] o_reel2 = new byte[250];    // 릴 2 배열
            public byte[] o_reel3 = new byte[250];    // 릴 3 배열
            public byte[] o_reel4 = new byte[250];    // 릴 4 배열
            public byte[] o_reel5 = new byte[250];    // 릴 5 배열
            public byte[] o_reel6 = new byte[250];    // 릴 6 배열
            public byte[] o_reel7 = new byte[250];    // 릴 7 배열
            public byte[] o_reel8 = new byte[250];    // 릴 8 배열
            public byte[] o_reel9 = new byte[250];    // 릴 9 배열
            public byte[] o_reel10 = new byte[250];    // 릴 10 배열
            public byte[] o_reel11 = new byte[250];    // 릴 11 배열
            public byte[] o_reel12 = new byte[250];    // 릴 12 배열
            public byte[] o_reel13 = new byte[250];    // 릴 13 배열
            public byte[] o_reel14 = new byte[250];    // 릴 14 배열
            public byte[] o_reel15 = new byte[250];    // 릴 15 배열
            public byte[] o_reel16 = new byte[250];    // 릴 16 배열
            public byte[] o_reel17 = new byte[250];    // 릴 17 배열
            public byte[] o_reel18 = new byte[250];    // 릴 18 배열
            public byte[] o_reel19 = new byte[250];    // 릴 19 배열
            public byte[] o_reel20 = new byte[250];    // 릴 20 배열
            public OnRecvReelData(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
                o_reel1 = Converter.GetByteArray(data, ref index);
                o_reel2 = Converter.GetByteArray(data, ref index);
                o_reel3 = Converter.GetByteArray(data, ref index);
                o_reel4 = Converter.GetByteArray(data, ref index);
                o_reel5 = Converter.GetByteArray(data, ref index);
                o_reel6 = Converter.GetByteArray(data, ref index);
                o_reel7 = Converter.GetByteArray(data, ref index);
                o_reel8 = Converter.GetByteArray(data, ref index);
                o_reel9 = Converter.GetByteArray(data, ref index);
                o_reel10 = Converter.GetByteArray(data, ref index);
                o_reel11 = Converter.GetByteArray(data, ref index);
                o_reel12 = Converter.GetByteArray(data, ref index);
                o_reel13 = Converter.GetByteArray(data, ref index);
                o_reel14 = Converter.GetByteArray(data, ref index);
                o_reel15 = Converter.GetByteArray(data, ref index);
                o_reel16 = Converter.GetByteArray(data, ref index);
                o_reel17 = Converter.GetByteArray(data, ref index);
                o_reel18 = Converter.GetByteArray(data, ref index);
                o_reel19 = Converter.GetByteArray(data, ref index);
                o_reel20 = Converter.GetByteArray(data, ref index);
            }
        }
        public class OnSpin : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public Server.E_CODE io_code;   // 제어 코드; 3001 게임 스핀 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short i_betindex;    // 베팅 금액; 사용자가 베팅한 금액의 인덱스
            public OnSpin(short gamecode, short betIndex)
            {
                io_code = E_CODE.SPIN;
                io_subcode = 0;
                io_gamecode = gamecode;
                i_betindex = betIndex;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                io_length += Converter.SetShort(newByte, ref index, i_betindex);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }
        public class OnRecvSpinData : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;   // 제어 코드; 3002 게임 스핀 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)

            public byte[] o_selectedline = new byte[20];    // 선택된 심볼 라인; 게임 서버 시스템에서 선택된 릴 인덱스
            public byte[] o_selecteditem = new byte[20];    // 선택된 심볼 아이템; 게임 서버 시스템에서 선택된 심볼 인덱스\n특수한 경우 인위적 심볼 변경

            public double o_wincoin;    // 당첨 금액; 당첨된 총 금액 (무당첨: 0)
            public double o_totalcoin;    // 총 금액; 사용자 총 금액

            public short o_freecount;    // 프리 스핀 수; 남은 프리 스핀수 (1 씩 감소)
            public short o_freeaddcount;    // 프리스핀 추가 수; 프리스핀 추가 수
            public short o_freestate;    // 프리 스핀 상태; 현재 스핀의 프리 게임 유무 (1 - 프리, 0 - 일반)

            public double[] o_progressive = new double[5];    // Progressive; Progressive 심볼이 5 ~ 9 개일때 적용될 적립계수

            public byte[] o_expansion1 = new byte[20];    // 확장 데이터; 다용도 확장으로 사용 // 0 : 배수
            public short[] o_expansion2 = new short[20];    // 확장 데이터; 다용도 확장으로 사용
            public int[] o_expansion3 = new int[20];    // 확장 데이터; 다용도 확장으로 사용 
            public long[] o_expansion4 = new long[20];    // 확장 데이터; 다용도 확장으로 사용
            public double[] o_expansion5 = new double[20];    // 확장 데이터; 다용도 확장으로 사용
            public string o_expansion6;    // 확장 데이터; 다용도 확장으로 사용
            // 5002 : 잭팟당첨금액
            public OnRecvSpinData(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
                o_selectedline = Converter.GetByteArray(data, ref index);
                o_selecteditem = Converter.GetByteArray(data, ref index);
                o_wincoin = Converter.GetDouble(data, ref index);
                o_totalcoin = Converter.GetDouble(data, ref index);
                o_freecount = Converter.GetShort(data, ref index);
                o_freeaddcount = Converter.GetShort(data, ref index);
                o_freestate = Converter.GetShort(data, ref index);
                o_progressive = Converter.GetDoubleArray(data, ref index);
                o_expansion1 = Converter.GetByteArray(data, ref index);
                o_expansion2 = Converter.GetShortArray(data, ref index);
                o_expansion3 = Converter.GetIntArray(data, ref index);
                o_expansion4 = Converter.GetLongArray(data, ref index);
                o_expansion5 = Converter.GetDoubleArray(data, ref index);
                o_expansion6 = Converter.GetString(data, ref index);
            }
        }
        public class OnSelectFreeSpin : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 3011 게임 스핀 보조 자료 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public byte[] i_extra1 = new byte[0];    // 보조 데이터; 다용도 확장으로 사용
            public short[] i_extra2 = new short[0];    // 보조 데이터; 다용도 확장으로 사용
            public int[] i_extra3 = new int[0];    // 보조 데이터; 다용도 확장으로 사용
            public long[] i_extra4 = new long[0];    // 보조 데이터; 다용도 확장으로 사용
            public double[] i_extra5 = new double[0];    // 보조 데이터; 다용도 확장으로 사용
            public string i_extra6 = "";    // 보조 데이터; 다용도 확장으로 사용
            public OnSelectFreeSpin(short gamecode, byte index)
            {
                io_code = (short)E_CODE.SELECT_FREE_SPIN;
                io_subcode = 0;
                io_gamecode = gamecode;
                i_extra1 = new byte[] { index };
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                io_length += Converter.SetByteArray(newByte, ref index, i_extra1);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }
        public class OnRecvFreeSpin : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 3012 게임 스핀 보조 자료 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)

            public byte[] o_extra1 = new byte[20];    // 보조 데이터; 다용도 확장으로 사용
            public short[] o_extra2 = new short[20];    // 보조 데이터; 다용도 확장으로 사용
            public int[] o_extra3 = new int[20];    // 보조 데이터; 다용도 확장으로 사용
            public long[] o_extra4 = new long[20];    // 보조 데이터; 다용도 확장으로 사용
            public double[] o_extra5 = new double[20];    // 보조 데이터; 다용도 확장으로 사용
            public string o_extra6 = "";    // 보조 데이터; 다용도 확장으로 사용
            public static int o_extra6__length = 4096;
            public OnRecvFreeSpin(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
                o_extra1 = Converter.GetByteArray(data, ref index);
                o_extra2 = Converter.GetShortArray(data, ref index);
                o_extra3 = Converter.GetIntArray(data, ref index);
                o_extra4 = Converter.GetLongArray(data, ref index);
                o_extra5 = Converter.GetDoubleArray(data, ref index);
                o_extra6 = Converter.GetString(data, ref index);
            }
        }
        public class OnJoinGame : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 4001 게임 시작 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드
            public OnJoinGame(short gamecode)
            {
                io_code = (short)E_CODE.JOIN_GAME;
                io_subcode = 0;
                io_gamecode = gamecode;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }

        public class OnRecvJoinGame : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 4002 게임 시작 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)
            public OnRecvJoinGame(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
            }
        }
        public class OnPing : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 4201 게임 시작 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드
            public OnPing(short gamecode)
            {
                io_code = (short)E_CODE.PING;
                io_subcode = 0;
                io_gamecode = gamecode;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }

        public class OnPingRecv : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 4202 게임 시작 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)
            public OnPingRecv(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
            }
        }
        //
        public class OnExitGame : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 4011 게임 종료 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드
            public OnExitGame(short gamecode)
            {
                io_code = (short)E_CODE.EXIT_GAME;
                io_subcode = 0;
                io_gamecode = gamecode;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }
        public class OnRecvExitGame : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 4012 게임 종료 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)
            public OnRecvExitGame(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
            }
        }
        //
        //
        public class OnRoomData : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 4101 게임 방 정보 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_roomindex;
            public OnRoomData(short gamecode, short roomindex)
            {
                io_code = (short)E_CODE.ROOM_DATA;
                io_subcode = 0;
                io_gamecode = gamecode;
                o_roomindex = roomindex;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                io_length += Converter.SetShort(newByte, ref index, o_roomindex);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }

        public class OnRecvRoomData : IPacket
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 4102 게임 방 정보 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)
            public OnRecvRoomData(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
            }
        }
        //
        //
       
        //

        // 로그인 관련
        public class OnAccount
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 5001 사용자 등록 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public string i_userid;    // 사용자 ID

            public OnAccount(short gamecode, string userId)
            {
                io_code = (short)E_CODE.ACCUONT;
                io_subcode = 0;
                io_gamecode = gamecode;
                i_userid = userId;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                io_length += Converter.SetString(newByte, ref index, i_userid);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }
        public class OnRecvAccount
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 5002 사용자 등록 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)

            public short o_userstate;    // 사용자 등록 상태; 1 - 신규 등록\n2 - 이미 등록\n3 - 재 등록
            public double o_totalcoin;
            public OnRecvAccount(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
                o_userstate = Converter.GetShort(data, ref index);
                o_totalcoin = Converter.GetDouble(data, ref index);
            }
        }
        //

        // 테스트 용
        public class OnTestAccount
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public short io_code;    // 제어 코드; 5011 테스터 사용자 ID 요청
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드
            public OnTestAccount(short gamecode)
            {
                io_code = (short)E_CODE.TEST_ACCOUNT;
                io_subcode = 0;
                io_gamecode = gamecode;
            }
            public byte[] Make()
            {
                byte[] newByte = new byte[1024];
                int index = 2;
                io_length = 2;
                io_length += Converter.SetShort(newByte, ref index, (short)io_code);
                io_length += Converter.SetShort(newByte, ref index, io_subcode);
                io_length += Converter.SetShort(newByte, ref index, io_gamecode);
                index = 0;
                Converter.SetShort(newByte, ref index, io_length);
                return newByte;
            }
        }

        public class OnRecvTestAccount
        {
            public short io_length;    // 전문 길이; 전체 전문 길이
            public E_CODE io_code;    // 제어 코드; 5012 테스터 사용자 ID 응답
            public short io_subcode;    // 보조 코드; 0000
            public short io_gamecode;    // 게임 코드; 게임 분류 코드

            public short o_errorcode;    // 오류 코드; 오류 코드 (0 - 정상)

            public string o_testeruserid;    // 테스터 사용자 ID
            public OnRecvTestAccount(byte[] data)
            {
                int index = 0;
                io_length = Converter.GetShort(data, ref index);
                io_code = (E_CODE)Converter.GetShort(data, ref index);
                io_subcode = Converter.GetShort(data, ref index);
                io_gamecode = Converter.GetShort(data, ref index);
                o_errorcode = Converter.GetShort(data, ref index);
                o_testeruserid = Converter.GetString(data, ref index);
            }
        }
    }
    //
}