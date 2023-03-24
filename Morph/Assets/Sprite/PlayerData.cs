using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData _pd;
    public static PlayerData Pd;

    // 
    public int temperature;
    public int gravity;



    private void Awake()
    {
        if (_pd != null && _pd != this)
        {
            Destroy(this.gameObject);
        } else {
            _pd = this;
        }
    }


}
