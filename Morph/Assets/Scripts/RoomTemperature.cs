using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemperature : MonoBehaviour
{
    private static RoomTemperature _rt;
    public static RoomTemperature Rt { get { return _rt; } }

    public bool inZone;
    bool test = true; // scuffed way of fixing this
    public float roomTemperature, multiplier;
    [SerializeField]
    float maxCooldown, cooldown;

    private void Awake()
    {
        inZone = false;
        if (_rt != null && _rt != this)
        {
            Destroy(this.gameObject);
        } else {
            _rt = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        if (test == true) {
            inZone = false;
            test = false;
        } 
        
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

        if(Input.GetKeyDown(KeyCode.R)){
            string a = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(a);
        }
    }
}
