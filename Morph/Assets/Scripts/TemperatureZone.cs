using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneType{
    Furnace,
    Freezer
}

public class TemperatureZone : MonoBehaviour
{
    public int zoneTemperature;
    public int multiplier;
    public ZoneType zoneType;
    [SerializeField]
    float maxCooldown, cooldown = 0;

    void Update() {
        if (cooldown > 0) cooldown -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other) {
        Debug.Log("Test");
        if (other.tag == "Player" && cooldown <= 0) {
            if (zoneType == ZoneType.Furnace && PlayerData.Pd.temperature < zoneTemperature) {
                cooldown = maxCooldown;
                PlayerData.Pd.temperature += multiplier;
            }
            if (zoneType == ZoneType.Freezer && PlayerData.Pd.temperature > zoneTemperature) {
                cooldown = maxCooldown;
                PlayerData.Pd.temperature -= multiplier;
                tempManager.instance.lowerTemp();
            }
        }
    }
}