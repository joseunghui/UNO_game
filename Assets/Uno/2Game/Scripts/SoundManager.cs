using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void SetBgmVolume(float volume){
        AudioSource bgmSource = GameManager.instance.GetComponent<AudioSource>();
        bgmSource.volume = volume;
    }
    public void SetEffectVolume(float volume){
        AudioSource effectSource = CardManager.instance.GetComponent<AudioSource>();
        effectSource.volume = volume;
    }
    public void CannotClick(){
        TurnManager.instance.isLoading = true;
    }
    public void CanClick(){
        TurnManager.instance.isLoading = false;
    }
}
