using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMunipulation : MonoBehaviour
{
    public float raycastDistance = 5f;
    public LayerMask surfaceLayer;
    public KeyCode changeGravityKey = KeyCode.G;

    private Camera cam;
    private RaycastHit lastHit;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(changeGravityKey))
        {
            TryChangeGravityFromLook();
        }

        if (Input.GetKeyDown(KeyCode.E)) // "E" to lock gravity to wall you're touching
        {
            TryChangeGravityFromTouch();
        }
    }

    // Change gravity by looking at surface and pressing G
    void TryChangeGravityFromLook()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, surfaceLayer))
        {
            GravityManager.Instance.SetGravityFromSurfaceNormal(hit.normal);
            lastHit = hit; // Save for possible use
        }
    }

    // Change gravity by touching a wall and pressing E 
    void TryChangeGravityFromTouch()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f, surfaceLayer);
        foreach (Collider col in hits)
        {
            Vector3 directionToSurface = (transform.position - col.ClosestPoint(transform.position)).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -directionToSurface, out hit, 2f, surfaceLayer))
            {
                GravityManager.Instance.SetGravityFromSurfaceNormal(hit.normal);
                Debug.Log("Touched wall gravity changed!");
                break;
            }
        }
    }
}
