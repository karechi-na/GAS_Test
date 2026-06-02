using UnityEngine;
using MyGame.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// タイトルに戻るボタンのイベントに対応するクラス
/// </summary>
public class TitleBackButton : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("タイトルシーンのアセットを指定")]
    [SerializeField] private SceneAsset titleScene;
#endif

    [SerializeField, HideInInspector]
    private string titleSceneName;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (titleScene != null)
        {
            titleSceneName = titleScene.name; // シーン名を取得してtitleSceneNameに設定
        }
    }
#endif

    /// <summary>
    /// ボタンのonClickイベントに対応する関数
    /// </summary>
    public void TitleBack()
    {
        GameSceneManager.ClearData(); // シーン遷移前にデータをクリア
        GameSceneManager.LoadScene(titleSceneName);
    }
}
