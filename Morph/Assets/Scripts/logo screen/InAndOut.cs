using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InAndOut : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject myselfObj;

    private void Start()
    {
        //myselfObj.SetActive(true);
        spriteRenderer = myselfObj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOutAndLoadScene());
    }

    IEnumerator FadeOutAndLoadScene()
    {
        // 让物体在1秒内变透明
        float fadeTime = 1f;
        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(originalColor, transparentColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = transparentColor;

        // 等待2秒
        yield return new WaitForSeconds(2f);

        // 让物体在1秒内变回原样
        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(transparentColor, originalColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;

        // 跳转到A场景
        SceneManager.LoadScene("Title Screen");
    }
}
