using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIndicator : MonoBehaviour
{
    
    private Transform solidIcon;
    private Transform liquidIcon;
    private Transform gasIcon;
    public TemperatureZone temperatureZone;

    private Transform middle;
    private Transform left;
    private Transform right;

    private State previousState;


    private void Start() {
        solidIcon = transform.Find("solid");
        liquidIcon = transform.Find("liquid");
        gasIcon = transform.Find("gas");

        updateIcon();
        previousState = PlayerData.Pd.state;

    }


    private void Update() {
        if(PlayerData.Pd.state != previousState){
            previousState = PlayerData.Pd.state;
            updateIcon();
        }
    }


    void updateIcon(){
        switch (PlayerData.Pd.state){
            case State.Solid:
                solidIcon.gameObject.SetActive(true);
                liquidIcon.gameObject.SetActive(false);
                gasIcon.gameObject.SetActive(false);

                if(temperatureZone.zoneType == ZoneType.HeaterMid){
                    liquidIcon.gameObject.SetActive(true);
                }
                else if(temperatureZone.zoneType == ZoneType.HeaterHigh){
                    liquidIcon.gameObject.SetActive(true);
                    gasIcon.gameObject.SetActive(true);
                }
                break;
            case State.Liquid:
                liquidIcon.gameObject.SetActive(true);
                solidIcon.gameObject.SetActive(false);
                gasIcon.gameObject.SetActive(false);
                if(temperatureZone.zoneType == ZoneType.CoolerLow){
                    solidIcon.gameObject.SetActive(true);
                }
                else if(temperatureZone.zoneType == ZoneType.HeaterHigh){
                    gasIcon.gameObject.SetActive(true);
                }
                break;
            case State.Gas:
                gasIcon.gameObject.SetActive(true);
                liquidIcon.gameObject.SetActive(false);
                solidIcon.gameObject.SetActive(false);

                if(temperatureZone.zoneType == ZoneType.CoolerMid){
                    liquidIcon.gameObject.SetActive(true);
                }
                else if(temperatureZone.zoneType == ZoneType.CoolerLow){
                    liquidIcon.gameObject.SetActive(true);
                    solidIcon.gameObject.SetActive(true);
                }
                break;
        }
    }
}
