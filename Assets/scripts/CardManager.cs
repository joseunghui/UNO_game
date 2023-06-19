using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;
    [SerializeField] Transform cardSpawnPoint;


    List<Item> itemBuffer;

    public Item PopItem(){          // 카드 다뽑으면 다시 만드는건데.. 이건 수정 필요함
        if(itemBuffer.Count == 0)
            SetUpItemBuffer();
        
        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    void SetUpItemBuffer(){
        itemBuffer = new List<Item>();
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
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Keypad1)){
            print(PopItem().sprite);
            AddCard(true);}
        if(Input.GetKeyDown(KeyCode.Keypad2)){
            print(PopItem().sprite);
            AddCard(false);}
    }
    void AddCard(bool isMine){
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        print(isMine);
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
        var targetCards = isMine ? myCards : otherCards;
        for(int i=0; i<targetCards.Count; i++){
            var targetCard = targetCards[i];

            targetCard.originPRS = new PRS(Vector3.zero, Utils.QI, Vector3.one * 1.9f);
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

}
