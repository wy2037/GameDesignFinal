using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Thermobar : MonoBehaviour
{

    public Slider slider;
    //public int temp;
    
    public void SetTemp(float temp){
        slider.value = temp;
    }

    void Start(){
        //temp = PlayerData.Pd.temperature;
        SetTemp(PlayerData.Pd.temperature);
    }

    void Update(){
        //temp = PlayerData.Pd.temperature;
        SetTemp(PlayerData.Pd.temperature);
    }
}
