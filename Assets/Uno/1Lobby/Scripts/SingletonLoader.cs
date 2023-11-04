using System.Transactions;
using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
    public GameObject entityManager;
    public GameObject gameManager;
    public GameObject turnManager;

    void Awake()
    {
        EntityManager.Load(entityManager);
        GameManager.Load(gameManager);
        TurnManager.Load(turnManager);
    }
}
