using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("アイテムの持ち点")]
    [SerializeField] private int point = 0;
    public int Point => point;

    private void Update()
    {
        if (transform.position.y < -1.0f)
        {
            ItemPool.Instance.ReleaseItem(gameObject);
        }
    }
}
