using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] GameObject entityPrefab;
    [SerializeField] List<Entity> entities;
    [SerializeField] Entity myEmptyEntity;
    const int MAX_ENTITY_COUNT = 1;
    int myEmptyEntityIndex => entities.FindIndex(x => x == myEmptyEntity);
    bool ExistMyEmptyEntity => entities.Exists(x => x == myEmptyEntity);
    int i = 0;

    public void EntityAlignment(){
        var targetEntity = entities[0];
        targetEntity.originPos = new Vector3(0, 0, 0);
        targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
        targetEntity.GetComponent<Order>()?.SetOriginOrder(i+10);           // 순서 정렬; 수정 필요
    }
    public void InsertMyEmptyEntity(float xPos){    // 엠티엔티티 만들어서 그안에 넣는..?
        if(!ExistMyEmptyEntity)
            entities.Add(myEmptyEntity);
        Vector3 emptyEntityPos = myEmptyEntity.transform.position;
        emptyEntityPos.x = xPos;
        myEmptyEntity.transform.position = emptyEntityPos;

        int _emptyEntityIndex = myEmptyEntityIndex;
        entities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform));
    }
    public void RemoveMyEmptyEntity(){
        if(!ExistMyEmptyEntity)
            return;
        entities.RemoveAt(myEmptyEntityIndex);
        EntityAlignment();
    }
    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos){
        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();

        entities[myEmptyEntityIndex] = entity;

        entity.isMine = isMine;
        entity.Setup(item);
        EntityAlignment();

        return true;
    }

}
