using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Vector3 _screenSize;
    public float rotationSpeed;

    public bool invertX = false;
    public bool invertY = true;
    
    void Start()
    {
        _screenSize = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }

    void Update()
    {
        var cursorPos = Input.mousePosition;

        var relativePos = cursorPos - _screenSize;
        var clampedPos = Vector2.ClampMagnitude(relativePos, 1000);
        
        var rotation = new Vector3(
            clampedPos.y * (invertX ? 1 : -1),
            clampedPos.x * (invertY ? -1 : 1),
            0);
        rotation *= rotationSpeed;
        rotation *= Time.deltaTime;
        
        transform.Rotate(rotation);
    }
}
