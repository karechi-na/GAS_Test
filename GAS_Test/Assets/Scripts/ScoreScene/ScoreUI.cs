using TMPro;
using UnityEngine;

/// <summary>
/// ScoreSenderにスコアを送信するためのUIクラス
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [Header("名前を入力するInputField")]
    [SerializeField] private TMP_InputField nameInputField = null;

    [Header("スコアを送信するためのScoreSender")]
    [SerializeField] private ScoreSender scoreSender = null;

    // スコア送信中かどうかを管理するフラグ
    private bool isSending = false;

    private void Start()
    {
        // InputFieldのonSubmitイベントにコールバックを登録
        nameInputField.onSubmit.AddListener(OnSubmitName);
    }

    /// <summary>
    /// InputFieldのonSubmitイベントのコールバックメソッド
    /// </summary>
    private void OnSubmitName(string text)
    {
        SendScore();
    }

    public void OnClickSendButton()
    {
        SendScore();
    }

    /// <summary>
    /// スコアを送信するための共通のメソッド
    /// </summary>
    private void SendScore()
    {
        // 送信中であれば、二重送信を防ぐために処理を中断
        if (isSending) return;

        // プレイヤー名を取得し、空白やnullでないことを確認
        string playerName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("プレイヤー名が入力されていません。");
            return;
        }

        // スコア送信中フラグを立てる
        isSending = true;

        // スコアを送信
        scoreSender.SendScore(playerName, ScoreManager.Instance.Score);
        Debug.Log($"スコア送信: {playerName} - {ScoreManager.Instance.Score}");

        nameInputField.text = ""; // 入力フィールドをクリア

        Invoke(nameof(ResetSending), 0.1f); // 送信状態をリセットするためのタイマー
        Invoke(nameof(ReActivateInputField), 0.1f); // 入力フィールドを再度アクティブにするためのタイマー
    }

    /// <summary>
    /// スコア送信状態をリセットするためのメソッド
    /// </summary>
    private void ResetSending()
    {
        isSending = false;
    }

    /// <summary>
    /// 入力フィールドを再度アクティブにするためのメソッド
    /// </summary>
    private void ReActivateInputField()
    {
        nameInputField.ActivateInputField();
    }
}

