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

    // Data
    public enum UpdateDateSort
    {
        UsingDia,
        UsingHeart,
        RecodingGameResult,
        ChangeNick,

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
        UI_LevelSelect,
    }

    public enum UI_Scene
    {
        UI_LoadingVideo,
        UI_Ranking,
    }

    public enum UI_SubItems
    {
        UI_GameMode,
    }

    public enum Images
    {
        LoadingImage,
        LoadingBar,
        MyGradeIconImage,
        NotiImage,

        DiaIconImage,
        HeartIconImage,

        GoodsIconImage,
    }

    public enum Buttons
    {
        CloseBtn,
        DoBtn,

        EnterGameButton,

        NickChangeBtn,
        EnterPVPGameBtn,
        EnterPVCGameBtn,

        EasyBtn,
        NormalBtn,
        HardBtn,

        OptionBtn,
        RestartGameBtn,
        StopGameBtn,


        HeartPlusBtn,

    }

    public enum Texts
    {
        EnterGameText,
        NotiText,
        IDText,

        RankText,
        RankerNickText,

        MyNicknameText,
        UpdateNicknameText,
        NeededDiaText,
        MyDiaText,

        GameModeText,
        TimerText,

        GoodsText,
    }

    public enum Groups
    {
        RankingList,
        GoodsList,
    }

    public enum Videos
    {
        LoadingVideo,

    }

    public enum Sliders
    {
        BGMVolumeSlider,
        EffectVolumeSlider,
    }

    public enum InputFields
    {
        IDInputField,
        PasswordInputField,
        NicknameInputField,

    }
    public enum LoadgingVideo
    {
        UI_LoadingVideo,
        LoadingImage,
        LoadingVideo,
        LoadingBar,
    }

    public enum Goods
    {
        Heart = 0,
        Dia = 1,
    }
    #endregion
}