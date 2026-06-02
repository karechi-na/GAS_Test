using TMPro;
using UnityEngine;
using MyGame.SceneManagement;

/// <summary>
/// スコア表示クラス
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour
{
    [Header("スコア表示用テキスト")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        // スコアを表示
        if (GameSceneManager.TryGetData("score", out int score))
        {
            scoreText.text = score.ToString() ;
        }
    }

    private void Reset()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
}
