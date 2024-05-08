using BackEnd;
using BackEnd.Tcp;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEndMatchManager : MonoBehaviour
{
    private bool isSetHost = false;                 // 호스트 세션 결정했는지 여부

    private MatchGameResult matchGameResult;

    // 게임 로그
    private string FAIL_ACCESS_INGAME = "인게임 접속 실패 : {0} - {1}";
    private string SUCCESS_ACCESS_INGAME = "유저 인게임 접속 성공 : {0}";
    private string NUM_INGAME_SESSION = "인게임 내 세션 갯수 : {0}";



    // 게임 레디 상태일 때 호출됨
    public void OnGameReady()
    {
        if (isSetHost == false)
        {
            // 호스트가 설정되지 않은 상태이면 호스트 설정
            isSetHost = SetHostSession();
        }
        Debug.Log("호스트 설정 완료");

        if (isSandBoxGame == true && IsHost() == true)
        {
            SetAIPlayer();
        }

        if (IsHost() == true)
        {
            // 0.5초 후 ReadyToLoadRoom 함수 호출
            Invoke("ReadyToLoadRoom", 0.5f);
        }
    }

    private void SetAIPlayer()
    {
        int aiCount = numOfClient - sessionIdList.Count;
        int numOfTeamOne = 0;
        int numOfTeamTwo = 0;

        Debug.Log("AI 플레이어 설정 : aiCount : " + aiCount);

        if (nowModeType == MatchModeType.TeamOnTeam)
        {
            foreach (var tmp in gameRecords)
            {
                if (tmp.Value.m_teamNumber == 0)
                {
                    numOfTeamOne += 1;
                }
                else
                {
                    numOfTeamTwo += 1;
                }
            }
        }
        int index = 0;
        for (int i = 0; i < aiCount; ++i)
        {
            MatchUserGameRecord aiRecord = new MatchUserGameRecord();
            aiRecord.m_nickname = "AIPlayer" + index;
            aiRecord.m_sessionId = (SessionId)index;
            aiRecord.m_numberOfMatches = 0;
            aiRecord.m_numberOfWin = 0;
            aiRecord.m_numberOfDefeats = 0;
            aiRecord.m_numberOfDraw = 0;
            if (nowMatchType == MatchType.MMR)
            {
                aiRecord.m_mmr = 1000;
            }
            else if (nowMatchType == MatchType.Point)
            {
                aiRecord.m_points = 1000;
            }

            if (nowModeType == MatchModeType.TeamOnTeam)
            {
                if (numOfTeamOne > numOfTeamTwo)
                {
                    aiRecord.m_teamNumber = 1;
                    numOfTeamTwo += 1;
                }
                else
                {
                    aiRecord.m_teamNumber = 0;
                    numOfTeamOne += 1;
                }
            }
            gameRecords.Add((SessionId)index, aiRecord);
            sessionIdList.Add((SessionId)index);
            index += 1;
        }
    }



    // 서버에서 게임 시작 패킷을 보냈을 때 호출
    // 모든 세션이 게임 룸에 참여 후 "콘솔에서 설정한 시간" 후에 게임 시작 패킷이 서버에서 온다
    private void GameSetup()
    {
        Debug.Log("게임 시작 메시지 수신. 게임 설정 시작");

        // 게임 시작 메시지가 오면 게임을 레디 상태로 변경
        //if (GameManager.GetInstance().GetGameState() != GameManager.GameState.Ready)
        //{
        isHost = false;
        isSetHost = false;
        OnGameReady();
        //}
        //GameManager.GetInstance().ChangeState(GameManager.GameState.Start);
    }

    // 인게임 룸 접속
    private void AccessInGameRoom(string roomToken)
    {
        Backend.Match.JoinGameRoom(roomToken);
    }

    // 인게임 서버 접속 종료
    public void LeaveInGameRoom()
    {
        isConnectInGameServer = false;
        Backend.Match.LeaveGameServer();
    }

    // 클라이언트 들의 게임 룸 접속에 대한 리턴값
    // 클라이언트가 게임 룸에 접속할 때마다 호출됨
    // 재접속 했을 때는 수신되지 않음
    private void ProcessMatchInGameAccess(MatchInGameSessionEventArgs args)
    {
        if (isReconnectProcess)
        {
            // 재접속 프로세스 인 경우
            // 이 메시지는 수신되지 않고, 만약 수신되어도 무시함
            Debug.Log("재접속 프로세스 진행중... 재접속 프로세스에서는 ProcessMatchInGameAccess 메시지는 수신되지 않습니다.\n" + args.ErrInfo);
            return;
        }

        Debug.Log(string.Format(SUCCESS_ACCESS_INGAME, args.ErrInfo));

        if (args.ErrInfo != ErrorCode.Success)
        {
            // 게임 룸 접속 실패
            var errorLog = string.Format(FAIL_ACCESS_INGAME, args.ErrInfo, args.Reason);
            Debug.Log(errorLog);
            LeaveInGameRoom();
            return;
        }

        // 게임 룸 접속 성공
        // 인자값에 방금 접속한 클라이언트(세션)의 세션ID와 매칭 기록이 들어있다.
        // 세션 정보는 누적되어 들어있기 때문에 이미 저장한 세션이면 건너뛴다.

        var record = args.GameRecord;
        Debug.Log(string.Format(string.Format("인게임 접속 유저 정보 [{0}] : {1}", args.GameRecord.m_sessionId, args.GameRecord.m_nickname)));
        if (!sessionIdList.Contains(args.GameRecord.m_sessionId))
        {
            // 세션 정보, 게임 기록 등을 저장
            sessionIdList.Add(record.m_sessionId);
            gameRecords.Add(record.m_sessionId, record);

            Debug.Log(string.Format(NUM_INGAME_SESSION, sessionIdList.Count));
        }
    }
    private void ProcessAIDate(Protocol.AIPlayerInfo aIPlayerInfo)
    {
        MatchInGameSessionEventArgs args = new MatchInGameSessionEventArgs();
        args.GameRecord = aIPlayerInfo.GetMatchRecord();

        ProcessMatchInGameAccess(args);
    }

    public bool PrevGameMessage(byte[] BinaryUserData)
    {
        Protocol.Message msg = DataParser.ReadJsonData<Protocol.Message>(BinaryUserData);
        if (msg == null)
        {
            return false;
        }

        // 게임 설정 사전 작업 패킷 검사 
        switch (msg.type)
        {
            case Protocol.Type.AIPlayerInfo:
                Protocol.AIPlayerInfo aiPlayerInfo = DataParser.ReadJsonData<Protocol.AIPlayerInfo>(BinaryUserData);
                ProcessAIDate(aiPlayerInfo);
                return true;
/*            case Protocol.Type.LoadRoomScene:
                LobbyUI.GetInstance().ChangeRoomLoadScene();
                if (IsHost() == true)
                {
                    Debug.Log("5초 후 게임 씬 전환 메시지 송신");
                    Invoke("SendChangeGameScene", 5f);
                }
                return true;
            case Protocol.Type.LoadGameScene:
                GameManager.GetInstance().ChangeState(GameManager.GameState.Start);
                return true;*/
        }
        return false;
    }

    private void ProcessSessionOnline(SessionId sessionId, string nickName)
    {
        //InGameUiManager.GetInstance().SetReconnectBoard(nickName);
        // 호스트가 아니면 아무 작업 안함 (호스트가 해줌)
        if (isHost)
        {
            // 재접속 한 클라이언트가 인게임 씬에 접속하기 전 게임 정보값을 전송 시 nullptr 예외가 발생하므로 조금
            // 2초정도 기다린 후 게임 정보 메시지를 보냄
            Invoke("SendGameSyncMessage", 2.0f);
        }
    }

    private void ProcessSessionOffline(SessionId sessionId)
    {
        if (hostSession.Equals(sessionId))
        {
            // 호스트 연결 대기를 띄움
            // InGameUiManager.GetInstance().SetHostWaitBoard();
        }
        else
        {
            // 호스트가 아니면 단순히 UI 만 띄운다.
        }
    }
}
