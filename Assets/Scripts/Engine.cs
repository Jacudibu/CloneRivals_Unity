using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class Engine : MonoBehaviour
{
    public float acceleration = 10;
    public float deceleration = 30;
    public float boostSpeed = 50;
    public float maxSpeed = 30;
    public float minSpeed = 0;
    public float boostDuration = 9;
    public float lateralSpeed = 10;

    public Vector3 requestedRotationAmount = Vector3.zero;
    public float rotationAtMinSpeed = 360;
    public float rotationAtMaxSpeed = 100;
    
    public float currentSpeed = 0;

    public float strafeValue = 0;

    public bool allowRotation = true;
    public bool allowMovement = true;
    
    private void Update()
    {
        if (allowRotation)
        {
            Rotate();
        }

        if (allowMovement)
        {
            MoveForward();
        }
    }

    private void Rotate()
    {
        // TODO: Actually I think right now we rotate from 0 to 100 - where 100 is 360° ? Easy to adjust, just needs some testing.
        var rotationSpeed = Mathf.Lerp(rotationAtMinSpeed, rotationAtMaxSpeed, currentSpeed / maxSpeed);
        
        transform.Rotate(Time.deltaTime * rotationSpeed * requestedRotationAmount);
    }

    private void MoveForward()
    {
        var movement = currentSpeed * Vector3.forward;
        movement.x -= strafeValue * lateralSpeed;
        
        transform.Translate(Time.deltaTime * movement);
    }
}
