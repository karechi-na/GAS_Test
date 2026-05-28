using UnityEngine;

/// <summary>
/// プレイヤーのアイテム衝突を検知し当たったらScoreManagerにポイントを送るクラス
/// </summary>
public class CollisionMonitoring : MonoBehaviour
{
    // アイテムにつけたタグの名前
    private const string ITEM_TAG = "Item";

    // アイテムと衝突したときの処理
    private void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトがアイテムだったら
        if (other.CompareTag(ITEM_TAG))
        {
            // アイテムのポイントをスコアマネージャーに送る
            if (other.TryGetComponent(out Item item))
                ScoreManager.Instance.AddScore(item.Point);

            // アイテムをプールに戻す
            ItemPool.Instance.ReleaseItem(other.gameObject);
        }
    }
}
