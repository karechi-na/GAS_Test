using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// タイトルシーンの管理クラス
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    #region EditorOnly
#if UNITY_EDITOR
    [Header("シーンアセットを指定するフィールド")]
    [SerializeField] private SceneAsset sceneAsset;

    // シーンアセットが変更されたときにシーン名を更新するためのエディタ専用のメソッド
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif
    #endregion

    // シーン名を保持するフィールド（シリアライズされるが、インスペクターには表示されない）
    [SerializeField, HideInInspector]
    private string sceneName;

    [Header("プレイヤー入力を管理するフィールド")]
    [SerializeField] private PlayerInput playerInput;

    private const float DEFAULT_TIME_SCALE = 1.0f;

    #region イベント登録、解除
    private void OnEnable()
    {
        playerInput.actions["Submit"].canceled += LoadSceneAsync;
        SceneTransitionManager.Instance.OnTransitionFinished += TimeScaleReset;
        
    }
    private void OnDisable()
    {
        playerInput.actions["Submit"].canceled -= LoadSceneAsync;
        SceneTransitionManager.Instance.OnTransitionFinished -= TimeScaleReset;
    }
    #endregion

    private void TimeScaleReset()
    {
        Time.timeScale = DEFAULT_TIME_SCALE;
    }

    /// <summary>
    /// InputActionから呼び出すためのメソッド
    /// </summary>
    private void LoadSceneAsync(InputAction.CallbackContext context)
    {
        SceneTransitionManager.Instance.Load(sceneName);
    }

    /// <summary>
    /// ボタンから呼び出すためのメソッド
    /// </summary>
    public void LoadSceneAsync()
    {
        SceneTransitionManager.Instance.Load(sceneName);
    }

}
