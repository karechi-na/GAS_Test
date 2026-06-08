using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITransitionEffect : MonoBehaviour
{
    [SerializeField] private RawImage transitionImage;
    [SerializeField] private float duration = 1.0f;

    private Material material = null;

    private static readonly int ProgressID = Shader.PropertyToID("_Progress");


    private void Awake()
    {
        material = Instantiate(transitionImage.material);
        transitionImage.material = material;

        SetProgress(0.0f);
    }

    public void SetProgress(float value)
    {
        material.SetFloat(ProgressID, value);
    }

    public IEnumerator PlayIn(float from, float to)
    {
        yield return Play(from, to);
    }

    public IEnumerator PlayOut(float from, float to)
    {
        yield return Play(from, to);
    }

    private IEnumerator Play(float from, float to)
    {
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float rate = timer / duration;

            material.SetFloat(
                ProgressID,
                Mathf.Lerp(from, to, rate)
            );

            yield return null;
        }

        material.SetFloat(ProgressID, to);
    }
}
