



using System.Collections;
using UnityEngine;

public class CastCameraLogic : MonoBehaviour
{
    public Transform startPoint;
    public Transform targetPoint;
    public float startSize = 2f;
    public float waitTime = 7f;
    public GameObject firstnames;

    //public Vector3 startPosition; //记得分配
    //public Vector3 targetPosition; //记得分配
    public float targetSize = 5f;
    public float transitionDuration = 5f;

    private SpriteRenderer spriteRenderer; // Sprite的渲染器


    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        spriteRenderer = firstnames.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        camera.transform.position = startPoint.transform.position;
        camera.orthographicSize = startSize;

        // 等待5秒
        yield return new WaitForSeconds(waitTime);

        //firstnames.SetActive(false);


        // 缓慢移动摄像机位置
        // 缓慢调整摄像机大小
        float elapsedTime = 0f;
        //Vector3 startPosition = transform.position;

        float initialSize = camera.orthographicSize;


        Color originalColor = spriteRenderer.color; // 获取初始的颜色


        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            camera.transform.position = Vector3.Lerp(startPoint.position, targetPoint.position, elapsedTime / transitionDuration);


            elapsedTime += Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / transitionDuration);

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            spriteRenderer.color = newColor;

            yield return null;
        }

        //// 
        //elapsedTime = 0f;
        //float initialSize = camera.orthographicSize;
        //while (elapsedTime < transitionDuration)
        //{
            
        //    yield return null;
        //}
    }
}
