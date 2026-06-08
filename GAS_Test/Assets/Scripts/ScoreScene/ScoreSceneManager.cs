using UnityEngine;

/// <summary>
/// シーン遷移時にTime.timeScaleを0にしているのでそれを1に戻す役目のみを持つ
/// </summary>
public class ScoreSceneManager : MonoBehaviour
{
    private const float DEFAULT_TIME_SCALE = 1.0f;

    #region イベント登録、解除
    private void OnEnable()
    {
        SceneTransitionManager.Instance.OnTransitionFinished += TimeScaleReset;
    }
    private void OnDisable()
    {
        SceneTransitionManager.Instance.OnTransitionFinished -= TimeScaleReset;
    }
    #endregion

    private void TimeScaleReset()
    {
        Time.timeScale = DEFAULT_TIME_SCALE;
    }
}
