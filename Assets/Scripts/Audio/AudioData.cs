using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    [SerializeField] private string _audioName;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private float minPitch = .9f, maxPitch = 1.1f;

    public string AudioName
    {
        get => _audioName;
    }

    public AudioClip GetAudioClip
    {
        get => _audioClips[Random.Range(0, _audioClips.Length)];
    }
    public float GetPitch
    {
        get => Random.Range(minPitch, maxPitch);
    }
    
}
