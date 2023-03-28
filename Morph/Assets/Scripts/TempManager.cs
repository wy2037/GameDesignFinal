using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempManager : MonoBehaviour
{
    public static TempManager instance;

    public Text tempText;
    int temp = 70;

    private void Awake(){
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        tempText.text = temp.ToString() + "°C";
    }

    // Update is called once per frame
    public void addTemp(){
        temp ++;
        tempText.text = temp.ToString() + "°C";
    }
    public void lowerTemp(){
        temp --;
        tempText.text = temp.ToString() + "°C";
    }
}
