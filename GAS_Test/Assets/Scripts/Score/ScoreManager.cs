using System;
using UnityEngine;

/// <summary>
/// スコアを管理するクラス
/// </summary>
public class ScoreManager : SingletonMonobehaviour<ScoreManager>
{
    [Header("現在のスコア")]
    [SerializeField]private int score = 0;

    /// <summary>
    /// スコアを外部から読み取るためのプロパティ
    /// </summary>
    public int Score => score;

    /// <summary>
    /// スコアが変わったときに通知するイベント
    /// </summary>
    public event Action<int> OnScoreChange;

    protected override void Awake()
    {
        base.Awake();
        // シーンを跨いでもスコアマネージャーが破壊されないようにする
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// プレイヤーがアイテムを取得すると呼ばれる
    /// </summary>
    public void AddScore(int score)
    {
        this.score += score;
        // スコアが変わったことを通知
        OnScoreChange?.Invoke(this.score);
    }
}
