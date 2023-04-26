using UnityEngine;
using System.Collections;

public class BounceSprite : MonoBehaviour
{
    public float amplitude = 0.1f;  // 起伏幅度
    public float speed = 1f;        // 起伏速度

    private float startY;           // Sprite 初始位置
    private float currentY;         // 当前 Sprite 位置

    public float timeoffset = 0.0f;

    void Start()
    {
        // 获取 Sprite 初始位置
        startY = transform.position.y;
    }

    void Update()
    {
        
        // 计算当前 Sprite 位置
        currentY = startY + amplitude * Mathf.Sin(speed * Time.time+timeoffset);

        // 更新 Sprite 位置
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
    }
}
