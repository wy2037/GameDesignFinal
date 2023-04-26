
using UnityEngine;
using System.Collections;

public class ShootingStarSprite : MonoBehaviour
{
    public float startTime = 0.0f;
    public int hasStarted = 0;
    public float moveTime = 4f;
    public float fadeTime = 1f;//fadeTime should be smaller than moveTime, default 1s
    public float waitTime = 3f;
    public float distanceToMove = 3f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        StartCoroutine(ShootingStarRoutine());
    }

    IEnumerator ShootingStarRoutine()
    {
        while (true)
        {
            if (hasStarted == 0)
            {
                yield return new WaitForSeconds(startTime);
            }
            hasStarted = 1;

            float timer = 0f;
            float timer2 = 0f;

            //// 出现
            //spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            
            //while (timer < fadeTime)
            //{
            //    float alpha = Mathf.Lerp(0f, 1f, timer / fadeTime);
            //    spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            //    timer += Time.deltaTime;
            //    yield return null;
            //}

            // 移动
            Vector3 endPosition = startPosition - new Vector3(0f, distanceToMove, 0f);
            //timer = 0f;

            while (timer < moveTime)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, timer / moveTime);
                timer += Time.deltaTime;
                yield return null;
                if(timer>= (moveTime - fadeTime))
                {
                    timer2 += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, timer2/ fadeTime);
                    spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
                }
                
                
            }

            // 等待
            yield return new WaitForSeconds(waitTime);

            // 恢复位置和透明度
            transform.position = startPosition;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}




//using UnityEngine;
//using System.Collections;

//public class ShootingStarSprite : MonoBehaviour
//{
//    public float speed = 5f;                // 流星速度
//    public float fadeSpeed = 1f;            // 消失速度
//    public float maxFadeTime = 1f;          // 最长消失时间

//    private SpriteRenderer spriteRenderer;  // SpriteRenderer 组件
//    private bool isFading = false;          // 是否正在消失
//    private float fadeTime = 0f;            // 已经消失的时间
//    public float waittime = 2.5f;

//    void Start()
//    {
//        // 获取 SpriteRenderer 组件
//        spriteRenderer = GetComponent<SpriteRenderer>();

//        StartCoroutine(Shooting());
//    }


//    IEnumerator Shooting()
//    {
//        float originalY = transform.position.y;
//        print("test");
//        while (true)
//        {
//            // 流星移动
//            transform.Translate(Vector2.down * speed * Time.deltaTime);

//            // 如果流星已经到达屏幕底部，开始消失
//            if (transform.position.y <= originalY - 5)
//            {
//                isFading = true;
//            }

//            // 如果正在消失，逐渐调整透明度
//            if (isFading)
//            {
//                fadeTime += Time.deltaTime;
//                float alpha = 1f - Mathf.Clamp01(fadeTime / maxFadeTime);
//                Color c = spriteRenderer.color;
//                c.a = alpha;
//                spriteRenderer.color = c;

//                // 如果透明度调整完成，销毁对象
//                if (fadeTime >= maxFadeTime)
//                {
//                    //Destroy(gameObject);
//                    yield return new WaitForSeconds(waittime);
//                    transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
//                    c.a = 1f;
//                    spriteRenderer.color = c;
//                }
//            }

//        }
//    }
//    void Update()
//    {


//    }
//}
