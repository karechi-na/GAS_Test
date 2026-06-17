using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

/// <summary>
/// スコアをGoogle Apps Scriptに送信するクラス
/// </summary>
public class ScoreSender : MonoBehaviour
{
    [Header("Google Apps ScriptのURL")]
    [SerializeField] private string gasUrl;

    [Header("送信状態を表示するテキスト")]
    [SerializeField] private TextMeshProUGUI logText;

    // スコアがすでに送信されたかどうかを管理するフラグ
    private bool hasSubmittedScore = false;

    private bool isGetting = false;

    private void Start()
    {
        // 初期メッセージを表示
        logText.text = GetLogText(SendScoreLog.FirstSentence);
    }

    /// <summary>
    /// プレイヤーの名前とスコアをGoogle Apps Scriptに送信するメソッド
    /// </summary>
    /// <param name="playerName">プレイヤーの名前</param>
    /// <param name="score">プレイヤーのスコア</param>
    public void SendScore(string playerName, int score)
    {
        SendScoreAsync(playerName, score).Forget();
    }

    private async UniTask SendScoreAsync(string playerName, int score)
    {
        if (string.IsNullOrWhiteSpace(playerName))
        {
            logText.text = GetLogText(SendScoreLog.NameNotEntered);
            await ResetTextFieldAsync();
            return;
        }

        if (hasSubmittedScore)
        {
            logText.text = GetLogText(SendScoreLog.AlreadySubmitted);
            await ResetTextFieldAsync();
            return;
        }

        if (isGetting) return;
        isGetting = true;

        // スコア送信中のメッセージを表示
        logText.text = GetLogText(SendScoreLog.Sending);

        // 送信するデータをJSON形式にシリアライズ
        string json = JsonUtility.ToJson(new ScoreData
        {
            name = playerName,
            score = score
        });

        // UnityWebRequestを使用してGoogle Apps ScriptにPOSTリクエストを送信
        using UnityWebRequest request = new UnityWebRequest(gasUrl, "POST");

        // JSONデータをリクエストのボディに設定
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        // アップロードハンドラーを使用してリクエストのボディを設定
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // ダウンロードハンドラーを使用してレスポンスを受け取る
        request.downloadHandler = new DownloadHandlerBuffer();
        // リクエストのヘッダーにContent-Typeを設定
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            await request.SendWebRequest()
                .WithCancellation(this.GetCancellationTokenOnDestroy());

            if (request.result == UnityWebRequest.Result.Success)
            {
                logText.text = GetLogText(SendScoreLog.Successfully);
                hasSubmittedScore = true;
            }
            else
            {
                Debug.LogWarning(request.error);
                logText.text = GetLogText(SendScoreLog.Failed);
            }

            await ResetTextFieldAsync();
        }
        finally
        {
            isGetting = false;
        }
    }

    private async UniTask ResetTextFieldAsync()
    {
        await UniTask.Delay(
            2000,
            cancellationToken: this.GetCancellationTokenOnDestroy()
        );

        logText.text = GetLogText(SendScoreLog.FirstSentence);
    }

    /// <summary>
    /// スコアデータを表すクラス
    /// </summary>
    [System.Serializable]
    private class ScoreData
    {
        public string name;
        public int score;
    }

    /// <summary>
    /// スコア送信の状態を表す列挙型
    /// </summary>
    private enum SendScoreLog
    {
        FirstSentence,
        Sending,
        Successfully,
        Failed,
        AlreadySubmitted,
        NameNotEntered
    }

    /// <summary>
    /// 指定されたスコア送信の状態に対応するログテキストを取得するメソッド
    /// </summary>
    /// <param name="log">スコア送信の状態</param>
    /// <returns>対応するログテキスト</returns>
    private string GetLogText(SendScoreLog log)
    {
        return log switch
        {
            SendScoreLog.FirstSentence => "If you want to submit your score, please enter your nickname.",
            SendScoreLog.Sending => "Sending score...",
            SendScoreLog.Successfully => "Score sent successfully!",
            SendScoreLog.Failed => "Score send failed!",
            SendScoreLog.AlreadySubmitted => "Score has already been submitted!",
            SendScoreLog.NameNotEntered => "Please enter your nickname before submitting your score.",
            _ => ""
        };
    }
}
