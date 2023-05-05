using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowCameraSprite : MonoBehaviour
{
    // 物体跟随的偏移量
    public Vector3 offset;

    private void Start()
    {
        //计算初始相对距离
        offset = transform.position- Camera.main.transform.position;
    }
    void LateUpdate()
    {
        
        // 获取主摄像机的位置
        Vector3 cameraPosition = Camera.main.transform.position;

        // 将偏移量（初始相对距离）应用到摄像机位置上
        Vector3 targetPosition = cameraPosition + offset;

        // 设置物体的位置
        transform.position = targetPosition;
    }
}
