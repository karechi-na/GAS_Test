using System;
using System.Collections;
using UnityEngine;
using MyGame.SceneManagement;

public class SceneTransitionManager : SingletonMonobehaviour<SceneTransitionManager>
{
    public event Action OnTransitionFinished;

    private UITransitionEffect transitionEffect;


    protected override void Awake()
    {
        base.Awake();

        transitionEffect = GetComponentInChildren<UITransitionEffect>();
        DontDestroyOnLoad(gameObject);
    }

    public void Load(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    public IEnumerator LoadAsync(string sceneName)
    {
        yield return transitionEffect.PlayIn(0.0f, 1.1f);

        AsyncOperation operation = GameSceneManager.LoadSceneAsync(sceneName);

        yield return operation;

        Time.timeScale = 0.0f;

        yield return transitionEffect.PlayOut(1.1f, 0.0f);

        OnTransitionFinished?.Invoke();
    }
}
