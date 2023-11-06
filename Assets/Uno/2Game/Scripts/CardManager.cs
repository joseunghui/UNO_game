using System;
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
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform otherCardRight;
    [SerializeField] ECardState eCardState;

    public List<Card> otherCards;
    public List<Card> myCards;
    public AudioClip AddCardSound;
    public AudioClip DownCardSound;
    List<Item> itemBuffer;
    List<Item> items;
    Card selectCard;
    bool isMyCardDrag;
    bool OnMyCardArea;
    enum ECardState {Nothing, CanMouseOver, CanMouseDrag}
    int putCount;
    private int timeLimit = StartGame.TurnlimitTime;

    public void play(int clip){
        AudioSource audioSource = GetComponent<AudioSource>();
        if(clip == 1) 
           audioSource.clip = AddCardSound;
        else
            audioSource.clip = DownCardSound;
        audioSource.Play();
    }

    public Item PopItem(){
        if(itemBuffer.Count == 0){
            items = EntityManager.Inst.items;
            for(int i = 0; i<items.Count-1; i++){
                itemBuffer.Add(items[i]);     
            }
            for(int i = items.Count-2; i >= 0; i--){
                EntityManager.Inst.items.Remove(items[i]);
            }
            MixCard();
        }
        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    void SetUpItemBuffer(){
        itemBuffer = new List<Item>(150);
        for(int i = 0; i<itemSO.items.Length; i++){ // 카드 갯수만큼 반복
            Item item = itemSO.items[i];
            for(int j=0; j<2; j++)                  // 카드 같은거 두개씩이니 두번 반복
                itemBuffer.Add(item);
        } 
        MixCard();
    }

    void MixCard(){
        for(int i=0; i<itemBuffer.Count; i++){      // 카드 섞기
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp; 
        }
    }

    void Start()
    {
        Debug.Log("사간제한 : " + timeLimit);
        SetUpItemBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
        TurnManager.onStartCard += StartCard;
    }
    void OnDestroy(){
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;
        TurnManager.onStartCard -= StartCard;

    }
    void OnTurnStarted(bool myTurn){    // 내턴 시작하면 놓을 수 있는 개수 초기화
        putCount = 0;
        if(myTurn == false){
            if(TryPutCard(myTurn))
                TurnManager.instance.EndTurn();
        }
    }
    void Update(){
        if(isMyCardDrag)
            CardDrag();
        
        DetectCardArea();
        SetECardState();
    }
    
    void StartCard(bool isFront){
        EntityManager.Inst.SpawnEntity(isFront, PopItem(), Vector3.zero);
    }

    void AddCard(bool isMine){
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        (isMine ? myCards : otherCards).Add(card);
        
        //play(1);

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
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one);
        else
            originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 0.8f);


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

    #region 카드 내는 과정
    public bool TryPutCard(bool isMine){
        if(putCount >= 1)   // 카드 하나 낼 수 있음
            return false;
        Debug.Log(isMine);
        items = EntityManager.Inst.items;
        Item item = items[items.Count-1]; // 마지막으로 낸 카드
        Card card = isMine ? selectCard : OtherCard(item, 1);
        
        if(card == null){
            Debug.Log("왔나");
            TurnManager.instance.EndTurn();
            return false;
        }
        var spawnPos = Vector3.zero;
        var targetCards = isMine ? myCards : otherCards;
        bool result = false;
        

        // 특수카드 중 색깔 블랙 (4드로우, 색깔 변경)
        if (card.item.color.Equals("black"))
        {
            EntityManager.Inst.SpawnEntity(isMine, card.item, spawnPos);
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            selectCard = null;
            putCount++;
            CardAlignment(isMine);

            // 색깔 바꾸기 카드 낸 경우
            if (card.item.num == 101 || card.item.num == 100)
            {
                Debug.Log("색깔 변경!");
            }
            else if (card.item.num == 200 || card.item.num == 201)
            {
                for (int i=0; i<4; i++)
                {
                    AddCard(!isMine);
                }
                CardAlignment(false);
            }
            result = true;
        }
        else
        {
            if(card.item.color == item.color || card.item.num == item.num || item.color.Equals("black")) { // 카드 낼 때 조건
                EntityManager.Inst.SpawnEntity(isMine, card.item, spawnPos);
                targetCards.Remove(card);
                card.transform.DOKill();
                DestroyImmediate(card.gameObject);
                selectCard = null;
                putCount++;
                CardAlignment(isMine);

                // 리버스 카드, 스킵 카드의 경우 내 차례 다시
                if (card.item.num == 10 || card.item.num == 11)
                {
                    OnTurnStarted(isMine);
                    return false;
                }
                // +2 드로우 카드 (색깔 구분 O)
                else if (card.item.num == 12)
                {
                    AddCard(!isMine);
                    AddCard(!isMine);
                    CardAlignment(false);
                    result = true;
                }
                result = true;
            } else {
                targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false)); //origin order 만들기
                CardAlignment(isMine);
                result = false;
            }
        }
        if(result){
            //play(2);
        }
        return result;
    }
    #endregion
    #region OtherCard
    public Card OtherCard(Item item, int count)// int count는 낼 카드 없을 때 계속 카드 먹는 거 방지용
    {
        Debug.Log(item.color + ", "+ item.num);
        Card card;

        if(item.color == "black")   //블랙이면 아무거나
            card = otherCards[Random.Range(0, otherCards.Count)];
        else{
            var targetCards = otherCards.FindAll(x => x.item.color.Equals(item.color) || x.item.num == item.num);
            for(int i=0;i<targetCards.Count-1;i++){
                Debug.Log(targetCards[i].item.color+ ",♡ "+targetCards[i].item.num);   
            }
            if(targetCards.Count <= 0 && count == 1){
                AddCard(false);
                OtherCard(item, 0);
            }  
            if(targetCards.Count <= 0){
                card = null;
            }
            else
                card = targetCards[0];
            //if(Mode == easy) card = targetCards.Find(p => p.item.num < 100)
        }
        return card;
    }
    #endregion
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
            EntityManager.Inst.EntityAlignment();
        else{
            if(TryPutCard(true)){
                TurnManager.instance.EndTurn();
            }
        }
            
            
    }
    void CardDrag(){
        if(eCardState != ECardState.CanMouseDrag)
            return;
        
        if(!OnMyCardArea){
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            EntityManager.Inst.EntityAlignment();
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
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.4f), false);
        } else
            card.MoveTransform(card.originPRS, false);
        
        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }
    void SetECardState(){
        if(TurnManager.instance.isLoading)  // 게임 로딩중일땐 아무것도 안되고
            eCardState = ECardState.Nothing;
        else if(!TurnManager.instance.myTurn || putCount == 1)   // 드래그 못하게
            eCardState = ECardState.CanMouseOver;
        else if(TurnManager.instance.myTurn && putCount == 0)    // 내 턴일땐 가능
            eCardState = ECardState.CanMouseDrag;
        
    }
    #endregion
}
