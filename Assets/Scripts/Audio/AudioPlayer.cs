using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set;}

    [SerializeField] private AudioLibery audioLib;
    [SerializeField] private int audioSourceNumber = 6;
    [SerializeField] private AudioMixerGroup sfxGroup, bgmGroup;

    private Queue<AudioSource> audioSource = new Queue<AudioSource>();
    private AudioSource bgmSource;

    private void Start()
    {
        Init();
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Init()
    {
        GameObject bgmObject = new GameObject("BGM Source");
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.spatialBlend = 0f;
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmObject.transform.SetParent(transform);
        for (int i = 0; i < audioSourceNumber; i++)
        {
            GameObject sfxObject = new GameObject("SFX Source" + (i + 1).ToString("00"));
            AudioSource temp = sfxObject.AddComponent<AudioSource>();
            temp.spatialBlend = 1f;
            temp.outputAudioMixerGroup = sfxGroup;
            sfxObject.transform.SetParent(transform);
            audioSource.Enqueue(temp);
        }
    }

    public void PlaySFX(AudioGetter audioSfx, Transform audioLocation = null)
    {
        AudioSource temp = audioSource.Dequeue();
        if (audioLocation != null)
        {
            temp.transform.position = audioLocation.position;
            temp.spatialBlend = 1f;
        }
        else
        {
            temp.spatialBlend = 0f;
        }

        temp.PlayAudioData(audioLib.GetAudioByName(audioSfx.AudioName));
        audioSource.Enqueue(temp);
    }

    public void PlayMusic(AudioGetter music)
    {
        bgmSource.PlayAudioData(audioLib.GetAudioByName(music.AudioName));
        bgmSource.pitch = 1f;
    }
}
