using UnityEngine;

public class WiggleSprite : MonoBehaviour
{
    public float wiggleDuration = 0.5f; // 掰动的时间
    public float wiggleIntensity = 15f; // 掰动的强度

    private float waitTimer = 0f; //掰动前等待的计时器
    private float wiggleTimer = 0f; // 掰动的计时器
    private bool isWiggling = false; // 是否在掰动

    public float rangeNum = 11f; //指定随机数的上边界，也就是秒

    private float randomNum = -1f; //掰动等待时间随机数


    private void Update()
    {
        if (!isWiggling)
        {
            if (randomNum == -1f) {
                randomNum = Random.Range(6, rangeNum); // 生成范围内的随机数
                print(randomNum);
                waitTimer = 0;
                waitTimer += Time.deltaTime;
            }
            else
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= randomNum)
                {
                    
                    wiggleTimer = 0f;
                    randomNum = -1f;
                    isWiggling = true;
                }
            }
            
            


            ////if (Random.Range(0f, 1f) < Time.deltaTime / wiggleDuration)
            //if (Random.Range(0f, 1f) >0.99f)
            //{
            //    wiggleTimer = 0f;
            //    isWiggling = true;
            //}
        }
        else
        {
            
            // 控制掰动的强度和时间
            if (wiggleTimer <= wiggleDuration)
            {
                float wiggleFactor = Mathf.Sin(wiggleTimer / wiggleDuration * Mathf.PI) * wiggleIntensity;
                transform.rotation = Quaternion.Euler(0f, 0f, -wiggleFactor);

                wiggleTimer += Time.deltaTime;
            }
            else if (wiggleTimer > wiggleDuration && wiggleTimer <= 2 * wiggleDuration)
            {
                float wiggleFactor = Mathf.Sin((wiggleTimer - wiggleDuration) / wiggleDuration * Mathf.PI) * wiggleIntensity;
                transform.rotation = Quaternion.Euler(0f, 0f, wiggleFactor);

                wiggleTimer += Time.deltaTime;
            }
            else if (wiggleTimer > 2*wiggleDuration && wiggleTimer <= 3 * wiggleDuration)
            {
                float wiggleFactor = Mathf.Sin((wiggleTimer - wiggleDuration) / wiggleDuration * Mathf.PI) * wiggleIntensity;
                transform.rotation = Quaternion.Euler(0f, 0f, wiggleFactor/3);

                wiggleTimer += Time.deltaTime;
            }
            else if (wiggleTimer > 3*wiggleDuration && wiggleTimer <= 4 * wiggleDuration)
            {
                float wiggleFactor = Mathf.Sin((wiggleTimer - wiggleDuration) / wiggleDuration * Mathf.PI) * wiggleIntensity;
                transform.rotation = Quaternion.Euler(0f, 0f, wiggleFactor/3);

                wiggleTimer += Time.deltaTime;
            }
            else
            {
                // 掰动结束
                //if (wiggleTimer > 2 * wiggleDuration)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    isWiggling = false;
                    print("here");
                }
            }

            
        }
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WiggleSprite : MonoBehaviour
//{
//    public float rotationSpeed = 30f; // 旋转速度
//    //public Vector3 rotatePoint; // 旋转中心点
//    public Transform rotatePoint;//

//    public float maxAngle = 30f; // 最大旋转角度

//    private Quaternion initialRotation; // 初始旋转
//    private float currentAngle; // 当前旋转角度
//    private bool isRotating; // 是否正在旋转

//    void Start()
//    {
//        initialRotation = transform.rotation; // 记录初始旋转
//        isRotating = false; // 初始不进行旋转
//    }

//    void Update()
//    {
//        if (!isRotating)
//        {
//            StartCoroutine(RotateAroundPointCoroutine()); // 开始协程，随机旋转一次
//        }
//    }

//    IEnumerator RotateAroundPointCoroutine()
//    {
//        isRotating = true; // 设置正在旋转状态
//        float randomTime = Random.Range(0.0f, 1.0f); // 随机旋转时间
//        float startTime = Time.time; // 记录开始时间

//        while (Time.time - startTime < randomTime) // 在指定时间内旋转
//        {
//            float angle = Mathf.Sin(Time.time * rotationSpeed) * maxAngle; // 计算旋转角度
//            Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward); // 计算新的旋转角度
//            transform.rotation = initialRotation * Quaternion.Inverse(initialRotation) * newRotation * initialRotation; // 根据中心点旋转
//            yield return null;
//        }

//        // 回到初始角度
//        while (currentAngle > 0)
//        {
//            float angle = Mathf.Lerp(currentAngle, 0, Time.deltaTime / (randomTime * 0.5f)); // 计算当前旋转角度
//            Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward); // 计算新的旋转角度
//            transform.rotation = initialRotation * Quaternion.Inverse(initialRotation) * newRotation * initialRotation; // 根据中心点旋转
//            currentAngle = angle;
//            yield return null;
//        }

//        isRotating = false; // 重置正在旋转状态
//    }
//}
