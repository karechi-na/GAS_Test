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
    [SerializeField] private SceneAsset titleScene; // タイトルシーンのアセットをインスペクターで指定
#endif

    [SerializeField, HideInInspector]
    private string titleText;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (titleScene != null)
        {
            titleText = titleScene.name; // シーン名を取得してtitleTextに設定
        }
    }
#endif

    /// <summary>
    /// ボタンのonClickイベントに対応する関数
    /// </summary>
    public void TitleBack()
    {
        GameSceneManager.LoadScene(titleText);
    }
}
