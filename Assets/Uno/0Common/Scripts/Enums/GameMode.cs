using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    // 유저간 대결(PVP), AI와 대결(PVC)
    public enum GameType
    {
        PVP,
        PVC,
    }

    // PVC 모드에서 레벨(난이도)에 따른 모드
    public enum PVCMode
    {
        EASY,
        NORMAL,
        HARD,
    }
}
