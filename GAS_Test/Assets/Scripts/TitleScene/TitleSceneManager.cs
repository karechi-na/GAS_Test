using UnityEngine;
using UnityEngine.InputSystem;
using MyGame.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// タイトルシーンの管理クラス
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    // シーンアセットをエディタ上で指定するためのフィールド
#if UNITY_EDITOR
    [Header("シーンアセットを指定するフィールド")]
    [SerializeField] private SceneAsset sceneAsset;
#endif

    // シーン名を保持するフィールド（シリアライズされるが、インスペクターには表示されない）
    [SerializeField, HideInInspector]
    private string sceneName;

    // シーンアセットが変更されたときにシーン名を更新するためのエディタ専用のメソッド
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    [Header("プレイヤー入力を管理するフィールド")]
    [SerializeField] private PlayerInput playerInput;

    #region イベント登録、解除
    private void OnEnable()
    {
        playerInput.actions["Submit"].canceled += LoadScene;
    }
    private void OnDisable()
    {
        playerInput.actions["Submit"].canceled -= LoadScene;
    }
    #endregion

    /// <summary>
    /// キーボードからの入力があった時にLoadメソッドを呼び出すためのコールバックメソッド
    /// </summary>
    private void LoadScene(InputAction.CallbackContext context)
    {
        Load();
    }

    /// <summary>
    /// シーンをロードするメソッド
    /// </summary>
    public void Load()
    {
        GameSceneManager.LoadScene(sceneName);
    }
}
