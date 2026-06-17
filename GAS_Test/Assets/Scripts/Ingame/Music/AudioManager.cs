using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [Header("--- AudioSource ---")]
    [Header("BGM用")]
    [SerializeField] private AudioSource bgmSource;
    [Header("SE用")]
    [SerializeField] private AudioSource seSource;


    [Header("--- AudioClipData ---")]
    [Header("BGM")]
    [SerializeField] private List<AudioClipData> bgmClipList = new List<AudioClipData>();
    [Header("SE")]
    [SerializeField] private List<AudioClipData> seClipList = new List<AudioClipData>();

    // 再生するAudioClipを持ったDictionary
    [Tooltip("BGMのAudioClipを持ったDictionary")]
    private Dictionary<string, AudioClip> bgmClips;
    [Tooltip("SEのAudioClipを持ったDictionary")]
    private Dictionary<string, AudioClip> seClips;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        // リストをDectionaryに変換
        bgmClips = ConvertDictionary(bgmClipList);
        seClips = ConvertDictionary(seClipList);
    }

    /// <summary>
    /// BGMをループで再生するメソッド
    /// </summary>
    /// <param name="key">再生するBGMのキー</param>
    public void LoopPlayBGM(string key)
    {
        if (!bgmClips.TryGetValue(key, out AudioClip clip))
        {
            Debug.LogWarning($"BGM with key '{key}' not found.");
            return;
        }
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>
    /// 再生しているBGMを停止
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// 再生しているBGMを一時停止
    /// </summary>
    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    /// <summary>
    /// 一時停止しているBGMを再スタート
    /// </summary>
    public void UnPauseBGM()
    {
        bgmSource.UnPause();
    }

    /// <summary>
    /// SEをワンショットプレイするメソッド
    /// </summary>
    /// <param name="key">再生するSEのキー</param>
    public void PlaySE(string key)
    {
        if (seClips.TryGetValue(key, out AudioClip clip))
        {
            seSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SE with key '{key}' not found.");
        }
    }

    /// <summary>
    /// ListをDictionaryに変換して返すメソッド
    /// </summary>
    /// <param name="clipList">変換元のList</param>
    private Dictionary<string, AudioClip> ConvertDictionary(List<AudioClipData> clipList)
    {
        //Dictionaryを定義
        Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

        // 引数のリストの中身を順に変換
        foreach (AudioClipData clipData in clipList)
        {
            // clipDataのキーと同名のキーがDictionaryになければDictionaryに追加
            if (!clipDictionary.ContainsKey(clipData.key))
            {
                clipDictionary.Add(clipData.key, clipData.audioClip);
            }
            else
            {
                Debug.LogWarning($"Duplicate key '{clipData.key}' found in AudioClipData list. Skipping.");
            }
        }

        // Dictionaryを返す
        return clipDictionary;
    }


    private void Reset()
    {
        bgmSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// リスト用クラス
    /// </summary>
    [System.Serializable]
    private class AudioClipData
    {
        [Tooltip("再生するclipのキー")]
        public string key;
        [Tooltip("再生するclip本体")]
        public AudioClip audioClip;
    }
}
