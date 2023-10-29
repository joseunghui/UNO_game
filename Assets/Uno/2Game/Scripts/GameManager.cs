using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 치트, UI, 랭킹, 게임오버 등등
public class GameManager : MonoBehaviour
{
    public static GameManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] NotificationPanel notificationPanel;
    public Button cardbtn;
    public Button turnbtn;

    
    void Start()
    {
        StartGame();    // 나중에 버튼으로 바꿔서 호출하는 시점에 게임이 시작되도록~
    }
    void Update(){
#if UNITY_EDITOR    // 유니티 에디터일 경우에만 치트 호출
        InputCheatKey();
#endif
    }

    void InputCheatKey(){
        if(Input.GetKeyDown(KeyCode.Keypad1))
            TurnManager.OnAddCard?.Invoke(true);
        if(Input.GetKeyDown(KeyCode.Keypad2))
            TurnManager.OnAddCard?.Invoke(false);
        if(Input.GetKeyDown(KeyCode.Keypad3))
            TurnManager.Inst.EndTurn();
    }
    
    public void StartGame(){
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message){
        notificationPanel.Show(message);
    }

    public void nonePutCard(){
        TurnManager.OnAddCard?.Invoke(true);
        cardbtn.interactable = false;
        ColorBlock btnColor = cardbtn.colors;
        btnColor.normalColor = new Color32(55,55,55,255);
        turnbtn.interactable = true;
        ColorBlock btnColor1 = turnbtn.colors;
        btnColor1.normalColor = new Color32(255,234,0,172);
    }
}
