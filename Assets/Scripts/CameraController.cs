using Settings.InputConfiguration;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float rightLerpSpeed = 5f;
    [SerializeField] private float upLerpSpeed = 2f;
    [SerializeField] private float forwardLerpSpeed = 2f;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera rearCamera;
    
    private Transform _mainCameraTransform;

    private void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        _mainCameraTransform = mainCamera.transform;
    }
    
    private void LateUpdate()
    {
        mainCamera.enabled = !KeyBindings.RearCamera.IsPressed();
        rearCamera.enabled = !mainCamera.enabled;
        
        _mainCameraTransform.rotation = targetTransform.rotation;

        var direction = targetTransform.position - _mainCameraTransform.position;

        var transformForward = _mainCameraTransform.forward;
        var transformRight = _mainCameraTransform.right;
        var transformUp = _mainCameraTransform.up;
        
        var forward = Vector3.Dot(direction, transformForward);
        var right = Vector3.Dot(direction, transformRight);
        var up = Vector3.Dot(direction, transformUp);

        forward = Mathf.Lerp(0, forward, Time.deltaTime * forwardLerpSpeed);
        right = Mathf.Lerp(0, right, Time.deltaTime * rightLerpSpeed);
        up = Mathf.Lerp(0, up, Time.deltaTime * upLerpSpeed);
        
        _mainCameraTransform.position +=
                     transformForward * forward
                   + transformRight * right
                   + transformUp * up;
    }
}
