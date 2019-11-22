using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform shipTransform;
    
    private Vector3 _screenSize;
    public float rotationSpeed;

    public bool invertX = false;
    public bool invertY = true;

    private int controlCircleRadius = 200;

    public float acceleration = 100;
    public float maxSpeed = 500;
    public float minSpeed = 300;
    public float currentSpeed;
    
    void Start()
    {
        _screenSize = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }

    void Update()
    {
        RotateShip();
        CalculateSpeed();
        MoveShip();
    }

    private void RotateShip()
    {
        var cursorPos = Input.mousePosition;
        var relativePos = cursorPos - _screenSize;
        var clampedPos = Vector2.ClampMagnitude(relativePos, controlCircleRadius);
        var clampedPercentage = Vector2.ClampMagnitude(clampedPos / controlCircleRadius, 1);
        Debug.Log(clampedPercentage);
        
        
        var rotation = new Vector3(
            clampedPos.y * (invertX ? 1 : -1),
            clampedPos.x * (invertY ? -1 : 1),
            0);
        
        transform.Rotate(Time.deltaTime * rotationSpeed * rotation);
        
        var targetShipRotation = Quaternion.Euler(clampedPercentage.y * -30, 0, clampedPercentage.x * -30);
        shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, targetShipRotation, Time.deltaTime); 
    }

    private void CalculateSpeed()
    {
        if (currentSpeed < minSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            return;
        }

        if (currentSpeed > maxSpeed)
        {
            currentSpeed -= acceleration * Time.deltaTime;
            return;
        }

        if (currentSpeed > minSpeed && Input.GetKey(InputConfiguration.KeyBindings.Brake))
        {
            currentSpeed -= acceleration * Time.deltaTime;
            return;
        }

        currentSpeed += acceleration * Time.deltaTime;
    }

    private void MoveShip()
    {
        transform.Translate(Time.deltaTime * currentSpeed * Vector3.forward);
    }
}
