using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameScene : BaseScene
{
    public GameMode.PVCMode gameMode = GameMode.PVCMode.None;

    [SerializeField] public Sprite cardBackImage;
    [SerializeField] public ItemSO itemSO;
    public List<Item> itemBuffer;

    Stack<Item> entityItems;

    CardController cardController;

    protected override void Init()
    {
        base.Init();

        UserInfoData data = Managers.Data.GetUserInfoData();

        ScenType = Define.Scene.Game; // here is Game Scene

        Managers.UI.ShowPopup<UI_LevelSelect>();

        UI_GameBar gameBar = Managers.UI.MakeSubItemInTop<UI_GameBar>();
        gameBar.transform.localScale = Vector3.one;
        gameBar.transform.localPosition = Vector3.zero;
        gameBar.SetUserData(data.nickname, (data.freeDia + data.payDia).ToString());
        gameBar.SetUserHeartDate(data.heart);

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        // CardSetting();
    }

    public GameMode.PVCMode GetGameMode()
    {
        return gameMode;
    }


    public void CardSetting()
    {
        // 전체 카드 생성 후 섞기
        SetUpItemBuffer();

        // 랜덤으로 턴 지정
        cardController = Utill.GetOrAddComponent<CardController>(gameObject);
        StartCoroutine(cardController.StartGameCo());

        // 낸 카드 쌓는 더미 위치 지정
        UI_Entity entity = Managers.UI.CardSpawn<UI_Entity>(parent: gameObject.transform.parent, _pos: Vector3.zero, _quat: Utils.QI);
        entity.GetOrAddComponent<UI_Entity>();

        // 시작 카드 한장 뒤집기
        entity.SpawnEntity(true, PopItem(), Vector3.zero);
    }

    public Item PopItem()
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
    }

    void SetUpItemBuffer()
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

    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }


}
