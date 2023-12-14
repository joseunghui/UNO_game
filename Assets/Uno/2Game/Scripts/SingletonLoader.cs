using System.Transactions;
using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
    public GameObject entityManager;
    public GameObject turnManager;
    public GameObject cardManager;

    void Awake()
    {
        EntityManager.Load(entityManager);
        TurnManager.Load(turnManager);
        CardManager.Load(cardManager);
    }
}
