using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastClickInput : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private LayerMask interactionLayer;

    public void Interact()
    {
        var unit = TryRaycast2D<Unit>(mainCamera, Input.mousePosition);
        if (unit != null)
            unit.SelectUnit();        
    }

    private T TryRaycast2D<T>(Camera cam, Vector2 pos)
    {
        Ray ray = cam.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, interactionLayer);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<T>();
        }

        return default;
    }
}
