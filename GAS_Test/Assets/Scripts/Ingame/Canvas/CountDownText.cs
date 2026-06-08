using UnityEngine;
using TMPro;

/// <summary>
/// カウントダウンを表示するTextMeshProUGUIにアタッチしているクラス
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class CountDownText : MonoBehaviour
{
    [Header("自身のTextMeshProUGUI")]
    [SerializeField] private TextMeshProUGUI countDownText = null;

    #region イベント登録、解除
    private void OnEnable()
    {
        InGameManager.Instance.OnCountDownChanged += TextChange;
    }

    private void OnDisable()
    {
        InGameManager.Instance.OnCountDownChanged -= TextChange;
    }
    #endregion

    /// <summary>
    /// イベントに登録するメソッド
    /// </summary>
    /// <param name="text">変更する文字列</param>
    private void TextChange(string text)
    {
        countDownText.text = text;
    }

    private void Reset()
    {
        countDownText = GetComponent<TextMeshProUGUI>();
    }
}
