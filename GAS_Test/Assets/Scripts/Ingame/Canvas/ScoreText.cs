using TMPro;
using UnityEngine;

/// <summary>
/// スコアを表示するクラス
/// </summary>
public class ScoreText : MonoBehaviour
{
    [Header("自身のTextMeshProUGUI")]
    [SerializeField] private TextMeshProUGUI scoreText = null;

    #region イベント登録、解除
    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreChange += ScoreDisplay;
    }
    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreChange -= ScoreDisplay;
    }
    #endregion

    /// <summary>
    /// スコアを表示
    /// </summary>
    /// <param name="score">ScoreManagerから送られてきたスコア</param>
    private void ScoreDisplay(int score)
    {
        scoreText.text = score.ToString();
    }
}
