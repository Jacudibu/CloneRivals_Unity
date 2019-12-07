using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public float acceleration = 10;
    public float deceleration = 30;
    public float boostSpeed = 50;
    public float maxSpeed = 30;
    public float minSpeed = 0;
    public float boostDuration = 9;
    public float lateralSpeed = 10;

    public float rotationAtMinSpeed = 360;
    public float rotationAtMaxSpeed = 100;
    
    public float currentSpeed = 0;
}
