using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Cysharp.Threading.Tasks;

/// <summary>
/// GASからランキングを取得し、画面に表示するクラス
/// </summary>
public class RankingGetter : MonoBehaviour
{
    private const int DISPLAY_RANK_OFFSET = 1;

    private const int NAME_TEXT_POSITION = 200;
    private const int SCORE_TEXT_POSITION = 1000;

    [Header("GAS")]
    [SerializeField] private string gasUrl;

    [Header("ランキングを表示するテキスト")]
    [SerializeField] private TextMeshProUGUI rankingText;

    // 多重通信防止フラグ
    private bool isGetting = false;

    /// <summary>
    /// ランキング取得処理を開始する
    /// ButtonのOnClickから呼び出す想定
    /// </summary>
    public void GetRanking()
    {
        GetRankingAsync().Forget();
    }

    /// <summary>
    /// GASでGET通信を行い、ランキングJSONを取得する
    /// </summary>
    private async UniTask GetRankingAsync()
    {
        // 連打による多重通信を防ぐ
        if (isGetting) return;
        isGetting = true;

        // 通信中と画面に表示
        rankingText.text = GetLogText(RankingGetLog.Obtaining);
        // SE再生
        AudioManager.Instance.PlaySE(SoundEffect_Key.SUBMIT_SE);

        try
        {
            // GASに対してGET通信を行う
            using UnityWebRequest request = UnityWebRequest.Get(gasUrl);

            // オブジェクト破棄時は通信待ちをキャンセルする
            await request.SendWebRequest()
                .WithCancellation(this.GetCancellationTokenOnDestroy());

            // 通信に失敗した場合はエラー表示して終了
            if (request.result != UnityWebRequest.Result.Success)
            {
                rankingText.text = GetLogText(RankingGetLog.Failure);
                Debug.LogError(request.error);
                return;
            }
            // GASから返ってきたJSONを取得
            string json = request.downloadHandler.text;
            // JSONをランキング用のクラスに変換
            RankingList rankingList = JsonUtility.FromJson<RankingList>(json);

            RankingDisplay(rankingList);
        }
        catch (OperationCanceledException)
        {
            // シーン遷移時やオブジェクト破棄時によってキャンセルされた場合
            Debug.Log("ランキング取得がキャンセルされました");
        }
        catch (Exception e)
        {
            // 想定外のエラーが発生した場合
            rankingText.text = GetLogText(RankingGetLog.Failure);
            Debug.LogError(e);
        }
        finally
        {
            // 成功・失敗に関わらず取得中フラグを解除
            isGetting = false;
        }
    }


    /// <summary>
    /// 取得したランキングデータをTextMeshProUGUIに表示
    /// </summary>
    private void RankingDisplay(RankingList rankingList)
    {
        // null時
        if (rankingList == null || rankingList.ranking == null)
        {
            rankingText.text = GetLogText(RankingGetLog.Failure);
            Debug.LogError("ランキングJSONの形式が正しくありません");
            return;
        }

        // rankingが空だった時
        if (rankingList.ranking.Length == 0)
        {
            rankingText.text = GetLogText(RankingGetLog.RankingListNull);
            return;
        }

        StringBuilder builder = new();

        // ランキングの件数分、表示用テキストを組み立てる
        for (int i = 0; i < rankingList.ranking.Length; i++)
        {
            // ランキングデータを取得
            RankingData data = rankingList.ranking[i];

            // テキストにランキングを追加
            builder.AppendLine(
                $"{i + DISPLAY_RANK_OFFSET}:<pos={NAME_TEXT_POSITION}> {data.name}<pos={SCORE_TEXT_POSITION}>{data.score}"
            );
        }

        // StringBuilderで生成した文字列をTextMeshProUGUIに表示
        rankingText.text = builder.ToString();
    }

    /// <summary>
    /// GASから返ってくるランキング一覧JSONを受け取るためのクラス
    /// </summary>
    [System.Serializable]
    private class RankingList
    {
        public RankingData[] ranking;
    }

    /// <summary>
    /// ランキング1件分のデータ
    /// </summary>
    [System.Serializable]
    private class RankingData
    {
        public string name;
        public int score;
    }

    /// <summary>
    /// ランキング取得時に表示するメッセージの種類を表す列挙型
    /// </summary>
	private enum RankingGetLog
    {
        Obtaining,
        Failure,
        RankingListNull,
    }

    /// <summary>
    /// ランキング取得状態に対応する表示テキストを取得する
    /// </summary>
	private string GetLogText(RankingGetLog log)
    {
        return log switch
        {
            RankingGetLog.Obtaining => "Ranking in progress...",
            RankingGetLog.Failure => "Ranking acquisition failed",
            RankingGetLog.RankingListNull => "No ranking data",
            _ => ""
        };
    }
}
