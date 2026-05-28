using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの数を管理するクラス
/// </summary>
public class ItemManager : MonoBehaviour
{
    // フェーズごとの有効化されるアイテムの数を管理する辞書
    private readonly Dictionary<InGamePhase, int> PHASE_COUNT_DICTIONARY = new Dictionary<InGamePhase, int>()
    {
        {InGamePhase.Phase1, 3},
        {InGamePhase.Phase2, 4},
        {InGamePhase.Phase3, 6},
        {InGamePhase.Phase4, 8},
        {InGamePhase.Phase5, 11},
        {InGamePhase.Phase6, 15},
        {InGamePhase.Finished, 0},
    };

    [Header("有効化されるオブジェクトの最大数")]
    [SerializeField] private int instanceMaxCount = 0;

    [Header("現在の有効化されているアイテムの数")]
    [SerializeField] private int currentInstanceCount = 0;

    #region イベント登録、解除
    private void OnEnable()
    {
        ItemPool.Instance.OnItemRelease += InstanceCountMinus;
        InGameManager.Instance.OnPhaseChanged += PhaseChange;
    }
    private void OnDisable()
    {
        ItemPool.Instance.OnItemRelease -= InstanceCountMinus;
        InGameManager.Instance.OnPhaseChanged -= PhaseChange;
    }
    #endregion

    /// <summary>
    /// アイテムの数を減らす
    /// アイテムがリリースされたときに発火するイベントに登録
    /// </summary>
    private void InstanceCountMinus()
    {
        currentInstanceCount--;
        RefillItem();
    }

    /// <summary>
    /// フェーズが変わったときに呼び出されるメソッド
    /// </summary>
    private void PhaseChange(InGamePhase newPhase)
    {
        instanceMaxCount = PHASE_COUNT_DICTIONARY[newPhase];
        RefillItem();
    }

    /// <summary>
    /// アイテムの数を補充する
    /// </summary>
    private void RefillItem()
    {
        // 現在の有効化されているアイテムの数が、フェーズごとの最大数に達していない場合は、アイテムを補充する
        while (currentInstanceCount < instanceMaxCount)
        {
            // アイテムを補充する
            ItemPool.Instance.GetItem();
            // 現在の有効化されているアイテムの数を増やす
            currentInstanceCount++;
        }
    }
}
