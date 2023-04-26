using UnityEngine;
using System.Collections;

public class FadeInAndOutSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float starttime = 0.0f;
    private int hasstarted = 0;

    void Start()
    {
        // 获取 SpriteRenderer 组件
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 开始逐渐消失再逐渐出现协程
        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        while (true)
        {
            if(hasstarted == 0)
            {
                yield return new WaitForSeconds(starttime);
            }

            hasstarted = 1;
            // 逐渐消失
            for (float f = 1f; f >= 0; f -= 0.1f)
            {
                Color c = spriteRenderer.color;
                c.a = f;
                spriteRenderer.color = c;
                yield return new WaitForSeconds(0.1f);
            }

            

            // 逐渐出现
            for (float f = 0f; f <= 1f; f += 0.1f)
            {
                Color c = spriteRenderer.color;
                c.a = f;
                spriteRenderer.color = c;
                yield return new WaitForSeconds(0.1f);
            }

            // 等待 3 秒
            yield return new WaitForSeconds(2f);
        }
    }
}
