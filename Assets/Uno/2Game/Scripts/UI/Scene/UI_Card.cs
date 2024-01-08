using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UI_Card : UI_Scene
{
    CardController cardController;

    Item item;
    PRS originPRS;
    bool isFront;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }
    
    // 시작 위치에 생성 되는 카드 더미
    public void FirstInitiation()
    {
        /*// 전체 카드 생성 후 섞기
        SetUpItemBuffer();

        // 랜덤으로 턴 지정
        cardController = Utill.GetOrAddComponent<CardController>(gameObject);
        StartCoroutine(cardController.StartGameCo());

        // 낸 카드 쌓는 더미 위치 지정
        UI_Entity entity = Managers.UI.CardSpawn<UI_Entity>(parent: gameObject.transform.parent,_pos: Vector3.zero, _quat: Utils.QI);

        Debug.Log("시작 카드 한장 뒤집기 전");
        Debug.Log($"entity >> {entity.name}");

        // 시작 카드 한장 뒤집기
        entity.SpawnEntity(true, PopItem(), Vector3.zero);*/
    }


    /*void SetUpItemBuffer()
    {
        itemBuffer = new List<Item>(150);
        for (int i = 0; i < itemSO.items.Length; i++)
        { // 카드 갯수만큼 반복
            Item item = itemSO.items[i];
            for (int j = 0; j < 2; j++)  // 카드 같은거 두개씩이니 두번 반복
                itemBuffer.Add(item);
        }
        MixCard();
    }

    // Card Mix 카드섞기
    void MixCard()
    {
        for (int i = 0; i < itemBuffer.Count; i++)
        {   
            // 카드 섞기
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    Item PopItem()
    {
        if (itemBuffer.Count == 0)
        {
            List<Item> items = entityItems.ToArray().ToList();
            int count = items.Count;
            for (int i = 0; i < count - 1; i++)
            {
                itemBuffer.Add(items[i]);
                items.RemoveAt(i);
            }
            MixCard();
        }
        Item item = itemBuffer[0];
        Debug.Log(item.num + ", " + item.color);
        itemBuffer.RemoveAt(0);
        return item;
    }*/

    public void Setup(bool isFront)
    {
        GameScene gameScene = GameObject.FindWithTag("Scene").GetComponent<GameScene>();

        Item item = gameScene.PopItem();

        this.item = item;
        this.isFront = isFront;

        Bind<Image>(typeof(Define.Images));

        Image cardImage = Get<Image>((int)Define.Images.CardImage);

        if (this.isFront)
        {
            cardImage.sprite = this.item.sprite;
        }
        else
            cardImage.sprite = gameScene.cardBackImage;
    }

    void OnMouseOver()
    {
        //if (isFront)
            //cardController.CardMouseOver(this);
    }


    void OnMouseExit()
    {
        //if (isFront)
            //cardController.CardMouseExit(this);
    }
    void OnMouseDown()
    {
        //if (isFront)
            //cardController.CardMouseDown();
    }
    void OnMouseUp()
    {
        //if (isFront)
            //cardController.CardMouseUp();
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
}
