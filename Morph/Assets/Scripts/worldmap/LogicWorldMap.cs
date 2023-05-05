using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicWorldMap : MonoBehaviour
{
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManagerScene ms = ManagerScene.Instance;
        if (ms.getFlag() == 1)
        {
            //触发自动开门进入Level1/World1的流程
            ms.setFlag(0);
        }
        //print(ms.firstTimeAtMap);
        {
            //如果不是第一次进入则无事发生，只需定位到已完成的World门口即可。
        }
        //测试使用，t进入turorial，c进入cast，enter进入level1
        if (Input.GetKeyDown(KeyCode.T))
        {
            ms.GoToScene("Tutorial Screen");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ms.GoToScene("Cast Screen Recovery");
            ManagerAudio.Instance.Stop();
            ManagerAudio.Instance.PlayThemeV();

        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    //ManagerAudio.Instance.Stop();
        //    ms.GoToScene("Title Screen");
        //}
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ms.GoToScene("Level 1");
        }
        
    }
}
