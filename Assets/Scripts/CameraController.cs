using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    //[SerializeField] private float rotationLerpSpeed = 2f;
    [SerializeField] private float positionLerpSpeed = 5f;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetTransform.position,
            Time.deltaTime * positionLerpSpeed);

        transform.rotation = targetTransform.rotation;

        /*
        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            targetTransform.rotation, 
            Time.deltaTime * rotationLerpSpeed);
        */
    }
}
