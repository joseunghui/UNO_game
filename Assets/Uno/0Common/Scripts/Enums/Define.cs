public class Define
{
    // Scene Ÿ�� ����
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
    public enum UI_Popup
    {
        UI_SignUp,
        UI_SignIn,
        UI_NotificationPopup,
    }

    public enum UI_Scene
    {
        UI_LoadingVideo
    }

    public enum Images
    {
        LoadingImage,
        LoadingBar,

        NotiImage,

    }

    public enum Buttons
    {
        EnterGameButton,

        CloseBtn,
        DoBtn,
    }

    public enum Texts
    {
        EnterGameText,
        NotiText,
        IDText,
        
    }

    public enum Videos
    {
        LoadingVideo,

    }

    public enum InputFields
    {
        IDInputField,
        PasswordInputField,

    }
    public enum LoadgingVideo
    {
        UI_LoadingVideo,
        LoadingImage,
        LoadingVideo,
        LoadingBar,
    }
    #endregion
}