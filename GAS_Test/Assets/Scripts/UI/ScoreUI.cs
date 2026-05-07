using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;

    [SerializeField] private ScoreSender scoreSender = null;

    private bool isSending = false;

    private void Start()
    {
        nameInputField.onSubmit.AddListener(OnSubmitName);
    }

    private void OnSubmitName(string text)
    {
        SendScore();
    }

    public void OnClickSendButton()
    {
       SendScore();
    }

    private void SendScore()
    {
        if (isSending) return;

        string playerName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("プレイヤー名が入力されていません。");
            return;
        }

        isSending = true;

        int score = Random.Range(0, 250); // ここではランダムなスコアを生成しています。実際のゲームのスコアを使用してください。
        scoreSender.SendScore(playerName, score);
        Debug.Log($"スコア送信: {playerName} - {score}");

        nameInputField.text = ""; // 入力フィールドをクリア

        Invoke(nameof(ResetSending), 0.1f); // 送信状態をリセットするためのタイマー
        Invoke(nameof(ReActivateInputField), 0.1f); // 入力フィールドを再度アクティブにするためのタイマー
    }

    private void ResetSending()
    {
        isSending = false;
    }

    private void ReActivateInputField()
    {
                nameInputField.ActivateInputField();
    }
}

