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

    // SO
    public enum SO
    {
        ItemSO,
    }

    // Data
    public enum UpdateDateSort
    {
        UsingDia,
        UsingHeart,
        RecodingGameResult,
        ChangeNick,

        PostReward,
    }

    // right - left
    public enum CardPRS
    {
        Right,
        Left,
        Bottom,
        Top,
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
        UI_Card,
    }


    public enum UI_SubItems
    {
        UI_GameMode,
        UI_Turn,
        UI_Entity,
    }

    public enum UI_Transform
    {
        PostionSpot,
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

        CardImage,
        PutCardImage,
    }

    public enum Buttons
    {
        CloseBtn,
        DoBtn,

        EnterGameButton,
        
        EnterPVPGameBtn,
        EnterPVCGameBtn,

        EasyBtn,
        NormalBtn,
        HardBtn,

        OptionBtn,
        RestartGameBtn,
        StopGameBtn,

        MailBtn,
        ShopBtn,
        NickChangeBtn,

        ReceiveAllBtn,

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

        MailContentText,
        GoodsText,

        AddCardText,
        EndTurnText,
    }

    public enum Groups
    {
        RankingList,
        GoodsList,
        MailList,
        HeartIconList,
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