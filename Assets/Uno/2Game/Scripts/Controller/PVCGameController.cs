using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVCGameController : MonoBehaviour
{
    public GameMode.PVCMode GameLevel { get; protected set; } = GameMode.PVCMode.EASY;


    static PVCGameController g_instance; // ���ϼ��� ����ȴ�
    static PVCGameController Instance { get { init(); return Instance; } } // ������ �Ŵ����� ����´�

    TurnManager _turn = new TurnManager();
    EntityManager _entity = new EntityManager();
    CardManager _card = new CardManager();

    public static TurnManager Turn { get { return Instance._turn; } }
    public static EntityManager Entity { get { return Instance._entity; } }
    public static CardManager Card { get { return Instance._card; } }

    static void init()
    {
        if (g_instance == null)
        {


        }
    }
}
