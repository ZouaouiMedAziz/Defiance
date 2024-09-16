using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public float torque = 200f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = torque;
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }
    }
}
