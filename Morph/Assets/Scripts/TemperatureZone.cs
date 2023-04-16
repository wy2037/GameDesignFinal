using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;


public enum ZoneType{
    HeaterMid,
    HeaterHigh,
    CoolerMid,
    CoolerLow
}

public class TemperatureZone : MonoBehaviour
{
    [SerializeField] public float zoneTemperature;
    [SerializeField] private float multiplier;
    [SerializeField] public ZoneType zoneType;
    [SerializeField] float maxCooldown, cooldown = 0;
    private Tilemap _tilemap;

    public Color heaterMidColor;
    public Color heaterHighColor;
    public Color coolerMidColor;
    public Color coolerLowColor;


    public Transform labelPrefab;
    private List<Transform> labelLocations = new List<Transform>{};

    private void Awake() {


    }
    private void Start() {
        
        _tilemap = GetComponent<Tilemap>();
        foreach (Transform child in transform){
            labelLocations.Add(child);
        }
        foreach (var l in labelLocations){
            GameObject obj = Instantiate(labelPrefab.gameObject, l.position, Quaternion.identity);
            obj.GetComponent<StateIndicator>().temperatureZone = this;
        }
        initializeZone();



    }

    public void initializeZone(){
        switch (zoneType){
            case ZoneType.HeaterMid:
                _tilemap.color = heaterMidColor;
                break;
            case ZoneType.HeaterHigh:
                _tilemap.color = heaterHighColor;
                break;
            case ZoneType.CoolerMid:
                _tilemap.color = coolerMidColor;
                break;
            case ZoneType.CoolerLow:
                _tilemap.color = coolerLowColor;
                break;

        }

    }

    void Update() {
        if (cooldown > 0) cooldown -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        try{
            RoomTemperature.Rt.inZone = true;
        }
        catch{

        }
        if (other.tag == "Player") {
            if (zoneType == ZoneType.HeaterMid || zoneType == ZoneType.HeaterHigh) {
                accelerate();
                GameFeelManager.Pm.heatUpEnter();
            }
            if (zoneType == ZoneType.CoolerMid || zoneType == ZoneType.CoolerLow) {
                accelerate();
                GameFeelManager.Pm.coolDownEnter();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" && cooldown <= 0) {
            if (zoneType == ZoneType.HeaterMid && PlayerData.Pd.temperature < zoneTemperature) {
                PlayerData.Pd.temperature += multiplier;
            }
            else if (zoneType == ZoneType.HeaterHigh && PlayerData.Pd.temperature < zoneTemperature) {
                PlayerData.Pd.temperature += (1.5f * multiplier);
            }
            else if (zoneType == ZoneType.CoolerMid && PlayerData.Pd.temperature > zoneTemperature) {
                PlayerData.Pd.temperature -= multiplier;
            }
            else if (zoneType == ZoneType.CoolerLow && PlayerData.Pd.temperature > zoneTemperature) {
                PlayerData.Pd.temperature -= (1.5f * multiplier);
            }
            cooldown = maxCooldown;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            GameFeelManager.Pm.normalEnter();
            try{
                RoomTemperature.Rt.inZone = false;
            }
            catch{
                
            }
        }
    }

    Tween accelerate(){
        multiplier = 0f;
        return DOTween.To(() => multiplier, x => multiplier = x, 1f, 2.5f);
    }
}