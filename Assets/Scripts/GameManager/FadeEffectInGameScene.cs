using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeEffectInGameScene : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private Image Background;

    private void Awake()
    {
        Background = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = Background.color;
            color.a = Mathf.Lerp(start, end, percent);
            Background.color = color;

            yield return null;
        }

    }
}
