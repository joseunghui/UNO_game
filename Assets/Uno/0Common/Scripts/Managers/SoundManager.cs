using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    // MP3 Player + MP3 Resource + Audience 
    // Audio Source + Audio Clip + Audio Listener

    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    // ĳ���� ���� Dic
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void init()
    {
        // ���� ������Ʈ �� @Sound �̸��ΰ� ã��
        GameObject root = GameObject.Find("@Sound");

        // ������ ����
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root); // �߸�����ϸ� �޸𸮰� ���ʿ��ϰ� ����

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                // GameObject�� ���� �����
                GameObject go = new GameObject { name = soundNames[i] };

                // _audioSources �迭�� �־��ֱ�
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform; // root �Ʒ��� ����
            }

            // BGM�� ��� ����(�ݺ����) �����ϱ�
            _audioSources[(int)Define.Sound.BGM].loop = true;
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

    // path�� ����� Play() ����
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // �⺻�� Effect
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    // ����� Ŭ���� ���� �޴� Play() ����
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // �⺻�� Effect
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.BGM)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.BGM];

            if (audioSource.isPlaying == true) // �̹� ������ ������� ������̶��
            {
                audioSource.Stop(); // ���� �� ����
            }
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // �ܹ߼� ����
        {
            // Play���ַ��� Audio Source ������Ʈ�� �־�� ��
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }


    // ĳ��ó��
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        // Ȥ�� �𸣴� path �� Sounds�� ������ ���� ������ �־��ֱ�
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        // BGM ���� �ƴ����� ����
        if (type == Define.Sound.BGM) // �ݺ� loop
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else // �ܹ߼� ����
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        // ������ ����
        if (audioClip == null)
            Debug.Log($"Audio Clip Missing! >> {path}");

        return audioClip;
    }

}
