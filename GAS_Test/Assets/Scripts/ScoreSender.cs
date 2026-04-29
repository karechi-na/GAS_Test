using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreSender : MonoBehaviour
{
	[SerializeField] private string gasUrl;

	public void SendScore(string playerName, int score)
	{
		StartCoroutine(SendScoreCoroutine(playerName, score));
	}

	private IEnumerator SendScoreCoroutine(string playerName, int score)
	{
		string json = JsonUtility.ToJson(new ScoreData
		{
			name = playerName,
			score = score
		});

		using UnityWebRequest request = new UnityWebRequest(gasUrl, "POST");

		byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("ëóêMê¨å˜" + request.downloadHandler.text);
		}
		else
		{
			Debug.LogError("ëóêMé∏îs" + request.error);
		}
	}

	[System.Serializable]
	private class ScoreData
	{
		public string name;
		public int score;
	}
}
