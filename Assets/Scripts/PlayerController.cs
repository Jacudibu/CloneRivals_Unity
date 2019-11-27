using System.Collections;
using System.Collections.Generic;
using InputConfiguration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Transform cameraAnchor;
    
    private Vector3 _screenSize;
    private Vector3 _defaultLocalCameraAnchorPosition;
    public float rotationAtMinSpeed = 360;
    public float rotationAtMaxSpeed = 100;
    
    public float shipAlignSpeed = 2;

    public bool invertX = false;
    public bool invertY = false;

    public int controlCircleRadius = 200;
    
    public float acceleration = 100;
    public float boostSpeed = 500;
    public float maxSpeed = 300;
    public float minSpeed = 0;
    public float currentSpeed = 0;

    public float strafeAnimationSpeed = 2;
    public float strafeValue = 0;
    public float lateralSpeed = 10;
    
    private bool _strafeLeft;
    private bool _strafeRight;

    public TextMeshProUGUI uiTextSpeed;
    
    void Start()
    {
        _screenSize = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        _defaultLocalCameraAnchorPosition = cameraAnchor.transform.localPosition;
    }
    
    void Update()
    {
        _strafeLeft = Input.GetKey(KeyBindings.StrafeLeft);
        _strafeRight = Input.GetKey(KeyBindings.StrafeRight);
        
        Strafe();
        RotateShip();
        CalculateSpeed();
        MoveShip();
        MoveCameraAnchor();

        uiTextSpeed.text = (currentSpeed * 10).ToString("F0");
    }

    private void MoveCameraAnchor()
    {
    }

    private void RotateShip()
    {
        var cursorPos = Input.mousePosition;
        var relativePos = cursorPos - _screenSize;
        var clampedPos = Vector2.ClampMagnitude(relativePos, controlCircleRadius);
        var clampedPercentage = Vector2.ClampMagnitude(clampedPos / controlCircleRadius, 1);
        
        var rotation = new Vector3(
            clampedPercentage.y * (invertX ? 1 : -1),
            clampedPercentage.x * (invertY ? -1 : 1),
            0);
        
        // TODO: Actually we rotate from 0 to 100 - where 100 is 360°
        var rotationSpeed = Mathf.Lerp(rotationAtMinSpeed, rotationAtMaxSpeed, currentSpeed / maxSpeed);
        
        transform.Rotate(Time.deltaTime * rotationSpeed * rotation);

        var strafeRotation = Mathf.Lerp(-120, 120, (strafeValue + 1) * 0.5f);

        var targetShipRotation = new Vector3(
            clampedPercentage.y * 20 * (invertX ? 1 : -1),
            0,
            Mathf.Clamp(clampedPercentage.x * 60 * (invertY ? 1 : -1) + strafeRotation, -60, 60));

        shipTransform.localRotation = Quaternion.Lerp(
            shipTransform.localRotation, 
            Quaternion.Euler(targetShipRotation),
            Time.deltaTime * shipAlignSpeed);
    }

    private void Strafe()
    {
        if ((!_strafeLeft && !_strafeRight) || (_strafeLeft && _strafeRight))
        {
            if (strafeValue > 0)
            {
                strafeValue -= strafeAnimationSpeed * Time.deltaTime;
                if (strafeValue < 0)
                {
                    strafeValue = 0;
                }
            } 
            else
            {
                strafeValue += strafeAnimationSpeed * Time.deltaTime;
                if (strafeValue > 0)
                {
                    strafeValue = 0;
                }
            }
        }
        else
        {
            if (_strafeLeft)
            {
                strafeValue += strafeAnimationSpeed * Time.deltaTime;
            } 
            else
            {
                strafeValue -= strafeAnimationSpeed * Time.deltaTime;
            }

            strafeValue = Mathf.Clamp(strafeValue, -1f, 1f);
        }
    }

    private void CalculateSpeed()
    {
        if (Input.GetKey(KeyBindings.Boost))
        {
            if (currentSpeed < boostSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed = boostSpeed;
            }

            return;
        }
    
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

        if (currentSpeed > minSpeed && Input.GetKey(KeyBindings.Brake))
        {
            currentSpeed -= acceleration * Time.deltaTime;
            return;
        }

        currentSpeed += acceleration * Time.deltaTime;
    }

    private void MoveShip()
    {
        var movement = currentSpeed * Vector3.forward;
        movement.x -= strafeValue * lateralSpeed;
        
        transform.Translate(Time.deltaTime * movement);
    }
}
