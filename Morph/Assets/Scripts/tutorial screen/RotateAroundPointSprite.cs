using UnityEngine;

public class RotateAroundPointSprite : MonoBehaviour
{
    public Transform centerPoint; // 指定位置为圆心的Transform组件
    public float rotationSpeed = 10f; // 旋转速度，单位为度/秒

    private void Update()
    {

        // 计算旋转角度
        float angle = rotationSpeed * Time.deltaTime;
        // 绕指定位置为圆心进行旋转
        transform.RotateAround(centerPoint.position, Vector3.forward, angle);
        // 保持角度不便（摩天轮效果）
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
