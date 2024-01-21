using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] ItemSO ItemBuffer;
    public List<Item> itemList;

    public int startCardCount;
    public bool isLoading; // 게임 끝나면 true로 해서 클릭 방지
    public bool myTurn;
    public int unoCount = 0;
    public List<UI_Card> otherCards = new List<UI_Card>();
    public List<UI_Card> myCards = new List<UI_Card>();
    public List<UI_Entity> entities = new List<UI_Entity>();


    // Start is called before the first frame update
    void Start()
    {
        // 카드 섞기 
        itemList = ShuffleList<Item>(SetCardList());

    }

    #region SetStartCardCountbyGameMode 
    public void SetStartCardCountbyGameMode()
    {
        GameMode.PVCMode _type = gameObject.GetComponent<GameScene>().GetGameMode();

        // 난이도에 따른 보유 카드 개수 차등
        switch (_type)
        {
            case GameMode.PVCMode.EASY:
                startCardCount = 5;
                break;
            case GameMode.PVCMode.NORMAL:
                startCardCount = 8;
                break;
            case GameMode.PVCMode.HARD:
                startCardCount = 10;
                break;
        }
        StartCoroutine(StartGameCo());
    }
    #endregion

    #region  Turn Manager
    IEnumerator StartGameCo()
    {
        myTurn = Random.Range(0, 2) == 0;
        isLoading = true;

        // 여기까지
        Debug.Log($"myTurn >> {myTurn}");

        //yield break;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            //AddCard(false);   // false : 상대 카드 추가
            yield return new WaitForSeconds(0.1f);
            AddCard(true);    // true : 내 카드 추가
        }
        yield return new WaitForSeconds(0.1f);
        SpawnEntity(true, PopItem(), Vector3.zero);
        //StartCoroutine(StartTurnCo());
    }
    IEnumerator StartTurnCo()
    {
        isLoading = true;

        // ButtonManager.Inst.turnStart(myTurn);

        yield return new WaitForSeconds(0.1f);
        isLoading = false;
        if (myTurn == false)
            yield return new WaitForSeconds(0.1f);
        //OnTurnStarted?.Invoke(myTurn);
    }
    #endregion

    #region Entity Manager(카드를 내는 위치에 오는 모든 카드들 관리)
    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos)
    {
        UI_Entity go = Managers.UI.CardSpawn<UI_Entity>(parent:gameObject.transform.parent, _pos: spawnPos, _quat: Utils.QI);
        Debug.Log($"before setUp : {item.num}");

        go.Setup(item);
        entities.Add(go);
        
        if (myTurn == false)
            go.MoveTransform(Vector3.zero, true, 1f);
        else
            go.MoveTransform(Vector3.zero, true, 0.5f);

        EntityAlignment();
        return true;
    }
    public void EntityAlignment()
    {
        var targetEntities = entities;

        for (int i = 0; i < targetEntities.Count; i++)
        {
            var targetEntity = targetEntities[i];
            
            targetEntity.transform.position = new Vector3(0, 0, 0);
            targetEntity.gameObject.GetComponent<OrderController>()?.SetOriginOrder(i+1);
        }
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

    #region 카드 뽑기
    public Item PopItem()
    {
        // if (itemBuffer.Count == 0)
        // {
        //     List<Item> items = _items;
        //     int count = items.Count;
        //     for (int i = 0; i < count - 1; i++)
        //     {
        //         itemBuffer.Add(items[i]);
        //         items.RemoveAt(i);
        //     }
        //     MixCard();
        // }
        Item item = itemList[0];
        Debug.Log(item.num + ", " + item.color);
        itemList.RemoveAt(0);
        return item;
    }
    public void AddCard(bool isMine)
    {
        Debug.Log($"AddCard");
        var spawnPoint = new Vector3(216f, -390f, 0);
        UI_Card card = Managers.UI.CardSpawn<UI_Card>(parent:null, _pos: spawnPoint, _quat: Utils.QI);
        
        card.Setup(isMine, PopItem());
        (isMine ? myCards : otherCards).Add(card);

        SetOriginOrder(isMine);
        //CardAlignment(isMine);
    }
    void SetOriginOrder(bool isMine)
    {
        int count = isMine ? myCards.Count : otherCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<OrderController>().SetOriginOrder(i + 1);
        }
    }

    public void CardAlignment(bool isMine)
    {
        // var myCardLeft = Managers.UI.CreatePositionSpot<PositionSpot>(_prs: Define.CardPRS.Left, isMine).transform;
        // var myCardRight = Managers.UI.CreatePositionSpot<PositionSpot>(_prs: Define.CardPRS.Right, isMine).transform;
        // var otherCardLeft = Managers.UI.CreatePositionSpot<PositionSpot>(_prs: Define.CardPRS.Left, isMine).transform;
        // var otherCardRight = Managers.UI.CreatePositionSpot<PositionSpot>(_prs: Define.CardPRS.Right, isMine).transform;
        
        // // 카드 정렬
        // List<PRS> originCardPRSs = new List<PRS>();
        // if (isMine)
        //     originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one);
        // else
        //     originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 0.8f);


        // var targetCards = isMine ? myCards : otherCards;
        // for (int i = 0; i < targetCards.Count; i++)
        // {
        //     var targetCard = targetCards[i];

        //     targetCard.originPRS = originCardPRSs[i];
        //     targetCard.MoveTransform(targetCard.originPRS, true, 0.97f);
        // }
    }
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount]; // 각 카드의 위치
        List<PRS> results = new List<PRS>(objCount);    // (objCount) : 할당할 메모리 크기

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;               // 카드 한장일 때
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;       // 두장
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;   // 세장
            default:                                                    // 그 이상
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;

        }

        for (int i = 0; i < objCount; i++)
        {   // 원의 방정식
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if (objCount >= 4)
            {  // 3개까지는 둥글게 배치할 필요가 없대요
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }
    #endregion

}