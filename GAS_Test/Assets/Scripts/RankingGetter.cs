using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RankingGetter : MonoBehaviour
{
	[SerializeField] private string gasUrl;

	[Header("UI")]
    [SerializeField] private TextMeshProUGUI rankingText;

    public void GetRanking()
	{
		StartCoroutine(GetRakingCoroutine());
	}

	private IEnumerator GetRakingCoroutine()
	{
		Debug.Log("ランキング取得開始");
		// GET通信する準備
        using UnityWebRequest request = UnityWebRequest.Get(gasUrl);

        // 通信開始
        yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("ランキング取得成功");

			string json = request.downloadHandler.text;
            Debug.Log(json);
			
            RankingList rankingList = JsonUtility.FromJson<RankingList>(json);

			RankingDisplay(rankingList);
        }
		else
		{
			Debug.LogError("ランキング取得失敗" + request.error);
		}
	}

	private void RankingDisplay(RankingList rankingList)
	{
		rankingText.text = "";

		for (int i = 0; i < rankingList.ranking.Length; i++)
		{
			RankingData data = rankingList.ranking[i];

			rankingText.text +=
				$"{i + 1}:<pos=120> {data.name}<pos=750>{data.score}\n";
        }
    }
	
    [System.Serializable]
	private class RankingList
	{
		public RankingData[] ranking;
    }

    [System.Serializable]
    private class RankingData
	{
		public string name;
		public int score;
    }
}
