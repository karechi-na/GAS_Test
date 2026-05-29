using System;
using UnityEngine;
using MyGame.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ゲームプレイ中のフェーズ管理と時間管理を行うクラス
/// </summary>
public class InGameManager : SingletonMonobehaviour<InGameManager>
{
#if UNITY_EDITOR
    [Header("シーン遷移設定")]
    [SerializeField] private SceneAsset sceneAsset;
#endif

    [SerializeField, HideInInspector]
    private string nextSceneName;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            nextSceneName = sceneAsset.name;
        }
    }
#endif

    private const int PHASE_CHANGE_SCORE = 50; // フェーズが変わるスコアの間隔

    // 各通知イベント
    public event Action<InGamePhase> OnPhaseChanged;    // フェーズが変わったときに通知するイベント
    public event Action<int> OnTimeChanged;             // 時間が変わったときに通知するイベント

    [Header("ゲームプレイ時間設定")]
    [SerializeField] private float remainingTime = 0.0f;

    [Header("シーン遷移の遅延時間")]
    [SerializeField] private float sceneChangeDelay = 2.0f;

    // 最後に表示した時間（秒）を記録する変数
    private int lastDisplayTime;

    // 現在のゲームフェーズを管理する変数
    private InGamePhase currentPhase = InGamePhase.Phase1;

    private void Start()
    {
        // 初期フェーズと時間を通知
        lastDisplayTime = Mathf.CeilToInt(remainingTime);
        OnTimeChanged?.Invoke(lastDisplayTime);
        OnPhaseChanged?.Invoke(currentPhase);
    }

    #region イベント登録、解除
    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreChange += PhaseCheck;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreChange -= PhaseCheck;
    }
    #endregion

    private void Update()
    {
        // ゲームが終了している場合は時間の更新を行わない
        if (currentPhase == InGamePhase.Finished) return;
        // 時間を減らす
        remainingTime -= Time.deltaTime;

        // 表示する時間を切り上げて整数にする
        int displayTime = Mathf.CeilToInt(remainingTime);

        // 時間表示を変更するときだけイベントを発火
        if (displayTime != lastDisplayTime)
        {
            // 最後に表示した時間を更新
            lastDisplayTime = displayTime;
            // 時間が変わったことを通知
            OnTimeChanged?.Invoke(displayTime);
        }

        // 時間切れになったらゲーム終了フェーズに移行
        if (remainingTime <= 0)
        {
            // ゲーム終了フェーズに移行
            currentPhase = InGamePhase.Finished;
            // フェーズが変わったことを通知
            OnPhaseChanged?.Invoke(currentPhase);
            // シーン遷移の遅延時間を使用
            Invoke(nameof(SceneChange), sceneChangeDelay);
        }
    }

    /// <summary>
    /// スコアに応じてフェーズを変更するメソッド
    /// </summary>
    private void PhaseCheck(int newScore)
    {
        // スコアが50点ごとにフェーズを変更
        if (newScore % PHASE_CHANGE_SCORE != 0) return;

        // ゲーム終了フェーズに達している場合はこれ以上フェーズを進めない
        NextPhase();
        // ゲーム終了フェーズに達したら2秒後にシーン遷移
        if (currentPhase == InGamePhase.Finished)
        {
            Invoke(nameof(SceneChange), sceneChangeDelay); // シーン遷移の遅延時間を使用
        }
    }

    /// <summary>
    /// フェーズを次に進めるメソッド
    /// </summary>
    private void NextPhase()
    {
        // ゲーム終了フェーズに達している場合はこれ以上フェーズを進めない
        if (currentPhase == InGamePhase.Finished) return;

        // フェーズを次に進める
        currentPhase++;

        // フェーズが変わったことを通知
        OnPhaseChanged?.Invoke(currentPhase);
    }

    /// <summary>
    /// シーン遷移の処理
    /// </summary>
    private void SceneChange()
    {
        // シーン遷移の処理
        GameSceneManager.SetData("score", ScoreManager.Instance.Score);
        GameSceneManager.LoadScene(nextSceneName);
    }
}
