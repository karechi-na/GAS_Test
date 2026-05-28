using TMPro;
using UnityEngine;

/// <summary>
/// スコア表示クラス
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    [Header("スコア表示用テキスト")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        // スコアを表示
        scoreText.text = $"{ScoreManager.Instance.Score}";
    }
}
