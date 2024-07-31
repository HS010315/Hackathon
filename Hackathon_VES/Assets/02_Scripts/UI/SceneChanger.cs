using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionWithFade : MonoBehaviour
{
    public Image fadeImage;
    public Text fadeText;
    public float fadeDuration = 1f;
    public Button fadeButton;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneIndex));
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color imageColor = fadeImage.color;
        Color textColor = fadeText.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = 1f - (elapsedTime / fadeDuration);
            imageColor.a = alpha;
            textColor.a = alpha;
            fadeImage.color = imageColor;
            fadeText.color = textColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageColor.a = 0f;
        textColor.a = 0f;
        fadeImage.color = imageColor;
        fadeText.color = textColor;
        fadeText.enabled = false;
        fadeImage.enabled = false;
    }

    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        fadeButton.enabled = false;
        fadeImage.enabled = true;
        fadeText.enabled = true;
        float elapsedTime = 0f;
        Color imageColor = fadeImage.color;
        Color textColor = fadeText.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration;
            imageColor.a = alpha;
            textColor.a = alpha;
            fadeImage.color = imageColor;
            fadeText.color = textColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageColor.a = 1f;
        textColor.a = 1f;
        fadeImage.color = imageColor;
        fadeText.color = textColor;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        StartCoroutine(FadeIn());
    }
}