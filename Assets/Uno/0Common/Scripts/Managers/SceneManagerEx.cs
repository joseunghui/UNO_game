using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Battlehub.Dispatcher;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public string nextSceneName;

    // Scene Change Method
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        nextSceneName = GetSceneName(type);

        // 일단 로딩 씬으로 이동 
        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));
    }

    public void LoadScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

    // Enum속 씬 타입을 string으로 가져오기
    public string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    // Loading popup(match)
    public void BeforeLoadScene(Define.GameState gameState)
    {
        if (gameState.Equals(null))
        {
            Dispatcher.Current.BeginInvoke(() => Managers.UI.ShowPopup<UI_MatchLoading>());
            
            if (Managers.Match.isConnectInGameServer)
            {
                Debug.Log("로딩 화면 어케 없애누...");
                Managers.Match.RequestMatchMaking(0);
            }
        }
    }
}