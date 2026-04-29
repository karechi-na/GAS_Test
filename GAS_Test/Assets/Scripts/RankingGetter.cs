using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RankingGetter : MonoBehaviour
{
	[SerializeField] private string gasUrl;

	public void GetRanking()
	{
		StartCoroutine(GetRakingCoroutine());
	}

	private IEnumerator GetRakingCoroutine()
	{
		using UnityWebRequest request = UnityWebRequest.Get(gasUrl);

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("ランキング取得成功");
			Debug.Log(request.downloadHandler.text);
		}
		else
		{
			Debug.LogError("ランキング取得失敗" + request.error);
		}
	}
}
