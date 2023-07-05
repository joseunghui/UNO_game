using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject entityPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;
    [SerializeField] List<Card> putCards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform otherCardRight;
    [SerializeField] ECardState eCardState;


    List<Item> itemBuffer;
    Card selectCard;
    bool isMyCardDrag;
    bool OnMyCardArea;
    enum ECardState {Nothing, CanMouseOver, CanMouseDrag}
    int myPutCount;

    public Item PopItem(){          // 카드 다뽑으면 다시 만드는건데.. 이건 수정 필요함
        if(itemBuffer.Count == 0)
            SetUpItemBuffer();
        
        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    void SetUpItemBuffer(){
        itemBuffer = new List<Item>(100);
        for(int i = 0; i<itemSO.items.Length; i++){ // 카드 갯수만큼 반복
            Item item = itemSO.items[i];
            for(int j=0; j<2; j++)                  // 카드 같은거 두개씩이니 두번 반복
                itemBuffer.Add(item);
        }
        
        for(int i=0; i<itemBuffer.Count; i++){      // 카드 섞기
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
            
        }
    }
    void Start()
    {
        SetUpItemBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
    }
    void OnDestroy(){
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }
    void OnTurnStarted(bool myTurn){    // 내턴 시작하면 놓을 수 있는 개수 초기화
        if(myTurn)
            myPutCount = 0;
    }
    void Update(){
        if(isMyCardDrag)
            CardDrag();
        
        DetectCardArea();
        SetECardState();
    }
    
    public void StartCard(){
        var cardObject = Instantiate(entityPrefab, Vector3.zero, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), true);print(card);
        putCards.Add(card);             
        EntityManager.Inst.EntityAlignment();
    }
    void AddCard(bool isMine){
        var spawnPos = Vector3.zero;
        var cardObject = Instantiate(cardPrefab, spawnPos, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        (isMine ? myCards : otherCards).Add(card);

        SetOriginOrder(isMine);
        CardAlignment(isMine);
    }
    void SetOriginOrder(bool isMine){
        int count = isMine ? myCards.Count : otherCards.Count;
        for(int i=0; i<count; i++){
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }
    void CardAlignment(bool isMine){    // 카드 정렬
        List<PRS> originCardPRSs = new List<PRS>();
        if(isMine)
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 1.4f);
        else
            originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one);


        var targetCards = isMine ? myCards : otherCards;
        for(int i=0; i<targetCards.Count; i++){
            var targetCard = targetCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.97f);
        }
    }
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale){
        float[] objLerps = new float[objCount]; // 각 카드의 위치
        List<PRS> results = new List<PRS>(objCount);    // (objCount) : 할당할 메모리 크기

        switch(objCount){
            case 1: objLerps = new float[] {0.5f}; break;               // 카드 한장일 때
            case 2: objLerps = new float[] {0.27f, 0.73f}; break;       // 두장
            case 3: objLerps = new float[] {0.1f, 0.5f, 0.9f}; break;   // 세장
            default:                                                    // 그 이상
                float interval = 1f / (objCount -1);
                for(int i=0; i<objCount; i++)
                    objLerps[i] = interval * i;
                break;

        }

        for(int i=0;i<objCount; i++){   // 원의 방정식
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if(objCount >= 4){  // 3개까지는 둥글게 배치할 필요가 없대요
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }
    public bool TryPutCard(bool isMine){
        if(isMine && myPutCount >= 1)   // 카드 하나 낼 수 있음
            return false;
        Card card = selectCard;
        var spawnPos = Vector3.zero;
        var targetCards = isMine ? myCards : otherCards;
        if(EntityManager.Inst.SpawnEntity(isMine, card.item, spawnPos)){
            putCards.Add(card);
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            if(isMine){
                selectCard = null;
                myPutCount++;
            }
            CardAlignment(isMine);
            return true;
        } else{
            targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false)); //origin order 만들기
            CardAlignment(isMine);
            return false;
        }
    }
    
    
#region MyCard
    public void CardMouseOver(Card card){
        if(eCardState == ECardState.Nothing)
            return;
        selectCard = card;
        EnlargeCard(true, card);
    }
    public void CardMouseExit(Card card){
        EnlargeCard(false, card);
    }
    public void CardMouseDown(){
        if(eCardState != ECardState.CanMouseDrag)
            return;
        isMyCardDrag = true;
        
    }
    public void CardMouseUp(){
        isMyCardDrag = false;
        if(eCardState != ECardState.CanMouseDrag)
            return;
        if(OnMyCardArea)
            EntityManager.Inst.RemoveMyEmptyEntity();
        else
            TryPutCard(true);
            TurnManager.Inst.EndTurn();
    }
    void CardDrag(){
        if(eCardState != ECardState.CanMouseDrag)
            return;
        
        if(!OnMyCardArea){
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            EntityManager.Inst.InsertMyEmptyEntity(Utils.MousePos.x);
        }
    }
    void DetectCardArea(){  // MyCardArea랑 마우스랑 겹치는 부분이 있으면 true
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        OnMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }
    void EnlargeCard(bool isEnlarge, Card card){
        if(isEnlarge){
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -2.1f, -5f);   // x는 그대로 y 올리고 z는 앞으로 뻄
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.6f), false);
        } else
            card.MoveTransform(card.originPRS, false);
        
        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }
    void SetECardState(){
        if(TurnManager.Inst.isLoading)  // 게임 로딩중일땐 아무것도 안되고
            eCardState = ECardState.Nothing;
        else if(!TurnManager.Inst.myTurn || myPutCount == 1)   // 드래그 못하게
            eCardState = ECardState.CanMouseOver;
        else if(TurnManager.Inst.myTurn && myPutCount == 0)    // 내 턴일땐 가능
            eCardState = ECardState.CanMouseDrag;
        
    }
#endregion
}
