using UnityEngine;

public class InputController : MonoBehaviour
{
    // Layer mask for the "Cell" layer
    [SerializeField] private LayerMask cellLayerMask;

    private void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        // Check for mouse input
        else if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);

        // Process touch input when the touch begins
        if (touch.phase == TouchPhase.Began)
        {
            CastRayFromPos(touch.position);
        }
    }

    private void HandleMouseInput()
    {
        // Get the mouse position and create a ray
        CastRayFromPos(Input.mousePosition);
    }

    /// <summary>
    /// Creates a ray from the screen position.
    /// </summary>
    private void CastRayFromPos(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit hit;

        // Cast a ray and check if it hits an object on the "Cell" layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayerMask))
        {
            // Get the GameObject that was hit
            GameObject hitObject = hit.collider.gameObject;

            // Example: Call a method on the hit object
            GridCell gridCell = hitObject.GetComponent<GridCell>();
            if (gridCell != null)
            {
                gridCell.CellClicked();
            }
        }
    }
}
