using UnityEngine;
using UnityEngine.InputSystem;

public class AgarrarObjeto : MonoBehaviour
{
    private GameObject currentDraggedObject;
    private float dragDistance;
    private Vector3 dragOffset;
    private bool isDragging = false;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryStartDragging();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopDragging();
        }

        if (isDragging && currentDraggedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 targetPosition = ray.GetPoint(dragDistance);
            currentDraggedObject.transform.position = targetPosition + dragOffset;
        }
    }

    private void TryStartDragging()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Movible"))
            {
                currentDraggedObject = hit.collider.gameObject;
                dragDistance = Vector3.Distance(Camera.main.transform.position, hit.point);
                dragOffset = currentDraggedObject.transform.position - hit.point;
                isDragging = true;
            }
        }
    }

    private void StopDragging()
    {
        isDragging = false;
        currentDraggedObject = null;
    }
}