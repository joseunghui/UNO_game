using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVCGameController : MonoBehaviour
{
    public GameMode.PVCMode GameLevel { get; protected set; } = GameMode.PVCMode.EASY;


    static PVCGameController g_instance; // ���ϼ��� ����ȴ�
    static PVCGameController Instance { get { init(); return Instance; } } // ������ �Ŵ����� ����´�

    // TurnManager _turn = new TurnManager();

    // public static TurnManager Turn { get { return Instance._turn; } }

    // ����
    public WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    public WaitForSeconds delay02 = new WaitForSeconds(1f);

    static void init()
    {
        if (g_instance == null)
        {


        }
    }
}
