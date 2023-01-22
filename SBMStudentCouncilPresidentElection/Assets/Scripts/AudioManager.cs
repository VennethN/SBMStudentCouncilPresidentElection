using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<AudioData> BGMAudio;
    public List<AudioData> effectsAudio;
    public AudioSource MusicSource;
    public AudioSource EffectsSource;
    public Transform AudioDebris;
    private Dictionary<string, AudioClip> audioEffects;
    private Dictionary<string, AudioClip> audioMusic;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        initAudioDatas();
    }
    public void initAudioDatas()
    {
        audioEffects = new Dictionary<string, AudioClip>();
        audioMusic = new Dictionary<string, AudioClip>();
        for (int i = 0; i < effectsAudio.Count; i++)
        {
            audioEffects.Add(effectsAudio[i].audioKey, effectsAudio[i].clip);
        }
        for (int i = 0; i < BGMAudio.Count; i++)
        {
            audioMusic.Add(BGMAudio[i].audioKey, BGMAudio[i].clip);
        }
    }
    public AudioClip GetEffectClip(string audioKey)
    {
        return audioEffects[audioKey];
    }
    public AudioClip GetMusicClip(string audioKey)
    {
        return audioMusic[audioKey];
    }

    public static void PlayAudioEffect(string audioName)
    {
        Instance.EffectsSource.PlayOneShot(Instance.audioEffects[audioName]);
    }
    public static void ChangeMusicAudio(string AudioName)
    {
        Instance.MusicSource.clip = Instance.GetMusicClip(AudioName);
        Instance.MusicSource.Play();
    }
    public static void ChangeMusicAudio(AudioClip aClip)
    {
        Instance.MusicSource.clip = aClip;
        Instance.MusicSource.Play();
    }
    public static void PauseMusic()
    {
        Instance.MusicSource.Pause();
    }
    public static void ResumeMusic()
    {
        Instance.MusicSource.UnPause();
    }
    public static IEnumerator LoadAudioFromPath(string requestPath)
    {
        WWW request = GetAudioFromFile(requestPath);
        yield return request;
        AudioClip aud = request.GetAudioClip();
        yield return aud;
    }
    public static WWW GetAudioFromFile(string path)
    {
        WWW request = new WWW(path);
        return request;
    }
    void DestroyClip()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public class AudioData
{
    public string audioKey;
    public AudioClip clip;
}
