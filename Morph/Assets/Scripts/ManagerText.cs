//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;



//public class ManagerText : MonoBehaviour
//{

//    public GameObject canvas;
//    public float floatnumber = 10f;

//    private static ManagerText _instance;
//    public static ManagerText Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = FindObjectOfType<ManagerText>();
//                if (_instance == null)
//                {
//                    GameObject singletonObject = new GameObject();
//                    _instance = singletonObject.AddComponent<ManagerText>();
//                    singletonObject.name = typeof(SingletonExample).ToString() + " (Singleton)";
//                    DontDestroyOnLoad(singletonObject);
//                }
//            }
//            return _instance;
//        }
//    }

//    public string text = ""; // 要显示的文本
//    public float speed = 0.1f;  // 文字显示的速度
//    public float delay = 1.0f;  // 文字显示完成后停留的时间
//    public Vector3 position = new Vector3(0, -3, 0); // 文字的初始位置
//    //public Transform parent = null; // 文字生成的父物体







//    private TextMeshProUGUI textMeshPro;
//    private string originalText;
//    private GameObject textObject;



//    void textShow(string str)
//    {
//        // 隐藏 TextMeshProUGUI 对象
//        //textMeshPro = GetComponent<TextMeshProUGUI>();
//        //textMeshPro.enabled = false;

//        // 生成 TextMeshProUGUI 对象
//        textObject = new GameObject("TextMeshPro Text");
//        textObject.transform.SetParent(canvas.transform, false);
//        textObject.transform.position = position;

//        // 添加 TextMeshProUGUI 组件
//        textMeshPro = textObject.AddComponent<TextMeshProUGUI>();
//        textMeshPro.text = "teseteseteseteseteseteset";
//        print(textMeshPro.text);
//        //textMeshPro.text = str;
//        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0f);

//        // 开始显示文本
//        StartCoroutine(ShowText());

//        IEnumerator ShowText()
//        {
//            // 显示文本
//            for (int i = 0; i <= text.Length; i++)
//            {
//                textMeshPro.text = text.Substring(0, i);
//                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, (float)i / text.Length);
//                yield return new WaitForSeconds(speed);
//            }

//            // 停留一段时间
//            yield return new WaitForSeconds(delay);

//            // 隐藏文本
//            for (int i = text.Length; i >= 0; i--)
//            {
//                textMeshPro.text = text.Substring(0, i);
//                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, (float)i / text.Length);
//                yield return new WaitForSeconds(speed);
//            }

//            // 销毁 TextMeshProUGUI 对象
//            Destroy(textObject);
//        }
//    }





//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.T))
//        {
//            ManagerText.Instance.textShow("hihihihihihihihi");
//        }
//    }
//}



using System.Collections;
using TMPro;
using UnityEngine;

public class ManagerText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    [SerializeField] private float typingSpeed = 0.1f;

    [SerializeField] private float delay = 3f;
    private string textToType;

    private Coroutine typingCoroutine;

    public void StartTyping(string text,int position)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        textToType = text;
        switch (position)
        {
            case 1:
                textMeshPro.alignment = TextAlignmentOptions.TopLeft;
                break;
            case 2:
                textMeshPro.alignment = TextAlignmentOptions.Top;
                break;
            case 3:
                textMeshPro.alignment = TextAlignmentOptions.TopRight;
                break;
            case 4:
                textMeshPro.alignment = TextAlignmentOptions.MidlineLeft;
                break;
            case 5:
                textMeshPro.alignment = TextAlignmentOptions.Center;
                break;
            case 6:
                textMeshPro.alignment = TextAlignmentOptions.MidlineRight;
                break;
            case 7:
                textMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                break;
            case 8:
                textMeshPro.alignment = TextAlignmentOptions.Bottom;
                break;
            case 9:
                textMeshPro.alignment = TextAlignmentOptions.BottomRight;
                break;
            default:
                print("Wrong position number");
                break;
        }


        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textMeshPro.text = string.Empty;

        for (int i = 0; i < textToType.Length; i++)
        {
            textMeshPro.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        //yield return new WaitForSeconds(delay);

        //textMeshPro.text = "";


        //如果文字没有颜色需要先设置。这里默认不用。
        //textMeshPro.color = Color.white; // 将文本颜色设置为白色

        // 显示文本
        textMeshPro.alpha = 1f;
        float duration = 3.0f;
        // 等待指定的时间
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // 计算当前的 alpha 值
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // 更新文本的 alpha 值
            textMeshPro.alpha = alpha;

            // 等待一帧
            yield return null;

            // 累加已经等待的时间
            elapsedTime += Time.deltaTime;
        }

        // 隐藏文本(完全）
        textMeshPro.alpha = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartTyping("Hello, world!",1);
        }

    }

    ////copy and paste below to the detailed code and assign obj
    ////do not for get to set text position.
    //[SerializeField] private ManagerText managerText;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        managerText.ShowText();
    //    }
    //}
}
