using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{
    Solid,
    Liquid,
    Gas
}

public class PlayerData : MonoBehaviour
{
    private static PlayerData _pd;
    public static PlayerData Pd { get { return _pd; } }

    // 
    public int temperature;
    public float speed = 5f;
    public float jumpForce = 3f;
    public State state;
    public int gravity;



    private void Awake()
    {
        if (_pd != null && _pd != this)
        {
            Destroy(this.gameObject);
        } else {
            _pd = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }


}
