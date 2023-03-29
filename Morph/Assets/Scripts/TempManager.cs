using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempManager : MonoBehaviour
{
    public static TempManager instance;

    
    public TextMeshProUGUI tempText;


    int temp = 70;

    private void Awake(){
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //tempText = getComponent<TextMeshProUGUI>();
        tempText.text = temp.ToString() + "°C";
    }

    // Update is called once per frame
    private void FixedUpdate() {
        temp = PlayerData.Pd.temperature;
        tempText.text = temp.ToString() + "°C";

    }
    public void addTemp(){
        temp ++;
        tempText.text = temp.ToString() + "°C";
    }
    public void lowerTemp(){
        temp --;
        tempText.text = temp.ToString() + "°C";
    }
}
