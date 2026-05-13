using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 1, 0);

    private void Start()
    {
        for (int i = 0; i < ItemPool.Instance.InitialPoolSize; i++)
        {
            GameObject item = ItemPool.Instance.GetItem();
            item.transform.position = transform.position + OffsetRandomSet();
        }
    }

    private Vector3 OffsetRandomSet()
    {
        int randomX = Random.Range(-5, 6);
        int randomY = Random.Range(1, 6);
        return new Vector3(randomX, randomY, 0.0f);
    }
}
