using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] ItemSO ItemBuffer;
    public List<Item> itemList;

    // Start is called before the first frame update
    void Start()
    {
        // 카드 섞기 
        itemList = ShuffleList<Item>(SetCardList());

    }

    #region SetStartCardCountbyGameMode
    public void SetStartCardCountbyGameMode()
    {
        TurnController turnController = gameObject.GetComponent<TurnController>();
        GameMode.PVCMode _type = gameObject.GetComponent<GameScene>().GetGameMode();

        // 난이도에 따른 보유 카드 개수 차등
        switch (_type)
        {
            case GameMode.PVCMode.EASY:
                turnController.startCardCount = 5;
                break;
            case GameMode.PVCMode.NORMAL:
                turnController.startCardCount = 8;
                break;
            case GameMode.PVCMode.HARD:
                turnController.startCardCount = 10;
                break;
        }
        turnController.StartGame();
    }
    #endregion


    #region Set & Mix Card
    List<Item> SetCardList()
    {
        itemList = new List<Item>();

        for (int i = 0; i < ItemBuffer.items.Length; i++)
        {
            itemList.Add(ItemBuffer.items[i]);
        }
        return itemList;
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
    #endregion
}