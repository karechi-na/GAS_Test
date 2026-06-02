using UnityEngine;

/// <summary>
/// アイテムが持つポイントを管理するクラス
/// </summary>
public class Item : MonoBehaviour
{
    // アイテムがリリースされるY座標
    private const float RELEASE_POINT_Y = -1.0f;

    [Header("アイテムの持ち点")]
    [SerializeField] private int point = 2;
    /// <summary>
    /// アイテムの持ち点を取得するプロパティ
    /// </summary>
    public int Point => point;

    private void Update()
    {
        // アイテムがリリースポイントを下回った場合、アイテムをプールに返す
        if (transform.position.y < RELEASE_POINT_Y)
        {
            ItemPool.Instance.ReleaseItem(gameObject);
        }
    }
}
