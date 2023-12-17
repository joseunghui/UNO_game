using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ButtonManager : TurnManager
{
    public static ButtonManager Inst {get; private set;}
    void Awake() => Inst = this;
    public Button cardbtn;
    public Button turnbtn;
    public Button unobtn;
    
    [SerializeField] ResultPanel resultPanel;
    void Start()
    {
        turnbtn.interactable = false;
        turnbtn.onClick.AddListener(() =>{
            EndTurn();
            turnbtn.interactable = false;
        });
    }
    public void Uno(){
        bool turn = myTurn;
        int random = Random.Range(0,100);
        int per = 0;

        if (random < per){
            TurnManager.OnAddCard?.Invoke(true);
            TurnManager.OnAddCard?.Invoke(true);
            Debug.Log("나 +2");
        } else{
            TurnManager.OnAddCard?.Invoke(false);
            TurnManager.OnAddCard?.Invoke(false);
            unobtn.interactable = false;
            Debug.Log("상대 +2");
        }
        unoCount = 0;
        unobtn.interactable = false;
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
        {
            Managers.UI.ShowPopup<UI_NotificationPopup>();
        }
            
    }
    public void endingPopUp(bool isMyWin){
        cardbtn.interactable = false;
        turnbtn.interactable = false;
        resultPanel.Show(isMyWin);
    }
}
