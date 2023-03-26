using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player" && PlayerData.Pd.state == State.Solid) {
            Debug.Log("Solid player hit spike!");
            // Call function in player to damage
        }
    }
}
