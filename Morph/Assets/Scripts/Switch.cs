using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Switch : MonoBehaviour
{
    [SerializeField] private TemperatureZone[] temperatureZones;
    [SerializeField] private GameObject[] windFields;
    [SerializeField] float[] zoneTemperature;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private bool state;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private LayerMask playerLayer;
    private bool playerNearby;

    int zoneTempIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == true) {
            sprite.color = Color.green;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        } else {
            sprite.color = Color.red;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        playerNearby = Physics2D.OverlapBox(transform.position, new Vector2(2, 2), 0, playerLayer);
        if (playerNearby && Input.GetKeyDown(KeyCode.F)) {
            state = !state;
            foreach (TemperatureZone temperatureZone in temperatureZones) {
                if(temperatureZone != null){
                    zoneTempIdx = (++zoneTempIdx) % zoneTemperature.Length;
                    float resTemp = zoneTemperature[zoneTempIdx];
                    temperatureZone.zoneType = getResultState(resTemp, temperatureZone.zoneType);
                    temperatureZone.zoneTemperature = resTemp;
                    temperatureZone.initializeZone();
                }
            }
            foreach (GameObject windField in windFields) {
                if(windField != null){
                    windField.SetActive(!windField.activeSelf);
                }
            }
        }
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

/*
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)){
            state = !state;
            foreach (TemperatureZone temperatureZone in temperatureZones) {
                if(temperatureZone != null){
                    zoneTempIdx = (++zoneTempIdx) % zoneTemperature.Length;
                    float resTemp = zoneTemperature[zoneTempIdx];
                    temperatureZone.zoneType = getResultState(resTemp, temperatureZone.zoneType);
                    temperatureZone.zoneTemperature = resTemp;
                    temperatureZone.initializeZone();
                }
            }
            foreach (GameObject windField in windFields) {
                if(windField != null){
                    windField.SetActive(!windField.activeSelf);
                }
            }
        }
    }
*/

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
