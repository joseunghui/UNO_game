using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    public GameObject entityPrefab;
    const int MAX_ENTITY_COUNT = 1;
    public List<Entity> entities;
    public List<GameObject> entitiesObj;
    public List<Item> items;

    public void EntityAlignment(){
        var targetEntities = entities;
        for(int i = 0; i < targetEntities.Count; i++){
            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(0, 0, 0);
            targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
        }
    }
    
    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos){
        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();

        entity.Setup(item);
        entities.Add(entity);
        items.Add(item);
        entitiesObj.Add(entityObject);
        if(TurnManager.instance.myTurn == false)
            entity.MoveTransform(Vector3.zero, true, 1f);
        else
            entity.MoveTransform(Vector3.zero, true, 0.5f);
        EntityAlignment();

        return true;
    }

}