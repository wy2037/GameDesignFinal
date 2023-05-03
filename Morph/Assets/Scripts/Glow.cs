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
    [SerializeField] float alphaValue;
    
    void Update() {
        if (objectType == Type.Hazard && PlayerData.Pd.state == stateType) {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            gameObject.GetComponent<Renderer>().material.color = new Vector4(1,0,0,1);
        } else if (objectType == Type.Interactable && PlayerData.Pd.state != stateType) {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
            gameObject.GetComponent<Renderer>().material.color = new Vector4(0,1,0,1);
        } else {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            gameObject.GetComponent<Renderer>().material.color = new Vector4(1,1,1,alphaValue);
        }
    }
}
