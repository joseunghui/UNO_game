using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PVCGameController : MonoBehaviour
{
    public Action<GameMode.PVCMode> GamePlayAction = null;

    public void Clear()
    {
        GamePlayAction = null;
    }
}
