using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager 
{
    // MP3 Player + MP3 Resource + Audience 
    // Audio Source + Audio Clip + Audio Listener

    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    // 캐싱을 위한 Dic
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        // 게임 오브젝트 중 @Sound 이름인거 찾기
        GameObject root = GameObject.Find("@Sound");

        // 없으면 생성
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root); // 잘못사용하면 메모리가 불필요하게 사용됨

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                // GameObject로 사운드 만들기
                GameObject go = new GameObject { name = soundNames[i] };

                // _audioSources 배열에 넣어주기
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform; // root 아래에 생성
            }

            // BGM인 경우 루프(반복재생) 돌게하기
            _audioSources[(int)Define.Sound.BGM].loop = true;

            // volume
            if (PlayerPrefs.HasKey("BGMVolume"))
            {
                _audioSources[(int)Define.Sound.BGM].volume = PlayerPrefs.GetFloat("BGMVolume");
            }

            if (PlayerPrefs.HasKey("EffectVolume"))
            {
                _audioSources[(int)Define.Sound.Effect].volume = PlayerPrefs.GetFloat("EffectVolume");
            }
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    // path를 사용한 Play() 버전
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // 기본은 Effect
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    // 오디오 클립을 직접 받는 Play() 버전
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // 기본은 Effect
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.BGM)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.BGM];

            if (audioSource.isPlaying == true) // 이미 딴곡을 브금으로 재생중이라면
            {
                audioSource.Stop(); // 이전 곡 중지
            }
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // 단발성 사운드
        {
            // Play해주려면 Audio Source 컴포넌트가 있어야 함
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }


    // 캐싱처리
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        // 혹시 모르니 path 에 Sounds를 가지고 있지 않으면 넣어주기
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        // BGM 인지 아닌지만 구분
        if (type == Define.Sound.BGM) // 반복 loop
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else // 단발성 사운드
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        // 없으면 리턴
        if (audioClip == null)
            Debug.Log($"Audio Clip Missing! >> {path}");

        return audioClip;
    }

    public float GetAudioSourceVolume(Define.Sound _audioType)
    {
        float volume;
        if (_audioType == Define.Sound.BGM)
        {
            volume = _audioSources[(int)Define.Sound.BGM].volume;
        }
        else
        {
            volume = _audioSources[(int)Define.Sound.Effect].volume;
        }
        return volume;
    }

    // Set BGM Volume
    public void SetAudioSourceVolume(Define.Sound _audioType, float _volume = 1.0f)
    {
        if (_audioType == Define.Sound.BGM)
        {
            _audioSources[(int)Define.Sound.BGM].volume = _volume;
            Debug.Log($"BGM Volume >> {_audioSources[(int)Define.Sound.BGM].volume}");
            Debug.Log($"value >> {_volume}");
        }
        else
        {
            _audioSources[(int)Define.Sound.Effect].volume = _volume;
            Debug.Log($"Effect Volume >> {_audioSources[(int)Define.Sound.Effect].volume}");
            Debug.Log($"value >> {_volume}");
        }
        
    }
}