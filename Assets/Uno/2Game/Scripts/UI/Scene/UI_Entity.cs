using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.EventSystems.EventTrigger;
using System.Linq;
using static UnityEditor.Progress;
using System.Drawing;
using UnityEngine.UI;

public class UI_Entity : UI_Scene
{
    CardController cardController;
    public List<UI_Entity> entities = new List<UI_Entity>();
    public List<GameObject> entitiesObj;
    public Stack<Item> _items = new Stack<Item>();

    Item item;
    public int num;
    public string color;
    public Sprite sprite;
    public Vector3 originPos;


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Debug.Log("Entity Instantiate");
        cardController = Utill.GetOrAddComponent<CardController>(gameObject);
    }


    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos)
    {
        var entityObject = Instantiate(gameObject, spawnPos, Utils.QI);
        var entity = entityObject.GetOrAddComponent<UI_Entity>();

        entity.Setup(item);

        entities.Add(entity);
        _items.Push(item);
        // entitiesObj.Add(entityObject);

        cardController = Utill.GetOrAddComponent<CardController>(gameObject);

        if (cardController.myTurn == false)
            MoveTransform(Vector3.zero, true, 1f);
        else
            MoveTransform(Vector3.zero, true, 0.5f);

        EntityAlignment();

        return true;
    }

    public void Setup(Item tempItem)
    {
        num = tempItem.num;
        color = tempItem.color;
        item = tempItem;

        Bind<GameObject>(typeof(Define.Images));
        GameObject putCardImage = Get<GameObject>((int)Define.Images.PutCardImage);
        putCardImage.GetComponent<SpriteRenderer>().sprite = tempItem.sprite;
        
    }

    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }

    public void EntityAlignment()
    {
        var targetEntities = entities;

        for (int i = 0; i < targetEntities.Count; i++)
        {
            var targetEntity = targetEntities[i];
            
            targetEntity.originPos = new Vector3(0, 0, 0);
            targetEntity.gameObject.GetComponent<CardController>()?.SetOriginOrder(i);
        }
    }
}
