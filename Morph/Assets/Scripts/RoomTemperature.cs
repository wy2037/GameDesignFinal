using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemperature : MonoBehaviour
{
    private static RoomTemperature _rt;
    public static RoomTemperature Rt { get { return _rt; } }

    public bool inZone = false;
    public int roomTemperature, multiplier;
    [SerializeField]
    float maxCooldown, cooldown;

    private void Awake()
    {
        if (_rt != null && _rt != this)
        {
            Destroy(this.gameObject);
        } else {
            _rt = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        if (cooldown > 0) {
            cooldown -= Time.deltaTime;
        }
    
        if (inZone == false) {
            if (PlayerData.Pd.temperature < roomTemperature) {
                if (cooldown <= 0) {
                    cooldown = maxCooldown;
                    PlayerData.Pd.temperature += multiplier;
                }
            }
            if (PlayerData.Pd.temperature > roomTemperature) {
                if (cooldown <= 0) {
                    cooldown = maxCooldown;
                    PlayerData.Pd.temperature -= multiplier;
                }
            }
        }
    }
}