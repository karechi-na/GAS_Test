using TMPro;
using UnityEngine;
using MyGame.SceneManagement;

/// <summary>
/// スコア表示クラス
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    [Header("スコア表示用テキスト")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        Debug.Log("Start！");
        // スコアを表示
        if (GameSceneManager.TryGetData<int>("score", out int score))
        {
            Debug.Log($"確かに通りました:{score}");
            scoreText.text = score.ToString() ;
        }
        else
        {
            scoreText.text = "Y";
        }
    }
}
