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
            Click();
        }
    }

    private void Click()
    {
        Touch touch = Input.GetTouch(0);

        // Process touch input when the touch begins
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
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
                    Debug.Log($"Cell position: {gridCell.Position}");
                }
            }
        }
    }
}
