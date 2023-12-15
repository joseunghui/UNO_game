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

    // Scene
    public enum LoadgingVideo
    {
        UI_LoadingVideo,
        LoadingImage,
        LoadingVideo,
        LoadingBar,
    }


    // Popup
    public enum Popups
    {
        UI_SignIn,
        UI_SignUp,
    }
    public enum NotiPopup
    {
        UI_NotificationPopup,
        NotiImage,
        NotiText,
    }
    public enum Progress
    {
        UI_ProgressBar,
    }

    // Texts
    public enum Texts
    {
        EnterGameText,
    }

    // Button
    public enum Buttons
    {
        EnterGameButton,
    }

    // Image
    public enum Images
    {
        EnterGameImage,
    }

    // Game Object(SubItem)
    public enum EnterGame
    {
        UI_EnterGame,
    }
    #endregion
}