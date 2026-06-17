using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シェーダーのパラメーターを操作して画面遷移演出を行うクラス
/// </summary>
public class UITransitionEffect : MonoBehaviour
{
    [Header("シェーダーで見た目を変えるImage")]
    [SerializeField] private RawImage transitionImage;

    [Header("イメージが全体に表示されるまでの時間")]
    [SerializeField] private float duration = 1.0f;

    /// <summary>
    /// RawImageで使用するマテリアル
    /// インスタンス化したものを保持する
    /// </summary>
    [Tooltip("RawImageのMaterial")]
    private Material material = null;

    /// <summary>
    /// シェーダー内の_ProgressプロパティID
    /// 毎フレーム文字列検索を行わないようID化している
    /// </summary>
    private static readonly int ProgressID = Shader.PropertyToID("_Progress");


    private void Awake()
    {
        // もとのマテリアルを直接変更しないように実行時専用のインスタンスを生成
        material = Instantiate(transitionImage.material);
        transitionImage.material = material;

        // 初期状態を設定
        SetProgress(0.0f);
    }

    /// <summary>
    /// シェーダーの進行度を設定
    /// </summary>
    /// <param name="value">
    /// シェーダーに渡す進行度 (0～1を想定)
    /// </param>
    public void SetProgress(float value)
    {
        material.SetFloat(ProgressID, value);
    }

    /// <summary>
    /// 画面を覆う方向の演出
    /// </summary>
    /// <param name="from">開始値</param>
    /// <param name="to">終了値</param>
    public IEnumerator PlayIn(float from, float to)
    {
        yield return Play(from, to);
    }

    /// <summary>
    /// 画面を解放する方向の演出
    /// </summary>
    /// <param name="from">開始値</param>
    /// <param name="to">終了値</param>
    public IEnumerator PlayOut(float from, float to)
    {
        yield return Play(from, to);
    }

    /// <summary>
    /// 指定時間をかけてProgress値を補間する共通処理
    /// </summary>
    /// <param name="from">開始値</param>
    /// <param name="to">終了値</param>
    private IEnumerator Play(float from, float to)
    {
        float timer = 0.0f;

        while (timer < duration)
        {
            // Time.timeScaleの影響を受けずに進行させる
            timer += Time.unscaledDeltaTime;

            // 経過率（0～1）
            float rate = timer / duration;

            // Progress値を補間
            material.SetFloat(
                ProgressID,
                Mathf.Lerp(from, to, rate)
            );

            yield return null;
        }

        // 最終値を保証
        material.SetFloat(ProgressID, to);
    }
}
