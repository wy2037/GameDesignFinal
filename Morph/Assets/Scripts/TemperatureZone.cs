using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneType{
    Heater,
    Cooler
}

public class TemperatureZone : MonoBehaviour
{
    public int zoneTemperature, roomTemperature;
    public int multiplier;
    public ZoneType zoneType;
    [SerializeField]
    float maxCooldown, cooldown = 0;
    [SerializeField]
    bool inZone;

    void Update() {
        if (cooldown > 0) cooldown -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        inZone = true;
        if (other.tag == "Player") {
            if (zoneType == ZoneType.Heater) {
                GameFeelManager.Pm.heatUpEnter();
            }
            if (zoneType == ZoneType.Cooler) {
                GameFeelManager.Pm.coolDownEnter();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" && cooldown <= 0) {
            if (zoneType == ZoneType.Heater && PlayerData.Pd.temperature < zoneTemperature) {
                cooldown = maxCooldown;
                PlayerData.Pd.temperature += multiplier;
            }
            if (zoneType == ZoneType.Cooler && PlayerData.Pd.temperature > zoneTemperature) {
                cooldown = maxCooldown;
                PlayerData.Pd.temperature -= multiplier;
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            inZone = false;
            GameFeelManager.Pm.normalEnter();
            if (zoneType == ZoneType.Cooler) {
                while (PlayerData.Pd.temperature < roomTemperature && inZone == false) {
                    if (cooldown <= 0) {
                        cooldown = maxCooldown;
                        PlayerData.Pd.temperature += multiplier;
                    }
                }
            }
            if (zoneType == ZoneType.Heater) {
                while (PlayerData.Pd.temperature > roomTemperature && inZone == false) {
                    if (cooldown <= 0) {
                        cooldown = maxCooldown;
                        PlayerData.Pd.temperature -= multiplier;
                    }
                }
            }
        }
    }
}