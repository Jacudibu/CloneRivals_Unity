using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float rightLerpSpeed = 5f;
    [SerializeField] private float upLerpSpeed = 2f;
    [SerializeField] private float forwardLerpSpeed = 2f;
    
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }
    
    private void LateUpdate()
    {
        _transform.rotation = targetTransform.rotation;

        var direction = targetTransform.position - _transform.position;

        var transformForward = _transform.forward;
        var transformRight = _transform.right;
        var transformUp = _transform.up;
        
        var forward = Vector3.Dot(direction, transformForward);
        var right = Vector3.Dot(direction, transformRight);
        var up = Vector3.Dot(direction, transformUp);

        forward = Mathf.Lerp(0, forward, Time.deltaTime * forwardLerpSpeed);
        right = Mathf.Lerp(0, right, Time.deltaTime * rightLerpSpeed);
        up = Mathf.Lerp(0, up, Time.deltaTime * upLerpSpeed);
        
        _transform.position +=
                     transformForward * forward
                   + transformRight * right
                   + transformUp * up;
    }
}
