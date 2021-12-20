using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibery",menuName="Audio Libery")]
public class AudioLibery : ScriptableObject
{
    [SerializeField] private AudioData[] _audioList;
    public static List<string> audioNameList = new List<string>();
    public AudioData GetAudioByName(string name)
    {
        AudioData value = null;
        foreach (var audio in _audioList)
        {
            if (audio.AudioName == name)
            {
                value = audio;
            }
        }

        return value;
    }

    private void OnValidate()
    {
        // Debug.Log("Audio is modify");
        audioNameList.Clear();
        foreach (var audio in _audioList)
        {
            audioNameList.Add(audio.AudioName);
        }
    }
}

[System.Serializable]
public class AudioGetter
{
    public string AudioName { get => AudioLibery.audioNameList[id];}
    public int id; 
}