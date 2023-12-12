using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    // ������ ���(PVP), AI�� ���(PVC)
    public enum GameType
    {
        PVP,
        PVC,
    }

    // PVC ��忡�� ����(���̵�)�� ���� ���
    public enum PVCMode
    {
        EASY,
        NORMAL,
        HARD,
    }
}
