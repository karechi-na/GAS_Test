using UnityEngine;

/// <summary>
/// ゲームプレイ中のフェーズを管理する列挙型
/// </summary>
public enum InGamePhase
{
    [Tooltip("スコア50未満")]
    Phase1,
    [Tooltip("スコア50以上100未満")]
    Phase2,
    [Tooltip("スコア100以上150未満")]
    Phase3,
    [Tooltip("スコア150以上200未満")]
    Phase4,
    [Tooltip("スコア200以上250未満")]
    Phase5,
    [Tooltip("スコア250以上300未満")]
    Phase6,
    [Tooltip("スコア300以上")]
    Finished,
}
