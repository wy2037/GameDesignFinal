using UnityEngine;
using System.Collections;


public class BridgeLogic : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject bridge1;
    public GameObject bridge2;
    //private GameObject bridgeNow;
    private Color originalColor;

    void Start()
    {

        bridge1.SetActive(false);
        bridge2.SetActive(false);

        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            bridge1.SetActive(true);

            spriteRenderer = bridge1.GetComponent<SpriteRenderer>();

            originalColor = spriteRenderer.color;
            // Fade in
            float timer = 0;
            while (timer < 2f)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, timer / 2f);
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                spriteRenderer.color = newColor;
                yield return null;
            }

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            // Fade out
            timer = 0;
            while (timer < 2f)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / 2f);
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                spriteRenderer.color = newColor;
                yield return null;
            }



            yield return new WaitForSeconds(1f);

            bridge2.SetActive(true);

            spriteRenderer = bridge2.GetComponent<SpriteRenderer>();

            originalColor = spriteRenderer.color;
            // Fade in
            timer = 0;
            while (timer < 2f)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, timer / 2f);
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                spriteRenderer.color = newColor;
                yield return null;
            }

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            // Fade out
            timer = 0;
            while (timer < 2f)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / 2f);
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                spriteRenderer.color = newColor;
                yield return null;
            }


        }

    }
}
