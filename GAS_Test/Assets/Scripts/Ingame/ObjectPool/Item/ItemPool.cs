using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

/// <summary>
/// アイテムのオブジェクトプールを管理するクラス
/// </summary>
public class ItemPool : SingletonMonobehaviour<ItemPool>
{
    /// <summary>
    /// Y座標オフセット
    /// </summary>
    private const float OFFSET_Y = 8.0f;

    [Header("アイテムのプレハブ")]
    [Tooltip("いずれはクラスそのものをGenericにして汎用的なクラスにしたいところ")]
    [SerializeField] private GameObject itemPrefab;

    [Header("--- オブジェクトプールの設定 ---")]
    [Header("初期生成数")]
    [SerializeField] private int initialPoolSize = 10;
    public int InitialPoolSize => initialPoolSize;
    [Header("最大生成数")]
    [SerializeField] private int maxPoolSize = 30;

    // オブジェクトプールのインスタンス
    private ObjectPool<GameObject> itemPool;

    // アイテムが返却されたときに呼ばれるイベント
    public event Action OnItemRelease;

    protected override void Awake()
    {
        base.Awake();

        // オブジェクトプールの初期化
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

    private void Start()
    {
        // 初期生成数分のアイテムを事前に生成してプールに登録する
        List<GameObject> preloadItems = new List<GameObject>();

        // 初期生成数文のアイテムを生成し、プールに登録しリストに追加
        for (int i = 0; i < initialPoolSize; i ++)
        {
            preloadItems.Add(itemPool.Get());
        }
        // 事前に生成したアイテムをプールに返す
        foreach (GameObject item in preloadItems) 
        {
            itemPool.Release(item);
        }
    }

    #region オブジェクトプールに登録するメソッド
    /// <summary>
    /// オブジェクトが足りなくなった時に使われるメソッド
    /// </summary>
    private GameObject CreateItem()
    {
        return Instantiate(itemPrefab);
    }

    /// <summary>
    /// 実際に使われるときに呼ばれるメソッド
    /// </summary>
    private void OnGetItem(GameObject item)
    {
        ItemVelocityReset(item);

        item.transform.position = transform.position + OffsetRandomSet();

        item.SetActive(true);
    }

    /// <summary>
    /// 返却するときに呼ばれるメソッド
    /// </summary>
    private void OnReleaseItem(GameObject item)
    {
        ItemVelocityReset(item);

        item.SetActive(false);
    }

    /// <summary>
    /// 生成数が一定値を超えたときに呼ばれるメソッド
    /// </summary>
    private void OnDestroyItem(GameObject item)
    {
        Destroy(item);
    }
    #endregion

    /// <summary>
    /// アイテムの速度をリセットするメソッド
    /// </summary>
    /// <param name="item">速度をリセットするアイテム</param>
    private void ItemVelocityReset(GameObject item)
    {
        // アイテムにRigidbodyがついている場合は速度をリセットする
        if (item.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    #region 外部からプールに登録してあるオブジェクトを使いたいときに使うメソッド
    /// <summary>
    /// プールに登録してあるアイテムを渡す
    /// </summary>
    public GameObject GetItem()
    {
        return itemPool.Get();
    }

    /// <summary>
    /// 外部で使われたアイテムを返してもらう
    /// </summary>
    public void ReleaseItem(GameObject item)
    {
        itemPool.Release(item);
        OnItemRelease?.Invoke();
    }
    #endregion

    /// <summary>
    /// ランダムに生成位置offsetを決めるメソッド
    /// </summary>
    /// <returns>x座標をランダムに決めたVector3型</returns>
    private Vector3 OffsetRandomSet()
    {
        return new Vector3(
            Random.Range(-5, 6), 
            OFFSET_Y, 
            0.0f
        );
    }
}
