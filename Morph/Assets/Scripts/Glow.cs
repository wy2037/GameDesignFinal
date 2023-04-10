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
    
    void Update() {
        if (PlayerData.Pd.state == stateType) {
            if (objectType == Type.Hazard) {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            if (objectType == Type.Interactable) {
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        } else {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
