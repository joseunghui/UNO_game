using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Define
{
    // Scene 타입 관리
    public enum Scene
    {
        UnKnown,
        Login,
        Loading,
        Main,
        Game,
        Match,
    }

    // Sound
    public enum Sound
    {
        BGM,
        Effect,
        MaxCount,
    }

    #region Event
    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        Click,
        DoubleClick,
    }
    #endregion



    #region UI
    // Popup
    public enum Popups
    {

    }

    // Scene
    public enum GameObjects
    {

    }

    // Game Object
    public enum Progress
    {
        ProgressBar,
    }
    #endregion
}