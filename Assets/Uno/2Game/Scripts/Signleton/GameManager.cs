using BackEnd;
using UnityEngine;

// 게임 시작
public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        Debug.Log("Game Manager Start!");
        TurnManager.instance.StartGame();
    }

    void Update()
    {
        Backend.AsyncPoll();
    }
}
