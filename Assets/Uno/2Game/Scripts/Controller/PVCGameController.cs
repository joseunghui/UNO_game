using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVCGameController : MonoBehaviour
{
    public GameMode.PVCMode GameLevel { get; set; } = GameMode.PVCMode.None;
    public Action<GameMode.PVCMode> GamePlayAction = null;


    private void Start()
    {
        
    }

    private void Update()
    {
        if (GameLevel != GameMode.PVCMode.None)
        {
            StartCoroutine("CoTimerSetting", (int)GameLevel);
        }
    }

    IEnumerator CoTimerSetting(int limitTime)
    {
        Debug.Log($"{GameLevel} Game Start! >> time : {limitTime}");
        yield break;
    }

    public void Clear()
    {
        GamePlayAction = null;
    }
}
