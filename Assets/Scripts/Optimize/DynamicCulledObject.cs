using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCulledObject : MonoBehaviour
{
    private Camera _playerCamera;
    private Renderer _renderer;
    private Renderer[] _childrenRenderers;
    
    private void Start()
    {
        _playerCamera = Camera.main;
        _renderer = GetComponent<Renderer>();
        _childrenRenderers = GetComponentsInChildren<Renderer>();
    }

    private void OnBecameVisible()
    {
        foreach (var childRenderer in _childrenRenderers)
        {
            childRenderer.enabled = true;
        }
    }
    
    private void OnBecameInvisible()
    {
        foreach (var childRenderer in _childrenRenderers)
        {
            childRenderer.enabled = false;
        }
    }


    /*
    private void Update()
    {
        if (InCameraView() && !BehindObject())
        {
            foreach (var childRenderer in _childrenRenderers)
            {
                childRenderer.enabled = true;
            }
        }
        else
        {
            foreach (var childRenderer in _childrenRenderers)
            {
                childRenderer.enabled = false;
            }
        }
    }
    
    private bool InCameraView()
    {
       var planes = GeometryUtility.CalculateFrustumPlanes(_playerCamera);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }

    private bool BehindObject()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(_playerCamera);
        var inView = GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
        
        if (Physics.Linecast(_playerCamera.transform.position, transform.position, out var hit))
        {
            if (hit.transform == transform)
            {
                return false;
            }
        }
        return true;
    }

    private void RenderLineHit()
    {
        if (Physics.Linecast(_playerCamera.transform.position, transform.position, out var hit))
        {
            var renderers = hit.collider.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = true;
            }
        }
    }
    */
}
