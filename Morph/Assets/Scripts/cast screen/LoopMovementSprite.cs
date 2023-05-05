using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMovementSprite : MonoBehaviour
{
    public float speed = 2f; // 移动速度
    public float startX = 18f; // 指定起始位置
    public float endX = -18f; // 指定结束位置

    public float startWaitTime = 0f;
    private bool hasStarted = false;
    public float waitTime = 0f; // 等待时间
    public bool backToOriginalPosition = false;
    private Vector3 startPos; // 起始位置
    private bool isMoving = true; // 是否正在移动

    void Start()
    {
        startPos = transform.position; // 记录起始位置
        StartCoroutine(Move()); // 开始移动协程
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (!hasStarted)
            {
                yield return new WaitForSeconds(startWaitTime); // 等待
                hasStarted = true;
            }
            
            if (isMoving)
            {
                // 向左移动
                transform.position += Vector3.left * speed * Time.deltaTime;

                if (speed >= 0)
                {
                    // 到达结束位置
                    if (transform.position.x <= endX)
                    {
                        isMoving = false; // 停止移动
                        yield return new WaitForSeconds(waitTime); // 等待
                                                                   //isMoving = true; // 继续移动
                    }
                }
                else
                {
                    // 到达结束位置
                    if (transform.position.x >= endX)
                    {
                        isMoving = false; // 停止移动
                        yield return new WaitForSeconds(waitTime); // 等待
                                                                   //isMoving = true; // 继续移动
                    }
                }
                
            }
            else
            {
                if (backToOriginalPosition)
                {
                    // 返回起始位置
                    transform.position = startPos;
                }
                else
                {
                    //返回指定位置
                    transform.position = new Vector3(startX, startPos.y, startPos.z);
                    //yield return new WaitForSeconds(waitTime); // 等待
                    isMoving = true; // 继续移动
                }

                
            }

            yield return null;
        }
    }
}
