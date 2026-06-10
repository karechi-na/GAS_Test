using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ランキングを取得するクラス
/// </summary>
public class RankingGetter : MonoBehaviour
{
	[Header("GAS")]
    [SerializeField] private string gasUrl;

	[Header("ランキングを表示するテキスト")]
    [SerializeField] private TextMeshProUGUI rankingText;

	/// <summary>
	/// ランキング取得コルーチンを開始する
	/// </summary>
    public void GetRanking()
	{
        AudioManager.Instance.PlaySE(SoundEffect_Key.SUBMIT_SE); 
		StartCoroutine(GetRankingCoroutine());
	}

    /// <summary>
    /// ランキング取得コルーチン
    /// </summary>
    private IEnumerator GetRankingCoroutine()
	{
        // ランキング取得中のログを表示
        rankingText.text = GetLogText(RankingGetLog.Obtaining);
		// GET通信する準備
        using UnityWebRequest request = UnityWebRequest.Get(gasUrl);

        // 通信開始
        yield return request.SendWebRequest();

        // 通信成功かどうかで処理を分ける
        if (request.result == UnityWebRequest.Result.Success)
		{
            // 通信成功したらJSONを取得
            string json = request.downloadHandler.text;
            // JSONをパース
            RankingList rankingList = JsonUtility.FromJson<RankingList>(json);
            // ランキングを表示
            RankingDisplay(rankingList);
        }
		else
		{
            // 通信失敗したらエラーログを表示
            rankingText.text = GetLogText(RankingGetLog.Failure);
		}
	}

    /// <summary>
    /// ランキングを表示する
    /// </summary>
    private void RankingDisplay(RankingList rankingList)
	{
        // ランキング表示用のテキストを初期化
        rankingText.text = "";

        // ランキングの数だけループしてテキストに追加していく
        for (int i = 0; i < rankingList.ranking.Length; i++)
		{
            // ランキングデータを取得
            RankingData data = rankingList.ranking[i];

            // テキストにランキングを追加
            rankingText.text +=
				$"{i + 1}:<pos=200> {data.name}<pos=1000>{data.score}\n";
        }
    }

    /// <summary>
    /// ランキングのリストを表すクラス
    /// </summary>
    [System.Serializable]
	private class RankingList
	{
		public RankingData[] ranking;
    }
    
    /// <summary>
    /// ランキングデータを表すクラス
    /// </summary>
    [System.Serializable]
    private class RankingData
	{
		public string name;
		public int score;
    }

    /// <summary>
    /// ランキング取得のログを表す列挙型
    /// </summary>
	private enum RankingGetLog
	{ 
		Obtaining,
		Failure
	}

    /// <summary>
    /// ランキング取得のログに対応するテキストを取得する
    /// </summary>
	private string GetLogText(RankingGetLog log)
	{
		return log switch
		{
			RankingGetLog.Obtaining => "Ranking in progress...",
			RankingGetLog.Failure => "Ranking acquisition failed",
			_ => ""
		};
    }
}
