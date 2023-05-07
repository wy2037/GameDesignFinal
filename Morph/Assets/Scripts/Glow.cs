using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
    Hazard,
    Interactable
}

public class Glow : MonoBehaviour
{
    public Type objectType;
    public State stateType;
    [SerializeField] GameObject light2D;
    [SerializeField] GameObject windZone;
    [SerializeField] float alphaValue;
    
    void Update() {
        if (objectType == Type.Hazard && PlayerData.Pd.state == stateType) {
            light2D.SetActive(true);
            gameObject.GetComponent<Renderer>().material.color = new Vector4(1,0.5f,0.5f,1);
        } else if (objectType == Type.Interactable && PlayerData.Pd.state != stateType) {
            light2D.SetActive(true);
            windZone.GetComponent<ParticleSystem>().Play();   
            gameObject.GetComponent<Renderer>().material.color = new Vector4(1,1,1,1);        
        } else {
            light2D.SetActive(false);
            if (objectType == Type.Interactable) {
                windZone.GetComponent<ParticleSystem>().Stop();  
            }
            gameObject.GetComponent<Renderer>().material.color = new Vector4(1,1,1,alphaValue);
        }
    }
}
