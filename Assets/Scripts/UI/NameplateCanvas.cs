using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NameplateCanvas : MonoBehaviour
{
    public GameObject namePlatePrefab;
    
    [Range(0, 50)] public float extraHeight;
    public float drawDistance = 500;
    private float _scaleDistance;
    
    private Plane[] _cameraFrustumPlanes;
    private Transform _playerTransform;
    private Camera _camera;
    private List<DrawableObjectData> _drawables;

    private struct DrawableObjectData
    {
        public readonly GameObject Object;
        public readonly GameObject NamePlate;

        public DrawableObjectData(GameObject obj, GameObject namePlate)
        {
            NamePlate = namePlate;
            Object = obj;
        }
    }

    private void OnValidate()
    {
        _scaleDistance = drawDistance * 0.01f;
    }

    private void Start()
    {
        OnValidate();
        _camera = Camera.main;
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        namePlatePrefab.SetActive(false);

        _drawables = FindObjectsOfType<Health>()
            .Where(x => x.GetComponent<PlayerController>() == null)
            .Select(x => new DrawableObjectData(x.gameObject, Instantiate(namePlatePrefab, this.transform, true)))
            .ToList();
        
        foreach (var drawable in _drawables)
        {
            drawable.NamePlate.GetComponent<TextMeshProUGUI>().text = drawable.Object.name;
        }
    }
    
    private void Update()
    {
        _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);

        foreach (var drawable in _drawables)
        {
            if (!IsObjectInScreenArea(drawable.Object.transform.position))
            {
                drawable.NamePlate.SetActive(false);
                continue;
            }

            var distance = (drawable.Object.transform.position - _playerTransform.position).magnitude;
            if (distance > drawDistance)
            {
                drawable.NamePlate.SetActive(false);
                continue;
            }

            var scaleFactor = 1f;
            
            if (drawDistance - distance < drawDistance * 0.01f)
            {
                scaleFactor = (drawDistance - distance) / (drawDistance * 0.01f);
            }
            
            drawable.NamePlate.SetActive(true);
            drawable.NamePlate.transform.localScale = Vector3.one * scaleFactor;
            drawable.NamePlate.transform.position = _camera.WorldToScreenPoint(drawable.Object.transform.position) + Vector3.up * extraHeight;
        }
    }
    
    private bool IsObjectInScreenArea(Vector3 position)
    {
        var bounds = new Bounds(position, Vector3.one);
        return GeometryUtility.TestPlanesAABB(_cameraFrustumPlanes, bounds);
    }
}