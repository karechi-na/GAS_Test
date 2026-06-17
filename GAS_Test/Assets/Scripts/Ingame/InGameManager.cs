using System;
using UnityEngine;
using MyGame.SceneManagement;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ゲームプレイ中のフェーズ管理と時間管理を行うクラス
/// </summary>
public class InGameManager : SingletonMonobehaviour<InGameManager>
{
    #region EditorOnly
#if UNITY_EDITOR
    [Header("シーン遷移設定")]
    [SerializeField] private SceneAsset sceneAsset;

    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            nextSceneName = sceneAsset.name;
        }
    }
#endif
    #endregion

    [SerializeField, HideInInspector]
    [Tooltip("シーンアセットの名前")]
    private string nextSceneName;

    private const float DEFAULT_TIME_SCALE = 1.0f;
    private const int PHASE_CHANGE_INTERVAL = 50;
    private const float COUNT_DOWN_TIME = 1.0f;

    // 各通知イベント
    /// <summary>
    /// カウントダウンのときにUIに通知する
    /// </summary>
    public event Action<string> OnCountDownChanged;
    /// <summary>
    /// フェーズが変わったときに通知するイベント
    /// </summary>
    public event Action<InGamePhase> OnPhaseChanged;
    /// <summary>
    /// 時間が変わったときに通知するイベント
    /// </summary>
    public event Action<int> OnTimeChanged;

    [Header("ゲームプレイ時間設定")]
    [SerializeField] private float remainingTime = 0.0f;

    [Header("シーン遷移の遅延時間")]
    [SerializeField] private float sceneChangeDelay = 1.5f;

    // 最後に表示した時間（秒）を記録する変数
    private int lastDisplayTime;

    // 現在のゲームフェーズを管理する変数
    private InGamePhase currentPhase = InGamePhase.Phase1;
    public InGamePhase CurrentPhase => currentPhase;

    /// <summary>
    /// カウントダウンの状況に応じてtextを変化させるためのDictionary
    /// </summary>
    private Dictionary<CountDownPhase, string> countDownText = new()
    {
        { CountDownPhase.Three, "3"},
        { CountDownPhase.Two, "2"},
        { CountDownPhase.One, "1"},
        { CountDownPhase.Start, "START"},
        { CountDownPhase.Finish, "FINISH" }
    };


    #region イベント登録、解除
    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreChange += PhaseCheck;
        SceneTransitionManager.Instance.OnTransitionFinished += StartCountDown;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreChange -= PhaseCheck;
        SceneTransitionManager.Instance.OnTransitionFinished -= StartCountDown;
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
            OnCountDownChanged?.Invoke(countDownText[CountDownPhase.Finish]);
            // シーン遷移の遅延時間を使用
            Invoke(nameof(SceneChange), sceneChangeDelay);
        }
    }


    /// <summary>
    /// ゲーム開始までのカウントダウンコルーチンを開始するメソッド
    /// </summary>
    private void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    /// <summary>
    /// カウントダウンコルーチン
    /// </summary>
    private IEnumerator CountDown()
    {
        // Time.timeScaleが0のタイミングで使用するので
        // WaitForSecondsではなくWaitForSecondsRealTimeを使用
        OnCountDownChanged?.Invoke(countDownText[CountDownPhase.Three]);
        AudioManager.Instance.PlaySE(SoundEffect_Key.SUBMIT_SE);
        yield return new WaitForSecondsRealtime(COUNT_DOWN_TIME);

        OnCountDownChanged?.Invoke(countDownText[CountDownPhase.Two]);
        AudioManager.Instance.PlaySE(SoundEffect_Key.SUBMIT_SE);
        yield return new WaitForSecondsRealtime(COUNT_DOWN_TIME);

        OnCountDownChanged?.Invoke(countDownText[CountDownPhase.One]);
        AudioManager.Instance.PlaySE(SoundEffect_Key.SUBMIT_SE);
        yield return new WaitForSecondsRealtime(COUNT_DOWN_TIME);

        OnCountDownChanged?.Invoke(countDownText[CountDownPhase.Start]);
        AudioManager.Instance.PlaySE(SoundEffect_Key.START_SE);
        yield return new WaitForSecondsRealtime(COUNT_DOWN_TIME);

        OnCountDownChanged?.Invoke("");
        StartGame();
    }

    /// <summary>
    /// Startメソッドではなくこちらでカウントダウンが終了後に実行
    /// </summary>
    private void StartGame()
    {
        // 初期フェーズと時間を通知
        lastDisplayTime = Mathf.CeilToInt(remainingTime);

        // 最初の各UIの状態を通知
        OnTimeChanged?.Invoke(lastDisplayTime);
        OnPhaseChanged?.Invoke(currentPhase);

        // BGMプレイ開始
        AudioManager.Instance.LoopPlayBGM(SoundEffect_Key.INGAME_BGM);

        //Time.timeScaleを基に戻す
        Time.timeScale = DEFAULT_TIME_SCALE;
    }

    /// <summary>
    /// スコアに応じてフェーズを変更するメソッド
    /// </summary>
    private void PhaseCheck(int newScore)
    {
        // スコアが50点ごとにフェーズを変更
        if (newScore % PHASE_CHANGE_INTERVAL != 0) return;

        // ゲーム終了フェーズに達している場合はこれ以上フェーズを進めない
        NextPhase();
        // ゲーム終了フェーズに達したら2秒後にシーン遷移
        if (currentPhase == InGamePhase.Finished)
        {
            OnCountDownChanged?.Invoke(countDownText[CountDownPhase.Finish]);
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
        // BGMを停止
        AudioManager.Instance.StopBGM();
        // シーン遷移の処理
        GameSceneManager.SetData(SetData_Key.SCORE, ScoreManager.Instance.Score);
        SceneTransitionManager.Instance.Load(nextSceneName);
    }

    /// <summary>
    /// カウントダウンの表示に使うDictionaryのキー
    /// </summary>
    private enum CountDownPhase
    {
        Three,
        Two,
        One,
        Start,
        Finish
    }
}
