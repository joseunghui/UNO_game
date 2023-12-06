using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        bool turn = TurnManager.instance.myTurn;
        int random = Random.Range(0,100);
        int per = 0;
        switch(StartGame.TurnlimitTime){
            case 5:
                per = 80; break;
            case 10:
                per = 50; break;
            default:
                per = 20; break;
        }Debug.Log("우노 TurnTimeLimit: "+StartGame.TurnlimitTime+", per: "+per);
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
        TurnManager.instance.unoCount = 0;
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
            notificationPanel.Show("내 차례!");
    }
    public void endingPopUp(bool isMyWin){
        cardbtn.interactable = false;
        turnbtn.interactable = false;
        resultPanel.Show(isMyWin);
    }
}
