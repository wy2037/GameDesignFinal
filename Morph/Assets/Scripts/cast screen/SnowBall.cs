
//可能废弃方案

using UnityEngine;

public class SnowBall : MonoBehaviour
{
    public GameObject snowflakePrefab;  // 雪花预制体
    public float snowflakeSpeed = 2f;   // 雪花下落速度
    public float snowflakeFrequency = 0.5f;  // 生成雪花的频率
    public float xRange = 5f;  // 雪花生成的x轴范围

    private float elapsedTime = 0f;  // 记录经过的时间

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // 每隔一定时间生成一个雪花
        if (elapsedTime >= snowflakeFrequency)
        {
            elapsedTime = 0f;

            // 随机生成雪花的x轴位置
            float xPos = Random.Range(-xRange, xRange);
            // 生成雪花并设置位置
            GameObject snowflake = Instantiate(snowflakePrefab, new Vector3(xPos, transform.position.y, 0f), Quaternion.identity);
            // 设置雪花下落速度
            snowflake.GetComponent<Rigidbody2D>().velocity = Vector2.down * snowflakeSpeed;
        }
    }
}
