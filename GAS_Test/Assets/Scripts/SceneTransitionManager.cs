using System;
using System.Collections;
using UnityEngine;
using MyGame.SceneManagement;

public class SceneTransitionManager : SingletonMonobehaviour<SceneTransitionManager>
{
    [Tooltip("シーン遷移完了時に発火するイベント")]
    public event Action OnTransitionFinished;

    private UITransitionEffect transitionEffect;


    protected override void Awake()
    {
        base.Awake();

        // 子オブジェクトからUITransitionEffectを取得
        transitionEffect = GetComponentInChildren<UITransitionEffect>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// シーン読み込み
    /// </summary>
    /// <param name="sceneName">読み込むシーンの名前</param>
    public void Load(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    /// <summary>
    /// シーンを非同期読み込み
    /// </summary>
    /// <param name="sceneName">読み込むシーンの名前</param>
    public IEnumerator LoadAsync(string sceneName)
    {
        // Image表示
        yield return transitionEffect.PlayIn(0.0f, 1.1f);

        // 非同期シーン読み込み
        AsyncOperation operation = GameSceneManager.LoadSceneAsync(sceneName);
        float timer = 0.0f;
        const float MIN_LOAD_TIME = 2.0f;

        while (!operation.isDone || timer < MIN_LOAD_TIME)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        //遷移後のシーンでImageの表示が切れるまで動作を防ぐためにtimeScaleを0にする
        Time.timeScale = 0.0f;

        // Image非表示
        yield return transitionEffect.PlayOut(1.1f, 0.0f);

        //シーン遷移完了を通知
        OnTransitionFinished?.Invoke();
    }
}
