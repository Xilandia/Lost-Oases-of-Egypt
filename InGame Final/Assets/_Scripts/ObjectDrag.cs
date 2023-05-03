using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }
    
    private void OnMouseDrag()
    {
        transform.position = BuildingSystem.SnapToGrid(BuildingSystem.GetMouseWorldPosition() + offset);
    }
}
