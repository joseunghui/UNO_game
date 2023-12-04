using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Inst {get; private set;}
    void Awake() => Inst = this;
    public Button cardbtn;
    public Button turnbtn;
    public Button unobtn;
     [SerializeField] NotificationPanel notificationPanel;
     [SerializeField] ResultPanel resultPanel;
    void Start()
    {
        turnbtn.interactable = false;
        turnbtn.onClick.AddListener(() =>{
            TurnManager.instance.EndTurn();
            turnbtn.interactable = false;
        });
    }
    public void Uno(){
        TurnManager.OnAddCard?.Invoke(false); // 게임이 내 기준에서 구현해서 일단 false로 넣어놓음
        TurnManager.OnAddCard?.Invoke(false);
        // 상대 움직임 구현에 따라 수정 필요
        unobtn.interactable = false;
        TurnManager.instance.unoCount = 0;
    }
    
    public void nonePutCard(){
        cardbtn.interactable = false;
        turnbtn.interactable = true;
        TurnManager.OnAddCard.Invoke(true);
        Debug.Log(cardbtn.interactable+",카드먹기 "+turnbtn.interactable);
    }
    public void turnStart(bool myTurn){
        cardbtn.interactable = myTurn;
        turnbtn.interactable = false;
        if (myTurn)
            notificationPanel.Show("내 차례!");
    }
    public void endingPopUp(bool isMyWin){
        cardbtn.interactable = false;
        turnbtn.interactable = false;
        resultPanel.Show(isMyWin);
    }
}
