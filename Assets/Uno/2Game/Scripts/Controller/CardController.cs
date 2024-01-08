using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class CardController : MonoBehaviour
{
    protected bool isLoading; // ���� ������ true�� �ؼ� Ŭ�� ����
    public bool myTurn;
    public int startCardCount = 5;

    int originOrder;
    Renderer[] backRenderers;
    string sortingLayerName;

    public List<UI_Card> otherCards = new List<UI_Card>();
    public List<UI_Card> myCards = new List<UI_Card>();
    public static Action<bool> OnAddCard;
    public static Action<bool> onStartCard;
    public static event Action<bool> OnTurnStarted;

    Transform myCardLeft;
    Transform myCardRight;
    Transform otherCardLeft;
    Transform otherCardRight;

    ECardState eCardState;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }

    UI_Card selectCard;
    bool isMyCardDrag;
    bool OnMyCardArea;
    int putCount;

    public IEnumerator StartGameCo()
    {
        myTurn = Random.Range(0, 2) == 0;

        isLoading = true;

        Debug.Log($"startCardCount >> {startCardCount}");

        for (int i = 0; i < startCardCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            AddCard(false);
            Debug.Log($"check");
            yield return new WaitForSeconds(0.1f);
            AddCard(true);
        }
        yield return new WaitForSeconds(0.1f);
        onStartCard?.Invoke(true);
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;
       
        yield return new WaitForSeconds(0.1f);
        isLoading = false;
        if (myTurn == false)
            yield return new WaitForSeconds(1f);
        OnTurnStarted?.Invoke(myTurn);

        Managers.UI.ShowPopup<UI_NotificationPopup>();

    }

    public void AddCard(bool isMine)
    {
        Debug.Log($"AddCard");

        UI_Card card = Managers.UI.CardSpawn<UI_Card>(parent:null, _pos: myCardLeft.position, _quat: Utils.QI);
        // var cardObject = Instantiate(this, gameObject.transform.position, Utils.QI);
        // var card = cardObject.GetOrAddComponent<UI_Card>();

        card.Setup(isMine);
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
            targetCard?.GetComponent<CardController>().SetOriginOrder(i + 1);
        }
    }

    public void CardAlignment(bool isMine)
    {
        myCardLeft = Managers.UI.CreatePostionSpot<PostionSpot>(_prs: Define.CardPRS.Left).transform;
        myCardRight = Managers.UI.CreatePostionSpot<PostionSpot>(_prs: Define.CardPRS.Right).transform;
        otherCardLeft = Managers.UI.CreatePostionSpot<PostionSpot>(_prs: Define.CardPRS.Left).transform;
        otherCardRight = Managers.UI.CreatePostionSpot<PostionSpot>(_prs: Define.CardPRS.Right).transform;
        
        // ī�� ����
        List<PRS> originCardPRSs = new List<PRS>();
        if (isMine)
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one);
        else
            originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 0.8f);


        var targetCards = isMine ? myCards : otherCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            // targetCard.originPRS = originCardPRSs[i];
            // targetCard.MoveTransform(targetCard.originPRS, true, 0.97f);
        }
    }
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount]; // �� ī���� ��ġ
        List<PRS> results = new List<PRS>(objCount);    // (objCount) : �Ҵ��� �޸� ũ��

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;               // ī�� ������ ��
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;       // ����
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;   // ����
            default:                                                    // �� �̻�
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;

        }

        for (int i = 0; i < objCount; i++)
        {   // ���� ������
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if (objCount >= 4)
            {  // 3�������� �ձ۰� ��ġ�� �ʿ䰡 �����
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 500 : originOrder);
    }

    public void SetOrder(int order)
    {
        int mulOrder = order * 10;

        foreach (var renderer in backRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }
    }
    /*
    #region MyCard
    public void CardMouseOver(UI_Card card)
    {
        if (eCardState == ECardState.Nothing)
            return;
        selectCard = card;
        EnlargeCard(true, card);
    }
    public void CardMouseExit(UI_Card card)
    {
        EnlargeCard(false, card);
    }
    public void CardMouseDown()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;
        isMyCardDrag = true;

    }
    public void CardMouseUp()
    {
        isMyCardDrag = false;
        if (eCardState != ECardState.CanMouseDrag)
            return;
        if (OnMyCardArea)
            //Entity.EntityAlignment();
        else
        {
            if (TryPutCard(true))
            {
                Turn.EndTurn();
            }
        }
    }
    void CardDrag()
    {
        if (eCardState != ECardState.CanMouseDrag)
            return;

        if (!OnMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            Entity.EntityAlignment();
        }
    }
    void DetectCardArea()
    {  // MyCardArea�� ���콺�� ��ġ�� �κ��� ������ true
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        OnMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }
    void EnlargeCard(bool isEnlarge, UI_Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -2.1f, -5f);   // x�� �״�� y �ø��� z�� ������ �M
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.4f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }
    void SetECardState()
    {
        if (isLoading)  // ���� �ε����϶� �ƹ��͵� �ȵǰ�
            eCardState = ECardState.Nothing;
        else if (!myTurn || putCount == 1)   // �巡�� ���ϰ�
            eCardState = ECardState.CanMouseOver;
        else if (myTurn && putCount == 0)    // �� ���϶� ����
            eCardState = ECardState.CanMouseDrag;

    }
    #endregion

    #region ī�� ���� ����
    public bool TryPutCard(bool isMine)
    {
        if (putCount >= 1)   // ī�� �ϳ� �� �� ����
            return false;
        List<Item> items = Entity.items;
        Item item = items[items.Count - 1]; // ���������� �� ī��
        Card card = isMine ? selectCard : OtherCard(item, 1);

        if (card == null)
        {
            Turn.EndTurn();
            return false;
        }
        var spawnPos = isMine ? Utils.MousePos : otherCardRight.position;
        var targetCards = isMine ? myCards : otherCards;
        bool result = false;


        // Ư��ī�� �� ���� �� (4��ο�, ���� ����)
        if (card.item.color.Equals("black"))
        {
            Entity.SpawnEntity(isMine, card.item, spawnPos);
            targetCards.Remove(card);
            card.transform.DOKill();
            DestroyImmediate(card.gameObject);
            selectCard = null;
            putCount++;
            CardAlignment(isMine);

            // ���� �ٲٱ� ī�� �� ���
            if (card.item.num == 101 || card.item.num == 100)
            {
                Debug.Log("���� ����!");
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
            if (card.item.color == item.color || card.item.num == item.num || item.color.Equals("black"))
            { // ī�� �� �� ����
                Entity.SpawnEntity(isMine, card.item, spawnPos);
                targetCards.Remove(card);
                card.transform.DOKill();
                DestroyImmediate(card.gameObject);
                selectCard = null;
                putCount++;
                CardAlignment(isMine);

                // ������ ī��, ��ŵ ī���� ��� �� ���� �ٽ�
                if (card.item.num == 10 || card.item.num == 11)
                {
                    OnTurnStarted(isMine);
                    return false;
                }
                // +2 ��ο� ī�� (���� ���� O)
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
                targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false)); //origin order �����
                CardAlignment(isMine);
                result = false;
            }
        }
        if (result)
        {
            play(2);
        }
        return result;
    }
    #endregion
    #region OtherCard
    public Card OtherCard(Item item, int count)// int count�� �� ī�� ���� �� ��� ī�� �Դ� �� ������
    {
        Card card;

        if (item.color == "black")   //���̸� �ƹ��ų�
            card = otherCards[Random.Range(0, otherCards.Count)];
        else
        {
            var targetCards = otherCards.FindAll(x => x.item.color.Equals(item.color) || x.item.num == item.num || x.item.color.Equals("black"));
            if (targetCards.Count <= 0 && count == 1)
            {
                AddCard(false);
                OtherCard(item, 0);
            }
            if (targetCards.Count <= 0)
            {
                card = null;
            }
            else
                card = targetCards[0];
            //if(Mode == easy) card = targetCards.Find(p => p.item.num < 100)
        }
        return card;
    }
    #endregion*/
}
