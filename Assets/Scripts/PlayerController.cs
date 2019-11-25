using System.Collections;
using System.Collections.Generic;
using InputConfiguration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Transform cameraAnchor;
    
    private Vector3 _screenSize;
    private Vector3 _defaultLocalCameraAnchorPosition;
    public float rotationSpeed = 0.4f;
    public float shipAlignSpeed = 4;

    public bool invertX = false;
    public bool invertY = true;

    public int controlCircleRadius = 200;
    
    public float acceleration = 100;
    public float maxSpeed = 500;
    public float minSpeed = 300;
    public float currentSpeed;

    public float strafeAnimationSpeed = 5;
    public float strafeValue;
    public float strafeTravelSpeed = 10;
    
    private bool _strafeLeft;
    private bool _strafeRight;
    
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
    }

    private void MoveCameraAnchor()
    {
        cameraAnchor.transform.localPosition = _defaultLocalCameraAnchorPosition + new Vector3(
                                                   strafeValue * 2,
                                                   0, 0);
    }

    private void RotateShip()
    {
        var cursorPos = Input.mousePosition;
        var relativePos = cursorPos - _screenSize;
        var clampedPos = Vector2.ClampMagnitude(relativePos, controlCircleRadius);
        var clampedPercentage = Vector2.ClampMagnitude(clampedPos / controlCircleRadius, 1);
        
        var rotation = new Vector3(
            clampedPos.y * (invertX ? 1 : -1),
            clampedPos.x * (invertY ? -1 : 1),
            0);
        
        transform.Rotate(Time.deltaTime * rotationSpeed * rotation);

        var strafeRotation = new Vector3(
            0,
            0,
            Mathf.Lerp(-70, 70, (strafeValue + 1) * 0.5f)
        );

        var targetShipRotation = new Vector3(
            clampedPercentage.y * 20 * (invertX ? 1 : -1),
            0,
            strafeRotation.z);  //clampedPercentage.x * 30 * (invertY ? -1 : 1) * (clampedPercentage.y > 0 ? -1 : 0));

        shipTransform.localRotation = Quaternion.Lerp(
            shipTransform.localRotation, 
            Quaternion.Euler(targetShipRotation),
            Time.deltaTime * shipAlignSpeed);
            
    }

    private void Strafe()
    {
        if ((!_strafeLeft && !_strafeRight) || (_strafeLeft && _strafeRight))
        {
            if (strafeValue > 0.01f)
            {
                strafeValue -= strafeAnimationSpeed * Time.deltaTime;
            } 
            else if (strafeValue < -0.01f)
            {
                strafeValue += strafeAnimationSpeed * Time.deltaTime;
            }
            else
            {
                strafeValue = 0f;
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
        var movement = currentSpeed * Vector3.forward;

        if (_strafeLeft)
        {
            movement.x -= strafeTravelSpeed;
        }
        if (_strafeRight)
        {
            movement.x += strafeTravelSpeed;
        }
        
        transform.Translate(Time.deltaTime * movement);
    }
}
