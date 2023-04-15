using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Switch : MonoBehaviour
{
    [SerializeField] private TemperatureZone temperatureZone;
    [SerializeField] private GameObject windField;
    [SerializeField] float[] zoneTemperature;
    int zoneTempIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnTriggerEnter2D(Collider2D collision) {
    //     if (collision.tag == "Player") {
    //         if (temperatureZone != null) {
    //             zoneTempIdx = (++zoneTempIdx) % zoneTemperature.Length;
    //             Debug.Log("Switch mode " + zoneTempIdx);
    //             temperatureZone.zoneTemperature = zoneTemperature[zoneTempIdx];
    //             if (PlayerData.Pd.temperature < zoneTemperature[zoneTempIdx]) {
    //                 temperatureZone.GetComponent<Tilemap>().color = Color.red;
    //                 temperatureZone.zoneType = ZoneType.Heater;
    //             }
    //             if (PlayerData.Pd.temperature > zoneTemperature[zoneTempIdx]) {
    //                 temperatureZone.GetComponent<Tilemap>().color = new Color(0, 234, 255);
    //                 temperatureZone.zoneType = ZoneType.Cooler;
    //             }
    //         }
    //         if (windField != null) {
    //             windField.SetActive(!windField.activeInHierarchy);
    //             Debug.Log(windField.activeInHierarchy);
    //         }
    //     }
    // }


    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)){
            if(temperatureZone != null){
                zoneTempIdx = (++zoneTempIdx) % zoneTemperature.Length;
                float resTemp = zoneTemperature[zoneTempIdx];
                temperatureZone.zoneType = getResultState(resTemp, temperatureZone.zoneType);
                temperatureZone.zoneTemperature = resTemp;
                temperatureZone.initializeZone();
            }

            if(windField != null){
                windField.SetActive(!windField.activeSelf);
            }
        }
    }



    ZoneType getResultState(float temp, ZoneType state){
        ZoneType res = ZoneType.HeaterMid;
        switch (state){
            case ZoneType.HeaterMid:
                if(temp >= 0 && temp < 100){
                    res = ZoneType.CoolerMid;
                }else{
                    res = ZoneType.CoolerLow;
                }
                break;
            case ZoneType.HeaterHigh:
                if(temp >= 0 && temp < 100){
                    res = ZoneType.CoolerMid;
                }else{
                    res = ZoneType.CoolerLow;
                }
                break;
            case ZoneType.CoolerLow:
                if(temp >= 0 && temp < 100){
                    res = ZoneType.HeaterMid;
                }else{
                    res = ZoneType.HeaterHigh;
                }
                break;
            case ZoneType.CoolerMid:
                if(temp >= 0 && temp < 100){
                    res = ZoneType.HeaterMid;
                }else{
                    res = ZoneType.HeaterHigh;
                }
                break;         
                       
        }
        return res;

    }
}
