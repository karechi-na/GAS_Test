using UnityEngine;
using UnityEngine.InputSystem;

public class GetterSender : MonoBehaviour
{
	[SerializeField] private RankingGetter rankingGetter = null;
	[SerializeField] private ScoreSender scoreSender = null;

	private void Update()
	{
		if (Keyboard.current == null) return;

		if (Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			rankingGetter.GetRanking();
		}

		if (Keyboard.current.rightShiftKey.wasPressedThisFrame)
		{
			scoreSender.SendScore("chi-kun", 200);
		}
	}
}