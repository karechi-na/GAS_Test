using TMPro;
using UnityEngine;

/// <summary>
/// ゲームのフェーズを表示するテキストを管理するクラス。
/// </summary>
public class PhaseText : MonoBehaviour
{
    [Header("表示に使うテキスト")]
    [SerializeField] private TextMeshProUGUI phaseText;

    #region イベント登録、解除
    private void OnEnable()
    {
        InGameManager.Instance.OnPhaseChanged += UpdatePhaseText;
    }

    private void OnDisable()
    {
        InGameManager.Instance.OnPhaseChanged -= UpdatePhaseText;
    }
    #endregion

    /// <summary>
    /// フェーズが変更されたときに呼び出されるメソッド。新しいフェーズをテキストに表示する。
    /// </summary>
    /// <param name="newPhase">新しいフェーズ</param>
    private void UpdatePhaseText(InGamePhase newPhase)
    {
        phaseText.text = $"{newPhase}";
    }
}
