using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    ItemSO ItemBuffer;
    public List<Item> itemList = new List<Item>();
    public int startCardCount;
    public bool myTurn;
    public List<UI_Card> otherCards = new List<UI_Card>();
    public List<UI_Card> myCards = new List<UI_Card>();
    public List<UI_Entity> entities = new List<UI_Entity>();
    public Define.GameState gameState;
    PositionSpot position;
    public int putCount = 0;
    int unoCount = 0;


    #region Modify Turn 
    public void ModifyMyTrunToFalse()
    {
        myTurn = false;
        putCount = 1;
    }

    public void ModifyMyTrunToTrue()
    {
        myTurn = true;
        putCount = 0;
    }
    #endregion

    #region GameScene에 할당된 ItemSO를 CardController로 가져오기
    public void SetItemSO(ItemSO tempSO)
    {
        ItemBuffer = tempSO;
    }
    #endregion
    #region SetStartCardCountbyGameMode 
    public void SetStartCardCountbyGameMode(GameMode.PVCMode? _type)
    {
        if (_type.IsUnityNull())
            _type = gameObject.GetComponent<GameScene>().GetGameMode();

        PositionSpot();
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
            case GameMode.PVCMode.None:
                startCardCount = 8;
                break;
        }
        // 초반 카드 섞기
        itemList = ShuffleList(SetCardList());
        StartCoroutine(StartGameCo());
    }
    #endregion

    #region  Turn Manager
    IEnumerator StartGameCo()
    {
        CreateCardDeck(); // 카드 덱 생성

        myTurn = Random.Range(0, 2) == 0;
        gameState = Define.GameState.GamePause;

        // sound 
        Managers.Sound.Play("StartSound", Define.Sound.Effect);

        for (int i = 0; i < startCardCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            AddCard(false);   // false : 상대 카드 추가
            yield return new WaitForSeconds(0.1f);
            AddCard(true);    // true : 내 카드 추가
        }
        
        yield return new WaitForSeconds(0.1f);

        SpawnEntity(true, PopItem(), Vector3.zero);
        StartCoroutine(CoTurnSet());
    }

    void CreateCardDeck()
    {
        // 카드 덱 생성
        UI_Card card = Managers.UI.CardSpawn<UI_Card>(parent: GameObject.FindWithTag("Deck").transform, _pos: Vector3.zero, _quat: Utils.QI);
        card.gameObject.name = "CardDeck";
        card.gameObject.transform.position = new Vector3(2, 0, 0);
    }

    IEnumerator CoTurnSet()
    {
        gameState = Define.GameState.GamePause;

        // ButtonManager.Inst.turnStart(myTurn);

        yield return new WaitForSeconds(0.1f);
        gameState = Define.GameState.GameStart;
        if (myTurn == false)
            yield return new WaitForSeconds(0.1f);
        //OnTurnStarted?.Invoke(myTurn);
    }
    #endregion

    #region Entity Manager(카드를 내는 위치에 오는 모든 카드들 관리)
    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos)
    {
        // 중앙 카드 내는 곳 생성
        UI_Entity go = Managers.UI.CardSpawn<UI_Entity>(parent: GameObject.FindWithTag("Deck").transform, _pos: spawnPos, _quat: Utils.QI);

        go.transform.SetAsLastSibling();
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
            Debug.Log("낸 카드 : "+targetEntity.item.color+", "+targetEntity.item.num);
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

    void MixCard()
    {
        for (int i = 0; i < itemList.Count; i++)
        {      // 카드 섞기
            int rand = Random.Range(i, itemList.Count);
            Item temp = itemList[i];
            itemList[i] = itemList[rand];
            itemList[rand] = temp;
        }
    }
    #endregion

    #region 카드 뽑기
    void PositionSpot()
    {
        position = Managers.UI.MakeSubItemInOldCanvas<PositionSpot>("PositionSpot");
        position.transform.localScale = Vector3.one;
        position.SetTransformPosition();
    }

    public Item PopItem()
    {
        if (itemList.Count == 0)
        {
            List<Item> items = ItemBuffer.items.OfType<Item>().ToList();
            int count = items.Count;
            for (int i = 0; i < count - 1; i++)
            {
                itemList.Add(items[i]);
                items.RemoveAt(i);
            }
            MixCard();
        }
        Item item = itemList[0];
        Debug.Log(item.num + ", " + item.color);
        itemList.RemoveAt(0);
        return item;
    }
    public void AddCard(bool isMine)
    {
        var spawnPoint = new Vector3(216f, -390f, 0);
        UI_Card card = Managers.UI.CardSpawn<UI_Card>(parent: GameObject.FindWithTag("Hand").transform, _pos: spawnPoint, _quat: Utils.QI);

        card.Setup(isMine, PopItem());
        (isMine ? myCards : otherCards).Add(card);

        SetOriginOrder(isMine);
        CardAlignment(isMine);
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
        
        var myCardLeft = position.GetMyLeft();
        var myCardRight = position.GetMyRight();
        var otherCardLeft = position.GetOtherLeft();
        var otherCardRight = position.GetOtherRight();

        // 카드 정렬
        List<PRS> originCardPRSs = new List<PRS>();
        if (isMine)
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one);
        else
            originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 0.8f);


        var targetCards = isMine ? myCards : otherCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard._originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard._originPRS, true, 0.97f);
        }
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
            case 4: objLerps = new float[] { 0, 0.3f, 0.6f, 0.9f }; break;   // 네장
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
            if (objCount >= 5)
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

    

    #region 카드 내는 과정
    public bool TryPutCard(bool isMine, UI_Card card)
    {
        if (putCount >= 1)   // 카드 하나 낼 수 있음
            return false;
        Item lastCard = entities[entities.Count - 1].item; // 마지막으로 낸 카드

        if (card == null)
        {
            Debug.Log("받아온 카드가 없습니다");
            return false;
        }
        var spawnPos = Utils.MousePos;
        bool result = false;

        // 특수카드 중 색깔 블랙 (4드로우, 색깔 변경)
        if (card.item.color.Equals("black"))
        {
            SpawnEntity(isMine, card.item, spawnPos);
            myCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            Managers.Input.selectCard = null;
            putCount++;
            CardAlignment(isMine);

            // 색깔 바꾸기 카드 낸 경우
            if (card.item.num == 101 || card.item.num == 100)
            {
                Debug.Log("색깔 변경!");    // 색변경 팝업!
            }
            else if (card.item.num == 200 || card.item.num == 201)
            {
                for (int i = 0; i < 4; i++)
                {
                    AddCard(!isMine);
                }
                CardAlignment(!isMine);
            }
            result = true;
        }
        else
        {
            if (card.item.color == lastCard.color || card.item.num == lastCard.num || lastCard.color.Equals("black"))
            { // 카드 낼 때 조건
                SpawnEntity(isMine, card.item, spawnPos);
                myCards.Remove(card);
                card.transform.DOKill();
                DestroyImmediate(card.gameObject);
                Managers.Input.selectCard = null;
                putCount++;
                CardAlignment(isMine);

                // 리버스 카드, 스킵 카드의 경우 내 차례 다시
                if (card.item.num == 10 || card.item.num == 11)
                {
                    //OnTurnStarted(isMine); 다시 내턴
                    return false;
                }
                // +2 드로우 카드 (색깔 구분 O)
                else if (card.item.num == 12)
                {
                    AddCard(!isMine);
                    AddCard(!isMine);
                    CardAlignment(!isMine);
                }
                result = true;
            }
            else
            {
                CardAlignment(isMine);
                result = false;
            }
        }
        if (result)
        {
            // play(2); 효과음 재생
        }
        return result;
    }
    #endregion
}