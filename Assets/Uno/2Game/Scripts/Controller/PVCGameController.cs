using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PVCGameController : MonoBehaviour
{
    GameMode.PVCMode GameLevel = GameMode.PVCMode.None;
    public Action<GameMode.PVCMode> GamePlayAction = null;

    private void Start()
    {

    }

    public void SetPVCGameLevel(GameMode.PVCMode _mode)
    {
        GameLevel = _mode;
    }

    public void Clear()
    {
        GamePlayAction = null;
    }
}
