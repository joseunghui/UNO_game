using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Option : UI_Popup
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Define.Buttons));
        Bind<Slider>(typeof(Define.Sliders));

        // sound volume
        Slider BGMVolumeSlider = Get<Slider>((int)Define.Sliders.BGMVolumeSlider);
        Slider EffectVolumeSlider = Get<Slider>((int)Define.Sliders.EffectVolumeSlider);

        BGMVolumeSlider.value = Managers.Sound.GetAudioSourceVolume(Define.Sound.BGM);
        EffectVolumeSlider.value = Managers.Sound.GetAudioSourceVolume(Define.Sound.Effect);

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.UI.ClosePopup(this);
        });

        GetButton((int)Define.Buttons.RestartGameBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.Scene.LoadScene(Define.Scene.Game);
        });

        GetButton((int)Define.Buttons.StopGameBtn).gameObject.gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.Scene.LoadScene(Define.Scene.Main);
        });


        BGMVolumeSlider.gameObject.BindEvent((PointerEventData) =>
        {
            SaveSoundVolumeData(Define.Sound.BGM, BGMVolumeSlider.value);
            Managers.Sound.SetAudioSourceVolume(Define.Sound.BGM, BGMVolumeSlider.value);
        });

        EffectVolumeSlider.gameObject.BindEvent((PointerEventData) =>
        {
            SaveSoundVolumeData(Define.Sound.Effect, EffectVolumeSlider.value);
            Managers.Sound.SetAudioSourceVolume(Define.Sound.Effect, EffectVolumeSlider.value);
        });
    }


    void SaveSoundVolumeData(Define.Sound _soundType, float _value)
    {
        // refresh
        PlayerPrefs.DeleteKey("BGMVolume");
        PlayerPrefs.DeleteKey("EffectVolume");

        if (_soundType == Define.Sound.BGM)
        {
            PlayerPrefs.SetFloat("BGMVolume", _value);
        } 
        else if (_soundType == Define.Sound.Effect)
        {
            PlayerPrefs.SetFloat("EffectVolume", _value);
        }
        PlayerPrefs.Save();
    }
}
