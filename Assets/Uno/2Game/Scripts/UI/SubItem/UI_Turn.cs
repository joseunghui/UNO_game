using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Turn : UI_SubItem
{
    Button AddCardBtn;
    Button EndTurnBtn;
    Button UnoBtn;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<Button>(typeof(Define.Buttons));

        Get<TextMeshProUGUI>((int)Define.Texts.AddCardText).GetComponent<TextMeshProUGUI>().text = "카드 가져오기";
        Get<TextMeshProUGUI>((int)Define.Texts.EndTurnText).GetComponent<TextMeshProUGUI>().text = "턴 넘기기";

        AddCardBtn = GetButton((int)Define.Buttons.AddCard).gameObject.GetComponent<Button>();
        EndTurnBtn = GetButton((int)Define.Buttons.EndTurn).gameObject.GetComponent<Button>();
        // UnoBtn = GetButton((int)Define.Buttons.UnoBtn).gameObject.GetComponent<Button>();


        AddCardBtn.onClick.AddListener(() => { Debug.Log("AddCardBtn Click"); });
        EndTurnBtn.onClick.AddListener(() => { Debug.Log("EndTurnBtn Click"); });

        FirstSetButton();
    }

    void FirstSetButton()
    {
        EndTurnBtn.interactable = false;
        EndTurnBtn.onClick.AddListener(() => {
            // EndTurn();
            EndTurnBtn.interactable = false;
        });
    }


    public void Uno()
    {
        /*bool turn = myTurn;
        int random = Random.Range(0, 100);
        int per = 0;

        if (random < per)
        {
            OnAddCard?.Invoke(true);
            OnAddCard?.Invoke(true);
            Debug.Log("나 +2");
        }
        else
        {
            OnAddCard?.Invoke(false);
            OnAddCard?.Invoke(false);
            UnoBtn.interactable = false;
            Debug.Log("상대 +2");
        }
        unoCount = 0;
        UnoBtn.interactable = false;*/
    }

    public void nonePutCard()
    {
        AddCardBtn.interactable = false;
        EndTurnBtn.interactable = true;
        //OnAddCard.Invoke(true);
        Debug.Log(AddCardBtn.interactable + ",카드먹기 " + EndTurnBtn.interactable);
    }
    public void turnStart(bool myTurn)
    {
        AddCardBtn.interactable = myTurn;
        EndTurnBtn.interactable = false;
        if (myTurn)
        {
            Managers.UI.ShowPopup<UI_NotificationPopup>();
        }

    }
    public void endingPopUp(bool isMyWin)
    {
        AddCardBtn.interactable = false;
        EndTurnBtn.interactable = false;
        // resultPanel.Show(isMyWin);
    }
}
