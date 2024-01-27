using System;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType {
    BombExplode,
}

[Serializable]
public class AudioItem
{
    public AudioType audioType;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;

    // 通过AudioType来获取AudioClip
    [SerializeField]
    public List<AudioItem> audioItems;

    public Dictionary<AudioType, AudioClip> audioClips = new Dictionary<AudioType, AudioClip>();

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        foreach (var item in audioItems)
        {
            audioClips[item.audioType] = item.audioClip;
        }
    }

    public void PlayAudioClip(AudioType audioType)
    {
        audioSource.PlayOneShot(audioClips[audioType]);
    }
}