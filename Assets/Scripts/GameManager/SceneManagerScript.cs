using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private Image FadeInOut;
    Player player;

    public void Awake()
    {
        FadeInOut = GetComponent<Image>();
    }

    public void NextSceneWithNum()
    {
        SceneManager.LoadScene(1);

        StartCoroutine(Fade(1, 0));
    }

    public IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = FadeInOut.color;
            color.a = Mathf.Lerp(start, end, percent);
            FadeInOut.color = color;

            yield return null;
        }

    }

    public void OnClickStart()
    {
        StartCoroutine(Fade(0, 1));

        Invoke("NextSceneWithNum", 4f);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
