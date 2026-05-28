using TMPro;
using UnityEngine;

/// <summary>
/// 残り時間を表示するのに使用するクラス
/// </summary>
public class RemainingTime : MonoBehaviour
{
    [Header("表示に使うテキスト")]
    [SerializeField] private TextMeshProUGUI remainingTimeText;
    
    #region イベント登録、解除
    private void OnEnable()
    {
        InGameManager.Instance.OnTimeChanged += UpdateRemainingTime;
    }

    private void OnDisable()
    {
        InGameManager.Instance.OnTimeChanged -= UpdateRemainingTime;
    }
    #endregion

    /// <summary>
    /// 時間が変わったときに呼び出される関数。残り時間をテキストに表示する。
    /// </summary>
    /// <param name="time">新しい残り時間</param>
    private void UpdateRemainingTime(int time)
    {
        // 残り時間が10秒以下になったら、テキストの色を赤にする
        if (time <= 10)
            remainingTimeText.color = Color.red;

        remainingTimeText.text = time.ToString();
    }
}
