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
        // ��ü ī�� ���� �� ����
        SetUpItemBuffer();

        // �������� �� ����
        cardController = Utill.GetOrAddComponent<CardController>(gameObject);
        StartCoroutine(cardController.StartGameCo());

        // �� ī�� �״� ���� ��ġ ����
        UI_Entity entity = Managers.UI.CardSpawn<UI_Entity>(parent: gameObject.transform.parent, _pos: Vector3.zero, _quat: Utils.QI);
        entity.GetOrAddComponent<UI_Entity>();

        // ���� ī�� ���� ������
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
        { // ī�� ������ŭ �ݺ�
            Item item = itemSO.items[i];
            for (int j = 0; j < 2; j++)  // ī�� ������ �ΰ����̴� �ι� �ݺ�
                itemBuffer.Add(item);
        }
        MixCard();
    }


    // Card Mix ī�弯��
    void MixCard()
    {
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            // ī�� ����
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
