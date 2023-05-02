using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InAndOut : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject myselfObj;
    public float startTime = 0.1f;
    public float stayTime = 2f;

    private void Start()
    {
        //myselfObj.SetActive(true);
        spriteRenderer = myselfObj.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOutAndLoadScene());
    }

    IEnumerator FadeOutAndLoadScene()
    {
        
        float fadeTime = 3f;
        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float elapsedTime = 0f;

        // 让物体在1秒内变透明
        //while (elapsedTime < fadeTime)
        //{
        //    spriteRenderer.color = Color.Lerp(originalColor, transparentColor, elapsedTime / fadeTime);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}
        //让物体直接变透明
        spriteRenderer.color = transparentColor;

        // 等待X秒
        yield return new WaitForSeconds(startTime);


        // 让物体在1秒内变回原样
        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(transparentColor, originalColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(stayTime);
        // 跳转到A场景
        SceneManager.LoadScene("Title Screen");
    }
}
