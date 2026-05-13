using UnityEngine;
using UnityEngine.Pool;

public class ItemPool : SingletonMonobehaviour<ItemPool>
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int initialPoolSize = 10;
    public int InitialPoolSize => initialPoolSize;
    [SerializeField] private int maxPoolSize = 30;

    private ObjectPool<GameObject> itemPool;

    protected override void Awake()
    {
        base.Awake();
        itemPool = new ObjectPool<GameObject>
        (
            CreateItem,
            OnGetItem,
            OnReleaseItem,
            OnDestroyItem,
            true,
            initialPoolSize,
            maxPoolSize
        );
    }

    private GameObject CreateItem()
    {
        return Instantiate(itemPrefab);
    }

    private void OnGetItem(GameObject item)
    {
        item.SetActive(true);
    }

    private void OnReleaseItem(GameObject item)
    {
        item.SetActive(false);
    }

    private void OnDestroyItem(GameObject item)
    {
        Destroy(item);
    }

    public GameObject GetItem()
    {
        return itemPool.Get();
    }

    public void ReleaseItem(GameObject item)
    {
        itemPool.Release(item);
    }
}
