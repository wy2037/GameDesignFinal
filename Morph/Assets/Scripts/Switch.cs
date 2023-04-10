using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (temperatureZone != null) {
                zoneTempIdx = (++zoneTempIdx) % zoneTemperature.Length;
                Debug.Log("Switch mode " + zoneTempIdx);
                temperatureZone.zoneTemperature = zoneTemperature[zoneTempIdx];
                if (PlayerData.Pd.temperature < zoneTemperature[zoneTempIdx]) {
                    temperatureZone.zoneType = ZoneType.Heater;
                }
                if (PlayerData.Pd.temperature > zoneTemperature[zoneTempIdx]) {
                    temperatureZone.zoneType = ZoneType.Cooler;
                }
            }
            if (windField != null) {
                windField.SetActive(!windField.activeInHierarchy);
                Debug.Log(windField.activeInHierarchy);
            }
        }
    }
}
