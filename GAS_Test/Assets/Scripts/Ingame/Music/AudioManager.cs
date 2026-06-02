using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [SerializeField] private AudioSource musicSource;

    [Header("--- AudioClipData ---")]
    [Header("BGM")]
    [SerializeField] private List<AudioClipData> bgmClipList = new List<AudioClipData>();
    [Header("SE")]
    [SerializeField] private List<AudioClipData> seClipList = new List<AudioClipData>();

    private Dictionary<string, AudioClip> bgmClips;
    private Dictionary<string, AudioClip> seClips;

    private void Start()
    {
        bgmClips = ConvertDictionary(bgmClipList);
        seClips = ConvertDictionary(seClipList);
    }


    public void PlaySE(string key)
    {
        if (seClips.TryGetValue(key, out AudioClip clip))
        {
            musicSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SE with key '{key}' not found.");
        }
    }

    private Dictionary<string, AudioClip> ConvertDictionary(List<AudioClipData> clipList)
    {
        Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClipData clipData in clipList)
        {
            if (!clipDictionary.ContainsKey(clipData.key))
            {
                clipDictionary.Add(clipData.key, clipData.audioClip);
            }
            else
            {
                Debug.LogWarning($"Duplicate key '{clipData.key}' found in AudioClipData list. Skipping.");
            }
        }
        return clipDictionary;
    }

    private void Reset()
    {
        musicSource = GetComponent<AudioSource>();
    }

    [System.Serializable]
    public class AudioClipData
    {
        public string key;
        public AudioClip audioClip;
    }
}
